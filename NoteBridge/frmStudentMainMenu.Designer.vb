<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStudentMainMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStudentMainMenu))
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.btnViewPN = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnViewPermits = New System.Windows.Forms.Button()
        Me.btnRequestPN = New System.Windows.Forms.Button()
        Me.btnViewPayments = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnChangePassword
        '
        Me.btnChangePassword.BackColor = System.Drawing.Color.Green
        Me.btnChangePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePassword.ForeColor = System.Drawing.Color.White
        Me.btnChangePassword.Location = New System.Drawing.Point(257, 349)
        Me.btnChangePassword.Margin = New System.Windows.Forms.Padding(4)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(221, 73)
        Me.btnChangePassword.TabIndex = 11
        Me.btnChangePassword.Text = "Change Password"
        Me.btnChangePassword.UseVisualStyleBackColor = False
        '
        'btnViewPN
        '
        Me.btnViewPN.BackColor = System.Drawing.Color.Green
        Me.btnViewPN.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewPN.ForeColor = System.Drawing.Color.White
        Me.btnViewPN.Location = New System.Drawing.Point(28, 349)
        Me.btnViewPN.Margin = New System.Windows.Forms.Padding(4)
        Me.btnViewPN.Name = "btnViewPN"
        Me.btnViewPN.Size = New System.Drawing.Size(221, 73)
        Me.btnViewPN.TabIndex = 10
        Me.btnViewPN.Text = "View My Promissory Note"
        Me.btnViewPN.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Green
        Me.btnLogout.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(487, 349)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(221, 73)
        Me.btnLogout.TabIndex = 9
        Me.btnLogout.Text = "Log Out"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'btnViewPermits
        '
        Me.btnViewPermits.BackColor = System.Drawing.Color.Green
        Me.btnViewPermits.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewPermits.ForeColor = System.Drawing.Color.White
        Me.btnViewPermits.Location = New System.Drawing.Point(257, 30)
        Me.btnViewPermits.Margin = New System.Windows.Forms.Padding(4)
        Me.btnViewPermits.Name = "btnViewPermits"
        Me.btnViewPermits.Size = New System.Drawing.Size(221, 73)
        Me.btnViewPermits.TabIndex = 8
        Me.btnViewPermits.Text = "View My Permits"
        Me.btnViewPermits.UseVisualStyleBackColor = False
        '
        'btnRequestPN
        '
        Me.btnRequestPN.BackColor = System.Drawing.Color.Green
        Me.btnRequestPN.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRequestPN.ForeColor = System.Drawing.Color.White
        Me.btnRequestPN.Location = New System.Drawing.Point(487, 30)
        Me.btnRequestPN.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRequestPN.Name = "btnRequestPN"
        Me.btnRequestPN.Size = New System.Drawing.Size(221, 73)
        Me.btnRequestPN.TabIndex = 7
        Me.btnRequestPN.Text = "Request Promissory Note"
        Me.btnRequestPN.UseVisualStyleBackColor = False
        '
        'btnViewPayments
        '
        Me.btnViewPayments.BackColor = System.Drawing.Color.Green
        Me.btnViewPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewPayments.ForeColor = System.Drawing.Color.White
        Me.btnViewPayments.Location = New System.Drawing.Point(28, 30)
        Me.btnViewPayments.Margin = New System.Windows.Forms.Padding(4)
        Me.btnViewPayments.Name = "btnViewPayments"
        Me.btnViewPayments.Size = New System.Drawing.Size(221, 73)
        Me.btnViewPayments.TabIndex = 6
        Me.btnViewPayments.Text = "View My Payments"
        Me.btnViewPayments.UseVisualStyleBackColor = False
        '
        'frmStudentMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.NoteBridge.My.Resources.Resources.logo_4
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(737, 452)
        Me.Controls.Add(Me.btnChangePassword)
        Me.Controls.Add(Me.btnViewPN)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.btnViewPermits)
        Me.Controls.Add(Me.btnRequestPN)
        Me.Controls.Add(Me.btnViewPayments)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStudentMainMenu"
        Me.Text = "NoteBridge"
        AddHandler Load, AddressOf Me.frmStudentMainMenu_Load
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnChangePassword As Button
    Friend WithEvents btnViewPN As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnViewPermits As Button
    Friend WithEvents btnRequestPN As Button
    Friend WithEvents btnViewPayments As Button
End Class
