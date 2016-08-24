<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UC_Baia
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lbl1 = New DevExpress.XtraEditors.LabelControl()
        Me.ProgressBarControl1 = New DevExpress.XtraEditors.ProgressBarControl()
        Me.lblMessage = New DevExpress.XtraEditors.LabelControl()
        Me.lblCorsiaInfo = New DevExpress.XtraEditors.LabelControl()
        Me.A = New DevExpress.XtraEditors.PictureEdit()
        Me.B = New DevExpress.XtraEditors.PictureEdit()
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.A.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.B.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'lbl1
        '
        Me.lbl1.Appearance.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.Location = New System.Drawing.Point(10, 16)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(113, 39)
        Me.lbl1.TabIndex = 2
        Me.lbl1.Text = "BAIA X"
        '
        'ProgressBarControl1
        '
        Me.ProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarControl1.EditValue = "50"
        Me.ProgressBarControl1.Location = New System.Drawing.Point(151, 12)
        Me.ProgressBarControl1.Name = "ProgressBarControl1"
        Me.ProgressBarControl1.Properties.Appearance.BackColor = System.Drawing.Color.DimGray
        Me.ProgressBarControl1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressBarControl1.Properties.Appearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.ProgressBarControl1.Properties.EndColor = System.Drawing.Color.Lime
        Me.ProgressBarControl1.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003
        Me.ProgressBarControl1.Properties.LookAndFeel.UseDefaultLookAndFeel = False
        Me.ProgressBarControl1.Properties.ShowTitle = True
        Me.ProgressBarControl1.Properties.Step = 1
        Me.ProgressBarControl1.ShowProgressInTaskBar = True
        Me.ProgressBarControl1.Size = New System.Drawing.Size(1113, 47)
        Me.ProgressBarControl1.TabIndex = 3
        '
        'lblMessage
        '
        Me.lblMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMessage.Appearance.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.lblMessage.Location = New System.Drawing.Point(151, 71)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(1113, 29)
        Me.lblMessage.TabIndex = 7
        '
        'lblCorsiaInfo
        '
        Me.lblCorsiaInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCorsiaInfo.Appearance.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.lblCorsiaInfo.Appearance.Font = New System.Drawing.Font("Tahoma", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCorsiaInfo.Appearance.ForeColor = System.Drawing.Color.White
        Me.lblCorsiaInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblCorsiaInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.lblCorsiaInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.lblCorsiaInfo.Location = New System.Drawing.Point(151, 12)
        Me.lblCorsiaInfo.Name = "lblCorsiaInfo"
        Me.lblCorsiaInfo.Size = New System.Drawing.Size(1113, 47)
        Me.lblCorsiaInfo.TabIndex = 12
        Me.lblCorsiaInfo.Text = "CORSIA LIBERA"
        '
        'A
        '
        Me.A.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.A.EditValue = Global.AG_InfoDisplay.My.Resources.Resources.verde
        Me.A.Location = New System.Drawing.Point(1306, 7)
        Me.A.Name = "A"
        Me.A.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.A.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.A.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.A.Size = New System.Drawing.Size(100, 96)
        Me.A.TabIndex = 11
        Me.A.Visible = False
        '
        'B
        '
        Me.B.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.B.EditValue = Global.AG_InfoDisplay.My.Resources.Resources.red
        Me.B.Location = New System.Drawing.Point(1412, 7)
        Me.B.Name = "B"
        Me.B.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.B.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.B.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.B.Size = New System.Drawing.Size(100, 96)
        Me.B.TabIndex = 10
        Me.B.Visible = False
        '
        'UC_Baia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.A)
        Me.Controls.Add(Me.B)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.ProgressBarControl1)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.lblCorsiaInfo)
        Me.Name = "UC_Baia"
        Me.Size = New System.Drawing.Size(1522, 110)
        CType(Me.ProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.A.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.B.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lbl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ProgressBarControl1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents lblMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents B As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents A As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lblCorsiaInfo As DevExpress.XtraEditors.LabelControl
End Class
