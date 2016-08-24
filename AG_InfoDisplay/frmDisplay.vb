Imports System.ComponentModel
Imports System.Text
Imports System.Speech.Recognition
Imports System.Speech.Synthesis
Imports System.Speech

Imports System.Globalization

Imports System.Threading
Imports AG_InfoDisplay

Partial Public Class frmDisplay
    Inherits DevExpress.XtraEditors.XtraForm
    Dim WithEvents CLI_InfoDisplay As ClassTCPIP_CLIENT
    Delegate Sub RefreshObjDelegate(ByVal theMessage As String)
    Dim RefreshObjMarshaler As RefreshObjDelegate = AddressOf Me.RefreshObj


    Private Engine As New SpeechRecognitionEngine
    '   Private Synt As New SpeechSynthesizer


    Public synth As New Speech.Synthesis.SpeechSynthesizer
    Public WithEvents recognizer As New Speech.Recognition.SpeechRecognitionEngine
    Dim gram As New System.Speech.Recognition.DictationGrammar()

    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
    End Sub
    Public Sub New()
        InitializeComponent()


        Dim Dt As New DataTable

        ' Dim newrow As New DataRow

        Baia_1.Init(1, 0)
        Baia_2.Init(2, 0)
        Baia_3.Init(3, 0)
        Baia_4.Init(4, 0)
        Baia_5.Init(5, 0)
        Baia_6.Init(6, 0)
        Baia_7.Init(7, 0)
        Baia_8.Init(8, 0)


        Baia_2.Init(2, 0)
        Baia_3.Init(3, 0)
        Baia_4.Init(4, 0)
        Baia_5.Init(5, 0)
        Baia_6.Init(6, 0)
        Baia_7.Init(7, 0)
        Baia_8.Init(8, 0)


        Baia_5.Alarm_on()
        Baia_5.Init(5, 40000)
        Baia_5.SetMessage("Corsia libera", True)
        Baia_5.SetValue(23500)


        Baia_2.CorsiaOutOfService(True)
        '   DataSet1.Tables("info").NewRow()

        test()
        Timer1.Enabled = True


        CLI_InfoDisplay = New ClassTCPIP_CLIENT(My.Settings.ServerIp, My.Settings.ServerPort, 0)

        CLI_InfoDisplay.Connect()



    End Sub

    Private Sub RefreshObj(ByVal theMessage As String)
        Try

        Catch ex As Exception

        End Try
    End Sub


    Dim mytext As String = "corsia uno libera"
    Dim f_synthAvailable As Boolean = False
    Private Sub test()
        '   recognizer.LoadGrammar(gram)
        '  recognizer.SetInputToDefaultAudioDevice()
        ' recognizer.RecognizeAsync()

        PlayBackgroundSoundResource()
        PlayBackgroundSoundResource()
        PlayBackgroundSoundResource()


        '  Dim Synt As New SpeechSynthesizer
        For Each voce As InstalledVoice In synth.GetInstalledVoices
            Debug.WriteLine(voce.VoiceInfo.Name.ToString)
        Next
        '  synth.SelectVoice("Microsoft Anna")
        '  synth.Speak("badge ")
        Try
            synth.SelectVoice("ScanSoft Silvia_Dri40_16kHz")
            synth.Rate = -3

            synth.Speak("bedge 45 targa AZ1234TR pronto per caricare alla corsia 8")
            synth.Rate = 0
            synth.Speak("marco, muovi il culo")
            f_synthAvailable = True
        Catch ex As Exception
            MsgBox("Error in Synth system: " + ex.Message)
            f_synthAvailable = False
        End Try


    End Sub



    Public Sub GotSpeech(ByVal sender As Object, ByVal phrase As System.Speech.Recognition.SpeechRecognizedEventArgs) Handles recognizer.SpeechRecognized
        '  words.Text += phrase.Result.Text & vbNewLine
        '  mytext
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Sub PlayBackgroundSoundResource()
        My.Computer.Audio.Play(My.Resources.chord,
            AudioPlayMode.Background)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Me.lblData.Text = Format(Now, "HH:mm dd/MM/yyyy")
        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            CLI_InfoDisplay.END_MESSAGE()
            CLI_InfoDisplay.Disconnect()
            Timer1.Enabled = False
        Catch ex As Exception

        End Try

        MyBase.Finalize()
    End Sub

    Private Sub CLI_InfoDisplay_Connection_Closed(ConnectionInfo As ConnectionInfo) Handles CLI_InfoDisplay.Connection_Closed

    End Sub

    Private Sub CLI_InfoDisplay_Connection_Error(ConnectionInfo As ConnectionInfo) Handles CLI_InfoDisplay.Connection_Error

    End Sub

    Private Sub CLI_InfoDisplay_Connection_Open(ConnectionInfo As ConnectionInfo) Handles CLI_InfoDisplay.Connection_Open

    End Sub

    Private Sub CLI_InfoDisplay_Message_Rx(IdClient As Integer, theMessage As String, ConnectionInfo As ConnectionInfo) Handles CLI_InfoDisplay.Message_Rx
        Try
            ' LED_Activity.BackColor = c_Waiting_color
            Dim args() As Object = {theMessage}
            MyBase.Invoke(RefreshObjMarshaler, args)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Sub PanelControl1_Paint(sender As Object, e As PaintEventArgs) Handles PanelControl1.Paint

    End Sub
End Class
