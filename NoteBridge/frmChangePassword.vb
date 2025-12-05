Imports System.Data.OleDb

Public Class frmChangePassword

    Private Sub frmChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Change Password"

        ' Ensure the username/student ID is displayed and read-only
        txtUsername.Text = LoggedStudentID
        txtUsername.ReadOnly = True

        ' Mask the password input for security
        txtCurrentPass.PasswordChar = "*"c
        txtNewPass.PasswordChar = "*"c
        txtConfirmPass.PasswordChar = "*"c
    End Sub

    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click

        ' --- 1. Client-Side Validation ---
        If txtCurrentPass.Text = "" Or txtNewPass.Text = "" Or txtConfirmPass.Text = "" Then
            MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If txtNewPass.Text <> txtConfirmPass.Text Then
            MessageBox.Show("New Password and Confirm Password do not match.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtNewPass.Clear()
            txtConfirmPass.Clear()
            txtNewPass.Focus()
            Exit Sub
        End If

        If txtNewPass.Text = txtCurrentPass.Text Then
            MessageBox.Show("New Password must be different from the Current Password.", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtNewPass.Clear()
            txtConfirmPass.Clear()
            txtNewPass.Focus()
            Exit Sub
        End If

        ' --- 2. Database Verification and Update ---
        If Not IsConnOpen() Then Exit Sub

        Try
            Dim username As String = txtUsername.Text
            Dim currentPass As String = txtCurrentPass.Text
            Dim newPass As String = txtNewPass.Text

            ' A. Verify Current Password
            Dim sqlVerify As String = "SELECT Username FROM tblUsers WHERE Username = @user AND [Password] = @currentPass"
            Dim cmdVerify As New OleDbCommand(sqlVerify, con)
            cmdVerify.Parameters.AddWithValue("@user", username)
            cmdVerify.Parameters.AddWithValue("@currentPass", currentPass)

            ' Use ExecuteScalar to quickly check if a record exists
            Dim result = cmdVerify.ExecuteScalar()

            If result Is Nothing Then
                MessageBox.Show("Invalid Current Password.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtCurrentPass.Clear()
                txtCurrentPass.Focus()
                Exit Sub ' Stop execution if current password is wrong
            End If

            ' B. Update the Password (if verification succeeded)
            ' NOTE: Using brackets around [Password] because it is a reserved word in Access SQL
            Dim sqlUpdate As String = "UPDATE tblUsers SET [Password] = @newPass WHERE Username = @user"
            Dim cmdUpdate As New OleDbCommand(sqlUpdate, con)
            cmdUpdate.Parameters.AddWithValue("@newPass", newPass)
            cmdUpdate.Parameters.AddWithValue("@user", username)

            cmdUpdate.ExecuteNonQuery()

            MessageBox.Show("Password successfully changed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message, "Password Change Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class