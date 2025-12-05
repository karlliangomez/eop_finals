Imports System.Data.OleDb
Imports System.Drawing
Imports System.Data ' Needed for DataTable

Public Class frmViewPromissoryNote

    ' NOTE: LoggedStudentID must be set by the calling form.
    Public LoggedStudentID As String

    ' Private DataTable to hold all PN request records for lookup (now includes Semester)
    Private pnHistoryDataTable As New DataTable()

    Private Sub frmViewPromissoryNote_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Promissory Note History"

        If String.IsNullOrEmpty(LoggedStudentID) Then
            MessageBox.Show("Error: Student Number not found. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Exit Sub
        End If

        ' Set all textboxes to Read-Only
        txtStudentNo.ReadOnly = True
        txtStudentName.ReadOnly = True
        txtCourse.ReadOnly = True
        txtPNNumber.ReadOnly = True
        txtExamType.ReadOnly = True
        txtSemester.ReadOnly = True ' 🛑 NEW TEXTBOX 🛑
        txtContactNo.ReadOnly = True
        txtStatus.ReadOnly = True

        LoadStudentDetails()
        LoadPNRequests()
        PopulatePNSelector()
    End Sub

    ' -------------------------------------------------------------------------
    ' ## Data Loading
    ' -------------------------------------------------------------------------

    Private Sub LoadStudentDetails()
        Dim studentNo As String = LoggedStudentID
        txtStudentNo.Text = studentNo

        If Not IsConnOpen() Then Exit Sub

        Try
            ' Fetch Name and Course from tblStudents
            Dim sql As String = "SELECT StudentName, Course FROM tblStudents WHERE StudentNo = @sno"
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@sno", studentNo)

            Using dr As OleDbDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    txtStudentName.Text = dr("StudentName").ToString()
                    txtCourse.Text = dr("Course").ToString()
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading student details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub LoadPNRequests()
        Dim studentNo As String = LoggedStudentID

        pnHistoryDataTable.Clear()

        If Not IsConnOpen() Then Exit Sub

        Try
            ' 🛑 MODIFIED SQL: Now selecting the Semester field 🛑
            Dim sql As String = "SELECT PNNumber, ExamType, Semester, ContactNo, GuardianName, RequestDate, Status " &
                                "FROM tblPNRequests WHERE StudentNo = @sno ORDER BY RequestDate DESC, RequestID DESC"

            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@sno", studentNo)

            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(pnHistoryDataTable)

        Catch ex As Exception
            MessageBox.Show("Error loading PN requests: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' -------------------------------------------------------------------------
    ' ## UI Population
    ' -------------------------------------------------------------------------

    Private Sub PopulatePNSelector()
        cmbPNSelector.Items.Clear()

        If pnHistoryDataTable.Rows.Count = 0 Then
            cmbPNSelector.Items.Add("--- No Requests Found ---")
            cmbPNSelector.SelectedIndex = 0
            cmbPNSelector.Enabled = False

            ' Clear summary textboxes if no data exists
            txtPNNumber.Clear()
            txtExamType.Clear()
            txtSemester.Clear() ' 🛑 CLEAR NEW TEXTBOX 🛑
            txtContactNo.Clear()
            txtStatus.Text = "N/A"
            Exit Sub
        End If

        ' 🛑 MODIFIED DISPLAY: Add Semester to the ComboBox item 🛑
        ' Format as "Exam Type (Semester) [PN Number]"
        For Each row As DataRow In pnHistoryDataTable.Rows
            Dim pnNumber As String = If(row("PNNumber") Is DBNull.Value, "N/A", row("PNNumber").ToString())
            Dim semester As String = If(row("Semester") Is DBNull.Value, "N/A", row("Semester").ToString())

            Dim displayItem As String = row("ExamType").ToString() & " (" & semester & ") [" & pnNumber & "]"
            cmbPNSelector.Items.Add(displayItem)
        Next

        ' Select the most recent request (index 0) to populate initial details
        cmbPNSelector.SelectedIndex = 0
        cmbPNSelector.Enabled = True
    End Sub

    Private Sub cmbPNSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPNSelector.SelectedIndexChanged
        If cmbPNSelector.SelectedItem Is Nothing OrElse cmbPNSelector.Enabled = False Then Exit Sub

        ' Extract the unique PN Number embedded in the ComboBox item: "Exam Type (Semester) [PN-YYMM-000]"
        Dim selectedItem As String = cmbPNSelector.SelectedItem.ToString()

        ' Find the PN number inside the square brackets (more robust than parentheses alone)
        Dim startIndex As Integer = selectedItem.IndexOf("[") + 1
        Dim endIndex As Integer = selectedItem.IndexOf("]")

        If startIndex <= 0 OrElse endIndex <= startIndex Then
            Exit Sub
        End If

        ' Extract the PN Number
        Dim pnNumberToFind As String = selectedItem.Substring(startIndex, endIndex - startIndex)

        ' Find the corresponding row in the DataTable using the extracted PN Number
        Dim foundRows() As DataRow = pnHistoryDataTable.Select("PNNumber = '" & pnNumberToFind.Replace("'", "''") & "'")

        If foundRows.Length > 0 Then
            Dim selectedRow As DataRow = foundRows(0)

            ' Update Summary Textboxes
            txtPNNumber.Text = selectedRow("PNNumber").ToString()
            txtExamType.Text = selectedRow("ExamType").ToString()
            txtSemester.Text = selectedRow("Semester").ToString() ' 🛑 DISPLAY NEW FIELD 🛑
            txtContactNo.Text = selectedRow("ContactNo").ToString()
            txtStatus.Text = selectedRow("Status").ToString()

            ' Set visual cue based on status
            Select Case selectedRow("Status").ToString()
                Case "Approved (Auto)", "Approved"
                    txtStatus.BackColor = Color.LightGreen
                Case "Pending (Review Required)", "Pending"
                    txtStatus.BackColor = Color.Yellow
                Case "Denied"
                    txtStatus.BackColor = Color.LightCoral
                Case Else
                    txtStatus.BackColor = SystemColors.Control
            End Select

        Else
            ' Clear summary fields if the PN is not found (shouldn't happen)
            txtPNNumber.Clear()
            txtExamType.Clear()
            txtSemester.Clear()
            txtContactNo.Clear()
            txtStatus.Clear()
        End If
    End Sub

End Class