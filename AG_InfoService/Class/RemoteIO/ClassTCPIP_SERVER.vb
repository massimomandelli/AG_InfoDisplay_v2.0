Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Threading.Thread
Imports System.Threading.Tasks


Public Class ClassTCPIP_SERVER
    Implements IDisposable
    Const TERM As String = "@@@"
    Const PREFIX As String = "$$$"
    Public _Listener As TcpListener
    Public _Connections As New List(Of ConnectionInfo)
    Private _ConnectionMontior As Task
    Public Property LocalPort As Integer

    Private f_start As Boolean = False
    Public Property ConnectionInfo As ConnectionInfo
    Public Property Device As String = String.Empty
    Public Property Status As Integer = 0
    '################################ EVENTS ######################################
    Public Event Message_Rx(ByVal IdClient As Integer, ByVal theMessageId As String, ByVal theMessage As Object, ByVal ConnectionInfo As ConnectionInfo)
    Public Event Message_Rxs(ByVal IdClient As Integer, ByVal theMessageId() As String, ByVal theMessage As Object, ByVal ConnectionInfo As ConnectionInfo)
    Public Event Connection_Open(ByVal IdClient As Integer)
    Public Event Connection_Closed(ByVal IdClient As Integer)


    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            For Each x As ConnectionInfo In _Connections
                x._Close()
            Next

        Catch ex As Exception
            ToLog(ex.Message)
        End Try
        Try
            _Listener.Stop()

            _Listener = Nothing
        Catch ex As Exception
            ToLog(ex.Message)
        End Try

        Try
            If _ConnectionMontior IsNot Nothing Then
                _ConnectionMontior.Dispose()
                _ConnectionMontior = Nothing
            End If
        Catch ex As Exception
            ToLog(ex.Message)
        End Try

    End Sub
    Public Sub New(ByVal theLocalPort As Integer)
        Me.LocalPort = theLocalPort
    End Sub

    Public Function SendMessage(ByVal theMessage As String)
        Try

            ConnectionInfo.SendMessage(theMessage)

        Catch ex As Exception
            ToLog(ex.Message)
        End Try
    End Function
    '    Private Sub WriteMessage(ByRef stream As System.Net.Sockets.NetworkStream, ByVal message As String)
    '        Try
    '            '{System.Net.Sockets.NetworkStream}
    '            Dim msg As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
    '            stream.Write(msg, 0, msg.Length)
    '            ' Console.WriteLine(message)

    '#If DEBUG Then
    '            '   tolog("-> msg " & message)
    '#End If
    '        Catch ex As Exception
    '            Console.WriteLine("WriteMessage error: " & ex.Message)
    '        End Try

    '    End Sub
    Public Sub Start()
        Try


            If f_start = False Then
                'StartStopButton.Text = "Stop"
                'StartStopButton.Image = My.Resources.Resources.StopServer

                _Listener = New TcpListener(IPAddress.Any, LocalPort)
                _Listener.Start()

                Dim monitor As New MonitorInfo(_Listener, _Connections)
                ListenForClient(monitor)
                '_ConnectionMontior = Task.Factory.StartNew(AddressOf DoMonitorConnections, monitor, TaskCreationOptions.None)
                _ConnectionMontior = Task.Factory.StartNew(AddressOf DoMonitorConnections, monitor, TaskCreationOptions.LongRunning)
                f_start = True
            Else
                'StartStopButton.Text = "Start"
                'StartStopButton.Image = My.Resources.Resources.StartServer
                CType(_ConnectionMontior.AsyncState, MonitorInfo).Cancel = True
                _Listener.Stop()
                _Listener = Nothing
                f_start = False
            End If

        Catch ex As Exception
            ToLog(ex.Message)
        End Try
    End Sub
    Public Sub StopConnection()

        Try
            CType(_ConnectionMontior.AsyncState, MonitorInfo).Cancel = True
            _Listener.Stop()
            _Listener = Nothing
            f_start = False
        Catch ex As Exception
            ToLog(ex.Message)
        End Try
    End Sub
    'Private Sub PortTextBox_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles PortTextBox.Validating
    '    Dim deltaPort As Integer
    '    If Not Integer.TryParse(PortTextBox.Text, deltaPort) OrElse deltaPort < 1 OrElse deltaPort > 65535 Then
    '        MessageBox.Show("Port number must be an integer between 1 and 65535.", "Invalid Port Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        PortTextBox.SelectAll()
    '        e.Cancel = True
    '    End If
    'End Sub

    Private Sub ListenForClient(monitor As MonitorInfo)
        Try

            Dim info As New ConnectionInfo(monitor)
            _Listener.BeginAcceptTcpClient(AddressOf DoAcceptClient, info)
        Catch ex As Exception
            ToLog(ex.Message)
        End Try

    End Sub

    Private Sub DoAcceptClient(result As IAsyncResult)
        Try
            Dim monitorInfo As MonitorInfo = CType(_ConnectionMontior.AsyncState, MonitorInfo)
            If monitorInfo.Listener IsNot Nothing AndAlso Not monitorInfo.Cancel Then
                Dim info As ConnectionInfo = CType(result.AsyncState, ConnectionInfo)
                monitorInfo.Connections.Add(info)
                info.AcceptClient(result)
                ListenForClient(monitorInfo)
                info.AwaitData()
                Dim doUpdateConnectionCountLabel As New Action(AddressOf UpdateConnectionCountLabel)
                'Invoke(doUpdateConnectionCountLabel)
                doUpdateConnectionCountLabel()
            End If
        Catch ex As Exception
            ToLog(ex.Message)
        End Try

    End Sub


    Private _BufferMessage As String = String.Empty


    Public DoMonitorConnectionsStatus As Integer = 0
    Public DoMonitorConnectionsIsAlive As Integer = 0
    Private Sub DoMonitorConnections()
        Try


            'Create delegate for updating output display 
            ' Dim doAppendOutput As New Action(Of String)(AddressOf AppendOutput)
            'Create delegate for updating connection count label 
            Dim doUpdateConnectionCountLabel As New Action(AddressOf UpdateConnectionCountLabel)

            'Get MonitorInfo instance from thread-save Task instance 
            Dim monitorInfo As MonitorInfo = CType(_ConnectionMontior.AsyncState, MonitorInfo)

            'Report progress 
            'Me.Invoke(doAppendOutput, "Monitor Started.")
            'doAppendOutput("Monitor Started.")

            'Implement client connection processing loop 
            DoMonitorConnectionsStatus = 1
            Do
                DoMonitorConnectionsIsAlive = 1
                'Create temporary list for recording closed connections 
                Dim lostCount As Integer = 0
                'Examine each connection for processing 

                ''Try
                ''    For index As Integer = monitorInfo.Connections.Count - 1 To 0 Step -1
                ''        Try
                ''            Dim info As ConnectionInfo = monitorInfo.Connections(index)
                ''            If DateDiff(DateInterval.Second, info.LastMessageTime, Now) > 10 Then
                ''                monitorInfo.Connections(index)._Close()
                ''            End If
                ''        Catch ex As Exception
                ''            ToLog(ex.Message)
                ''            DoMonitorConnectionsStatus = 0
                ''        End Try

                ''    Next
                ''Catch ex As Exception
                ''    ToLog(ex.Message)
                ''End Try




                For index As Integer = monitorInfo.Connections.Count - 1 To 0 Step -1
                    Dim info As ConnectionInfo = monitorInfo.Connections(index)

                    Debug.Print(DateDiff(DateInterval.Second, info.LastMessageTime, Now))

                    If info.Client.Connected Then
                        'Process connected client 
                        If info.DataQueue.Count > 0 Then
                            'The code in this If-Block should be modified to build 'message' objects 
                            'according to the protocol you defined for your data transmissions. 
                            'This example simply sends all pending message bytes to the output textbox. 
                            'Without a protocol we cannot know what constitutes a complete message, so 
                            'with multiple active clients we could see part of client1's first message, 
                            'then part of a message from client2, followed by the rest of client1's 
                            'first message (assuming client1 sent more than 64 bytes). 
                            Dim messageBytes As New List(Of Byte)
                            While info.DataQueue.Count > 0
                                Dim value As Byte
                                If info.DataQueue.TryDequeue(value) Then
                                    messageBytes.Add(value)
                                End If
                                'ToLog("+")
                                Sleep(1)
                            End While
                            'Me.Invoke(doAppendOutput, System.Text.Encoding.ASCII.GetString(messageBytes.ToArray))
                            Dim Message As String = _BufferMessage + System.Text.Encoding.ASCII.GetString(messageBytes.ToArray)


                            Message = Message.Replace(PREFIX, String.Empty)
                            Dim s As String() = Message.Split(TERM)
                            Try
                                If s(s.Length - 1) <> String.Empty Then
                                    Debug.Print("")
                                    _BufferMessage = s(s.Length - 1)
                                    s(s.Length - 1) = String.Empty
                                Else
                                    _BufferMessage = String.Empty
                                End If
                            Catch ex As Exception
                                Debug.Print("")
                            End Try


                            RaiseEvent Message_Rxs(1, s, "null", info)

                            'For Each sStr As String In s
                            '    If sStr <> "" Then
                            '      '  doAppendOutput(sStr)
                            '        RaiseEvent Message_Rx(1, sStr, "null", info)
                            '    End If
                            '    ToLog("-")
                            '    Sleep(1)
                            'Next
                        End If
                    Else
                        'Clean-up any closed client connections 
                        monitorInfo.Connections.Remove(info)
                        lostCount += 1
                    End If
                Next
                If lostCount > 0 Then
                    doUpdateConnectionCountLabel()
                    'Invoke(doUpdateConnectionCountLabel)
                End If

                'Throttle loop to avoid wasting CPU time 
                _ConnectionMontior.Wait(1) '100     'm.m da 1 a 10
            Loop While Not monitorInfo.Cancel

            'Close all connections before exiting monitor 
            For Each info As ConnectionInfo In monitorInfo.Connections
                info.Client.Close()
            Next
            monitorInfo.Connections.Clear()

            'Update the connection count label and report status 
            'Invoke(doUpdateConnectionCountLabel)
            doUpdateConnectionCountLabel()
            'doAppendOutput("Monitor Stopped.")
            'Me.Invoke(doAppendOutput, "Monitor Stopped.")
            DoMonitorConnectionsStatus = 0
        Catch ex As Exception
            DoMonitorConnectionsStatus = -1
        End Try
    End Sub

    Private _Message As String = ""
    Private Sub AppendOutput(message As String)
        'If _Message.Length > 0 Then
        Try

            _Message = _Message + message + vbCrLf
            ToLog(message)

        Catch ex As Exception
            ToLog(ex.Message)
        End Try
        'message.AppendText(ControlChars.NewLine)
        'End If
        'message.AppendText(message)
        ' message.ScrollToCaret()

    End Sub

    Dim ConnectionCountLabel As String
    Private Sub UpdateConnectionCountLabel()
        ConnectionCountLabel = String.Format("{0} Connections", _Connections.Count)
        ToLog(ConnectionCountLabel)
    End Sub
    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False

    Public Sub ToLog(ByVal LogMessage As String)
        Try
            Console.WriteLine(String.Format("{0} {1} {2}", Device, Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
            '  LG.WriteToFile(String.Format("{0} {1} {2}", Device, Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
        Catch ex As Exception

        End Try
    End Sub



    Private Sub ClassTCPIP_SERVER_Connection_Closed(IdClient As Integer) Handles Me.Connection_Closed
        ToLog("Connection_Closed:" + IdClient.ToString)
    End Sub

    Private Sub ClassTCPIP_SERVER_Connection_Open(IdClient As Integer) Handles Me.Connection_Open
        ToLog("Connection_Open:" + IdClient.ToString)
    End Sub

    Private Sub ClassTCPIP_SERVER_Message_Rx(IdClient As Integer, theMessageId As String, theMessage As Object, ConnectionInfo As ConnectionInfo) Handles Me.Message_Rx
        '  ToLog("Connection_Closed:" + IdClient.ToString)
    End Sub
End Class

Public Class MonitorInfo
    '  Const TERM As String = "@@@"
    Public Property Cancel As Boolean

    Private _Connections As List(Of ConnectionInfo)
    Public ReadOnly Property Connections As List(Of ConnectionInfo)
        Get
            Return _Connections
        End Get
    End Property

    Private _Listener As TcpListener
    Public ReadOnly Property Listener As TcpListener
        Get
            Return _Listener
        End Get
    End Property

    Public Sub New(tcpListener As TcpListener, connectionInfoList As List(Of ConnectionInfo))
        _Listener = tcpListener
        _Connections = connectionInfoList
    End Sub

    Protected Overrides Sub Finalize()
        Try
            For Each x As ConnectionInfo In _Connections
                x._Close()
            Next
        Catch ex As Exception
            ToLog(ex.Message)
        End Try

        MyBase.Finalize()
    End Sub
    Public Sub ToLog(ByVal LogMessage As String)
        Try
            Console.WriteLine(String.Format("{0} {1} {2}", "", Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
            ' LG.WriteToFile(String.Format("{0} {1} {2}", Device, Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
        Catch ex As Exception

        End Try
    End Sub
End Class

'Provides a container object to serve as the state object for async client and stream operations 
Public Class ConnectionInfo
    Public LastMessageTime As Date
    Const TERM As String = "@@@"
    Const PREFIX As String = "$$$"
    Public Sub _Close()
        Try
            ' test m.m.
            '_Monitor.Listener.Stop()
            ''''' Stream.Close()

            _Client.Close()
            'Sleep(500)

            ' Finalize()

        Catch ex As Exception
            ToLog("_Close:" + ex.Message)
        End Try

    End Sub
    Public Sub ToLog(ByVal LogMessage As String)
        Try
            '   Console.WriteLine(String.Format("{0} {1} {2}", Device, Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
            ' LG.WriteToFile(String.Format("{0} {1} {2}", Device, Format(Now, "dd/MM/yyyy HH:mm:ss:ffff"), LogMessage))
        Catch ex As Exception

        End Try
    End Sub

    'hold a reference to entire monitor instead of just the listener 
    Private _Monitor As MonitorInfo
    Public ReadOnly Property Monitor As MonitorInfo
        Get
            Return _Monitor
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

    Private _DataQueue As System.Collections.Concurrent.ConcurrentQueue(Of Byte)
    Public ReadOnly Property DataQueue As System.Collections.Concurrent.ConcurrentQueue(Of Byte)
        Get
            Return _DataQueue
        End Get
    End Property

    Private _LastReadLength As Integer
    Public ReadOnly Property LastReadLength As Integer
        Get
            Return _LastReadLength
        End Get
    End Property

    'The buffer size is an arbitrary value which should be selected based on the 
    'amount of data you need to transmit, the rate of transmissions, and the 
    'anticipalted number of clients. These are the considerations for designing 
    'the communicaition protocol for data transmissions, and the size of the read 
    'buffer must be based upon the needs of the protocol. 
    Private _Buffer(500) As Byte

    Public Sub New(monitor As MonitorInfo)
        Try

            _Monitor = monitor
            _DataQueue = New System.Collections.Concurrent.ConcurrentQueue(Of Byte)
        Catch ex As Exception
            ToLog("New:" + ex.Message)
        End Try

    End Sub

    Public Sub AcceptClient(result As IAsyncResult)
        Try

            _Client = _Monitor.Listener.EndAcceptTcpClient(result)
            If _Client IsNot Nothing AndAlso _Client.Connected Then
                _Stream = _Client.GetStream
            End If
        Catch ex As Exception
            ToLog("AcceptClient:" + ex.Message)
        End Try

    End Sub

    Public Sub AwaitData()
        Try
            LastMessageTime = Now
            Me._Stream.BeginRead(_Buffer, 0, _Buffer.Length, AddressOf DoReadData, Me)
        Catch ex As Exception
            ToLog("AwaitData:" + ex.Message)
        End Try

    End Sub

    Private Sub DoReadData(result As IAsyncResult)
        Dim info As ConnectionInfo = CType(result.AsyncState, ConnectionInfo)
        Try
            'If the stream is valid for reading, get the current data and then begin another async read 



            If info.Stream IsNot Nothing AndAlso info.Stream.CanRead Then
                info._LastReadLength = info.Stream.EndRead(result)

                'Console.WriteLine(_LastReadLength.ToString)

                For index As Integer = 0 To _LastReadLength - 1
                    info._DataQueue.Enqueue(info._Buffer(index))
                    Sleep(1) ' 10
                Next


                If _LastReadLength = 0 Then
                    Sleep(1)
                Else
                    LastMessageTime = Now
                End If


                'The example responds to all data reception with the number of bytes received; 
                'you would likely change this behavior when implementing your own protocol. 
                'info.SendMessage("Received " & info._LastReadLength & " Bytes")
                ' info.SendMessage("CMD1")

                'ààààààààààààààà

                'For Each otherInfo As ConnectionInfo In info.Monitor.Connections
                '    If Not otherInfo Is info Then
                '        otherInfo.SendMessage(System.Text.Encoding.ASCII.GetString(info._Buffer))
                '    End If
                'Next



                info.AwaitData()
                'Sleep(10) 'm.m.
                Sleep(1) ' 10
            Else
                'If we cannot read from the stream, the example assumes the connection is 
                'invalid and closes the client connection. You might modify this behavior 
                'when implementing your own protocol. 
                info.Client.Close()
            End If
        Catch ex As Exception
            info._LastReadLength = -1
            ToLog("DoReadData:" + ex.Message)
            info.Stream.Close()
            info.Client.Close() 'm.m.

        End Try
    End Sub

    Public Sub SendMessage(message As String)
        Try
            If _Stream IsNot Nothing Then
                Dim messageData() As Byte = System.Text.Encoding.ASCII.GetBytes(PREFIX + message + TERM)
                '  Stream.Write(messageData, 0, messageData.Length)
                Stream.WriteAsync(messageData, 0, messageData.Length)
            End If
        Catch ex As Exception
            ToLog("SendMessage:" + ex.Message)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        ' _Close()
        MyBase.Finalize()
    End Sub
End Class
