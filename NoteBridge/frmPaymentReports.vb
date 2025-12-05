Imports System.Data.OleDb
Imports System.IO
Imports System.Text

Public Class frmPaymentReports

    Private Sub frmPaymentReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Payment Reports"
        ' Configure the DataGridView settings (assuming it is named dgvPayments)
        dgvPayments.ReadOnly = True
        dgvPayments.AllowUserToAddRows = False
        dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        LoadPayments()
    End Sub

    Private Sub LoadPayments()
        If Not IsConnOpen() Then Exit Sub

        Try
            ' SQL to fetch all necessary payment data, joining with Students table
            Dim sql As String = "SELECT P.ReceiptNo, P.PaymentDate, P.AmountPaid, P.StudentNo, S.StudentName, S.Course " &
                                "FROM tblPayments AS P INNER JOIN tblStudents AS S ON P.StudentNo = S.StudentNo " &
                                "ORDER BY P.PaymentDate DESC"

            Dim cmd As New OleDbCommand(sql, con)
            Dim da As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable()

            da.Fill(dt)

            dgvPayments.DataSource = dt

            ' Rename columns for display
            If dgvPayments.Columns.Contains("ReceiptNo") Then dgvPayments.Columns("ReceiptNo").HeaderText = "Receipt No."
            If dgvPayments.Columns.Contains("PaymentDate") Then dgvPayments.Columns("PaymentDate").HeaderText = "Date Paid"
            If dgvPayments.Columns.Contains("AmountPaid") Then
                dgvPayments.Columns("AmountPaid").HeaderText = "Amount Paid"
                dgvPayments.Columns("AmountPaid").DefaultCellStyle.Format = "C2" ' Format as Currency
            End If
            If dgvPayments.Columns.Contains("StudentNo") Then dgvPayments.Columns("StudentNo").HeaderText = "Student No."
            If dgvPayments.Columns.Contains("StudentName") Then dgvPayments.Columns("StudentName").HeaderText = "Student Name"
            If dgvPayments.Columns.Contains("Course") Then dgvPayments.Columns("Course").HeaderText = "Course"


        Catch ex As Exception
            MessageBox.Show("Error loading payment records: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        If dgvPayments.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Try
            Dim saveFileDialog1 As New SaveFileDialog()
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            saveFileDialog1.FilterIndex = 1
            saveFileDialog1.RestoreDirectory = True
            saveFileDialog1.FileName = "PaymentReport_" & DateTime.Now.ToString("yyyyMMdd")

            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                Dim filePath As String = saveFileDialog1.FileName

                Dim sb As New StringBuilder()

                ' Add the column headers
                For i As Integer = 0 To dgvPayments.Columns.Count - 1
                    sb.Append(dgvPayments.Columns(i).HeaderText)
                    If i < dgvPayments.Columns.Count - 1 Then
                        sb.Append(",")
                    End If
                Next
                sb.AppendLine()

                ' Add the data rows
                For Each row As DataGridViewRow In dgvPayments.Rows
                    If Not row.IsNewRow Then
                        For i As Integer = 0 To dgvPayments.Columns.Count - 1
                            Dim cellValue As String = If(row.Cells(i).Value IsNot Nothing, row.Cells(i).Value.ToString().Replace("""", """"""), "")
                            sb.Append("""" & cellValue & """")
                            If i < dgvPayments.Columns.Count - 1 Then
                                sb.Append(",")
                            End If
                        Next
                        sb.AppendLine()
                    End If
                Next

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8)

                MessageBox.Show("Report successfully exported to: " & filePath, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred during export: " & ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadPayments()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class