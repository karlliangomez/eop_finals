Public Class frmStudentMainMenu

    Private Sub frmStudentMainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Display the logged-in user's Student ID in the form title or a label
        Me.Text = "Student Main Menu - Welcome, Student No: " & LoggedStudentID
    End Sub

    ' --- 1. View My Payments ---
    Private Sub btnViewPayments_Click(sender As Object, e As EventArgs) Handles btnViewPayments.Click
        ' This form will show the student's payment history and schedule details
        Dim frm As New frmViewPayments()
        frm.Show()
    End Sub

    ' --- 2. View Permits ---
    Private Sub btnViewPermits_Click(sender As Object, e As EventArgs) Handles btnViewPermits.Click
        ' This form will display any academic or financial permits (e.g., examination permit)
        Dim frm As New frmViewPermits()
        frm.Show()
    End Sub

    ' --- 3. Request Promissory Note ---
    Private Sub btnRequestPN_Click(sender As Object, e As EventArgs) Handles btnRequestPN.Click
        ' This form allows the student to submit a new PN request
        Dim frm As New frmRequestPromissoryNote()
        frm.Show()
    End Sub

    ' --- 4. View Promissory Note Status/Content ---
    Private Sub btnViewPN_Click(sender As Object, e As EventArgs) Handles btnViewPN.Click
        ' This form allows the student to check the status or view the final PN document
        Dim frm As New frmViewPromissoryNote()
        frm.Show()
    End Sub

    ' --- 5. Change Password (Existing Form) ---
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        ' We use ShowDialog to ensure the user finishes the process before resuming the menu
        Dim frm As New frmChangePassword()
        frm.ShowDialog()
    End Sub

    ' --- 6. Log Out ---
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            ' Clear the globally stored student ID
            LoggedStudentID = ""

            ' Close the main form
            Me.Close()

            ' Show the Login form again
            Dim frm As New frmLogin()
            frm.Show()
        End If
    End Sub

End Class