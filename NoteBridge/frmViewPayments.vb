Imports System.Data.OleDb

Public Class frmViewPayments

    Private Sub frmViewPayments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "My Payments - Student: " & LoggedStudentID

        ' --- Set Textboxes to Read-Only ---
        txtStudentName.ReadOnly = True
        txtTotalAssessment.ReadOnly = True
        txtTotalPaid.ReadOnly = True
        txtRemainingBalance.ReadOnly = True

        ' Configure DataGridView
        dgvSchedule.ReadOnly = True
        dgvSchedule.AllowUserToAddRows = False
        dgvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        LoadStudentFinancials()
    End Sub

    Private Sub LoadStudentFinancials()
        Dim studentNo As String = LoggedStudentID

        ' Clear previous data
        txtStudentName.Clear()
        txtTotalAssessment.Clear()
        txtTotalPaid.Clear()
        txtRemainingBalance.Clear()
        dgvSchedule.DataSource = Nothing ' Clear the grid

        If Not IsConnOpen() Then Exit Sub

        Try
            Dim totalAssessment As Decimal = 0D
            Dim studentName As String = ""

            ' --- A. Fetch Student Details (Name and Total Assessment) ---
            Dim sqlDetails As String = "SELECT StudentName, TotalAssessment FROM tblStudents WHERE StudentNo = @sno"
            Dim cmdDetails As New OleDbCommand(sqlDetails, con)
            cmdDetails.Parameters.AddWithValue("@sno", studentNo)

            Using dr As OleDbDataReader = cmdDetails.ExecuteReader()
                If dr.Read() Then
                    studentName = dr("StudentName").ToString()
                    If dr("TotalAssessment") IsNot DBNull.Value Then
                        totalAssessment = Convert.ToDecimal(dr("TotalAssessment"))
                    End If
                End If
            End Using

            ' Populate Textboxes
            txtStudentName.Text = studentName
            txtTotalAssessment.Text = totalAssessment.ToString("C2") ' Display as currency

            ' --- B. Calculate Total Paid ---
            Dim sqlPaid As String = "SELECT SUM(AmountPaid) FROM tblPayments WHERE StudentNo = @sno"
            Dim cmdPaid As New OleDbCommand(sqlPaid, con)
            cmdPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim totalPaid As Decimal = 0D
            Dim paidResult = cmdPaid.ExecuteScalar()
            If paidResult IsNot DBNull.Value AndAlso paidResult IsNot Nothing Then
                totalPaid = Convert.ToDecimal(paidResult)
            End If

            ' Populate Textboxes
            txtTotalPaid.Text = totalPaid.ToString("C2") ' Display as currency

            ' --- C. Calculate Remaining Balance ---
            Dim remainingBalance As Decimal = totalAssessment - totalPaid
            txtRemainingBalance.Text = remainingBalance.ToString("C2") ' Display as currency

            ' --- D. Load Payment Schedule into DataGridView ---
            Dim sqlSchedule As String = "SELECT InstallmentNo, AmountDue, Balance, IsPaid " &
                                        "FROM tblPaymentSchedule WHERE StudentNo = @sno ORDER BY InstallmentNo ASC"

            Dim cmdSchedule As New OleDbCommand(sqlSchedule, con)
            cmdSchedule.Parameters.AddWithValue("@sno", studentNo)

            Dim da As New OleDbDataAdapter(cmdSchedule)
            Dim dt As New DataTable()

            da.Fill(dt)
            dgvSchedule.DataSource = dt

            ' Rename and format columns for display
            If dgvSchedule.Columns.Contains("InstallmentNo") Then dgvSchedule.Columns("InstallmentNo").HeaderText = "Installment No."
            If dgvSchedule.Columns.Contains("AmountDue") Then
                dgvSchedule.Columns("AmountDue").HeaderText = "Amount Due"
                dgvSchedule.Columns("AmountDue").DefaultCellStyle.Format = "C2"
            End If
            If dgvSchedule.Columns.Contains("Balance") Then
                dgvSchedule.Columns("Balance").HeaderText = "Remaining Installment Balance"
                dgvSchedule.Columns("Balance").DefaultCellStyle.Format = "C2"
            End If
            If dgvSchedule.Columns.Contains("IsPaid") Then
                dgvSchedule.Columns("IsPaid").HeaderText = "Paid Status"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading financial data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

End Class