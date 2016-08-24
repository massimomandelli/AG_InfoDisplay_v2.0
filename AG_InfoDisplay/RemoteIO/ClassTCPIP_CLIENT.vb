Imports System.Net
Imports System.Net.Sockets
Public Class ClassTCPIP_CLIENT
    '127.0.0.1
    Public Property RemotePort As Integer
    Public Property LocalPort As Integer
    Private f_start As Boolean = False
    Public WithEvents _Connection As ConnectionInfo
    Private _ServerAddress As IPAddress
    '################################ EVENTS ######################################
    Public Event Message_Rx(ByVal IdClient As Integer, ByVal theMessage As String, ByVal ConnectionInfo As ConnectionInfo)
    Public Event Connection_Open(ByVal ConnectionInfo As ConnectionInfo)
    Public Event Connection_Closed(ByVal ConnectionInfo As ConnectionInfo)
    Public Event Connection_Error(ByVal ConnectionInfo As ConnectionInfo)
    Const TERM As String = "@@@"
    Public Sub New()

    End Sub
    Public Sub New(ByVal RemoteServer As String, ByVal theRemotePort As Integer, ByVal theLocalPort As Integer)
        'MyClass.RemotePort = RemotePort
        ' LocalPort = theLocalPort
        RemotePort = theRemotePort
        Server(RemoteServer)
    End Sub


    Public Sub ToLog(ByVal LogMessage As String)
        Try
            Console.WriteLine(String.Format("{0} {1}", Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
        Catch ex As Exception
            ToLog("Error." + ex.Message)
        End Try
    End Sub
    'Private Sub ClientForm_Load(sender As System.Object, e As System.EventArgs)
    '    ValidateChildren()
    'End Sub






    Public Property ClientId As Integer = 0
    Public Sub Connect()
        Try
            If f_start = False Then
                If _Connection IsNot Nothing Then _Connection.Close()
                _Connection = Nothing

                If _ServerAddress IsNot Nothing Then
                    'ConnectButton.Text = "Disconnect"
                    'ConnectButton.Image = My.Resources.Disconnect
                    Try
                        _Connection = New ConnectionInfo(_ServerAddress, RemotePort, AddressOf InvokeAppendOutput)
                        _Connection.AwaitData()
                        f_start = True

                        RaiseEvent Connection_Open(_Connection)
                    Catch ex As Exception
                        ToLog(ex.Message + " Error Connecting to Server")
                        f_start = False
                    End Try
                Else
                    ToLog("Invalid server name or address." + " Cannot Connect to Server")
                    f_start = False
                End If
            Else
                'ConnectButton.Text = "Connect"
                'ConnectButton.Image = My.Resources.Connect
                If _Connection IsNot Nothing Then _Connection.Close()
                _Connection = Nothing
                f_start = False
                RaiseEvent Connection_Closed(_Connection)
            End If
        Catch ex As Exception
            ToLog("Error." + ex.Message)
        End Try

    End Sub


    Public Sub END_MESSAGE()
        SendMessage("END:END@@@")
    End Sub
    Public Sub Disconnect()
        Try




            If _Connection IsNot Nothing Then
                _Connection.Close()
            End If

            _Connection = Nothing
            f_start = False

            RaiseEvent Connection_Closed(_Connection)
        Catch ex As Exception
            ToLog("Error." + ex.Message)
        End Try

    End Sub
    Private Sub CheckConnection()
        'If _Connection Is Nothing Then
        '    Connect()
        'End If
        'If _Connection.Client.Connected = False Then
        '    Connect()
        'End If
    End Sub

    Public Sub SendMessage(ByVal theMessage As String)
        Try
            If theMessage = "" Then Exit Sub
            ' _Connection.Stream.WriteTimeout = 50
            If _Connection IsNot Nothing AndAlso _Connection.Client.Connected AndAlso _Connection.Stream IsNot Nothing Then
                Dim buffer() As Byte = System.Text.Encoding.ASCII.GetBytes(theMessage)
                '_Connection.Stream.Write(buffer, 0, buffer.Length)

                '  Await()
                _Connection.Stream.WriteAsync(buffer, 0, buffer.Length)
            Else
                f_start = False
                Connect()
            End If
        Catch ex As Exception
            ToLog("Error." + ex.Message)
            f_start = False
            Connect()
        End Try

    End Sub

    Private Sub Server(ByVal Host As String)
        Try

            _ServerAddress = IPAddress.Parse(Host)
            Exit Sub


            _ServerAddress = Nothing
            Dim remoteHost As IPHostEntry = Dns.GetHostEntry(Host)
            If remoteHost IsNot Nothing AndAlso remoteHost.AddressList.Length > 0 Then
                For Each deltaAddress As IPAddress In remoteHost.AddressList
                    If deltaAddress.AddressFamily = AddressFamily.InterNetwork Then
                        _ServerAddress = deltaAddress
                        Exit For
                    End If
                Next
            End If
            If _ServerAddress Is Nothing Then
                ToLog("Cannot resolove server address." + "Invalid Server")
            End If
        Catch ex As Exception
            ToLog("Error." + ex.Message)
        End Try
    End Sub

    'Private Sub SetPort(ByVal thePort As Integer)
    '    Dim deltaPort As Integer
    '    If Not Integer.TryParse(PortTextBox.Text, deltaPort) OrElse deltaPort < 1 OrElse deltaPort > 65535 Then
    '        ToLog("Port number must be an integer between 1 and 65535." + " Invalid Port Number")

    '    End If
    'End Sub

    'The InvokeAppendOutput method could easily be replaced with a lambda method passed 
    'to the ConnectionInfo contstructor in the ConnectButton_CheckChanged event handler 
    Private Sub InvokeAppendOutput(message As String)
        Try
            Dim doAppendOutput As New Action(Of String)(AddressOf AppendOutput)
            doAppendOutput(message)
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try

        'Me.Invoke(doAppendOutput, message)
    End Sub

    Public _Message As String = String.Empty
    Private Sub AppendOutput(message As String)
        Try
            'If _Message.Length > 0 Then
            '_Message = _Message + message + vbCrLf
            'message.AppendText(ControlChars.NewLine)
            'End If
            'message.AppendText(message)
            ' message.ScrollToCaret()
            ToLog(message)
            ' RaiseEvent Message_Rx(ClientId, message)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub _Connection_Connection_Closed() Handles _Connection.Connection_Closed
        RaiseEvent Connection_Closed(_Connection)
    End Sub

    Private Sub _Connection_Connection_Error() Handles _Connection.Connection_Error
        ToLog("Connection error....retry connection")
        f_start = False
        RaiseEvent Connection_Error(_Connection)
        Do Until f_start
            Connect()
            Threading.Thread.Sleep(5000)
        Loop
    End Sub



    Private Sub _Connection_Message_Rx(IdClient As Integer, theMessage As String, ConnectionInfo As ConnectionInfo) Handles _Connection.Message_Rx
        RaiseEvent Message_Rx(IdClient, theMessage, ConnectionInfo)
    End Sub

    'Private Sub ClassTCPIP_CLIENT_Connection_Closed(ConnectionInfo As ConnectionInfo) Handles Me.Connection_Closed
    '    ' RaiseEvent Connection_Closed(_Connection)
    'End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    'Private Sub ClassTCPIP_CLIENT_Message_Rx(IdClient As Integer, theMessage As String, ConnectionInfo As ConnectionInfo) Handles Me.Message_Rx
    '    Debug.Print("")

    '    'If theMessage = "END" Then
    '    '    RaiseEvent Connection_Closed(_Connection)
    '    'End If

    'End Sub
End Class
'Encapuslates the client connection and provides a state object for async read operations 
Public Class ConnectionInfo
    Const TERM As String = "@@@"
    Public Event Message_Rx(ByVal IdClient As Integer, ByVal theMessage As String, ByVal ConnectionInfo As ConnectionInfo)
    Public Event Connection_Open()
    Public Event Connection_Closed()
    Public Event Connection_Error()


    Private _AppendMethod As Action(Of String)
    Public ReadOnly Property AppendMethod As Action(Of String)
        Get
            Return _AppendMethod
        End Get
    End Property

    Private _Client As TcpClient
    Public ReadOnly Property Client As TcpClient
        Get
            Return _Client
        End Get
    End Property

    Private _Stream As NetworkStream
    Public ReadOnly Property Stream As NetworkStream
        Get
            Return _Stream
        End Get
    End Property

    Private _LastReadLength As Integer
    Public ReadOnly Property LastReadLength As Integer
        Get
            Return _LastReadLength
        End Get
    End Property

    Private _Buffer(2000) As Byte
    '   Private _Buffer(1024) As Byte

    Public Sub New(address As IPAddress, port As Integer, append As Action(Of String))
        _AppendMethod = append
        _Client = New TcpClient
        _Client.Connect(address, port)
        _Stream = _Client.GetStream
    End Sub

    Public Sub AwaitData()
        Try
            _Stream.BeginRead(_Buffer, 0, _Buffer.Length, AddressOf DoReadData, Me)
        Catch ex As Exception
            Debug.Print("")
            RaiseEvent Connection_Error()
        End Try

    End Sub

    Public Sub Close()
        Try
            If _Client IsNot Nothing Then
                ''''_Client.EndConnect  
                'Dim IARes As System.IAsyncResult
                '_Client.EndConnect(IARes)
                _Client.Client.Close()
                _Stream.Close()
                _Client.Close()
            End If

            _Client = Nothing
            _Stream = Nothing
            RaiseEvent Connection_Closed()
        Catch ex As Exception
            Debug.Print("")
        End Try

    End Sub
    Private Sub SendMessage(message As String)
        Try
            If _Stream IsNot Nothing Then
                Dim messageData() As Byte = System.Text.Encoding.ASCII.GetBytes(message)
                Stream.Write(messageData, 0, messageData.Length)
            End If
        Catch ex As Exception
            Debug.Print("")
        End Try

    End Sub



    Private _BufferMessage As String = String.Empty
    Private Sub DoReadData(result As IAsyncResult)
        Dim info As ConnectionInfo = CType(result.AsyncState, ConnectionInfo)
        Try
            If info._Stream IsNot Nothing AndAlso info._Stream.CanRead Then
                info._LastReadLength = info._Stream.EndRead(result)
                If info._LastReadLength > 0 Then
                    Dim message As String = _BufferMessage + System.Text.Encoding.ASCII.GetString(info._Buffer, 0, info._LastReadLength)

                    'Select Case message
                    '    Case "END"
                    '        info.SendMessage(message + " OK")
                    '    Case "INFO"
                    '        info.SendMessage(message + " OK")
                    'End Select

                    Dim s As String() = message.Split(TERM)
                    Try
                        If s(s.Length - 1) <> "" Then
                            Debug.Print("")
                            _BufferMessage = s(s.Length - 1)
                            s(s.Length - 1) = ""
                        Else
                            _BufferMessage = ""
                        End If
                    Catch ex As Exception
                        Debug.Print("")
                    End Try

                    For Each sStr As String In s
                        If sStr <> "" Then
                            info._AppendMethod(sStr)
                            RaiseEvent Message_Rx(1, sStr, info)
                        End If
                    Next


                End If
                info.AwaitData()
            End If
        Catch ex As Exception
            info._LastReadLength = -1
            info._AppendMethod(ex.Message)
            info.Close() 'm.m.
            Console.WriteLine(ex.Message)
        End Try
    End Sub


End Class
