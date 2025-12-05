Imports System.Data.OleDb
Imports System.Linq
Imports System.IO
Imports System.Windows.Forms
Imports System.Diagnostics ' Added for Process.Start (used in ExportToExcel)

Public Class frmPromissoryNoteSummary

    ' Dictionary to map Installment Number to Exam Type (Must match other forms)
    ' Finals Exam (Installment 7) is EXCLUDED.
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"}
    }

    ' 🛑 Read-only dictionary to map ExamType strings back to InstallmentNo 🛑
    Private ReadOnly ExamMapReverse As New Dictionary(Of String, Integer) From {
        {"Long Test (Prelims)", 2},
        {"Prelim Exam", 3},
        {"Long Test (Midterms)", 4},
        {"Midterm Exam", 5},
        {"Pre-Finals Exam", 6}
    }

    Private Sub frmPromissoryNoteSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Promissory Note Summary (Admin View)"
        PopulateExamFilter()
        PopulateSemesterFilter() ' 🛑 NEW: Populate Semester filter 🛑
        LoadPNData() ' Load initial data
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Data Loading and Filtering
    ' --------------------------------------------------------------------------------

    Private Sub PopulateExamFilter()
        cmbExamType.Items.Clear()
        cmbExamType.Items.Add("--- All Exam Types ---") ' Renamed for clarity

        ' Add all valid exam types from the map
        For Each examType In ExamMap.Values
            cmbExamType.Items.Add(examType)
        Next

        ' Set this filter to be the default
        If cmbExamType.Items.Count > 0 Then
            cmbExamType.SelectedIndex = 0
        End If
    End Sub

    ' 🛑 NEW SUBROUTINE: Populate Semester Filter from the database 🛑
    Private Sub PopulateSemesterFilter()
        cmbSemesterFilter.Items.Clear()
        cmbSemesterFilter.Items.Add("--- All Semesters ---")

        If Not IsConnOpen() Then Exit Sub

        Try
            Dim sql As String = "SELECT DISTINCT Semester FROM tblPNRequests ORDER BY Semester DESC"
            Dim cmd As New OleDbCommand(sql, con)

            Using dr As OleDbDataReader = cmd.ExecuteReader()
                While dr.Read()
                    If dr("Semester") IsNot DBNull.Value Then
                        cmbSemesterFilter.Items.Add(dr("Semester").ToString())
                    End If
                End While
            End Using

        Catch ex As Exception
            ' Log error but don't prevent form loading
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try

        If cmbSemesterFilter.Items.Count > 0 Then
            cmbSemesterFilter.SelectedIndex = 0
        End If
    End Sub

    Public Sub LoadPNData()
        If Not IsConnOpen() Then Exit Sub

        Dim whereClause As New System.Text.StringBuilder(" WHERE 1=1 ") ' Start with always true condition

        ' --- 1. Filter by Exam Type ---
        If cmbExamType.SelectedIndex > 0 Then
            Dim selectedExam = cmbExamType.SelectedItem.ToString()
            whereClause.Append(" AND ExamType = '" & selectedExam.Replace("'", "''") & "'")
        End If

        ' --- 2. Filter by Semester ---
        If cmbSemesterFilter.SelectedIndex > 0 Then
            Dim selectedSemester = cmbSemesterFilter.SelectedItem.ToString()
            whereClause.Append(" AND Semester = '" & selectedSemester.Replace("'", "''") & "'")
        End If

        Try
            ' 🛑 CHANGE: Added Semester to the SELECT statement 🛑
            Dim sql As String = "SELECT PNNumber, StudentNo, ExamType, Status, RequestDate, GuardianName, Semester " &
                                "FROM tblPNRequests" & whereClause.ToString() & " ORDER BY Semester DESC, RequestDate DESC, PNNumber DESC"

            Dim cmd As New OleDbCommand(sql, con)
            Dim da As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable()

            da.Fill(dt)
            dgvPNRequests.DataSource = dt

            ' Formatting the DataGridView
            dgvPNRequests.ReadOnly = True
            dgvPNRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvPNRequests.AllowUserToAddRows = False
            dgvPNRequests.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

            ' 🛑 NEW HEADERS: Semester and StudentNo 🛑
            If dgvPNRequests.Columns.Contains("Semester") Then
                dgvPNRequests.Columns("Semester").HeaderText = "Semester"
            End If
            If dgvPNRequests.Columns.Contains("StudentNo") Then
                dgvPNRequests.Columns("StudentNo").HeaderText = "Student No."
            End If
            If dgvPNRequests.Columns.Contains("PNNumber") Then
                dgvPNRequests.Columns("PNNumber").HeaderText = "PN No."
            End If
            If dgvPNRequests.Columns.Contains("ExamType") Then
                dgvPNRequests.Columns("ExamType").HeaderText = "Exam Permit"
            End If
            If dgvPNRequests.Columns.Contains("Status") Then
                dgvPNRequests.Columns("Status").HeaderText = "Approval Status"
            End If
            If dgvPNRequests.Columns.Contains("RequestDate") Then
                dgvPNRequests.Columns("RequestDate").HeaderText = "Date Requested"
            End If


        Catch ex As Exception
            MessageBox.Show("Error loading Promissory Note data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Event Handlers
    ' --------------------------------------------------------------------------------

    Private Sub cmbExamType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExamType.SelectedIndexChanged
        LoadPNData() ' Reload data whenever exam filter changes
    End Sub

    ' 🛑 NEW HANDLER: For Semester filter 🛑
    Private Sub cmbSemesterFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSemesterFilter.SelectedIndexChanged
        LoadPNData() ' Reload data whenever semester filter changes
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadPNData()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportToExcel(dgvPNRequests)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Export Subroutine (CSV File Creation)
    ' --------------------------------------------------------------------------------

    Private Sub ExportToExcel(dgv As DataGridView)
        ' ... (ExportToExcel logic is unchanged and correctly handles all columns)
        If dgv.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' 1. Use a Save File Dialog to get the desired path
        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            sfd.Title = "Save Promissory Note Summary File"
            sfd.FileName = "PN_Summary_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"

            If sfd.ShowDialog() = DialogResult.OK Then
                Dim filePath As String = sfd.FileName

                Try
                    ' 2. Use StreamWriter to write data to the file
                    Using sw As New StreamWriter(filePath)

                        ' --- Write Header Row ---
                        Dim headerLine As New System.Text.StringBuilder()
                        For i As Integer = 0 To dgv.ColumnCount - 1
                            ' Replace commas in headers to avoid breaking the header line structure
                            headerLine.Append(dgv.Columns(i).HeaderText.Replace(",", "") & ",")
                        Next
                        sw.WriteLine(headerLine.ToString().TrimEnd(","c)) ' Write and remove trailing comma

                        ' --- Write Data Rows ---
                        For Each row As DataGridViewRow In dgv.Rows
                            If Not row.IsNewRow Then
                                Dim dataLine As New System.Text.StringBuilder()
                                For i As Integer = 0 To dgv.ColumnCount - 1
                                    ' Get value safely
                                    Dim cellValue As String = If(row.Cells(i).Value Is DBNull.Value, "", row.Cells(i).Value.ToString())

                                    ' Simple CSV sanitation: enclose in quotes if value contains a comma or quote
                                    If cellValue.Contains(",") Or cellValue.Contains("""") Then
                                        cellValue = """" & cellValue.Replace("""", """""") & """"
                                    End If

                                    dataLine.Append(cellValue & ",")
                                Next
                                sw.WriteLine(dataLine.ToString().TrimEnd(","c)) ' Write and remove trailing comma
                            End If
                        Next
                    End Using

                    MessageBox.Show("Export successful! File saved to: " & filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Optional: Offer to open the file with Excel immediately
                    If MessageBox.Show("Do you want to open the exported file now?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        ' Need to ensure System.Diagnostics is imported (added above)
                        Process.Start(filePath)
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error writing file: " & ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

End Class