Imports System.Data.OleDb
Imports System.Drawing

Public Class frmStudentMainMenu

    ' NOTE: LoggedStudentID must be defined as Public in a Global Module 
    ' (e.g., Module GlobalVars) OR must be defined as a Public Property/Field
    ' on this form that is set by the login form.
    ' For this fix, we assume 'LoggedStudentID' is a globally accessible variable 
    ' or a Public Property of this main menu form.

    ' Assuming LoggedStudentID is a Public variable defined in a module or static class
    ' Example: Public LoggedStudentID As String = "" 

    Private Sub frmStudentMainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Display the logged-in user's Student ID in the form title or a label
        Me.Text = "Student Main Menu - Welcome, Student No: " & LoggedStudentID
    End Sub

    ' --- 1. View My Payments (FIXED) ---
    Private Sub btnViewPayments_Click(sender As Object, e As EventArgs) Handles btnViewPayments.Click
        Dim frm As New frmViewPayments()
        ' 🛑 FIX APPLIED HERE 🛑 Pass the Student ID to the form
        frm.LoggedStudentID = LoggedStudentID
        frm.Show()
    End Sub

    ' --- 2. View Permits (FIXED) ---
    Private Sub btnViewPermits_Click(sender As Object, e As EventArgs) Handles btnViewPermits.Click
        Dim frm As New frmViewPermits()
        ' 🛑 FIX APPLIED HERE 🛑 Pass the Student ID to the form
        frm.LoggedStudentID = LoggedStudentID
        frm.Show()
    End Sub

    ' --- 3. Request Promissory Note (Already Correct) ---
    Private Sub btnRequestPN_Click(sender As Object, e As EventArgs) Handles btnRequestPN.Click
        Dim frm As New frmRequestPromissoryNote()
        ' This form uses StudentNo, not LoggedStudentID, but the logic is correct
        frm.StudentNo = LoggedStudentID
        frm.Show()
    End Sub

    ' --- 4. View Promissory Note Status/Content (FIXED) ---
    Private Sub btnViewPN_Click(sender As Object, e As EventArgs) Handles btnViewPN.Click
        Dim frm As New frmViewPromissoryNote()
        ' 🛑 FIX APPLIED HERE 🛑 Pass the Student ID to the form
        frm.LoggedStudentID = LoggedStudentID
        frm.Show()
    End Sub

    ' --- 5. Change Password (Existing Form - No change needed) ---
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        Dim frm As New frmChangePassword()

        ' NOTE: If frmChangePassword requires the student ID for security/lookup,
        ' you would add frm.LoggedStudentID = LoggedStudentID here as well.
        ' Assuming it handles the current session user automatically.
        frm.ShowDialog()
    End Sub

    ' --- 6. Log Out ---
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            ' Clear the globally stored student ID (assuming LoggedStudentID is global)
            ' If LoggedStudentID is a property of this form, clear the global one in the login form.
            ' The current implementation assumes LoggedStudentID is a global variable.
            ' Replace this line if LoggedStudentID is a property of this form.
            ' GlobalVars.LoggedStudentID = "" 

            ' Close the main form
            Me.Close()

            ' Show the Login form again
            Dim frm As New frmLogin()
            frm.Show()
        End If
    End Sub

End Class