Imports System.Data.OleDb
Imports System.Drawing

Public Class frmViewPermits

    ' Dictionary to map Installment Number (after Installment 1) to Exam Type
    Private ReadOnly ExamMap As New Dictionary(Of Integer, String) From {
        {2, "Long Test (Prelims)"},
        {3, "Prelim Exam"},
        {4, "Long Test (Midterms)"},
        {5, "Midterm Exam"},
        {6, "Pre-Finals Exam"},
        {7, "Finals Exam"}
    }

    ' Private DataTable to hold all permit records (for filtering and lookup)
    Private permitDataTable As New DataTable()

    Private Sub frmViewPermits_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Permit Status and History - Student: " & LoggedStudentID

        ' --- Set Textboxes to Read-Only for Summary ---
        txtStudentName.ReadOnly = True
        txtCurrentPermit.ReadOnly = True
        txtCurrentReceiptNo.ReadOnly = True

        ' --- Configure DataGridView ---
        dgvPermits.ReadOnly = True
        dgvPermits.AllowUserToAddRows = False
        dgvPermits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Load Data
        LoadStudentPermitData()
        PopulateFilterComboBox()

        ' Set the ComboBox back to "Show All" / Current Status
        cmbExamTypeFilter.SelectedIndex = 0
    End Sub

    ' --- Main Data Loading Subroutine ---
    Private Sub LoadStudentPermitData()
        Dim studentNo As String = LoggedStudentID

        ' Clear previous data
        txtStudentName.Clear()
        txtCurrentPermit.Clear()
        txtCurrentReceiptNo.Clear()
        permitDataTable.Clear()
        dgvPermits.DataSource = Nothing

        If Not IsConnOpen() Then Exit Sub

        Try
            Dim studentName As String = ""

            ' A. Fetch Student Name
            Dim sqlName As String = "SELECT StudentName FROM tblStudents WHERE StudentNo = @sno"
            Dim cmdName As New OleDbCommand(sqlName, con)
            cmdName.Parameters.AddWithValue("@sno", studentNo)

            Using dr As OleDbDataReader = cmdName.ExecuteReader()
                If dr.Read() Then
                    studentName = dr("StudentName").ToString()
                End If
            End Using
            txtStudentName.Text = studentName

            ' --- B. Find the highest PAID installment number for Summary Status ---
            Dim sqlMaxPaid As String = "SELECT MAX(InstallmentNo) FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE"
            Dim cmdMaxPaid As New OleDbCommand(sqlMaxPaid, con)
            cmdMaxPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim maxPaidInstallment As Integer = 1
            Dim result = cmdMaxPaid.ExecuteScalar()

            If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                maxPaidInstallment = Convert.ToInt32(result)
            End If

            ' --- C. SUMMARY SECTION LOGIC (BASED ONLY ON maxPaidInstallment) ---
            Dim currentPermitText As String = ""
            Dim currentReceiptNo As String = ""

            If maxPaidInstallment >= 2 AndAlso ExamMap.ContainsKey(maxPaidInstallment) Then
                currentPermitText = "CLEARED: " & ExamMap(maxPaidInstallment)

                ' Get the Receipt No. for the payment that granted this permit (the last payment made)
                Dim sqlReceipt As String = "SELECT TOP 1 ReceiptNo FROM tblPayments WHERE StudentNo = @sno ORDER BY PaymentDate DESC, ReceiptNo DESC"
                Dim cmdReceipt As New OleDbCommand(sqlReceipt, con)
                cmdReceipt.Parameters.AddWithValue("@sno", studentNo)

                Dim receiptResult = cmdReceipt.ExecuteScalar()
                If receiptResult IsNot DBNull.Value AndAlso receiptResult IsNot Nothing Then
                    currentReceiptNo = receiptResult.ToString()
                End If

            ElseIf maxPaidInstallment >= 7 Then
                currentPermitText = "CLEARED: ALL EXAMS"
            Else
                currentPermitText = "PENDING 2ND INSTALLMENT"
                currentReceiptNo = "N/A"
            End If

            txtCurrentPermit.Text = currentPermitText
            txtCurrentReceiptNo.Text = currentReceiptNo

            If currentPermitText.StartsWith("CLEARED") Then
                txtCurrentPermit.BackColor = Color.LightGreen
            Else
                txtCurrentPermit.BackColor = Color.Yellow
            End If

            ' --- D. HISTORY LOGIC (For DataGridView) ---
            permitDataTable.Columns.Clear()
            permitDataTable.Columns.Add("InstallmentNo", GetType(Integer))
            permitDataTable.Columns.Add("ExamType", GetType(String))
            permitDataTable.Columns.Add("ReceiptNo", GetType(String))
            permitDataTable.Columns.Add("DatePaid", GetType(String))

            ' Fetch paid installments and payment receipts (same logic as before)
            Dim sqlSchedule As String = "SELECT InstallmentNo FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE ORDER BY InstallmentNo ASC"
            Dim cmdSchedule As New OleDbCommand(sqlSchedule, con)
            cmdSchedule.Parameters.AddWithValue("@sno", studentNo)

            Dim sqlReceipts As String = "SELECT ReceiptNo, PaymentDate FROM tblPayments WHERE StudentNo = @sno ORDER BY PaymentDate ASC"
            Dim cmdReceipts As New OleDbCommand(sqlReceipts, con)
            cmdReceipts.Parameters.AddWithValue("@sno", studentNo)

            Dim dtSchedule As New DataTable()
            Dim daSchedule As New OleDbDataAdapter(cmdSchedule)
            daSchedule.Fill(dtSchedule)

            Dim dtReceipts As New DataTable()
            Dim daReceipts As New OleDbDataAdapter(cmdReceipts)
            daReceipts.Fill(dtReceipts)

            ' Manually correlate paid installments with payment receipts
            For i As Integer = 0 To Math.Min(dtSchedule.Rows.Count, dtReceipts.Rows.Count) - 1
                Dim installmentNo As Integer = CInt(dtSchedule.Rows(i)("InstallmentNo"))

                If ExamMap.ContainsKey(installmentNo) Then
                    Dim newRow As DataRow = permitDataTable.NewRow()
                    newRow("InstallmentNo") = installmentNo
                    newRow("ExamType") = ExamMap(installmentNo)
                    newRow("ReceiptNo") = dtReceipts.Rows(i)("ReceiptNo").ToString()
                    newRow("DatePaid") = CDate(dtReceipts.Rows(i)("PaymentDate")).ToShortDateString()

                    permitDataTable.Rows.Add(newRow)
                End If
            Next

            ' Display History data
            dgvPermits.DataSource = permitDataTable.DefaultView

            ' --- C. Column Renaming (NullReferenceException Fix) ---
            If permitDataTable.Rows.Count > 0 Then
                If dgvPermits.Columns.Contains("InstallmentNo") Then
                    dgvPermits.Columns("InstallmentNo").HeaderText = "Inst. No."
                End If
                If dgvPermits.Columns.Contains("ExamType") Then
                    dgvPermits.Columns("ExamType").HeaderText = "Permit Granted For"
                End If
                If dgvPermits.Columns.Contains("ReceiptNo") Then
                    dgvPermits.Columns("ReceiptNo").HeaderText = "Receipt No."
                End If
                If dgvPermits.Columns.Contains("DatePaid") Then
                    dgvPermits.Columns("DatePaid").HeaderText = "Payment Date"
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading permit data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --- Filter Population (Same as before) ---
    Private Sub PopulateFilterComboBox()
        cmbExamTypeFilter.Items.Clear()
        cmbExamTypeFilter.Items.Add("--- Show All Permits ---")

        For Each row As DataRow In permitDataTable.Rows
            Dim examType As String = row("ExamType").ToString()
            If Not cmbExamTypeFilter.Items.Contains(examType) Then
                cmbExamTypeFilter.Items.Add(examType)
            End If
        Next

        cmbExamTypeFilter.SelectedIndex = 0
    End Sub

    ' --- Filtering Logic (No Textbox Modification) ---
    Private Sub cmbExamTypeFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExamTypeFilter.SelectedIndexChanged
        If cmbExamTypeFilter.SelectedItem Is Nothing Then Exit Sub

        Dim filterValue As String = cmbExamTypeFilter.SelectedItem.ToString()
        Dim dv As DataView = permitDataTable.DefaultView

        ' Only modify the DataGridView filter
        If filterValue = "--- Show All Permits ---" Then
            dv.RowFilter = "" ' Show all permits in the history grid
        Else
            ' Filter the grid to show only the selected permit
            dv.RowFilter = String.Format("ExamType = '{0}'", filterValue.Replace("'", "''"))
        End If

        dgvPermits.DataSource = dv
    End Sub

End Class