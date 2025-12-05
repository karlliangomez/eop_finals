<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmStudentPayments
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStudentPayments))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtStudentNo = New System.Windows.Forms.TextBox()
        Me.txtStudentName = New System.Windows.Forms.TextBox()
        Me.txtCourse = New System.Windows.Forms.TextBox()
        Me.txtYearSection = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReceiptNo = New System.Windows.Forms.TextBox()
        Me.txtAmountPaid = New System.Windows.Forms.TextBox()
        Me.txtPendingBalance = New System.Windows.Forms.TextBox()
        Me.txtTotalAssessment = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnUpdateAssessment = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.cmbUpdateSemester = New System.Windows.Forms.ComboBox()
        Me.lblActiveSemester = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 54)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(239, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Student Information"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 97)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Student No.:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 125)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Name:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 154)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 16)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Course:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 182)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(113, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Year and Section:"
        '
        'txtStudentNo
        '
        Me.txtStudentNo.Location = New System.Drawing.Point(157, 93)
        Me.txtStudentNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtStudentNo.Name = "txtStudentNo"
        Me.txtStudentNo.ReadOnly = True
        Me.txtStudentNo.Size = New System.Drawing.Size(221, 22)
        Me.txtStudentNo.TabIndex = 5
        '
        'txtStudentName
        '
        Me.txtStudentName.Location = New System.Drawing.Point(157, 122)
        Me.txtStudentName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtStudentName.Name = "txtStudentName"
        Me.txtStudentName.ReadOnly = True
        Me.txtStudentName.Size = New System.Drawing.Size(221, 22)
        Me.txtStudentName.TabIndex = 6
        '
        'txtCourse
        '
        Me.txtCourse.Location = New System.Drawing.Point(157, 150)
        Me.txtCourse.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCourse.Name = "txtCourse"
        Me.txtCourse.ReadOnly = True
        Me.txtCourse.Size = New System.Drawing.Size(221, 22)
        Me.txtCourse.TabIndex = 7
        '
        'txtYearSection
        '
        Me.txtYearSection.Location = New System.Drawing.Point(157, 178)
        Me.txtYearSection.Margin = New System.Windows.Forms.Padding(4)
        Me.txtYearSection.Name = "txtYearSection"
        Me.txtYearSection.ReadOnly = True
        Me.txtYearSection.Size = New System.Drawing.Size(221, 22)
        Me.txtYearSection.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 221)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(201, 29)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Payment Details"
        '
        'txtReceiptNo
        '
        Me.txtReceiptNo.Location = New System.Drawing.Point(157, 345)
        Me.txtReceiptNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtReceiptNo.Name = "txtReceiptNo"
        Me.txtReceiptNo.Size = New System.Drawing.Size(221, 22)
        Me.txtReceiptNo.TabIndex = 17
        '
        'txtAmountPaid
        '
        Me.txtAmountPaid.Location = New System.Drawing.Point(157, 316)
        Me.txtAmountPaid.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAmountPaid.Name = "txtAmountPaid"
        Me.txtAmountPaid.Size = New System.Drawing.Size(221, 22)
        Me.txtAmountPaid.TabIndex = 16
        '
        'txtPendingBalance
        '
        Me.txtPendingBalance.Location = New System.Drawing.Point(157, 288)
        Me.txtPendingBalance.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPendingBalance.Name = "txtPendingBalance"
        Me.txtPendingBalance.ReadOnly = True
        Me.txtPendingBalance.Size = New System.Drawing.Size(221, 22)
        Me.txtPendingBalance.TabIndex = 15
        '
        'txtTotalAssessment
        '
        Me.txtTotalAssessment.Location = New System.Drawing.Point(157, 260)
        Me.txtTotalAssessment.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTotalAssessment.Name = "txtTotalAssessment"
        Me.txtTotalAssessment.Size = New System.Drawing.Size(221, 22)
        Me.txtTotalAssessment.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(17, 348)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 16)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Receipt Number:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(17, 320)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(86, 16)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Amount Paid:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(17, 292)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(113, 16)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Pending Balance:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 263)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(118, 16)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Total Assessment:"
        '
        'txtDate
        '
        Me.txtDate.Location = New System.Drawing.Point(157, 373)
        Me.txtDate.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.Size = New System.Drawing.Size(221, 22)
        Me.txtDate.TabIndex = 19
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(17, 377)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(39, 16)
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "Date:"
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Green
        Me.btnSave.Location = New System.Drawing.Point(16, 439)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(109, 28)
        Me.btnSave.TabIndex = 23
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnUpdateAssessment
        '
        Me.btnUpdateAssessment.BackColor = System.Drawing.Color.Green
        Me.btnUpdateAssessment.Location = New System.Drawing.Point(146, 439)
        Me.btnUpdateAssessment.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpdateAssessment.Name = "btnUpdateAssessment"
        Me.btnUpdateAssessment.Size = New System.Drawing.Size(109, 28)
        Me.btnUpdateAssessment.TabIndex = 24
        Me.btnUpdateAssessment.Text = "Update"
        Me.btnUpdateAssessment.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.BackColor = System.Drawing.Color.Green
        Me.btnClear.Location = New System.Drawing.Point(269, 439)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(109, 28)
        Me.btnClear.TabIndex = 25
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Green
        Me.btnSearch.Location = New System.Drawing.Point(278, 13)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 28)
        Me.btnSearch.TabIndex = 21
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(17, 15)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.Multiline = True
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(269, 25)
        Me.txtSearch.TabIndex = 20
        '
        'cmbUpdateSemester
        '
        Me.cmbUpdateSemester.FormattingEnabled = True
        Me.cmbUpdateSemester.Location = New System.Drawing.Point(17, 402)
        Me.cmbUpdateSemester.Name = "cmbUpdateSemester"
        Me.cmbUpdateSemester.Size = New System.Drawing.Size(361, 24)
        Me.cmbUpdateSemester.TabIndex = 26
        '
        'lblActiveSemester
        '
        Me.lblActiveSemester.AutoSize = True
        Me.lblActiveSemester.Location = New System.Drawing.Point(313, 231)
        Me.lblActiveSemester.Name = "lblActiveSemester"
        Me.lblActiveSemester.Size = New System.Drawing.Size(65, 16)
        Me.lblActiveSemester.TabIndex = 27
        Me.lblActiveSemester.Text = "Semester"
        '
        'frmStudentPayments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.ClientSize = New System.Drawing.Size(399, 488)
        Me.Controls.Add(Me.lblActiveSemester)
        Me.Controls.Add(Me.cmbUpdateSemester)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnUpdateAssessment)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtReceiptNo)
        Me.Controls.Add(Me.txtAmountPaid)
        Me.Controls.Add(Me.txtPendingBalance)
        Me.Controls.Add(Me.txtTotalAssessment)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtYearSection)
        Me.Controls.Add(Me.txtCourse)
        Me.Controls.Add(Me.txtStudentName)
        Me.Controls.Add(Me.txtStudentNo)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.White
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmStudentPayments"
        Me.Text = "Student Payments"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtStudentNo As TextBox
    Friend WithEvents txtStudentName As TextBox
    Friend WithEvents txtCourse As TextBox
    Friend WithEvents txtYearSection As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReceiptNo As TextBox
    Friend WithEvents txtAmountPaid As TextBox
    Friend WithEvents txtPendingBalance As TextBox
    Friend WithEvents txtTotalAssessment As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents txtDate As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnUpdateAssessment As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents StudentNo As DataGridViewTextBoxColumn
    Friend WithEvents StudentName As DataGridViewTextBoxColumn
    Friend WithEvents Course As DataGridViewTextBoxColumn
    Friend WithEvents YearSection As DataGridViewTextBoxColumn
    Friend WithEvents DbNoteBridgeDataSetBindingSource As BindingSource
    Friend WithEvents btnSearch As Button
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents cmbUpdateSemester As ComboBox
    Friend WithEvents lblActiveSemester As Label
End Class
