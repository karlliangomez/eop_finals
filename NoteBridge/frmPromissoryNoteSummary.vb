Imports System.Data.OleDb
Imports System.Linq
Imports System.IO ' Required for StreamWriter and File operations
Imports System.Windows.Forms ' Required for SaveFileDialog and Clipboard (though mostly replaced by file I/O)

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

    Private Sub frmPromissoryNoteSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Promissory Note Summary (Admin View)"
        PopulateExamFilter()
        LoadPNData() ' Load initial data
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Data Loading and Filtering
    ' --------------------------------------------------------------------------------

    Private Sub PopulateExamFilter()
        cmbExamType.Items.Clear()
        cmbExamType.Items.Add("All Exam Types")

        ' Add all valid exam types from the map
        For Each examType In ExamMap.Values
            cmbExamType.Items.Add(examType)
        Next

        cmbExamType.SelectedIndex = 0 ' Default to "All Exam Types"
    End Sub

    Public Sub LoadPNData()
        If Not IsConnOpen() Then Exit Sub

        Dim filterCondition As String = ""

        ' Build the WHERE clause if a specific exam type is selected
        If cmbExamType.SelectedIndex > 0 Then
            Dim selectedExam = cmbExamType.SelectedItem.ToString()
            filterCondition = " WHERE ExamType = '" & selectedExam & "'"
        End If

        Try
            ' Select all relevant fields for the summary
            Dim sql As String = "SELECT PNNumber, StudentNo, ExamType, Status, RequestDate, GuardianName " &
                                "FROM tblPNRequests" & filterCondition & " ORDER BY RequestDate DESC, PNNumber DESC"

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
        LoadPNData() ' Reload data whenever filter changes
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
                        Process.Start(filePath)
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error writing file: " & ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

End Class