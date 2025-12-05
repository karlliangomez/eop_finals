Imports System.Data.OleDb
Imports System.Linq

Public Class frmNewStudent

    ' Global connection, command, and data reader definitions (assumed to be defined in your module)
    ' Public con As New OleDbConnection(...)
    ' Public dr As OleDbDataReader

    Private Sub frmNewStudent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set default values
        txtUsername.Text = "Student_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second

        PopulateSemesterDropdown()

        ClearFields()
        txtStudentName.Focus()
    End Sub

    Private Sub PopulateSemesterDropdown()
        ' Assumed ComboBox Name: cmbSemester
        cmbSemester.Items.Clear()

        ' Dynamically generate or set your current academic semesters here
        Dim currentYear As Integer = Date.Now.Year
        Dim nextYear As Integer = currentYear + 1

        ' Add common semester formats
        cmbSemester.Items.Add("1st Semester S.Y. " & currentYear & "-" & nextYear)
        cmbSemester.Items.Add("2nd Semester S.Y. " & currentYear & "-" & nextYear)
        cmbSemester.Items.Add("Summer " & nextYear)

        ' Set a default value
        If cmbSemester.Items.Count > 0 Then
            cmbSemester.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click

        ' -------------------------
        ' ## 1. Validation and Setup
        ' -------------------------

        If String.IsNullOrEmpty(txtStudentNo.Text) OrElse String.IsNullOrEmpty(txtStudentName.Text) OrElse
           String.IsNullOrEmpty(txtCourse.Text) OrElse String.IsNullOrEmpty(txtYearSection.Text) OrElse
           String.IsNullOrEmpty(txtTotalAssessment.Text) OrElse String.IsNullOrEmpty(cmbSemester.Text) Then

            MessageBox.Show("Please fill out all student details and select the Semester.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim totalAssessment As Decimal
        If Not Decimal.TryParse(txtTotalAssessment.Text, totalAssessment) OrElse totalAssessment <= 0 Then
            MessageBox.Show("Please enter a valid amount for Total Assessment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' CRITICAL VALIDATION: Total Assessment must be greater than the fixed downpayment
        Const FixedDownpayment As Decimal = 2500D
        If totalAssessment < FixedDownpayment Then
            MessageBox.Show("Total Assessment (" & totalAssessment.ToString("N2") & ") must be greater than the minimum downpayment of " & FixedDownpayment.ToString("N2") & ".", "Financial Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Dim selectedSemester As String = cmbSemester.SelectedItem.ToString()

        If Not IsConnOpen() Then Exit Sub

        Try
            ' ----------------------------------------------------
            ' ## 2. Insert into tblStudents 
            ' ----------------------------------------------------

            Dim sqlStudent As String = "INSERT INTO tblStudents (StudentNo, StudentName, Course, YearSection, TotalAssessment) " &
                                       "VALUES (@sno, @sname, @course, @ys, @assess)"
            Dim cmdStudent As New OleDbCommand(sqlStudent, con)

            cmdStudent.Parameters.AddWithValue("@sno", txtStudentNo.Text)
            cmdStudent.Parameters.AddWithValue("@sname", txtStudentName.Text)
            cmdStudent.Parameters.AddWithValue("@course", txtCourse.Text)
            cmdStudent.Parameters.AddWithValue("@ys", txtYearSection.Text)
            cmdStudent.Parameters.Add("@assess", OleDbType.Currency).Value = totalAssessment

            cmdStudent.ExecuteNonQuery()

            ' ----------------------------------------------------
            ' ## 3. Insert into tblUsers (Login Credentials)
            ' ----------------------------------------------------

            Dim sqlLogin As String = "INSERT INTO tblUsers (Username, [Password], UserType, StudentNo) " &
                                     "VALUES (@user, @pass, @type, @sno)"
            Dim cmdLogin As New OleDbCommand(sqlLogin, con)

            cmdLogin.Parameters.AddWithValue("@user", txtUsername.Text)
            cmdLogin.Parameters.AddWithValue("@pass", txtPassword.Text)
            cmdLogin.Parameters.AddWithValue("@type", "Student") ' Default user type
            cmdLogin.Parameters.AddWithValue("@sno", txtStudentNo.Text)

            cmdLogin.ExecuteNonQuery()

            ' ----------------------------------------------------------------------
            ' ## 4. Insert into tblPaymentSchedule (Fixed Downpayment Logic)
            ' ----------------------------------------------------------------------

            Dim remainingBalanceToSplit As Decimal = totalAssessment - FixedDownpayment
            Const RemainingInstallments As Integer = 6

            ' Calculate the equal amount for installments 2 through 7
            Dim equalInstallmentAmount As Decimal = 0D
            If RemainingInstallments > 0 Then
                ' Round the installment amount for even distribution
                equalInstallmentAmount = Math.Round(remainingBalanceToSplit / RemainingInstallments, 2)
            End If

            Dim installmentAmount As Decimal
            Dim balance As Decimal
            Dim isPaid As Boolean = False

            ' Loop through all 7 installments
            For installmentNo As Integer = 1 To 7

                If installmentNo = 1 Then
                    ' Installment 1: Fixed Downpayment
                    installmentAmount = FixedDownpayment
                Else
                    ' Installments 2-7: Equal split of the remaining amount
                    installmentAmount = equalInstallmentAmount
                End If

                balance = installmentAmount
                isPaid = False

                ' If the total assessment is 0 (e.g., fully subsidized scholarship), treat as paid
                If totalAssessment = 0 Then
                    balance = 0
                    isPaid = True
                End If

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

            MessageBox.Show("New student record created successfully, and the payment schedule for the " & selectedSemester & " has been generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ClearFields()

        Catch ex As Exception
            MessageBox.Show("Error saving student record: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --- Utility Functions ---

    Private Sub ClearFields()
        txtStudentNo.Clear()
        txtStudentName.Clear()
        txtCourse.Clear()
        txtYearSection.Clear()
        txtTotalAssessment.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        If cmbSemester.Items.Count > 0 Then cmbSemester.SelectedIndex = 0
        txtStudentName.Focus()
    End Sub

End Class