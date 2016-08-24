Public Class UC_Baia
    Public Property Id As Integer

    Public Property SetPoint As Decimal
    Public Property Weight As Decimal

    Public Property Trigger As Decimal = 90
    Public Property Status As Int16 = 0
    Public Property Badge As String = ""
    Public Property Targa As String = ""
    Public Property Autista As String = ""
    Public Property Message As String = ""

    Public Property ValuePerc As Decimal

    Public Event EventoBaia(ByVal Id As Integer, ByVal EventId As Integer, ByVal Message As String)

    Public Property TimerShowAlarm As Int16 = 30
    Public Property Song As IO.UnmanagedMemoryStream = My.Resources.ding
    Public Sub Init(ByVal theId As Integer, ByVal theSetPoint As Decimal)
        Id = theId
        lbl1.Text = "BAIA " + theId.ToString
        SetPoint = theSetPoint
        ' SetValue(10)
        CorsiaLibera(True)
    End Sub

    Private f_CorsiaLibera As Boolean
    Private f_outofservice As Boolean
    Public Sub CorsiaLibera(ByVal state As Boolean)
        Try
            lblCorsiaInfo.BackColor = Color.DeepSkyBlue
            lblCorsiaInfo.Text = "Corsia libera"
            f_CorsiaLibera = state
            If state Then
                ProgressBarControl1.Hide()
                lblCorsiaInfo.Show()


            Else
                ProgressBarControl1.Show()
                lblCorsiaInfo.Hide()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub CorsiaOutOfService(ByVal state As Boolean)
        Try
            A.Image = AG_InfoDisplay.My.Resources.Resources.fuori_servizio
            B.Image = AG_InfoDisplay.My.Resources.Resources.danger_bd9

            lblCorsiaInfo.BackColor = Color.Red
            lblCorsiaInfo.Text = "Corsia fuori servizio"

            f_outofservice = state
            If state Then
                ProgressBarControl1.Hide()
                lblCorsiaInfo.Show()
                A.Show()
                B.Show()

            Else
                ProgressBarControl1.Show()
                lblCorsiaInfo.Hide()
                A.Hide()
                B.Hide()

            End If
        Catch ex As Exception

        End Try
    End Sub



    Public Sub SetValue(ByVal Value As Decimal)
        Try
            If f_outofservice Then Exit Sub

            If Value > 0 Then
                If f_CorsiaLibera Then
                    CorsiaLibera(False)
                End If
            End If
            Weight = Value
            ValuePerc = Int((Value * 100) / SetPoint)
            Me.ProgressBarControl1.EditValue = ValuePerc
            '   Me.ProgressBarControl1.Text = ValuePerc.ToString + "%"
        Catch ex As Exception
            Me.ProgressBarControl1.EditValue = 0

        End Try
    End Sub

    Public Property Def_backColor As Color = Color.White

    Public Sub SetMessage(ByVal theMessage As String, Optional ByVal Reverse As Boolean = False)

        Try
            If Reverse Then
                '   Me.lblMessage.BackColor = Color.Red
                '    Me.lblMessage.ForeColor = Color.Red
            End If
            Me.lblMessage.Text = theMessage
        Catch ex As Exception

        End Try
    End Sub


    Private AlarmStart As Date
    Private f_flipflop As Boolean = False
    Private Dim _theBackColor As Color
    Public Sub Alarm_on()
        A.Image = AG_InfoDisplay.My.Resources.Resources.red
        B.Image = AG_InfoDisplay.My.Resources.Resources.verde
        A.Show()
        B.Show()

        AlarmStart = Now
        Timer1.Enabled = True
    End Sub

    Public Sub Alarm_off()
        Timer1.Enabled = False
        A.Hide()
        B.Hide()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If DateDiff(DateInterval.Second, AlarmStart, Now) > TimerShowAlarm Then
            Alarm_off()
        End If


        If f_flipflop Then
            'StateIndicatorGauge1
            'StateIndicatorComponent1.Shapes. 
            A.Image = AG_InfoDisplay.My.Resources.Resources.red
            B.Image = AG_InfoDisplay.My.Resources.Resources.verde
            '   PlayBackgroundSoundResource()

        Else
            B.Image = AG_InfoDisplay.My.Resources.Resources.red
            A.Image = AG_InfoDisplay.My.Resources.Resources.verde
        End If
        f_flipflop = Not f_flipflop
    End Sub

    Sub PlayBackgroundSoundResource()
        My.Computer.Audio.Play(My.Resources.Windows_Ding, AudioPlayMode.Background)
    End Sub
End Class
