<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdminMainMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdminMainMenu))
        Me.btnStudentPayments = New System.Windows.Forms.Button()
        Me.btnPromissoryNoteSummary = New System.Windows.Forms.Button()
        Me.btnPaymentReports = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnAddNewStudent = New System.Windows.Forms.Button()
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnStudentPayments
        '
        Me.btnStudentPayments.BackColor = System.Drawing.Color.Green
        Me.btnStudentPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentPayments.ForeColor = System.Drawing.Color.White
        Me.btnStudentPayments.Location = New System.Drawing.Point(28, 31)
        Me.btnStudentPayments.Margin = New System.Windows.Forms.Padding(4)
        Me.btnStudentPayments.Name = "btnStudentPayments"
        Me.btnStudentPayments.Size = New System.Drawing.Size(221, 73)
        Me.btnStudentPayments.TabIndex = 0
        Me.btnStudentPayments.Text = "Student Payments"
        Me.btnStudentPayments.UseVisualStyleBackColor = False
        '
        'btnPromissoryNoteSummary
        '
        Me.btnPromissoryNoteSummary.BackColor = System.Drawing.Color.Green
        Me.btnPromissoryNoteSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPromissoryNoteSummary.ForeColor = System.Drawing.Color.White
        Me.btnPromissoryNoteSummary.Location = New System.Drawing.Point(487, 31)
        Me.btnPromissoryNoteSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPromissoryNoteSummary.Name = "btnPromissoryNoteSummary"
        Me.btnPromissoryNoteSummary.Size = New System.Drawing.Size(221, 73)
        Me.btnPromissoryNoteSummary.TabIndex = 1
        Me.btnPromissoryNoteSummary.Text = "Promissory Note Summary"
        Me.btnPromissoryNoteSummary.UseVisualStyleBackColor = False
        '
        'btnPaymentReports
        '
        Me.btnPaymentReports.BackColor = System.Drawing.Color.Green
        Me.btnPaymentReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPaymentReports.ForeColor = System.Drawing.Color.White
        Me.btnPaymentReports.Location = New System.Drawing.Point(257, 31)
        Me.btnPaymentReports.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPaymentReports.Name = "btnPaymentReports"
        Me.btnPaymentReports.Size = New System.Drawing.Size(221, 73)
        Me.btnPaymentReports.TabIndex = 2
        Me.btnPaymentReports.Text = "Payment Reports"
        Me.btnPaymentReports.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Green
        Me.btnLogout.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(487, 350)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(221, 73)
        Me.btnLogout.TabIndex = 3
        Me.btnLogout.Text = "Log Out"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'btnAddNewStudent
        '
        Me.btnAddNewStudent.BackColor = System.Drawing.Color.Green
        Me.btnAddNewStudent.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddNewStudent.ForeColor = System.Drawing.Color.White
        Me.btnAddNewStudent.Location = New System.Drawing.Point(28, 350)
        Me.btnAddNewStudent.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAddNewStudent.Name = "btnAddNewStudent"
        Me.btnAddNewStudent.Size = New System.Drawing.Size(221, 73)
        Me.btnAddNewStudent.TabIndex = 4
        Me.btnAddNewStudent.Text = "Add New Student"
        Me.btnAddNewStudent.UseVisualStyleBackColor = False
        '
        'btnChangePassword
        '
        Me.btnChangePassword.BackColor = System.Drawing.Color.Green
        Me.btnChangePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePassword.ForeColor = System.Drawing.Color.White
        Me.btnChangePassword.Location = New System.Drawing.Point(257, 350)
        Me.btnChangePassword.Margin = New System.Windows.Forms.Padding(4)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(221, 73)
        Me.btnChangePassword.TabIndex = 5
        Me.btnChangePassword.Text = "Change Password"
        Me.btnChangePassword.UseVisualStyleBackColor = False
        '
        'frmAdminMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.BackgroundImage = Global.NoteBridge.My.Resources.Resources.logo_4
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(733, 458)
        Me.Controls.Add(Me.btnChangePassword)
        Me.Controls.Add(Me.btnAddNewStudent)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.btnPaymentReports)
        Me.Controls.Add(Me.btnPromissoryNoteSummary)
        Me.Controls.Add(Me.btnStudentPayments)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAdminMainMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "NoteBridge"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnStudentPayments As Button
    Friend WithEvents btnPromissoryNoteSummary As Button
    Friend WithEvents btnPaymentReports As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents btnAddNewStudent As Button
    Friend WithEvents btnChangePassword As Button
End Class
