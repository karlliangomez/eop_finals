<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewPermits
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewPermits))
        Me.txtPaymentStatus = New System.Windows.Forms.TextBox()
        Me.txtCurrentReceiptNo = New System.Windows.Forms.TextBox()
        Me.txtCurrentPermit = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStudentName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbExamTypeFilter = New System.Windows.Forms.ComboBox()
        Me.dgvPermits = New System.Windows.Forms.DataGridView()
        CType(Me.dgvPermits, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtPaymentStatus
        '
        Me.txtPaymentStatus.Location = New System.Drawing.Point(228, 116)
        Me.txtPaymentStatus.Name = "txtPaymentStatus"
        Me.txtPaymentStatus.Size = New System.Drawing.Size(187, 22)
        Me.txtPaymentStatus.TabIndex = 24
        '
        'txtCurrentReceiptNo
        '
        Me.txtCurrentReceiptNo.Location = New System.Drawing.Point(228, 88)
        Me.txtCurrentReceiptNo.Name = "txtCurrentReceiptNo"
        Me.txtCurrentReceiptNo.Size = New System.Drawing.Size(187, 22)
        Me.txtCurrentReceiptNo.TabIndex = 23
        '
        'txtCurrentPermit
        '
        Me.txtCurrentPermit.Location = New System.Drawing.Point(228, 60)
        Me.txtCurrentPermit.Name = "txtCurrentPermit"
        Me.txtCurrentPermit.Size = New System.Drawing.Size(187, 22)
        Me.txtCurrentPermit.TabIndex = 22
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(30, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 16)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Payment Status:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(30, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 16)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Receipt No.:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(30, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Permit No.:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(55, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 16)
        Me.Label1.TabIndex = 16
        '
        'txtStudentName
        '
        Me.txtStudentName.Location = New System.Drawing.Point(228, 32)
        Me.txtStudentName.Name = "txtStudentName"
        Me.txtStudentName.ReadOnly = True
        Me.txtStudentName.Size = New System.Drawing.Size(187, 22)
        Me.txtStudentName.TabIndex = 21
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(30, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Student Name:"
        '
        'cmbExamTypeFilter
        '
        Me.cmbExamTypeFilter.FormattingEnabled = True
        Me.cmbExamTypeFilter.Location = New System.Drawing.Point(30, 149)
        Me.cmbExamTypeFilter.Name = "cmbExamTypeFilter"
        Me.cmbExamTypeFilter.Size = New System.Drawing.Size(385, 24)
        Me.cmbExamTypeFilter.TabIndex = 25
        '
        'dgvPermits
        '
        Me.dgvPermits.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPermits.BackgroundColor = System.Drawing.Color.Green
        Me.dgvPermits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPermits.GridColor = System.Drawing.Color.Green
        Me.dgvPermits.Location = New System.Drawing.Point(30, 179)
        Me.dgvPermits.Name = "dgvPermits"
        Me.dgvPermits.RowTemplate.Height = 24
        Me.dgvPermits.Size = New System.Drawing.Size(385, 150)
        Me.dgvPermits.TabIndex = 26
        '
        'frmViewPermits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.ClientSize = New System.Drawing.Size(446, 351)
        Me.Controls.Add(Me.dgvPermits)
        Me.Controls.Add(Me.cmbExamTypeFilter)
        Me.Controls.Add(Me.txtPaymentStatus)
        Me.Controls.Add(Me.txtCurrentReceiptNo)
        Me.Controls.Add(Me.txtCurrentPermit)
        Me.Controls.Add(Me.txtStudentName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmViewPermits"
        Me.Text = "View Permits"
        AddHandler Load, AddressOf Me.frmViewPermits_Load
        CType(Me.dgvPermits, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtPaymentStatus As TextBox
    Friend WithEvents txtCurrentReceiptNo As TextBox
    Friend WithEvents txtCurrentPermit As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtStudentName As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbExamTypeFilter As ComboBox
    Friend WithEvents dgvPermits As DataGridView
End Class
