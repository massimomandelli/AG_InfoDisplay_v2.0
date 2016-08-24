Partial Public Class frmDisplay
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.lblData = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Baia_8 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_7 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_6 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_5 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_4 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_3 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_2 = New AG_InfoDisplay.UC_Baia()
        Me.Baia_1 = New AG_InfoDisplay.UC_Baia()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.lblData)
        Me.PanelControl1.Controls.Add(Me.lblTitle)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1884, 125)
        Me.PanelControl1.TabIndex = 0
        '
        'lblData
        '
        Me.lblData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblData.AutoSize = True
        Me.lblData.Font = New System.Drawing.Font("Tahoma", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData.Location = New System.Drawing.Point(1328, 9)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(544, 77)
        Me.lblData.TabIndex = 1
        Me.lblData.Text = "00:00 00/00/0000"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(658, 58)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Monitor sala attesa autisti"
        '
        'Baia_8
        '
        Me.Baia_8.Autista = ""
        Me.Baia_8.BackColor = System.Drawing.Color.White
        Me.Baia_8.Badge = ""
        Me.Baia_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_8.Def_backColor = System.Drawing.Color.White
        Me.Baia_8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_8.Id = 0
        Me.Baia_8.Location = New System.Drawing.Point(0, 895)
        Me.Baia_8.Message = ""
        Me.Baia_8.Name = "Baia_8"
        Me.Baia_8.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_8.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_8.Status = CType(0, Short)
        Me.Baia_8.TabIndex = 8
        Me.Baia_8.Targa = ""
        Me.Baia_8.TimerShowAlarm = CType(30, Short)
        Me.Baia_8.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_8.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_8.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_7
        '
        Me.Baia_7.Autista = ""
        Me.Baia_7.BackColor = System.Drawing.Color.White
        Me.Baia_7.Badge = ""
        Me.Baia_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_7.Def_backColor = System.Drawing.Color.White
        Me.Baia_7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_7.Id = 0
        Me.Baia_7.Location = New System.Drawing.Point(0, 785)
        Me.Baia_7.Message = ""
        Me.Baia_7.Name = "Baia_7"
        Me.Baia_7.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_7.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_7.Status = CType(0, Short)
        Me.Baia_7.TabIndex = 7
        Me.Baia_7.Targa = ""
        Me.Baia_7.TimerShowAlarm = CType(30, Short)
        Me.Baia_7.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_7.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_7.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_6
        '
        Me.Baia_6.Autista = ""
        Me.Baia_6.BackColor = System.Drawing.Color.White
        Me.Baia_6.Badge = ""
        Me.Baia_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_6.Def_backColor = System.Drawing.Color.White
        Me.Baia_6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_6.Id = 0
        Me.Baia_6.Location = New System.Drawing.Point(0, 675)
        Me.Baia_6.Message = ""
        Me.Baia_6.Name = "Baia_6"
        Me.Baia_6.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_6.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_6.Status = CType(0, Short)
        Me.Baia_6.TabIndex = 6
        Me.Baia_6.Targa = ""
        Me.Baia_6.TimerShowAlarm = CType(30, Short)
        Me.Baia_6.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_6.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_6.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_5
        '
        Me.Baia_5.Autista = ""
        Me.Baia_5.BackColor = System.Drawing.Color.White
        Me.Baia_5.Badge = ""
        Me.Baia_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_5.Def_backColor = System.Drawing.Color.White
        Me.Baia_5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_5.Id = 0
        Me.Baia_5.Location = New System.Drawing.Point(0, 565)
        Me.Baia_5.Message = ""
        Me.Baia_5.Name = "Baia_5"
        Me.Baia_5.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_5.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_5.Status = CType(0, Short)
        Me.Baia_5.TabIndex = 5
        Me.Baia_5.Targa = ""
        Me.Baia_5.TimerShowAlarm = CType(30, Short)
        Me.Baia_5.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_5.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_5.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_4
        '
        Me.Baia_4.Autista = ""
        Me.Baia_4.BackColor = System.Drawing.Color.White
        Me.Baia_4.Badge = ""
        Me.Baia_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_4.Def_backColor = System.Drawing.Color.White
        Me.Baia_4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_4.Id = 0
        Me.Baia_4.Location = New System.Drawing.Point(0, 455)
        Me.Baia_4.Message = ""
        Me.Baia_4.Name = "Baia_4"
        Me.Baia_4.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_4.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_4.Status = CType(0, Short)
        Me.Baia_4.TabIndex = 4
        Me.Baia_4.Targa = ""
        Me.Baia_4.TimerShowAlarm = CType(30, Short)
        Me.Baia_4.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_4.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_4.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_3
        '
        Me.Baia_3.Autista = ""
        Me.Baia_3.BackColor = System.Drawing.Color.White
        Me.Baia_3.Badge = ""
        Me.Baia_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_3.Def_backColor = System.Drawing.Color.White
        Me.Baia_3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_3.Id = 0
        Me.Baia_3.Location = New System.Drawing.Point(0, 345)
        Me.Baia_3.Message = ""
        Me.Baia_3.Name = "Baia_3"
        Me.Baia_3.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_3.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_3.Status = CType(0, Short)
        Me.Baia_3.TabIndex = 3
        Me.Baia_3.Targa = ""
        Me.Baia_3.TimerShowAlarm = CType(30, Short)
        Me.Baia_3.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_3.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_3.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_2
        '
        Me.Baia_2.Autista = ""
        Me.Baia_2.BackColor = System.Drawing.Color.White
        Me.Baia_2.Badge = ""
        Me.Baia_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_2.Def_backColor = System.Drawing.Color.White
        Me.Baia_2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_2.Id = 0
        Me.Baia_2.Location = New System.Drawing.Point(0, 235)
        Me.Baia_2.Message = ""
        Me.Baia_2.Name = "Baia_2"
        Me.Baia_2.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_2.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_2.Status = CType(0, Short)
        Me.Baia_2.TabIndex = 2
        Me.Baia_2.Targa = ""
        Me.Baia_2.TimerShowAlarm = CType(30, Short)
        Me.Baia_2.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_2.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_2.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'Baia_1
        '
        Me.Baia_1.Autista = ""
        Me.Baia_1.BackColor = System.Drawing.Color.White
        Me.Baia_1.Badge = ""
        Me.Baia_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Baia_1.Def_backColor = System.Drawing.Color.White
        Me.Baia_1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Baia_1.Id = 0
        Me.Baia_1.Location = New System.Drawing.Point(0, 125)
        Me.Baia_1.Message = ""
        Me.Baia_1.Name = "Baia_1"
        Me.Baia_1.SetPoint = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_1.Size = New System.Drawing.Size(1884, 110)
        Me.Baia_1.Status = CType(0, Short)
        Me.Baia_1.TabIndex = 1
        Me.Baia_1.Targa = ""
        Me.Baia_1.TimerShowAlarm = CType(30, Short)
        Me.Baia_1.Trigger = New Decimal(New Integer() {90, 0, 0, 0})
        Me.Baia_1.ValuePerc = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Baia_1.Weight = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'frmDisplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1884, 1042)
        Me.Controls.Add(Me.Baia_8)
        Me.Controls.Add(Me.Baia_7)
        Me.Controls.Add(Me.Baia_6)
        Me.Controls.Add(Me.Baia_5)
        Me.Controls.Add(Me.Baia_4)
        Me.Controls.Add(Me.Baia_3)
        Me.Controls.Add(Me.Baia_2)
        Me.Controls.Add(Me.Baia_1)
        Me.Controls.Add(Me.PanelControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmDisplay"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub


    Friend WithEvents Timer1 As Timer
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents Baia_1 As UC_Baia
    Friend WithEvents Baia_2 As UC_Baia
    Friend WithEvents Baia_3 As UC_Baia
    Friend WithEvents Baia_4 As UC_Baia
    Friend WithEvents Baia_5 As UC_Baia
    Friend WithEvents Baia_6 As UC_Baia
    Friend WithEvents Baia_7 As UC_Baia
    Friend WithEvents Baia_8 As UC_Baia
    Friend WithEvents lblData As Label
    Friend WithEvents lblTitle As Label

#End Region

End Class
