Imports System.Data.OleDb
Imports System.Drawing
Imports System.Linq

Public Class frmStudentPayments

    ' IMPORTANT: You must add the following controls to your form:
    ' 1. Label: lblActiveSemester (for displaying the active semester for payment)
    ' 2. ComboBox: cmbUpdateSemester (for selecting the semester when updating assessment)
    ' 3. Button: btnUpdateAssessment (for triggering the assessment update)

    ' Variable to hold the active semester determined during search
    Private activeSemester As String

    ' Dictionary to map Installment Number to Exam Type (Finals Excluded)
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"}
    }

    Private Sub frmStudentPayments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Student Payments Processing"

        ' Set Read-Only Fields
        txtStudentNo.ReadOnly = True
        txtStudentName.ReadOnly = True
        txtCourse.ReadOnly = True
        txtYearSection.ReadOnly = True

        ' txtTotalAssessment is editable

        txtPendingBalance.ReadOnly = True

        ' Set Automated Date
        txtDate.Text = DateTime.Now.ToShortDateString()
        txtDate.ReadOnly = True

        PopulateUpdateSemesterDropdown() ' Load semesters for the update function

        ClearAllFields()
        txtSearch.Enabled = True
        btnSearch.Enabled = True
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Semester Dropdown for Assessment Update
    ' --------------------------------------------------------------------------------

    Private Sub PopulateUpdateSemesterDropdown()
        cmbUpdateSemester.Items.Clear()

        ' Dynamically generate your current/next academic semesters here
        Dim currentYear As Integer = Date.Now.Year
        Dim nextYear As Integer = currentYear + 1

        cmbUpdateSemester.Items.Add("1st Semester S.Y. " & currentYear & "-" & nextYear)
        cmbUpdateSemester.Items.Add("2nd Semester S.Y. " & currentYear & "-" & nextYear)
        cmbUpdateSemester.Items.Add("Summer " & nextYear)

        If cmbUpdateSemester.Items.Count > 0 Then
            cmbUpdateSemester.SelectedIndex = 0
        End If
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Search and Financial Loading (Semester Filtered)
    ' --------------------------------------------------------------------------------

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim searchStudentNo As String = txtSearch.Text.Trim()

        If String.IsNullOrEmpty(searchStudentNo) Then
            MessageBox.Show("Please enter a Student Number to search.", "Search Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ClearStudentInfoFields()

        If Not IsConnOpen() Then Exit Sub

        Dim studentFound As Boolean = False

        Try
            ' 1. Fetch Student Details & Total Assessment
            Dim sql As String = "SELECT StudentNo, StudentName, Course, YearSection, TotalAssessment FROM tblStudents WHERE StudentNo = @sno"
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@sno", searchStudentNo)

            dr = cmd.ExecuteReader()

            If dr.Read() Then
                studentFound = True

                ' Fill basic student information fields
                txtStudentNo.Text = dr("StudentNo").ToString()
                txtStudentName.Text = dr("StudentName").ToString()
                txtCourse.Text = dr("Course").ToString()
                txtYearSection.Text = dr("YearSection").ToString()

                ' Get Total Assessment (Note: This is the old/initial value from tblStudents)
                Dim totalAssessment As Decimal = 0
                If dr("TotalAssessment") IsNot DBNull.Value Then
                    totalAssessment = Convert.ToDecimal(dr("TotalAssessment"))
                End If
                txtTotalAssessment.Text = totalAssessment.ToString("N2")

                dr.Close()

                ' 2. Calculate balance, which will now determine the active semester
                LoadStudentFinancials(searchStudentNo, totalAssessment)
            Else
                MessageBox.Show("Student Number '" & searchStudentNo & "' not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error fetching student details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If dr IsNot Nothing AndAlso Not dr.IsClosed Then dr.Close()
            If con.State = ConnectionState.Open Then con.Close()
        End Try

        If studentFound Then txtSearch.Clear()
    End Sub

    Private Sub LoadStudentFinancials(studentNo As String, totalAssessment As Decimal)

        If Not IsConnOpen() Then Exit Sub
        activeSemester = "" ' Reset active semester
        lblActiveSemester.Text = "N/A" ' Reset visual display

        Try
            ' A. FIRST, determine the active semester (the one with the NEXT unpaid installment)
            Dim sqlActiveSem As String = "SELECT TOP 1 Semester FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE ORDER BY Semester ASC, InstallmentNo ASC"
            Dim cmdActiveSem As New OleDbCommand(sqlActiveSem, con)
            cmdActiveSem.Parameters.AddWithValue("@sno", studentNo)

            Dim semResult = cmdActiveSem.ExecuteScalar()

            If semResult IsNot DBNull.Value AndAlso semResult IsNot Nothing Then
                activeSemester = semResult.ToString()
            Else
                ' If all installments are paid, find the last recorded semester for display/update purposes.
                sqlActiveSem = "SELECT TOP 1 Semester FROM tblPaymentSchedule WHERE StudentNo = @sno ORDER BY Semester DESC, InstallmentNo DESC"
                cmdActiveSem = New OleDbCommand(sqlActiveSem, con)
                cmdActiveSem.Parameters.AddWithValue("@sno", studentNo)

                semResult = cmdActiveSem.ExecuteScalar()
                If semResult IsNot DBNull.Value AndAlso semResult IsNot Nothing Then
                    activeSemester = semResult.ToString()
                    MessageBox.Show("All payments for the current schedule (" & activeSemester & ") are clear.", "Payment Clear", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    txtPendingBalance.Text = 0.ToString("N2")
                    activeSemester = ""
                    MessageBox.Show("Student has no payment schedule records.", "Schedule Missing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            ' Display the Active Semester
            lblActiveSemester.Text = activeSemester

            ' 2. Determine the NEXT INSTALLMENT DUE AMOUNT (Filtered by Active Semester)
            Dim nextInstallmentDue As Decimal = 0D

            ' Query for the Balance of the first installment that is NOT paid in the Active Semester
            Dim sqlNextDue As String = "SELECT TOP 1 Balance FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE AND Semester = @sem ORDER BY InstallmentNo ASC"
            Dim cmdNextDue As New OleDbCommand(sqlNextDue, con)
            cmdNextDue.Parameters.AddWithValue("@sno", studentNo)
            cmdNextDue.Parameters.AddWithValue("@sem", activeSemester)

            Dim nextDueResult = cmdNextDue.ExecuteScalar()
            If nextDueResult IsNot DBNull.Value AndAlso nextDueResult IsNot Nothing Then
                nextInstallmentDue = Convert.ToDecimal(nextDueResult)
            End If

            ' 3. Display the Next Installment Due
            txtPendingBalance.Text = nextInstallmentDue.ToString("N2")

            txtAmountPaid.Clear()
            txtReceiptNo.Clear()
            txtAmountPaid.Focus()

        Catch ex As Exception
            MessageBox.Show("Error calculating financial data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Total Assessment Update Logic (Rewritten for Semester)
    ' --------------------------------------------------------------------------------

    Private Sub btnUpdateAssessment_Click(sender As Object, e As EventArgs) Handles btnUpdateAssessment.Click

        If String.IsNullOrEmpty(txtStudentNo.Text) Then
            MessageBox.Show("Please search and select a student first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If cmbUpdateSemester.SelectedIndex = -1 Then
            MessageBox.Show("Please select a Semester for the updated assessment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim newAssessment As Decimal
        If Not Decimal.TryParse(txtTotalAssessment.Text, newAssessment) OrElse newAssessment <= 0 Then
            MessageBox.Show("Please enter a valid amount for Total Assessment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim selectedSemester As String = cmbUpdateSemester.SelectedItem.ToString()
        Const FixedDownpayment As Decimal = 2500D

        If newAssessment < FixedDownpayment Then
            MessageBox.Show("The new Total Assessment (" & newAssessment.ToString("N2") & ") must be greater than or equal to the minimum downpayment of " & FixedDownpayment.ToString("N2") & ".", "Financial Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        If Not IsConnOpen() Then Exit Sub

        Try
            ' 1. CHECK FOR DUPLICATE: Ensure no schedule exists for this student and this semester
            Dim sqlCheck As String = "SELECT COUNT(*) FROM tblPaymentSchedule WHERE StudentNo = @sno AND Semester = @sem"
            Dim cmdCheck As New OleDbCommand(sqlCheck, con)
            cmdCheck.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdCheck.Parameters.AddWithValue("@sem", selectedSemester)

            If CInt(cmdCheck.ExecuteScalar()) > 0 Then
                MessageBox.Show("A payment schedule already exists for " & selectedSemester & ". Please choose a new semester or manually adjust the existing schedule.", "Duplicate Schedule", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' Note: We ignore the tblStudents.TotalAssessment field during this update.

            ' 2. CALCULATE AND INSERT NEW PAYMENT SCHEDULE (Fixed Downpayment Logic)

            Dim remainingBalanceToSplit As Decimal = newAssessment - FixedDownpayment
            Const RemainingInstallments As Integer = 6

            Dim equalInstallmentAmount As Decimal = 0D
            If RemainingInstallments > 0 Then
                equalInstallmentAmount = Math.Round(remainingBalanceToSplit / RemainingInstallments, 2)
            End If

            Dim installmentAmount As Decimal
            Dim balance As Decimal
            Dim isPaid As Boolean = False

            For installmentNo As Integer = 1 To 7

                If installmentNo = 1 Then
                    installmentAmount = FixedDownpayment
                Else
                    installmentAmount = equalInstallmentAmount
                End If

                balance = installmentAmount
                isPaid = (newAssessment = 0) ' Mark as paid if total assessment is zero

                ' INSERT into the schedule table
                Dim sqlSchedule As String = "INSERT INTO tblPaymentSchedule (StudentNo, InstallmentNo, AmountDue, Balance, IsPaid, Semester) " &
                                            "VALUES (@sno, @installno, @amt, @bal, @paid, @sem)"

                Dim cmdSchedule As New OleDbCommand(sqlSchedule, con)
                cmdSchedule.Parameters.AddWithValue("@sno", txtStudentNo.Text)
                cmdSchedule.Parameters.AddWithValue("@installno", installmentNo)
                cmdSchedule.Parameters.Add("@amt", OleDbType.Currency).Value = installmentAmount
                cmdSchedule.Parameters.Add("@bal", OleDbType.Currency).Value = balance
                cmdSchedule.Parameters.AddWithValue("@paid", isPaid)
                cmdSchedule.Parameters.AddWithValue("@sem", selectedSemester)

                cmdSchedule.ExecuteNonQuery()
            Next

            MessageBox.Show("New payment schedule created for the " & selectedSemester & " with a Total Assessment of " & newAssessment.ToString("N2") & ".", "Schedule Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh the financial data to reflect the *new* active semester if it's the next due one
            If con.State = ConnectionState.Open Then con.Close()
            LoadStudentFinancials(txtStudentNo.Text, newAssessment)

        Catch ex As Exception
            MessageBox.Show("Error updating assessment: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Payment Saving Logic (Semester Filtered)
    ' --------------------------------------------------------------------------------

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If String.IsNullOrEmpty(txtStudentNo.Text) Then
            MessageBox.Show("Please search and select a student first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtAmountPaid.Text) Or String.IsNullOrEmpty(txtReceiptNo.Text) Then
            MessageBox.Show("Please enter Amount Paid and Receipt Number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim amountPaid As Decimal
        If Not Decimal.TryParse(txtAmountPaid.Text, amountPaid) OrElse amountPaid <= 0 Then
            MessageBox.Show("Please enter a valid amount paid.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If String.IsNullOrEmpty(activeSemester) Then
            MessageBox.Show("No active semester payment schedule found for this student. Search again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not IsConnOpen() Then Exit Sub

        Try
            ' --- A. Find the InstallmentNo of the next due installment (FILTERED) ---
            Dim nextInstallmentNo As Integer = 0
            Dim sqlGetNextInstallment As String = "SELECT TOP 1 InstallmentNo FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE AND Semester = @sem ORDER BY InstallmentNo ASC"
            Dim cmdGetNextInstallment As New OleDbCommand(sqlGetNextInstallment, con)
            cmdGetNextInstallment.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdGetNextInstallment.Parameters.AddWithValue("@sem", activeSemester)

            Dim nextInstallmentResult = cmdGetNextInstallment.ExecuteScalar()

            If nextInstallmentResult IsNot DBNull.Value AndAlso nextInstallmentResult IsNot Nothing Then
                nextInstallmentNo = Convert.ToInt32(nextInstallmentResult)
            Else
                MessageBox.Show("All installments for the current semester are paid. No payment necessary.", "No Balance Due", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If


            ' --- 1. Insert into tblPayments (Transaction Log) ---
            Dim sqlInsert As String = "INSERT INTO tblPayments (StudentNo, AmountPaid, ReceiptNo, PaymentDate, Semester) " &
                                     "VALUES (@sno, @paid, @receipt, @pdate, @sem)"
            Dim cmdInsert As New OleDbCommand(sqlInsert, con)

            cmdInsert.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdInsert.Parameters.Add("@paid", OleDbType.Currency).Value = amountPaid
            cmdInsert.Parameters.AddWithValue("@receipt", txtReceiptNo.Text)
            cmdInsert.Parameters.AddWithValue("@pdate", DateTime.Now.ToShortDateString())
            cmdInsert.Parameters.AddWithValue("@sem", activeSemester)

            cmdInsert.ExecuteNonQuery()

            ' --- 2. UPDATE THE SCHEDULE RECORD (Partial Payment Deduction) ---

            ' Updates Balance and sets IsPaid to TRUE if the Balance falls to 0 or below
            Dim sqlUpdateSchedule As String = "UPDATE tblPaymentSchedule SET " &
                                            "Balance = Balance - @paid, " &
                                            "IsPaid = IIF(Balance - @paid <= 0, TRUE, FALSE) " &
                                            "WHERE StudentNo = @sno AND InstallmentNo = @installno AND Semester = @sem"

            Dim cmdUpdateSchedule As New OleDbCommand(sqlUpdateSchedule, con)
            cmdUpdateSchedule.Parameters.Add("@paid", OleDbType.Currency).Value = amountPaid
            cmdUpdateSchedule.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdUpdateSchedule.Parameters.AddWithValue("@installno", nextInstallmentNo)
            cmdUpdateSchedule.Parameters.AddWithValue("@sem", activeSemester)

            cmdUpdateSchedule.ExecuteNonQuery()

            ' --- 3. CHECK AND AUTO-APPROVE PENDING PNs ---
            If con.State = ConnectionState.Open Then con.Close() ' Close to allow new data read
            CheckAndAutoApprovePNs(txtStudentNo.Text, activeSemester)


            MessageBox.Show("Payment saved successfully, and installment balance updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh financials after saving
            Dim currentTotalAssessment As Decimal = 0
            If Decimal.TryParse(txtTotalAssessment.Text, currentTotalAssessment) Then
                LoadStudentFinancials(txtStudentNo.Text, currentTotalAssessment)
            Else
                LoadStudentFinancials(txtStudentNo.Text, 0)
            End If


        Catch ex As Exception
            MessageBox.Show("Error saving payment: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## PN Auto-Approval Subroutine (Semester Filtered)
    ' --------------------------------------------------------------------------------

    Private Sub CheckAndAutoApprovePNs(studentNo As String, semester As String)

        If Not IsConnOpen() Then Exit Sub

        Try
            ' --- A. Find the highest PAID installment number (FILTERED) ---
            Dim sqlMaxPaid As String = "SELECT MAX(InstallmentNo) FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE AND Semester = @sem"
            Dim cmdMaxPaid As New OleDbCommand(sqlMaxPaid, con)
            cmdMaxPaid.Parameters.AddWithValue("@sno", studentNo)
            cmdMaxPaid.Parameters.AddWithValue("@sem", semester)

            Dim maxPaidInstallment As Integer = 0
            Dim paidResult = cmdMaxPaid.ExecuteScalar()

            If paidResult IsNot DBNull.Value AndAlso paidResult IsNot Nothing Then
                maxPaidInstallment = Convert.ToInt32(paidResult)
            End If

            If maxPaidInstallment = 0 Then Exit Sub

            ' --- B. Find all PENDING PN requests for this student (FILTERED) ---
            Dim sqlPending As String = "SELECT RequestID, ExamType FROM tblPNRequests WHERE StudentNo = @sno AND Status = 'Pending (Review Required)' AND Semester = @sem"
            Dim cmdPending As New OleDbCommand(sqlPending, con)
            cmdPending.Parameters.AddWithValue("@sno", studentNo)
            cmdPending.Parameters.AddWithValue("@sem", semester)

            Dim dtPending As New DataTable()
            Dim daPending As New OleDbDataAdapter(cmdPending)
            daPending.Fill(dtPending)

            If dtPending.Rows.Count = 0 Then Exit Sub

            ' --- C. Loop through pending PNs and check if the new payment clears them ---
            Dim approvedCount As Integer = 0

            For Each row As DataRow In dtPending.Rows
                Dim requestID As Integer = CInt(row("RequestID"))
                Dim examType As String = row("ExamType").ToString()

                ' Find the required installment number for this ExamType
                Dim requestedInstallment As Integer = ExamMap.FirstOrDefault(Function(x) x.Value = examType).Key

                ' Auto-Approval Rule: Approved if the highest paid installment covers the preceding installment.
                ' (e.g., Prelim Exam (Installment 3) requires Installment 2 to be fully paid.)
                If maxPaidInstallment >= (requestedInstallment - 1) Then

                    ' Update the PN Status to Approved
                    Dim sqlUpdate As String = "UPDATE tblPNRequests SET Status = 'Approved (Auto)' WHERE RequestID = @rid"
                    Dim cmdUpdate As New OleDbCommand(sqlUpdate, con)
                    cmdUpdate.Parameters.AddWithValue("@rid", requestID)

                    cmdUpdate.ExecuteNonQuery()
                    approvedCount += 1
                End If
            Next

            If approvedCount > 0 Then
                MessageBox.Show("SUCCESS! " & approvedCount.ToString() & " Promissory Note request(s) have been automatically **APPROVED** due to the recent payment.", "PN Status Update", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error during PN auto-approval check: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --------------------------------------------------------------------------------
    ' ## Utility Subroutines
    ' --------------------------------------------------------------------------------

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearAllFields()
    End Sub


    Private Sub ClearAllFields()
        activeSemester = ""
        txtSearch.Clear()
        ClearStudentInfoFields()
        If Not IsNothing(lblActiveSemester) Then lblActiveSemester.Text = "N/A"
        txtAmountPaid.Clear()
        txtReceiptNo.Clear()
        txtSearch.Focus()
    End Sub

    Private Sub ClearStudentInfoFields()
        txtStudentNo.Clear()
        txtStudentName.Clear()
        txtCourse.Clear()
        txtYearSection.Clear()
        txtTotalAssessment.Clear()
        txtPendingBalance.Clear()
    End Sub

End Class