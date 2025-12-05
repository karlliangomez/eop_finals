Imports System.Data.OleDb
Imports System.Drawing
Imports System.Linq

Public Class frmRequestPromissoryNote

    ' Dictionary to map Installment Number (after Installment 1/Downpayment) to Exam Type
    ' 🛑 FINALS EXAM (Installment 7) IS INTENTIONALLY EXCLUDED 🛑
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"}
    }

    Private Sub frmRequestPromissoryNote_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Request Promissory Note"

        ' Set read-only fields for student details
        txtStudentNo.ReadOnly = True
        txtStudentName.ReadOnly = True
        txtCourse.ReadOnly = True
        txtYearSection.ReadOnly = True

        LoadStudentDetails()
        PopulateExamComboBox()
    End Sub

    Private Sub LoadStudentDetails()
        Dim studentNo As String = LoggedStudentID
        txtStudentNo.Text = studentNo

        If Not IsConnOpen() Then Exit Sub

        Try
            Dim sql As String = "SELECT StudentName, Course, YearSection FROM tblStudents WHERE StudentNo = @sno"
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@sno", studentNo)

            Using dr As OleDbDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    txtStudentName.Text = dr("StudentName").ToString()
                    txtCourse.Text = dr("Course").ToString()
                    txtYearSection.Text = dr("YearSection").ToString()
                Else
                    MessageBox.Show("Student details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading student details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Logic to populate ComboBox, excluding already requested PNs and cleared payments.
    Private Sub PopulateExamComboBox()
        Dim studentNo As String = LoggedStudentID

        If Not IsConnOpen() Then Exit Sub

        Try
            ' --- A. Find the smallest InstallmentNo that is NOT paid (Next Due Exam) ---
            Dim sqlNextDue As String = "SELECT MIN(InstallmentNo) FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE"
            Dim cmdNextDue As New OleDbCommand(sqlNextDue, con)
            cmdNextDue.Parameters.AddWithValue("@sno", studentNo)

            Dim nextDueInstallment As Integer = 0
            Dim result = cmdNextDue.ExecuteScalar()

            If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                nextDueInstallment = Convert.ToInt32(result)
            End If

            ' --- B. Get list of Exam Types that already have a PN Request ---
            Dim requestedExams As New List(Of String)
            Dim sqlRequested As String = "SELECT ExamType FROM tblPNRequests WHERE StudentNo = @sno"
            Dim cmdRequested As New OleDbCommand(sqlRequested, con)
            cmdRequested.Parameters.AddWithValue("@sno", studentNo)

            Using dr As OleDbDataReader = cmdRequested.ExecuteReader()
                While dr.Read()
                    requestedExams.Add(dr("ExamType").ToString())
                End While
            End Using

            cmbExamType.Items.Clear()

            ' --- C. Populate ComboBox with only available exams ---
            For Each entry In ExamMap
                If entry.Key >= nextDueInstallment Then
                    ' Check if the exam type has ALREADY been requested
                    If Not requestedExams.Contains(entry.Value) Then
                        cmbExamType.Items.Add(entry.Value)
                    End If
                End If
            Next

            If cmbExamType.Items.Count > 0 Then
                cmbExamType.SelectedIndex = 0
                cmbExamType.Enabled = True
            Else
                ' Determine why the ComboBox is empty: either fully paid, or all needed PNs requested
                If nextDueInstallment = 0 OrElse nextDueInstallment > ExamMap.Keys.Max() Then
                    cmbExamType.Items.Add("All Payments Cleared")
                Else
                    cmbExamType.Items.Add("PN Already Requested for Next Exam")
                End If

                cmbExamType.SelectedIndex = 0
                cmbExamType.Enabled = False
                MessageBox.Show("You cannot request a Promissory Note at this time. Either all necessary PNs have been requested, or all your payments are cleared.", "Request Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error populating exam list: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub btnSubmitRequest_Click(sender As Object, e As EventArgs) Handles btnSubmitRequest.Click
        ' --- 1. Validation ---
        If txtContactNo.Text.Length < 7 Or txtGuardianName.Text = "" Or cmbExamType.SelectedItem Is Nothing Or cmbExamType.SelectedItem.ToString().Contains("Cleared") Or cmbExamType.SelectedItem.ToString().Contains("Already Requested") Then
            MessageBox.Show("Please ensure all fields are filled and a valid Exam Type is selected.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim examType As String = cmbExamType.SelectedItem.ToString()
        Dim studentNo As String = txtStudentNo.Text
        Dim currentStatus As String = ""
        Dim insertedRequestID As Integer = 0

        If Not IsConnOpen() Then Exit Sub

        Try
            ' --- 2. Check for Existing Pending Request (Safeguard) ---
            Dim sqlCheck As String = "SELECT COUNT(*) FROM tblPNRequests WHERE StudentNo = @sno AND Status = 'Pending (Review Required)'"
            Dim cmdCheck As New OleDbCommand(sqlCheck, con)
            cmdCheck.Parameters.AddWithValue("@sno", studentNo)

            If Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0 Then
                MessageBox.Show("You already have a Pending Promissory Note Request. Please wait for approval.", "Request Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' --- 3. Determine Auto-Approval Status (before insertion) ---

            Dim sqlMaxPaid As String = "SELECT MAX(InstallmentNo) FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE"
            Dim cmdMaxPaid As New OleDbCommand(sqlMaxPaid, con)
            cmdMaxPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim maxPaidInstallment As Integer = 0
            Dim paidResult = cmdMaxPaid.ExecuteScalar()

            If paidResult IsNot DBNull.Value AndAlso paidResult IsNot Nothing Then
                maxPaidInstallment = Convert.ToInt32(paidResult)
            End If

            ' Get the installment number corresponding to the requested ExamType
            ' .Key is safe because we filtered the ComboBox based on the ExamMap.
            Dim requestedInstallment As Integer = ExamMap.FirstOrDefault(Function(x) x.Value = examType).Key

            If requestedInstallment = 0 Then
                ' Should not happen with the filtered ExamMap, but handle defensively
                MessageBox.Show("Error: Requested exam type not found in installment map.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' CRITICAL AUTO-APPROVAL RULE:
            If maxPaidInstallment >= (requestedInstallment - 1) Then
                currentStatus = "Approved (Auto)"
            Else
                currentStatus = "Pending (Review Required)"
            End If

            ' --- 4. INSERT REQUEST & RETRIEVE AUTONUMBER (RequestID) ---

            ' Insert without PNNumber first
            Dim sqlInsert As String = "INSERT INTO tblPNRequests (StudentNo, ContactNo, GuardianName, ExamType, RequestDate, Status) " &
                                      "VALUES (@sno, @contact, @guardian, @exam, @date, @status)"

            Dim cmdInsert As New OleDbCommand(sqlInsert, con)
            cmdInsert.Parameters.AddWithValue("@sno", studentNo)
            cmdInsert.Parameters.AddWithValue("@contact", txtContactNo.Text)
            cmdInsert.Parameters.AddWithValue("@guardian", txtGuardianName.Text)
            cmdInsert.Parameters.AddWithValue("@exam", examType)
            cmdInsert.Parameters.AddWithValue("@date", Date.Today.ToShortDateString())
            cmdInsert.Parameters.AddWithValue("@status", currentStatus)

            cmdInsert.ExecuteNonQuery()

            ' Retrieve the last generated AutoNumber (RequestID)
            Dim cmdGetID As New OleDbCommand("SELECT @@IDENTITY", con)
            insertedRequestID = CInt(cmdGetID.ExecuteScalar())

            ' --- 5. GENERATE PN NUMBER using RequestID ---
            Dim yearMonth As String = DateTime.Now.ToString("yyMM")
            Dim newPNNumber As String = "PN-" & yearMonth & "-" & insertedRequestID.ToString("D3")

            ' --- 6. UPDATE RECORD with the new PN Number ---
            Dim sqlUpdatePN As String = "UPDATE tblPNRequests SET PNNumber = @pn WHERE RequestID = @rid"
            Dim cmdUpdatePN As New OleDbCommand(sqlUpdatePN, con)
            cmdUpdatePN.Parameters.AddWithValue("@pn", newPNNumber)
            cmdUpdatePN.Parameters.AddWithValue("@rid", insertedRequestID)

            cmdUpdatePN.ExecuteNonQuery()

            ' --- 7. Final User Feedback ---
            If currentStatus.StartsWith("Approved") Then
                MessageBox.Show("PN Request (" & newPNNumber & ") for " & examType & " is automatically **APPROVED**!", "Request Approved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("PN Request (" & newPNNumber & ") for " & examType & " is set to **PENDING**.", "Request Pending", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error submitting request: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class