Imports System.Data.OleDb
Imports System.Linq
Imports System.Drawing

Public Class frmRequestPromissoryNote

    Public StudentNo As String

    ' Global variables to hold the financial context
    Private activeSemester As String
    Private nextInstallmentNo As Integer = 1

    ' Dictionary to map Installment Number to Exam Type 
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"},
        {7, "Finals Exam"}
    }

    Private Sub frmRequestPromissoryNote_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Promissory Note Request"

        ' Assume txtAmountDue, txtStudentNo, txtStudentName, txtCourse, txtYearSection, 
        ' lblActiveSemester, cmbExamType, txtGuardianName exist.

        If String.IsNullOrEmpty(StudentNo) Then
            MessageBox.Show("Student ID not loaded. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Exit Sub
        End If

        LoadStudentInfo(StudentNo)
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Data Loading and Semester Detection
    ' --------------------------------------------------------------------------------

    Private Sub LoadStudentInfo(sno As String)
        If Not IsConnOpen() Then Exit Sub

        Try
            ' 1. Fetch Student Details
            Dim sql As String = "SELECT StudentName, Course, YearSection FROM tblStudents WHERE StudentNo = @sno"
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@sno", sno)

            Using dr As OleDbDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    txtStudentNo.Text = sno
                    txtStudentName.Text = dr("StudentName").ToString()
                    txtCourse.Text = dr("Course").ToString()
                    txtYearSection.Text = dr("YearSection").ToString()

                    ' 2. Determine and Store Active Semester
                    DetermineActiveSemester(sno)
                Else
                    MessageBox.Show("Student record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub
                End If
            End Using

            ' 3. Populate Exam Types based on the determined next installment
            PopulateExamTypes()

        Catch ex As Exception
            MessageBox.Show("Error loading student details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub DetermineActiveSemester(sno As String)
        If Not IsConnOpen() Then Exit Sub

        Dim sqlActiveSem As String = "SELECT TOP 1 Semester, InstallmentNo FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE ORDER BY Semester ASC, InstallmentNo ASC"
        Dim cmdActiveSem As New OleDbCommand(sqlActiveSem, con)
        cmdActiveSem.Parameters.AddWithValue("@sno", sno)

        Using drSem As OleDbDataReader = cmdActiveSem.ExecuteReader()
            If drSem.Read() Then
                activeSemester = drSem("Semester").ToString()
                nextInstallmentNo = Convert.ToInt32(drSem("InstallmentNo"))

                lblActiveSemester.Text = "Requesting for: " & activeSemester
                lblActiveSemester.BackColor = System.Drawing.Color.LightYellow
            Else
                activeSemester = ""
                nextInstallmentNo = 7
                lblActiveSemester.Text = "Payment schedule clear."
                lblActiveSemester.BackColor = System.Drawing.Color.LightGreen
            End If
        End Using
    End Sub

    Private Sub PopulateExamTypes()
        cmbExamType.Items.Clear()

        ' Only show exam types that correspond to the next required installment or later
        Dim validExams = ExamMap.Where(Function(kvp) kvp.Key >= nextInstallmentNo) _
                               .OrderBy(Function(kvp) kvp.Key)

        For Each kvp In validExams
            cmbExamType.Items.Add(kvp.Value)
        Next

        If cmbExamType.Items.Count > 0 Then
            cmbExamType.SelectedIndex = 0
            cmbExamType.Enabled = True
        Else
            cmbExamType.Enabled = False
            txtAmountDue.Text = "N/A"
        End If
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Check Amount Due when Exam Type changes
    ' --------------------------------------------------------------------------------

    Private Sub cmbExamType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExamType.SelectedIndexChanged
        If cmbExamType.SelectedIndex = -1 OrElse String.IsNullOrEmpty(activeSemester) Then
            txtAmountDue.Text = ""
            Exit Sub
        End If

        If Not IsConnOpen() Then Exit Sub

        Dim selectedExamType As String = cmbExamType.SelectedItem.ToString()
        Dim instNo As Integer = ExamMap.FirstOrDefault(Function(kvp) kvp.Value = selectedExamType).Key

        If instNo = 0 Then
            txtAmountDue.Text = "Error"
            Exit Sub
        End If

        Try
            Dim sqlBalance As String = "SELECT Balance FROM tblPaymentSchedule WHERE StudentNo = @sno AND Semester = @sem AND InstallmentNo = @instNo"
            Dim cmdBalance As New OleDbCommand(sqlBalance, con)
            cmdBalance.Parameters.AddWithValue("@sno", StudentNo)
            cmdBalance.Parameters.AddWithValue("@sem", activeSemester)
            cmdBalance.Parameters.AddWithValue("@instNo", instNo)

            Dim balanceResult = cmdBalance.ExecuteScalar()
            Dim balance As Decimal = 0D

            If balanceResult IsNot DBNull.Value AndAlso balanceResult IsNot Nothing Then
                balance = Convert.ToDecimal(balanceResult)
            End If

            txtAmountDue.Text = balance.ToString("C2")

            If balance > 0 Then
                txtAmountDue.BackColor = System.Drawing.Color.Red
                txtAmountDue.ForeColor = System.Drawing.Color.White
            Else
                txtAmountDue.BackColor = System.Drawing.Color.LightGreen
                txtAmountDue.ForeColor = System.Drawing.Color.Black
            End If

        Catch ex As Exception
            txtAmountDue.Text = "Error"
            MessageBox.Show("Error checking installment balance: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## NEW FUNCTION: Check if the installment *before* the current one is paid
    ' --------------------------------------------------------------------------------

    Private Function IsPreviousInstallmentPaid(studentNo As String, semester As String, currentInstNo As Integer) As Boolean
        ' For Installment 2 (Long Test), the previous one is 1 (Down Payment).
        Dim prevInstNo As Integer = currentInstNo - 1

        ' Installment 1 is the down payment; payment is always required from Inst. 2 onward.
        If prevInstNo < 1 Then Return True

        If Not IsConnOpen() Then Return False

        Try
            ' Check if the previous installment is marked as paid
            Dim sqlCheck As String = "SELECT IsPaid FROM tblPaymentSchedule WHERE StudentNo = @sno AND Semester = @sem AND InstallmentNo = @prevInstNo"
            Using cmdCheck As New OleDbCommand(sqlCheck, con)
                cmdCheck.Parameters.AddWithValue("@sno", studentNo)
                cmdCheck.Parameters.AddWithValue("@sem", semester)
                cmdCheck.Parameters.AddWithValue("@prevInstNo", prevInstNo)

                Dim result = cmdCheck.ExecuteScalar()

                ' Access stores True as -1 and False as 0 for Yes/No fields.
                If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                    Return CBool(result)
                Else
                    Return False ' Previous installment record not found
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error checking previous payment: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Function

    ' --------------------------------------------------------------------------------
    ' ## Request Submission Logic (MODIFIED for Auto-Approval)
    ' --------------------------------------------------------------------------------

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        If String.IsNullOrEmpty(activeSemester) OrElse txtAmountDue.Text = 0.ToString("C2") Then
            MessageBox.Show("Cannot submit request. Payment schedule is clear or selected installment is fully paid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If cmbExamType.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtGuardianName.Text) Then
            MessageBox.Show("Please select an Exam Type and enter Guardian Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim selectedExamType As String = cmbExamType.SelectedItem.ToString()
        Dim guardianName As String = txtGuardianName.Text.Trim()

        ' --- 1. DETERMINE STATUS ---
        Dim requestStatus As String = "Pending (Review Required)"
        Dim successMessage As String = "Promissory Note request for the " & selectedExamType & " submitted successfully for the " & activeSemester & ". Status is Pending."

        ' Find the installment number for the selected exam
        Dim currentInstNo As Integer = ExamMap.FirstOrDefault(Function(kvp) kvp.Value = selectedExamType).Key

        If currentInstNo > 1 Then ' Only check previous payment if we are past the initial installment
            If IsPreviousInstallmentPaid(StudentNo, activeSemester, currentInstNo) Then
                requestStatus = "Approved (Auto)"
                successMessage = "Promissory Note request for the " & selectedExamType & " has been **AUTOMATICALLY APPROVED** for the " & activeSemester & "."
            End If
        End If

        ' --- 2. VALIDATION & INSERTION ---
        If Not IsConnOpen() Then Exit Sub

        Try
            ' 2.1. Check if a PN already exists for this exam type and semester
            Dim sqlCheck As String = "SELECT COUNT(*) FROM tblPNRequests WHERE StudentNo = @sno AND ExamType = @exam AND Semester = @sem"
            Using cmdCheck As New OleDbCommand(sqlCheck, con)
                cmdCheck.Parameters.AddWithValue("@sno", StudentNo)
                cmdCheck.Parameters.AddWithValue("@exam", selectedExamType)
                cmdCheck.Parameters.AddWithValue("@sem", activeSemester)

                Dim count As Integer = CInt(cmdCheck.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("A Promissory Note request for the " & selectedExamType & " in the " & activeSemester & " already exists.", "Request Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End Using

            ' 2.2. Insert the New PN Request with the determined status
            Dim sqlInsert As String = "INSERT INTO tblPNRequests (StudentNo, ExamType, GuardianName, RequestDate, Status, Semester) " &
                                      "VALUES (@sno, @exam, @gname, @rdate, @status, @sem)"
            Using cmdInsert As New OleDbCommand(sqlInsert, con)
                cmdInsert.Parameters.AddWithValue("@sno", StudentNo)
                cmdInsert.Parameters.AddWithValue("@exam", selectedExamType)
                cmdInsert.Parameters.AddWithValue("@gname", guardianName)
                cmdInsert.Parameters.AddWithValue("@rdate", DateTime.Now.ToShortDateString())
                cmdInsert.Parameters.AddWithValue("@status", requestStatus) ' 🛑 Use determined status 🛑
                cmdInsert.Parameters.AddWithValue("@sem", activeSemester)

                cmdInsert.ExecuteNonQuery()
            End Using

            MessageBox.Show(successMessage, "Request Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ClearFields()
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error submitting PN request: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Utility
    ' --------------------------------------------------------------------------------

    Private Sub ClearFields()
        txtGuardianName.Clear()
        If cmbExamType.Items.Count > 0 Then cmbExamType.SelectedIndex = 0
        txtAmountDue.Clear()
        txtAmountDue.BackColor = System.Drawing.SystemColors.Control
        txtAmountDue.ForeColor = System.Drawing.Color.Black
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class