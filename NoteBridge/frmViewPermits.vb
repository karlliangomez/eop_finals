Imports System.Data.OleDb
Imports System.Drawing

Public Class frmViewPermits

    ' Assuming LoggedStudentID is a Public variable defined in a module or static class
    Public LoggedStudentID As String ' This must be set by the calling form (e.g., frmStudentMainMenu)

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

        ' 🛑 Ensure LoggedStudentID is available 🛑
        If String.IsNullOrEmpty(LoggedStudentID) Then
            MessageBox.Show("Error: Student Number not found. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Exit Sub
        End If

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
            ' We prioritize the LATEST semester first for the summary status
            Dim sqlMaxPaid As String = "SELECT TOP 1 InstallmentNo, Semester FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE ORDER BY Semester DESC, InstallmentNo DESC"
            Dim cmdMaxPaid As New OleDbCommand(sqlMaxPaid, con)
            cmdMaxPaid.Parameters.AddWithValue("@sno", studentNo)

            Dim maxPaidInstallment As Integer = 1
            Dim activeSemesterSummary As String = ""
            Dim drSummary As OleDbDataReader = cmdMaxPaid.ExecuteReader()

            If drSummary.Read() Then
                If drSummary("InstallmentNo") IsNot DBNull.Value Then
                    maxPaidInstallment = Convert.ToInt32(drSummary("InstallmentNo"))
                End If
                activeSemesterSummary = drSummary("Semester").ToString()
            End If
            drSummary.Close()

            ' --- C. SUMMARY SECTION LOGIC (BASED ONLY ON maxPaidInstallment) ---
            Dim currentPermitText As String = ""
            Dim currentReceiptNo As String = ""

            If maxPaidInstallment >= 2 AndAlso ExamMap.ContainsKey(maxPaidInstallment) Then
                currentPermitText = "CLEARED: " & ExamMap(maxPaidInstallment) & " (" & activeSemesterSummary & ")"

                ' Get the Receipt No. for the payment that granted this permit (the last payment made)
                Dim sqlReceipt As String = "SELECT TOP 1 ReceiptNo FROM tblPayments WHERE StudentNo = @sno AND Semester = @sem ORDER BY PaymentDate DESC, ReceiptNo DESC"
                Dim cmdReceipt As New OleDbCommand(sqlReceipt, con)
                cmdReceipt.Parameters.AddWithValue("@sno", studentNo)
                cmdReceipt.Parameters.AddWithValue("@sem", activeSemesterSummary)

                Dim receiptResult = cmdReceipt.ExecuteScalar()
                If receiptResult IsNot DBNull.Value AndAlso receiptResult IsNot Nothing Then
                    currentReceiptNo = receiptResult.ToString()
                End If

            ElseIf maxPaidInstallment >= 7 Then
                currentPermitText = "CLEARED: ALL EXAMS (" & activeSemesterSummary & ")"
                currentReceiptNo = "N/A" ' Assuming Finals is the last, receipt is likely already shown for PFE
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
            permitDataTable.Columns.Add("Semester", GetType(String)) ' 🛑 NEW COLUMN 🛑
            permitDataTable.Columns.Add("ReceiptNo", GetType(String))
            permitDataTable.Columns.Add("DatePaid", GetType(String))

            ' Fetch paid installments and their corresponding semester
            Dim sqlSchedule As String = "SELECT InstallmentNo, Semester FROM tblPaymentSchedule WHERE StudentNo = @sno AND IsPaid = TRUE ORDER BY Semester ASC, InstallmentNo ASC"
            Dim cmdSchedule As New OleDbCommand(sqlSchedule, con)
            cmdSchedule.Parameters.AddWithValue("@sno", studentNo)

            ' Fetch payment receipts (Ordered by date to correlate with installments)
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

                ' We only log installments that grant a permit (Installment 2 and above, or the last installment)
                If ExamMap.ContainsKey(installmentNo) OrElse installmentNo = 7 Then
                    Dim newRow As DataRow = permitDataTable.NewRow()

                    newRow("InstallmentNo") = installmentNo

                    ' Use ExamMap for Installments 2-6, and manually set for 7
                    If ExamMap.ContainsKey(installmentNo) Then
                        newRow("ExamType") = ExamMap(installmentNo)
                    ElseIf installmentNo = 7 Then
                        newRow("ExamType") = "Finals Exam"
                    End If

                    newRow("Semester") = dtSchedule.Rows(i)("Semester").ToString() ' 🛑 SEMESTER DATA 🛑
                    newRow("ReceiptNo") = dtReceipts.Rows(i)("ReceiptNo").ToString()
                    newRow("DatePaid") = CDate(dtReceipts.Rows(i)("PaymentDate")).ToShortDateString()

                    permitDataTable.Rows.Add(newRow)
                End If
            Next

            ' Display History data
            dgvPermits.DataSource = permitDataTable.DefaultView

            ' --- E. Column Renaming ---
            If permitDataTable.Rows.Count > 0 Then
                If dgvPermits.Columns.Contains("InstallmentNo") Then
                    dgvPermits.Columns("InstallmentNo").HeaderText = "Inst. No."
                End If
                If dgvPermits.Columns.Contains("ExamType") Then
                    dgvPermits.Columns("ExamType").HeaderText = "Permit Granted For"
                End If
                If dgvPermits.Columns.Contains("Semester") Then
                    dgvPermits.Columns("Semester").HeaderText = "Semester" ' 🛑 NEW HEADER 🛑
                End If
                If dgvPermits.Columns.Contains("ReceiptNo") Then
                    dgvPermits.Columns("ReceiptNo").HeaderText = "Receipt No."
                End If
                If dgvPermits.Columns.Contains("DatePaid") Then
                    dgvPermits.Columns("DatePaid").HeaderText = "Payment Date"
                End If

                dgvPermits.AutoResizeColumns()
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading permit data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' --- Filter Population (Updated for new column) ---
    Private Sub PopulateFilterComboBox()
        cmbExamTypeFilter.Items.Clear()
        cmbExamTypeFilter.Items.Add("--- Show All Permits ---")

        ' Add all unique Exam Types (Permit Granted For) to the filter list
        For Each row As DataRow In permitDataTable.Rows
            Dim examType As String = row("ExamType").ToString()
            If Not cmbExamTypeFilter.Items.Contains(examType) Then
                cmbExamTypeFilter.Items.Add(examType)
            End If
        Next

        ' Add all unique Semesters to the filter list
        For Each row As DataRow In permitDataTable.Rows
            Dim semester As String = row("Semester").ToString()
            If Not cmbExamTypeFilter.Items.Contains(semester) Then
                cmbExamTypeFilter.Items.Add(semester)
            End If
        Next

        cmbExamTypeFilter.SelectedIndex = 0
    End Sub

    ' --- Filtering Logic (Allows filtering by ExamType or Semester) ---
    Private Sub cmbExamTypeFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExamTypeFilter.SelectedIndexChanged
        If cmbExamTypeFilter.SelectedItem Is Nothing Then Exit Sub

        Dim filterValue As String = cmbExamTypeFilter.SelectedItem.ToString()
        Dim dv As DataView = permitDataTable.DefaultView

        If filterValue = "--- Show All Permits ---" Then
            dv.RowFilter = ""
        Else
            ' Check if the selected value is an Exam Type or a Semester
            Dim isExamTypeFilter As Boolean = ExamMap.ContainsValue(filterValue)
            Dim isSemesterFilter As Boolean = permitDataTable.AsEnumerable().Any(Function(row) row.Field(Of String)("Semester") = filterValue)

            If isExamTypeFilter Then
                dv.RowFilter = String.Format("ExamType = '{0}'", filterValue.Replace("'", "''"))
            ElseIf isSemesterFilter Then
                dv.RowFilter = String.Format("Semester = '{0}'", filterValue.Replace("'", "''"))
            Else
                ' Handle cases where selection isn't clearly a semester or exam type (e.g., if you add other filters later)
                dv.RowFilter = ""
            End If
        End If

        dgvPermits.DataSource = dv
    End Sub

End Class