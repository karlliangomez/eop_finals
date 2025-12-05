Imports System.Data.OleDb

Public Class frmLogin

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectDB()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUsername.Text = "" Or txtPassword.Text = "" Then
            MsgBox("Please enter username and password.", vbExclamation)
            Exit Sub
        End If

        If Not IsConnOpen() Then Exit Sub

        Try
            ' NOTE: Using brackets around [Password] and [UserType] for safety
            cmd = New OleDbCommand("SELECT [UserType], Username FROM tblUsers WHERE Username=@user AND [Password]=@pass", con)
            cmd.Parameters.AddWithValue("@user", txtUsername.Text)
            cmd.Parameters.AddWithValue("@pass", txtPassword.Text)

            dr = cmd.ExecuteReader()

            If dr.Read() Then
                Dim userType As String = dr("UserType").ToString()

                ' --- CRITICAL FIX: Set the global variable here ---
                LoggedStudentID = dr("Username").ToString()

                MsgBox("Login successful!", vbInformation)
                dr.Close()

                If userType = "Admin" Then
                    Dim frm As New frmAdminMainMenu()
                    frm.Show()
                    Me.Hide()
                ElseIf userType = "Student" Then
                    ' LoggedStudentID is already set above.
                    Dim frm As New frmStudentMainMenu()
                    frm.Show()
                    Me.Hide()
                End If
            Else
                MsgBox("Invalid username or password.", vbCritical)
            End If

        Catch ex As Exception
            MsgBox("Error during login: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

End Class