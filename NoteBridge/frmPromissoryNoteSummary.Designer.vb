<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPromissoryNoteSummary
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPromissoryNoteSummary))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.dgvPNRequests = New System.Windows.Forms.DataGridView()
        Me.cmbExamType = New System.Windows.Forms.ComboBox()
        Me.cmbSemesterFilter = New System.Windows.Forms.ComboBox()
        CType(Me.dgvPNRequests, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Green
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(347, 364)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(113, 36)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.Green
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(228, 364)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(113, 36)
        Me.btnRefresh.TabIndex = 6
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.Green
        Me.btnExport.ForeColor = System.Drawing.Color.White
        Me.btnExport.Location = New System.Drawing.Point(12, 364)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(210, 36)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "Export to Excel"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'dgvPNRequests
        '
        Me.dgvPNRequests.AllowUserToAddRows = False
        Me.dgvPNRequests.AllowUserToDeleteRows = False
        Me.dgvPNRequests.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPNRequests.BackgroundColor = System.Drawing.Color.Green
        Me.dgvPNRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPNRequests.GridColor = System.Drawing.Color.Green
        Me.dgvPNRequests.Location = New System.Drawing.Point(12, 52)
        Me.dgvPNRequests.Name = "dgvPNRequests"
        Me.dgvPNRequests.ReadOnly = True
        Me.dgvPNRequests.RowTemplate.Height = 24
        Me.dgvPNRequests.Size = New System.Drawing.Size(1132, 306)
        Me.dgvPNRequests.TabIndex = 4
        '
        'cmbExamType
        '
        Me.cmbExamType.FormattingEnabled = True
        Me.cmbExamType.Location = New System.Drawing.Point(12, 12)
        Me.cmbExamType.Name = "cmbExamType"
        Me.cmbExamType.Size = New System.Drawing.Size(343, 24)
        Me.cmbExamType.TabIndex = 8
        '
        'cmbSemesterFilter
        '
        Me.cmbSemesterFilter.FormattingEnabled = True
        Me.cmbSemesterFilter.Location = New System.Drawing.Point(361, 12)
        Me.cmbSemesterFilter.Name = "cmbSemesterFilter"
        Me.cmbSemesterFilter.Size = New System.Drawing.Size(343, 24)
        Me.cmbSemesterFilter.TabIndex = 9
        '
        'frmPromissoryNoteSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.ClientSize = New System.Drawing.Size(1156, 410)
        Me.Controls.Add(Me.cmbSemesterFilter)
        Me.Controls.Add(Me.cmbExamType)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.dgvPNRequests)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPromissoryNoteSummary"
        Me.Text = "Promissory Note Summary"
        AddHandler Load, AddressOf Me.frmPromissoryNoteSummary_Load
        CType(Me.dgvPNRequests, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents dgvPNRequests As DataGridView
    Friend WithEvents cmbExamType As ComboBox
    Friend WithEvents cmbSemesterFilter As ComboBox
End Class
