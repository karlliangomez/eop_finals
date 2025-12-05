Imports System.Data.OleDb

Public Class frmNewStudent


    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles MyBase.Click

        ' --- 1. Validation ---
        If txtStudentNo.Text = "" Or txtStudentName.Text = "" Or txtCourse.Text = "" Or
           txtYearSection.Text = "" Or txtTotalAssessment.Text = "" Or txtPassword.Text = "" Then
            MessageBox.Show("All fields must be filled out to register a new student.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim studentNo As String = txtStudentNo.Text
        Dim studentName As String = txtStudentName.Text
        Dim totalAssessmentValue As Decimal

        If Not Decimal.TryParse(txtTotalAssessment.Text, totalAssessmentValue) Then
            MessageBox.Show("Total Assessment must be a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' --- 2. Step A & B: Insert Student/User ---
        ' Check connection only once for the first block
        If Not IsConnOpen() Then Exit Sub

        Try

            ' A. Insert into tblUsers (Login Credentials) - NOTE THE BRACKETS for reserved words
            Dim sqlUsers As String = "INSERT INTO tblUsers (Username, [Password], [UserType]) VALUES (@user, @pass, @type)"
            Dim cmdUsers As New OleDbCommand(sqlUsers, con)
            cmdUsers.Parameters.AddWithValue("@user", studentNo)
            cmdUsers.Parameters.AddWithValue("@pass", txtPassword.Text)
            cmdUsers.Parameters.AddWithValue("@type", "Student")
            cmdUsers.ExecuteNonQuery()

            ' B. Insert into tblStudents (Student Details)
            Dim sqlStudents As String = "INSERT INTO tblStudents (StudentNo, StudentName, Course, YearSection, TotalAssessment) " &
                                        "VALUES (@sno, @sname, @course, @ys, @ta)"
            Dim cmdStudents As New OleDbCommand(sqlStudents, con)
            cmdStudents.Parameters.AddWithValue("@sno", studentNo)
            cmdStudents.Parameters.AddWithValue("@sname", studentName)
            cmdStudents.Parameters.AddWithValue("@course", txtCourse.Text)
            cmdStudents.Parameters.AddWithValue("@ys", txtYearSection.Text)
            cmdStudents.Parameters.Add("@ta", OleDbType.Currency).Value = totalAssessmentValue
            cmdStudents.ExecuteNonQuery()

        Catch ex As OleDbException
            If ex.Message.Contains("duplicate") Or ex.Message.Contains("unique") Then
                MessageBox.Show("Error: Student Number already exists in the database.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Database Error during user/student creation: " & ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            GoTo Cleanup

        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred during user/student creation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GoTo Cleanup
        Finally
            ' Close the connection after the first block
            If con.State = ConnectionState.Open Then con.Close()
        End Try


        ' --- 3. Step C: AUTOMATE PAYMENT SCHEDULE GENERATION ---

        ' RE-OPEN CONNECTION FOR SCHEDULE INSERTS
        If Not IsConnOpen() Then GoTo Cleanup

        Try
            Const INITIAL_PAYMENT As Decimal = 2500D
            Const NUM_INSTALLMENTS As Integer = 6

            Dim installmentValue As Decimal = 0D
            Dim amountForInstallments As Decimal = totalAssessmentValue - INITIAL_PAYMENT

            If amountForInstallments > 0 Then
                installmentValue = Math.Round(amountForInstallments / NUM_INSTALLMENTS, 2)
            End If

            Dim scheduleCmd As OleDbCommand

            ' Installment 1 (Downpayment: $2,500)
            Dim sql1 As String = "INSERT INTO tblPaymentSchedule (StudentNo, InstallmentNo, AmountDue, Balance, IsPaid) VALUES (@sno1, 1, @due1, @bal1, @paid1)"
            scheduleCmd = New OleDbCommand(sql1, con)
            scheduleCmd.Parameters.AddWithValue("@sno1", studentNo)
            scheduleCmd.Parameters.Add("@due1", OleDbType.Currency).Value = INITIAL_PAYMENT
            scheduleCmd.Parameters.Add("@bal1", OleDbType.Currency).Value = INITIAL_PAYMENT
            scheduleCmd.Parameters.AddWithValue("@paid1", False)
            scheduleCmd.ExecuteNonQuery()

            ' Installments 2 through 7 (The rest divided by 6)
            Dim sql2 As String = "INSERT INTO tblPaymentSchedule (StudentNo, InstallmentNo, AmountDue, Balance, IsPaid) VALUES (@sno2, @num, @due2, @bal2, @paid2)"

            For i As Integer = 2 To 7
                scheduleCmd = New OleDbCommand(sql2, con)
                scheduleCmd.Parameters.AddWithValue("@sno2", studentNo)
                scheduleCmd.Parameters.AddWithValue("@num", i)
                scheduleCmd.Parameters.Add("@due2", OleDbType.Currency).Value = installmentValue
                scheduleCmd.Parameters.Add("@bal2", OleDbType.Currency).Value = installmentValue
                scheduleCmd.Parameters.AddWithValue("@paid2", False)
                scheduleCmd.ExecuteNonQuery()
            Next

            MessageBox.Show("New Student '" & studentName & "' registered successfully! Payment schedule generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFormControls()

        Catch ex As Exception
            MessageBox.Show("Schedule error: " & ex.Message, "Schedule Generation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Close the connection after schedule generation
            If con.State = ConnectionState.Open Then con.Close()
        End Try

        Exit Sub

Cleanup:
        If con.State = ConnectionState.Open Then con.Close()

    End Sub

    Private Sub ClearFormControls()
        txtStudentNo.Clear()
        txtStudentName.Clear()
        txtCourse.Clear()
        txtYearSection.Clear()
        txtTotalAssessment.Clear()
        txtPassword.Clear()
        txtStudentNo.Focus()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles MyBase.Click
        Me.Close()
    End Sub

End Class