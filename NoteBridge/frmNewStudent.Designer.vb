<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewStudent
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewStudent))
        Me.txtTotalAssessment = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtYearSection = New System.Windows.Forms.TextBox()
        Me.txtCourse = New System.Windows.Forms.TextBox()
        Me.txtStudentName = New System.Windows.Forms.TextBox()
        Me.txtStudentNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtUserType = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cmbSemester = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'txtTotalAssessment
        '
        Me.txtTotalAssessment.Location = New System.Drawing.Point(152, 162)
        Me.txtTotalAssessment.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTotalAssessment.Name = "txtTotalAssessment"
        Me.txtTotalAssessment.Size = New System.Drawing.Size(221, 22)
        Me.txtTotalAssessment.TabIndex = 26
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(11, 166)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(118, 16)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Total Assessment:"
        '
        'txtYearSection
        '
        Me.txtYearSection.Location = New System.Drawing.Point(152, 134)
        Me.txtYearSection.Margin = New System.Windows.Forms.Padding(4)
        Me.txtYearSection.Name = "txtYearSection"
        Me.txtYearSection.Size = New System.Drawing.Size(221, 22)
        Me.txtYearSection.TabIndex = 23
        '
        'txtCourse
        '
        Me.txtCourse.Location = New System.Drawing.Point(152, 106)
        Me.txtCourse.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCourse.Name = "txtCourse"
        Me.txtCourse.Size = New System.Drawing.Size(221, 22)
        Me.txtCourse.TabIndex = 22
        '
        'txtStudentName
        '
        Me.txtStudentName.Location = New System.Drawing.Point(152, 78)
        Me.txtStudentName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtStudentName.Name = "txtStudentName"
        Me.txtStudentName.Size = New System.Drawing.Size(221, 22)
        Me.txtStudentName.TabIndex = 21
        '
        'txtStudentNo
        '
        Me.txtStudentNo.Location = New System.Drawing.Point(152, 49)
        Me.txtStudentNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtStudentNo.Name = "txtStudentNo"
        Me.txtStudentNo.Size = New System.Drawing.Size(221, 22)
        Me.txtStudentNo.TabIndex = 20
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 138)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(113, 16)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Year and Section:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 110)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 16)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Course:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 81)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 16)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 53)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Student No.:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 10)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(239, 29)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Student Information"
        '
        'btnConfirm
        '
        Me.btnConfirm.BackColor = System.Drawing.Color.Green
        Me.btnConfirm.Location = New System.Drawing.Point(201, 362)
        Me.btnConfirm.Margin = New System.Windows.Forms.Padding(4)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(91, 28)
        Me.btnConfirm.TabIndex = 34
        Me.btnConfirm.Text = "Confirm"
        Me.btnConfirm.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 234)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(73, 16)
        Me.Label11.TabIndex = 28
        Me.Label11.Text = "Username:"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(152, 230)
        Me.txtUsername.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(221, 22)
        Me.txtUsername.TabIndex = 29
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 262)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(70, 16)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Password:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(152, 258)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(221, 22)
        Me.txtPassword.TabIndex = 31
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 290)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 16)
        Me.Label13.TabIndex = 32
        Me.Label13.Text = "User Type:"
        '
        'txtUserType
        '
        Me.txtUserType.Location = New System.Drawing.Point(152, 287)
        Me.txtUserType.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUserType.Name = "txtUserType"
        Me.txtUserType.ReadOnly = True
        Me.txtUserType.Size = New System.Drawing.Size(221, 22)
        Me.txtUserType.TabIndex = 33
        Me.txtUserType.TabStop = False
        Me.txtUserType.Text = "Student"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(11, 197)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(242, 29)
        Me.Label6.TabIndex = 35
        Me.Label6.Text = "Account Information"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Green
        Me.btnCancel.Location = New System.Drawing.Point(103, 362)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(91, 28)
        Me.btnCancel.TabIndex = 36
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'cmbSemester
        '
        Me.cmbSemester.FormattingEnabled = True
        Me.cmbSemester.Location = New System.Drawing.Point(12, 316)
        Me.cmbSemester.Name = "cmbSemester"
        Me.cmbSemester.Size = New System.Drawing.Size(361, 24)
        Me.cmbSemester.TabIndex = 37
        '
        'frmNewStudent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Green
        Me.ClientSize = New System.Drawing.Size(400, 423)
        Me.Controls.Add(Me.cmbSemester)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.txtUserType)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtTotalAssessment)
        Me.Controls.Add(Me.Label10)
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
        Me.Name = "frmNewStudent"
        Me.Text = "New Student"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtTotalAssessment As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtYearSection As TextBox
    Friend WithEvents txtCourse As TextBox
    Friend WithEvents txtStudentName As TextBox
    Friend WithEvents txtStudentNo As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnConfirm As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtUserType As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents cmbSemester As ComboBox
End Class
