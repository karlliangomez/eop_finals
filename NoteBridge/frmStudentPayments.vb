Imports System.Data.OleDb
Imports System.Drawing
Imports System.Linq

Public Class frmStudentPayments

    ' Dictionary to map Installment Number to Exam Type 
    ' Finals Exam (Installment 7) is EXCLUDED.
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"}
    }

    Private Sub frmStudentPayments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Read-Only Fields
        txtStudentNo.ReadOnly = True
        txtStudentName.ReadOnly = True
        txtCourse.ReadOnly = True
        txtYearSection.ReadOnly = True
        txtTotalAssessment.ReadOnly = True
        txtPendingBalance.ReadOnly = True

        ' Set Automated Date
        txtDate.Text = DateTime.Now.ToShortDateString()
        txtDate.ReadOnly = True

        ClearAllFields()
        txtSearch.Enabled = True
        btnSearch.Enabled = True
    End Sub

    ' --- Search Button Click (Direct Lookup) ---
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

                ' Get Total Assessment
                Dim totalAssessment As Decimal = 0
                If dr("TotalAssessment") IsNot DBNull.Value Then
                    totalAssessment = Convert.ToDecimal(dr("TotalAssessment"))
                End If
                txtTotalAssessment.Text = totalAssessment.ToString("N2")

                dr.Close()

                ' 2. Calculate balance
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

    ' --- Load Student Financials ---
    Private Sub LoadStudentFinancials(studentNo As String, totalAssessment As Decimal)

        If Not IsConnOpen() Then Exit Sub

        Try
            ' 1. Calculate Total Amount Paid (from all transactions in tblPayments)
            Dim sqlPaid As String = "SELECT SUM(AmountPaid) FROM tblPayments WHERE StudentNo = @sno"
            Dim cmdPaid As New OleDbCommand(sqlPaid, con)
            cmdPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim totalPaid As Decimal = 0
            Dim paidResult = cmdPaid.ExecuteScalar()
            If paidResult IsNot DBNull.Value AndAlso paidResult IsNot Nothing Then
                totalPaid = Convert.ToDecimal(paidResult)
            End If

            ' 2. Determine the NEXT INSTALLMENT DUE AMOUNT (from tblPaymentSchedule)
            Dim nextInstallmentDue As Decimal = 0D

            ' Query for the Balance of the first installment that is NOT paid
            Dim sqlNextDue As String = "SELECT TOP 1 Balance FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE ORDER BY InstallmentNo ASC"
            Dim cmdNextDue As New OleDbCommand(sqlNextDue, con)
            cmdNextDue.Parameters.AddWithValue("@sno", studentNo)

            Dim nextDueResult = cmdNextDue.ExecuteScalar()
            If nextDueResult IsNot DBNull.Value AndAlso nextDueResult IsNot Nothing Then
                nextInstallmentDue = Convert.ToDecimal(nextDueResult)
            End If

            ' 3. Display the Next Installment Due as the Pending Balance
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

    ' --- Payment Saving Logic ---
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

        ' 🛑 REMOVE strict validation, as partial payments are now allowed. 🛑

        If Not IsConnOpen() Then Exit Sub

        Try
            ' --- A. Find the InstallmentNo of the next due installment ---
            Dim nextInstallmentNo As Integer = 0
            Dim sqlGetNextInstallment As String = "SELECT TOP 1 InstallmentNo FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = FALSE ORDER BY InstallmentNo ASC"
            Dim cmdGetNextInstallment As New OleDbCommand(sqlGetNextInstallment, con)
            cmdGetNextInstallment.Parameters.AddWithValue("@sno", txtStudentNo.Text)

            Dim nextInstallmentResult = cmdGetNextInstallment.ExecuteScalar()

            If nextInstallmentResult IsNot DBNull.Value AndAlso nextInstallmentResult IsNot Nothing Then
                nextInstallmentNo = Convert.ToInt32(nextInstallmentResult)
            Else
                MessageBox.Show("All installments for this student appear to be paid.", "No Balance Due", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If


            ' --- 1. Insert into tblPayments (Transaction Log) ---
            Dim sqlInsert As String = "INSERT INTO tblPayments (StudentNo, AmountPaid, ReceiptNo, PaymentDate, InstallmentNo) " &
                                     "VALUES (@sno, @paid, @receipt, @pdate, @installno)"
            Dim cmdInsert As New OleDbCommand(sqlInsert, con)

            cmdInsert.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdInsert.Parameters.Add("@paid", OleDbType.Currency).Value = amountPaid
            cmdInsert.Parameters.AddWithValue("@receipt", txtReceiptNo.Text)
            cmdInsert.Parameters.AddWithValue("@pdate", DateTime.Now.ToShortDateString())
            cmdInsert.Parameters.AddWithValue("@installno", nextInstallmentNo) ' Link payment to the installment

            cmdInsert.ExecuteNonQuery()

            ' --- 2. UPDATE THE SCHEDULE RECORD (Implement partial payment deduction) ---

            ' New Balance = Current Balance - AmountPaid. 
            ' IsPaid is set to TRUE only if the new Balance is <= 0.
            Dim sqlUpdateSchedule As String = "UPDATE tblPaymentSchedule SET " &
                                            "Balance = Balance - @paid, " &
                                            "IsPaid = IIF(Balance - @paid <= 0, TRUE, FALSE) " &
                                            "WHERE StudentNo = @sno AND InstallmentNo = @installno"

            Dim cmdUpdateSchedule As New OleDbCommand(sqlUpdateSchedule, con)
            cmdUpdateSchedule.Parameters.Add("@paid", OleDbType.Currency).Value = amountPaid
            cmdUpdateSchedule.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdUpdateSchedule.Parameters.AddWithValue("@installno", nextInstallmentNo)

            cmdUpdateSchedule.ExecuteNonQuery()

            ' --- 3. CHECK AND AUTO-APPROVE PENDING PNs ---
            CheckAndAutoApprovePNs(txtStudentNo.Text)

            MessageBox.Show("Payment saved successfully, and balance updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh financials after saving
            Dim currentTotalAssessment As Decimal = Decimal.Parse(txtTotalAssessment.Text)
            LoadStudentFinancials(txtStudentNo.Text, currentTotalAssessment)

        Catch ex As Exception
            MessageBox.Show("Error saving payment: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' 🛑 PN AUTO-APPROVAL SUBROUTINE 🛑
    Private Sub CheckAndAutoApprovePNs(studentNo As String)
        If Not IsConnOpen() Then Exit Sub ' Ensure connection is open

        Try
            ' --- A. Find the highest PAID installment number ---
            Dim sqlMaxPaid As String = "SELECT MAX(InstallmentNo) FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE"
            Dim cmdMaxPaid As New OleDbCommand(sqlMaxPaid, con)
            cmdMaxPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim maxPaidInstallment As Integer = 0
            Dim paidResult = cmdMaxPaid.ExecuteScalar()

            If paidResult IsNot DBNull.Value AndAlso paidResult IsNot Nothing Then
                maxPaidInstallment = Convert.ToInt32(paidResult)
            End If

            If maxPaidInstallment = 0 Then Exit Sub ' Nothing paid yet, exit.

            ' --- B. Find all PENDING PN requests for this student ---
            Dim sqlPending As String = "SELECT RequestID, ExamType FROM tblPNRequests WHERE StudentNo = @sno AND Status = 'Pending (Review Required)'"
            Dim cmdPending As New OleDbCommand(sqlPending, con)
            cmdPending.Parameters.AddWithValue("@sno", studentNo)

            Dim dtPending As New DataTable()
            Dim daPending As New OleDbDataAdapter(cmdPending)
            daPending.Fill(dtPending)

            If dtPending.Rows.Count = 0 Then Exit Sub ' No pending PNs to check

            ' --- C. Loop through pending PNs and check if the new payment clears them ---
            Dim approvedCount As Integer = 0

            For Each row As DataRow In dtPending.Rows
                Dim requestID As Integer = CInt(row("RequestID"))
                Dim examType As String = row("ExamType").ToString()

                ' Find the required installment number for this ExamType
                Dim requestedInstallment As Integer = ExamMap.FirstOrDefault(Function(x) x.Value = examType).Key

                ' Auto-Approval Rule: Approved if the highest paid installment covers the preceding installment.
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

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearAllFields()
    End Sub


    Private Sub ClearAllFields()
        txtSearch.Clear()
        ClearStudentInfoFields()
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