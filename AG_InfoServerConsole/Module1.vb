Imports System.Threading
Imports System.Threading.Thread
Imports System.Diagnostics
' serial port
Imports System.IO
Imports System.IO.Ports
' network
Imports System.Net
Imports System.Net.Sockets
Imports System.Net.Sockets.TcpClient
Imports System.Text
Imports System.Object
Imports System.MarshalByRefObject
Imports System.IO.Stream
Imports System.Net.Sockets.NetworkStream
Imports System
Imports System.Collections
Imports OPCDA.NET
Imports OPC

Imports AG_InfoServerConsole

Module Module1

    Dim ClassDisplayInfo(My.Settings.NrBaie) As ClassInfoDisplay

#Region "OPC"


    '--------------------------------------------------------------------
    ' OPC
    Private opcSrv As OpcServer = Nothing
    Private readGrp As OpcGroup = Nothing
    Private writeGrp As OpcGroup = Nothing
    Private itemsRead As OPCItemDef()
    Private itemsWrite As OPCItemDef()
    Private handlesWrite As Integer()
    Private Device_OPC As String = String.Empty
    Private Const OpcServerName As String = "GWT.OPCServer.1"
    Private WGT_UM As String
    Private WGT_IsStable As Integer
    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False
    Private WithEvents BAIA_1 As clsX4
    Private WithEvents BAIA_2 As clsX4
    Private WithEvents BAIA_3 As clsX4
    Private WithEvents BAIA_4 As clsX4
    Private WithEvents BAIA_5 As clsX4
    Private WithEvents BAIA_6 As clsX4
    Private WithEvents BAIA_7 As clsX4
    Private WithEvents BAIA_8 As clsX4
    '  Private WithEvents OPCDEVICE As clsX4




    Private WP_num As Integer = 1
    Private WP As Collection
    ' Private WP_WGT As Collection

    Dim ST_WGT_A As String
    Private GROSS_WEIGHT As Decimal
    Private NET_WEIGHT As Decimal
    Private TARE_WEIGHT As Decimal
    Private ST_COMM_OK As Integer = 1




    Private ARG As Object = My.Application.CommandLineArgs
    ' Dim port_SCADA As Int32 = 11113

    Private BoardNumber As String
    Private ERROR_str As String
    Private ERROR_A_str As String
    Private ERROR_B_str As String
    Private ERROR_C_str As String
    Private ERROR_D_str As String

#End Region



#Region "SOCKET_DISPLAY"
    '---------------------------------------------------
    ' SERVER TCPIP ASYNC SOCKET for iPod
    '---------------------------------------------------
    Public WithEvents SOCKET_DISPLAY As ClassTCPIP_SERVER
    Public port_SOCKET_DISPLAY As Int32 = 11111

#End Region
    <MTAThread()>
    Sub Main()
        tolog("START .....")
        'WP = New Collection
        'For i = 1 To WP_num
        '    Dim tmpWP As New ClassWP
        '    tmpWP.WP = Chr(64 + i)
        '    WP.Add(tmpWP, tmpWP.WP)
        '    tmpWP = Nothing
        'Next
        For i = 1 To My.Settings.NrBaie
            ClassDisplayInfo(i) = New ClassInfoDisplay(i)
        Next


        StartOPC("ATB1", "Wgt_X3")

        '        BAIA_1 = New clsX4("ATB1", True)
        ' BAIA_2 = New clsX4("ATB2", True)
        'BAIA_3 = New clsX4("ATB3", True)
        'BAIA_4 = New clsX4("ATB4", True)
        'BAIA_5 = New clsX4("ATB5", True)
        'BAIA_6 = New clsX4("ATB6", True)
        'BAIA_7 = New clsX4("ATB7", True)
        'BAIA_8 = New clsX4("ATB8", True)





        SOCKET_DISPLAY = New ClassTCPIP_SERVER(port_SOCKET_DISPLAY)
        SOCKET_DISPLAY.Start()

        Do Until False
            Sleep(1000)
        Loop

        Try
            SOCKET_DISPLAY.StopConnection()
            SOCKET_DISPLAY = Nothing
        Catch ex As Exception
            tolog("Errore in chiusura socket : " + ex.Message)
        End Try

        Try
            BAIA_1.OPCClient.Disconnect()
            BAIA_2.OPCClient.Disconnect()
            BAIA_3.OPCClient.Disconnect()
            BAIA_4.OPCClient.Disconnect()
            BAIA_5.OPCClient.Disconnect()
            BAIA_6.OPCClient.Disconnect()
            BAIA_7.OPCClient.Disconnect()
            BAIA_8.OPCClient.Disconnect()
        Catch ex As Exception
            tolog("Errore in chiusura OPC Client : " + ex.Message)
        End Try

        ' MainCycle()
        tolog("END .....")
    End Sub
    Private Sub tolog(ByVal message As String)
        Try
            Console.WriteLine(String.Format("{0} {1} {2}", Format(Now, "dd-MM-yyyy HH:mm:ss.fff"), "", message))
            'dd-mm-yyyy HH:MM:ss
        Catch ex As Exception

        End Try

    End Sub


    '=================================================================================================
    '  Start OPC X4
    '=================================================================================================
    Private OpcIsConnect As Boolean
    Public Function StartOPC(ByVal theDevice As String, Optional theDevType As String = "<>") As Boolean
        Try
            BAIA_1 = Nothing
            BAIA_1 = New clsX4

            If theDevType <> "<>" Then
                BAIA_1.DevType = theDevType
            End If

            With BAIA_1
                .Device = theDevice
                If .StartOPC() = True Then
                    'Me.myX4.MSG.StartMessageCollector()
                    OpcIsConnect = True
                    'MyLog(String.Format("Start OPC {0} OK", theDevice))
                    Return True
                Else
                    'MyLog(String.Format("Start OPC {0} FALSE", theDevice))
                    OpcIsConnect = False
                End If
            End With
        Catch Ex As Exception
            MsgBox("@ Error in StartOPC() Err:" & Ex.Message)
        End Try
    End Function
    Private Function MyIp() As String
        Try
            Dim Ipmio As IPAddress = Dns.GetHostByName(Dns.GetHostName.ToString).AddressList(0)
            Dim miohost As IPHostEntry = Dns.GetHostByAddress(Ipmio.ToString)
            Return Ipmio.ToString
        Catch ex As Exception
            Return "127,.0.0.1"
        End Try
    End Function

    Private Sub BAIA_1_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_1.theGrp_DataChange
        Try
            Const id As Integer = 1
            Dim theValue As Object 'As String = String.Empty
            Dim theTagName As String = String.Empty
            Dim OpNumber As String = String.Empty

            For Each s As OPCItemState In e.sts
                Try
                    If s.Error > 0 And s.Quality <> 192 Then
                    Else
                        If s.DataValue Is Nothing Then
                        Else
                            theValue = s.DataValue
                            BAIA_1.OPCClient.LocalML.Item(s.HandleClient).Value = theValue
                            BAIA_1.OPCClient.LocalML.Item(s.HandleClient).OPCDataType = s.DataValue.GetType
                            theTagName = BAIA_1.OPCClient.LocalML.Item(s.HandleClient).Name
                            tolog(String.Format("{0}={1}", theTagName.Trim, theValue))

                            Select Case theTagName.Trim
                                Case "NET_WEIGHT_A"
                                    ClassDisplayInfo(id).NET_WEIGHT_A = CDec(s.DataValue)
                                Case "NET_WEIGHT_B"
                                    ClassDisplayInfo(id).NET_WEIGHT_B = CDec(s.DataValue)
                                Case "NET_WEIGHT_C"
                                    ClassDisplayInfo(id).NET_WEIGHT_C = CDec(s.DataValue)
                                Case "SETPOINT_A"
                                    ClassDisplayInfo(id).SETPOINT_A = CDec(s.DataValue)
                                Case "SETPOINT_B"
                                    ClassDisplayInfo(id).SETPOINT_B = CDec(s.DataValue)
                                Case "SETPOINT_C"
                                    ClassDisplayInfo(id).SETPOINT_C = CDec(s.DataValue)
                                Case "TruckType"
                                    ClassDisplayInfo(id).TruckType = CInt(s.DataValue)
                                Case "TruckPhase"
                                    ClassDisplayInfo(id).TruckPhase = CInt(s.DataValue)
                                Case "ST_WGT_A"


                                Case "ST_COMM_OK"
                                    ST_COMM_OK = IIf(s.DataValue, 1, 0)

                                    If ST_COMM_OK = 0 Then
                                        ST_WGT_A = s.DataValue

                                        If ST_WGT_A.Contains("ERR") Then
                                            ERROR_str = ST_WGT_A
                                        Else
                                            ERROR_str = "<ERROR>"
                                        End If
                                    Else
                                        ERROR_str = String.Empty

                                    End If
                                    ClassDisplayInfo(id).ST_COMM_OK = (s.DataValue)

                                Case Else
                            End Select
                        End If
                    End If
                Catch Ex As Exception
                    MsgBox(Ex)
                End Try
            Next


        Catch Ex As Exception
            Debug.Print("@ Error in theGrp_DataChange() Err:" & Ex.Message)
        End Try
    End Sub

    Private Sub BAIA_2_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_2.theGrp_DataChange
        Try
            Const id As Integer = 2
            Dim theValue As Object 'As String = String.Empty
            Dim theTagName As String = String.Empty
            Dim OpNumber As String = String.Empty

            For Each s As OPCItemState In e.sts
                Try
                    If s.Error > 0 And s.Quality <> 192 Then
                    Else
                        If s.DataValue Is Nothing Then
                        Else
                            theValue = s.DataValue
                            BAIA_2.OPCClient.LocalML.Item(s.HandleClient).Value = theValue
                            BAIA_2.OPCClient.LocalML.Item(s.HandleClient).OPCDataType = s.DataValue.GetType
                            theTagName = BAIA_2.OPCClient.LocalML.Item(s.HandleClient).Name
                            tolog(String.Format("{0}={1}", theTagName.Trim, theValue))

                            Select Case theTagName.Trim
                                Case "NET_WEIGHT_A"
                                    ClassDisplayInfo(id).NET_WEIGHT_A = CDec(s.DataValue)
                                Case "NET_WEIGHT_B"
                                    ClassDisplayInfo(id).NET_WEIGHT_B = CDec(s.DataValue)
                                Case "NET_WEIGHT_C"
                                    ClassDisplayInfo(id).NET_WEIGHT_C = CDec(s.DataValue)
                                Case "SETPOINT_A"
                                    ClassDisplayInfo(id).SETPOINT_A = CDec(s.DataValue)
                                Case "SETPOINT_B"
                                    ClassDisplayInfo(id).SETPOINT_B = CDec(s.DataValue)
                                Case "SETPOINT_C"
                                    ClassDisplayInfo(id).SETPOINT_C = CDec(s.DataValue)
                                Case "TruckType"
                                    ClassDisplayInfo(id).TruckType = CInt(s.DataValue)
                                Case "TruckPhase"
                                    ClassDisplayInfo(id).TruckPhase = CInt(s.DataValue)
                                Case "ST_WGT_A"


                                Case "ST_COMM_OK"
                                    ST_COMM_OK = IIf(s.DataValue, 1, 0)

                                    If ST_COMM_OK = 0 Then
                                        ST_WGT_A = s.DataValue

                                        If ST_WGT_A.Contains("ERR") Then
                                            ERROR_str = ST_WGT_A
                                        Else
                                            ERROR_str = "<ERROR>"
                                        End If
                                    Else
                                        ERROR_str = String.Empty

                                    End If
                                    ClassDisplayInfo(id).ST_COMM_OK = (s.DataValue)

                                Case Else
                            End Select
                        End If
                    End If
                Catch Ex As Exception
                    MsgBox(Ex)
                End Try
            Next


        Catch Ex As Exception
            Debug.Print("@ Error in theGrp_DataChange() Err:" & Ex.Message)
        End Try
    End Sub

    Private Sub BAIA_3_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_3.theGrp_DataChange

    End Sub

    Private Sub BAIA_4_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_4.theGrp_DataChange

    End Sub

    Private Sub BAIA_5_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_5.theGrp_DataChange

    End Sub

    Private Sub BAIA_6_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_6.theGrp_DataChange

    End Sub

    Private Sub BAIA_7_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_7.theGrp_DataChange

    End Sub

    Private Sub BAIA_8_theGrp_DataChange(source As Object, e As DataChangeEventArgs) Handles BAIA_8.theGrp_DataChange

    End Sub

    Private Sub SOCKET_DISPLAY_Message_Rx(IdClient As Integer, theMessageId As String, theMessage As Object, ConnectionInfo As ConnectionInfo) Handles SOCKET_DISPLAY.Message_Rx

    End Sub

    Private Sub SOCKET_DISPLAY_Message_Rxs(IdClient As Integer, theMessageId() As String, theMessage As Object, ConnectionInfo As ConnectionInfo) Handles SOCKET_DISPLAY.Message_Rxs
        Dim q As Integer = theMessageId.Count - 1
        q = 0
        Do Until q > theMessageId.Count - 1


            If theMessageId(q) <> String.Empty Then
                Try
                    Dim strMessageId = theMessageId(q)

                    Dim MSG As String = String.Empty
                    Dim tmpWP As String = String.Empty
                    Dim s As String() = strMessageId.Split(":")

                    Try
                        If s(s.Length - 1) <> String.Empty Then
                            Debug.Print(String.Empty)
                            '_BufferMessage = s(s.Length - 1)

                            ' s(s.Length - 1) = string.empty

                            ' tmpWP = s(0)
                            strMessageId = s(0)
                        Else
                            strMessageId = String.Empty
                        End If
                    Catch ex As Exception
                        Debug.Print(String.Empty)
                    End Try


                    tolog(String.Format("----CMD---- ({0})", strMessageId))

                    Select Case strMessageId

                        Case "END"
                            tolog("@@@@@@@@@@@@@@  Start END command! @@@@@@@@@@@@@@")
                            ConnectionInfo.SendMessage("END")
                            tolog("############  SEND END command to client! ############")
                            ConnectionInfo._Close()
                            ConnectionInfo = Nothing

                        Case "GET_INFO"


                    End Select
                    '  i = i + 1
                    tolog(MSG)
                    'If i = 10000 Then i = 1

                Catch ex As Exception
                    tolog(ex.Message)
                End Try
            End If
            q = q + 1
        Loop
    End Sub

    Private Sub SOCKET_DISPLAY_Connection_Open(IdClient As Integer) Handles SOCKET_DISPLAY.Connection_Open

    End Sub

    Private Sub SOCKET_DISPLAY_Connection_Closed(IdClient As Integer) Handles SOCKET_DISPLAY.Connection_Closed

    End Sub
End Module
