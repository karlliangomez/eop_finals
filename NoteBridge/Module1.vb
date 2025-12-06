
Imports System.Data.OleDb

Module Module1

    ' --- Global Connection and Command Objects ---
    Public con As New OleDbConnection()
    Public cmd As OleDbCommand
    Public dr As OleDbDataReader
    Public da As OleDbDataAdapter
    Public dt As DataTable

    ' --- Global Variables ---
    Public LoggedStudentID As String = ""

    ' --- Connection Subroutine ---
    Public Sub ConnectDB()
        ' *** IMPORTANT: VERIFY AND REPLACE THE CONNECTION STRING BELOW WITH YOUR ACTUAL PATH ***
        con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ID_PICTURE\Music\eop_finals\NoteBridge\db_NoteBridge.accdb"
    End Sub

    ' --- Safe Connection Check ---
    Public Function IsConnOpen() As Boolean
        Try
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If
            Return True
        Catch ex As Exception
            MessageBox.Show("Database connection failed: " & ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Module