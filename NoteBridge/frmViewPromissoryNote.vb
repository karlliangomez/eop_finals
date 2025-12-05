Imports System.Data.OleDb
Imports System.Drawing

Public Class frmViewPromissoryNote

    ' Private DataTable to hold all PN request records for lookup
    Private pnHistoryDataTable As New DataTable()

    Private Sub frmViewPromissoryNote_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Promissory Note History"

        ' Set all textboxes to Read-Only
        txtStudentNo.ReadOnly = True
        txtStudentName.ReadOnly = True
        txtCourse.ReadOnly = True
        txtPNNumber.ReadOnly = True
        txtExamType.ReadOnly = True
        txtContactNo.ReadOnly = True
        txtStatus.ReadOnly = True

        LoadStudentDetails()
        LoadPNRequests()
        PopulatePNSelector()
    End Sub

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
            ' Fetch all PN requests for the student, ordered by most recent date
            Dim sql As String = "SELECT PNNumber, ExamType, ContactNo, GuardianName, RequestDate, Status " &
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

    Private Sub PopulatePNSelector()
        cmbPNSelector.Items.Clear()

        If pnHistoryDataTable.Rows.Count = 0 Then
            cmbPNSelector.Items.Add("--- No Requests Found ---")
            cmbPNSelector.SelectedIndex = 0
            cmbPNSelector.Enabled = False
            ' Clear summary textboxes if no data exists
            txtPNNumber.Clear()
            txtExamType.Clear()
            txtContactNo.Clear()
            txtStatus.Text = "N/A"
            Exit Sub
        End If

        ' Add all PN requests, formatting as "Exam Type (PN Number)"
        For Each row As DataRow In pnHistoryDataTable.Rows
            Dim displayItem As String = row("ExamType").ToString() & " (" & row("PNNumber").ToString() & ")"
            cmbPNSelector.Items.Add(displayItem)
        Next

        ' Select the most recent request (index 0) to populate initial details
        cmbPNSelector.SelectedIndex = 0
        cmbPNSelector.Enabled = True
    End Sub

    Private Sub cmbPNSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPNSelector.SelectedIndexChanged
        If cmbPNSelector.SelectedItem Is Nothing OrElse cmbPNSelector.Enabled = False Then Exit Sub

        ' Extract the unique PN Number embedded in the ComboBox item: "Exam Type (PN-YYMM-000)"
        Dim selectedItem As String = cmbPNSelector.SelectedItem.ToString()

        ' Find the PN number inside the parentheses
        Dim startIndex As Integer = selectedItem.IndexOf("(") + 1
        Dim endIndex As Integer = selectedItem.IndexOf(")")

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
            txtContactNo.Clear()
            txtStatus.Clear()
        End If
    End Sub

End Class