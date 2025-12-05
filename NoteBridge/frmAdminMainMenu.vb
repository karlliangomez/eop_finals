Public Class frmAdminMainMenu

    ' --- Navigation Button Click Events ---

    Private Sub btnStudentPayments_Click(sender As Object, e As EventArgs) Handles btnStudentPayments.Click
        ' Open the Student Payments form
        Dim frm As New frmStudentPayments()
        frm.Show()

    End Sub

    Private Sub btnPaymentReports_Click(sender As Object, e As EventArgs) Handles btnPaymentReports.Click
        ' Open the Payment Reports form
        Dim frm As New frmPaymentReports()
        frm.Show()

    End Sub

    Private Sub btnPromissoryNoteSummary_Click(sender As Object, e As EventArgs) Handles btnPromissoryNoteSummary.Click
        ' Open the Promissory Note Summary form
        Dim frm As New frmPromissoryNoteSummary
        frm.Show()
    End Sub

    Private Sub btnAddNewStudent_Click(sender As Object, e As EventArgs) Handles btnAddNewStudent.Click
        ' Open the Add New Student form
        Dim frm As New frmNewStudent()
        frm.Show()
    End Sub

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        ' Open the Change Password form
        If String.IsNullOrEmpty(LoggedStudentID) Then
            MessageBox.Show("You must be logged in to change your password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim frm As New frmChangePassword()
        frm.ShowDialog()
    End Sub

    ' --- Log Out Functionality ---

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        ' 1. Confirm with the user
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' 2. Hide the current Admin Menu
            Me.Hide()

            ' 3. Show the Login form
            Dim frm As New frmLogin()
            frm.Show()

            ' Optional: Dispose of the current form if you want to completely destroy it.
            ' Me.Dispose()
        End If
    End Sub






End Class