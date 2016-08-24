Imports System
Imports System.Runtime.InteropServices
Imports System.Threading


Public Class clsX4
    '=================================================================================================
    '
    '
    '
    '=================================================================================================
    ' Copyright © 1999-2020 MA AUTOMAZIONE of Massimo Mandelli
    ' by Mandelli M.     Date creation: 02/02/2005   Time: 12:23                         (°°°)
    '=================================================================================================
    ' Revision
    '
    '
    '=================================================================================================
    '----------------- giu-2007 ---------------------------------------------------------------------
    Public Event theGrp_ReadComplete(ByVal source As Object, ByVal e As OPCDA.NET.ReadCompleteEventArgs)
    Public Event theGrp_WriteComplete(ByVal source As Object, ByVal e As OPCDA.NET.WriteCompleteEventArgs)
    Public Event theGrp_DataChange(ByVal source As Object, ByVal e As OPCDA.NET.DataChangeEventArgs)
    '----------------- giu-2007 ---------------------------------------------------------------------

    Friend WithEvents MSG As clsX4Message
    Public WithEvents OPCClient As ClsOPCSartorius
    Private m_strDevice As String

    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False

    'Friend DeviceProperties As New clsDeviceProperties
    Public Property DevType As String = "Wgt_X3"

    '--------------------------------------------------
    ' Device
    '--------------------------------------------------
    Public Property Device() As String
        Get
            Device = m_strDevice
        End Get
        Set(ByVal Value As String)
            Try
                m_strDevice = Value

                OPCClient.Device = Value
                ' DeviceProperties.Load(m_strDevice)
            Catch Ex As Exception
                LG.WriteErr(m_strDevice, "@ Error in clsX4.Device() Err:" & Ex.Message)
            End Try
        End Set
    End Property

    Public Sub DevicePropertiesRefresh()
        Try
            ' DeviceProperties.Load(m_strDevice)
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsX4.DevicePropertiesRefresh() Err:" & Ex.Message)
        End Try

    End Sub



    Public Function StartOPC() As Boolean
        Try
            'Init()
            StartOPC = OPCClient.OPCServerConnect()
            If StartOPC = True Then
                Init()
                OPCClient.AddOpcItemsFromCollection()
                OPCClient.AddHandlers()

            End If
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsX4.StartOPC() Err:" & Ex.Message)
        End Try
    End Function

    Public Sub AddOpcItemsFromCollection()
        OPCClient.AddOpcItemsFromCollection()
    End Sub

    Public Property OpcIsConnect As Boolean
    Public Sub New(ByVal theDevice As String, ByVal Start As Boolean)
        Device = theDevice
        MSG = New clsX4Message
        OPCClient = New ClsOPCSartorius
        If Start Then
            OpcIsConnect = StartOPC()

        End If
    End Sub

    Public Sub New()
        MSG = New clsX4Message
        OPCClient = New ClsOPCSartorius
    End Sub

    Protected Overrides Sub Finalize()
        Try
            MSG = Nothing
            OPCClient = Nothing
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsX4.Finalize() Err:" & Ex.Message)
        End Try

        MyBase.Finalize()
    End Sub

    Public Function DeviceIsConnected() As Boolean
        Try
            Dim mStatus As String = OPCClient.LocalML.KeyItem(m_strDevice, "A", "ST_COMM_OK").Value
            If mStatus <> -99 Then
                DeviceIsConnected = True
            End If
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsX4.DeviceIsConnected() Err:" & Ex.Message)
        End Try
    End Function


    Public Property ConfigurationSPM As String = "X4"

    Public Sub Init()
        Try

            'Select Case DevType
            '    Case "Wgt_X3"
            With OPCClient.LocalML
                .Clear()
                Select Case ConfigurationSPM
                    'Case "MAXIS5_BASIC"
                    '    .Add(m_strDevice, "_SYS", "ST_COMM_OK", String.Empty, m_strDevice & ".ST_COMM_OK", 1)
                    '    .Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
                    '    .Add(m_strDevice, "_SYS", "ST_WGT_A_NET_FORMATTED", String.Empty, m_strDevice & ".ST_WGT_A_NET_FORMATTED", 1)
                    '    .Add(m_strDevice, "_SYS", "ST_WGT_A_GROSS_FORMATTED", String.Empty, m_strDevice & ".ST_WGT_A_GROSS_FORMATTED", 1)
                    '    .Add(m_strDevice, "_SYS", "ST_WGT_A_GROSS_TARED", String.Empty, m_strDevice & ".ST_WGT_A_GROSS_TARED", 1)
                    '.Add(m_strDevice, "_SYS", "GROSS_WEIGHT_A", String.Empty, m_strDevice & ".MD8", 1)
                    '.Add(m_strDevice, "_SYS", "NET_WEIGHT_A", String.Empty, m_strDevice & ".MD9", 1)
                    '.Add(m_strDevice, "_SYS", "TARE_WEIGHT_A", String.Empty, m_strDevice & ".MD10", 1)

                    Case "X4"
                        .Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
                        .Add(m_strDevice, "_SYS", "ST_WGT_B", String.Empty, m_strDevice & ".ST_WGT_B", 1)
                        .Add(m_strDevice, "_SYS", "ST_WGT_C", String.Empty, m_strDevice & ".ST_WGT_C", 1)

                        .Add(m_strDevice, "_SYS", "ST_COMM_OK", String.Empty, m_strDevice & ".ST_COMM_OK", 1)
                        '.Add(m_strDevice, "_SYS", "BoardNumber", String.Empty, m_strDevice & ".MD6", 1)
                        .Add(m_strDevice, "_SYS", "NET_WEIGHT_A", String.Empty, m_strDevice & ".MD20", 1)
                        .Add(m_strDevice, "_SYS", "NET_WEIGHT_B", String.Empty, m_strDevice & ".MD24", 1)
                        .Add(m_strDevice, "_SYS", "NET_WEIGHT_C", String.Empty, m_strDevice & ".MD28", 1)

                        .Add(m_strDevice, "_SYS", "SETPOINT_A", String.Empty, m_strDevice & ".MD19", 1)
                        .Add(m_strDevice, "_SYS", "SETPOINT_B", String.Empty, m_strDevice & ".MD23", 1)
                        .Add(m_strDevice, "_SYS", "SETPOINT_C", String.Empty, m_strDevice & ".MD27", 1)

                        .Add(m_strDevice, "_SYS", "TruckType", String.Empty, m_strDevice & ".MB51", 1)  '1=motrice A   2=motrice+rimorchio A+B 3=Autoarticolato C
                        .Add(m_strDevice, "_SYS", "TruckPhase", String.Empty, m_strDevice & ".MW31", 1)



                        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        '  WP-A
                        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        '.Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
                        '.Add(m_strDevice, "_SYS", "ST_WGT_A_NET_FORMATTED", String.Empty, m_strDevice & ".ST_WGT_A_NET_FORMATTED", 1)
                        ''.Add(m_strDevice, "_SYS", "ST_WGT_A_GROSS_STANDSTILL", String.Empty, m_strDevice & ".ST_WGT_A_GROSS_STANDSTILL", 1)

                        '.Add(m_strDevice, "_SYS", "L_STATUS", String.Empty, m_strDevice & ".MW265", 1)
                        '.Add(m_strDevice, "_SYS", "MATERIAL_TYPE", String.Empty, m_strDevice & ".MB604L2", 1)      'TIPO COMPONENTE

                        ''COMPONENTE AUTOMATICO - B1 - B6
                        '0:           (* idle *)
                        '11..12:      (* wait for SPM input *)
                        '13:          (* coarse *)
                        '14:          (* fine *)
                        '15:          (* calming *)
                        '16, 17:      (* tolerance check *)
                        '30..35, 38:  (* held by operator *)           
                        '36:          (* tolerance alarm *)
                        '60, 61:      (* stopped *)
                        '70, 71:      (* completed *)

                        'COMPONENTE(MANUALE - D1, D2)
                        '0:           (* idle *)
                        '10:          (* start manual *)
                        '11:          (* dialog *)
                        '12:          (* tare *)
                        '13:          (* filling *)
                        '18, 38:      (* sending *)
                        '30..34, 39:  (* held *)
                        '35:          (* out of tolerance *)
                        '60, 61:      (* stopped *)
                        '70, 71:      (* completed *)

                        'COMPONENTE DUMMY - D5 .. D8, A1, A2
                        '0:           (* idle *)
                        '11:          (* wait for SPM *)
                        '31..39:      (* held *)
                        '60, 61:      (* stopped *)
                        '70, 71:      (* completed *)



                        '.Add(m_strDevice, "_SYS", "WS_STATUS", String.Empty, m_strDevice & ".MW301", 1)
                        '.Add(m_strDevice, "_SYS", "I_SPM_OUT", String.Empty, m_strDevice & ".MW264", 1)      'codice prodotto
                        '.Add(m_strDevice, "_SYS", "O_Actual", String.Empty, m_strDevice & ".MW272", 1)      'peso netto (*100)

                        '.Add(m_strDevice, "_SYS", "B_SetPoint", String.Empty, m_strDevice & ".MW275", 1)      'setpoint (*100) kg 300.00

                        ''  .Add(m_strDevice, "_SYS", "START", String.Empty, m_strDevice & ".MX", 1)      
                        '.Add(m_strDevice, "_SYS", "STOP", String.Empty, m_strDevice & ".MX275", 1)
                        '.Add(m_strDevice, "_SYS", "CANCEL", String.Empty, m_strDevice & ".MX277", 1)
                        '.Add(m_strDevice, "_SYS", "RESTART", String.Empty, m_strDevice & ".MX276", 1)

                        ''   .Add(m_strDevice, "_SYS", "CoarseFlow", String.Empty, m_strDevice & ".MX260", 1)    '1=gross
                        ''  .Add(m_strDevice, "_SYS", "FineFlow", String.Empty, m_strDevice & ".MX260", 1)    '1=fine   0=fermo tutto


                        '.Add(m_strDevice, "_SYS", "CoarseFlow", String.Empty, m_strDevice & ".MX8002", 1)    '1=gross
                        '.Add(m_strDevice, "_SYS", "FineFlow", String.Empty, m_strDevice & ".MX8003", 1)    '1=fine   0=fermo tutto

                        '.Add(m_strDevice, "_SYS", "W_FLOW_RATE", String.Empty, m_strDevice & ".MX268", 1)    'ALLARME DI FLUSSO
                        '.Add(m_strDevice, "_SYS", "E_TOLERANCE_ALARM", String.Empty, m_strDevice & ".MX269", 1)    'ALLARME DI TOLLERANZA



                        '.Add(m_strDevice, "_SYS", "SET_ZERO_A", String.Empty, m_strDevice & ".MX112", 1)
                        '.Add(m_strDevice, "_SYS", "SET_TARE_A", String.Empty, m_strDevice & ".MX113", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_TARE_A", String.Empty, m_strDevice & ".MX114", 1)
                        '.Add(m_strDevice, "_SYS", "SET_FIX_TARE_A", String.Empty, m_strDevice & ".MX118", 1)
                        '.Add(m_strDevice, "_SYS", "VALUE_FIX_TARE_A", String.Empty, m_strDevice & ".MD31", 1)
                        '.Add(m_strDevice, "_SYS", "CALIBRATION_ACTIVE_A", String.Empty, m_strDevice & ".MX57", 1)
                        '.Add(m_strDevice, "_SYS", "DEVICE_IS_TARED_A", String.Empty, m_strDevice & ".MX58", 1)
                        '.Add(m_strDevice, "_SYS", "WEIGHT_UNIT_A", String.Empty, m_strDevice & ".MB17", 1)
                        '.Add(m_strDevice, "_SYS", "MAX_WEIGHT_A", String.Empty, m_strDevice & ".MD14", 1)
                        '.Add(m_strDevice, "_SYS", "MIN_WEIGHT_A", String.Empty, m_strDevice & ".MD15", 1)
                        '.Add(m_strDevice, "_SYS", "ScaleInterval_A", String.Empty, m_strDevice & ".MB18", 1)
                        '.Add(m_strDevice, "_SYS", "Exponent_A", String.Empty, m_strDevice & ".MB16", 1)
                        ''.Add(m_strDevice, "_SYS", "Serialnumber_A", String.Empty, m_strDevice & ".MD6", 1)
                        '.Add(m_strDevice, "_SYS", "GROSS_WEIGHT_A", String.Empty, m_strDevice & ".MD8", 1)
                        '.Add(m_strDevice, "_SYS", "NET_WEIGHT_A", String.Empty, m_strDevice & ".MD9", 1)
                        '.Add(m_strDevice, "_SYS", "TARE_WEIGHT_A", String.Empty, m_strDevice & ".MD10", 1)
                        '.Add(m_strDevice, "_SYS", "CURRENT_WEIGHT_A", String.Empty, m_strDevice & ".MD11", 1)
                        '.Add(m_strDevice, "_SYS", "SWITCH_WEIGHT_A", String.Empty, m_strDevice & ".MX72", 1)
                        '.Add(m_strDevice, "_SYS", "OVERLOAD_A", String.Empty, m_strDevice & ".MX34", 1)
                        '.Add(m_strDevice, "_SYS", "ZERO_PERMESSO_A", String.Empty, m_strDevice & ".MX37", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_7_A", String.Empty, m_strDevice & ".MX40", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_3_A", String.Empty, m_strDevice & ".MX41", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_6_A", String.Empty, m_strDevice & ".MX43", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_9_A", String.Empty, m_strDevice & ".MX44", 1)
                        '.Add(m_strDevice, "_SYS", "PowerFail_A", String.Empty, m_strDevice & ".MX50", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_PowerFail_A", String.Empty, m_strDevice & ".MX117", 1)
                        '.Add(m_strDevice, "_SYS", "LAST_ERROR_A", String.Empty, m_strDevice & ".MB19", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_ERROR_A", String.Empty, m_strDevice & ".MX121", 1)


                        ''@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        ''  WP-B
                        ''@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                        '.Add(m_strDevice, "_SYS", "ST_WGT_B", String.Empty, m_strDevice & ".ST_WGT_B", 1)
                        '.Add(m_strDevice, "_SYS", "ST_WGT_B_GROSS_STANDSTILL", String.Empty, m_strDevice & ".ST_WGT_B_GROSS_STANDSTILL", 1)
                        '.Add(m_strDevice, "_SYS", "SET_ZERO_B", String.Empty, m_strDevice & ".MX4208", 1)
                        '.Add(m_strDevice, "_SYS", "SET_TARE_B", String.Empty, m_strDevice & ".MX4209", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_TARE_B", String.Empty, m_strDevice & ".MX4210", 1)
                        '.Add(m_strDevice, "_SYS", "SET_FIX_TARE_B", String.Empty, m_strDevice & ".MX4214", 1)
                        '.Add(m_strDevice, "_SYS", "VALUE_FIX_TARE_B", String.Empty, m_strDevice & ".MD159", 1)
                        '.Add(m_strDevice, "_SYS", "CALIBRATION_ACTIVE_B", String.Empty, m_strDevice & ".MX4153", 1)
                        '.Add(m_strDevice, "_SYS", "DEVICE_IS_TARED_B", String.Empty, m_strDevice & ".MX4154", 1)
                        '.Add(m_strDevice, "_SYS", "WEIGHT_UNIT_B", String.Empty, m_strDevice & ".MB529", 1)
                        '.Add(m_strDevice, "_SYS", "MAX_WEIGHT_B", String.Empty, m_strDevice & ".MD142", 1)
                        '.Add(m_strDevice, "_SYS", "MIN_WEIGHT_B", String.Empty, m_strDevice & ".MD143", 1)
                        '.Add(m_strDevice, "_SYS", "ScaleInterval_B", String.Empty, m_strDevice & ".MB530", 1)
                        '.Add(m_strDevice, "_SYS", "Exponent_B", String.Empty, m_strDevice & ".MB528", 1)
                        '.Add(m_strDevice, "_SYS", "Serialnumber_B", String.Empty, m_strDevice & ".MD134", 1)
                        '.Add(m_strDevice, "_SYS", "GROSS_WEIGHT_B", String.Empty, m_strDevice & ".MD136", 1)
                        '.Add(m_strDevice, "_SYS", "NET_WEIGHT_B", String.Empty, m_strDevice & ".MD137", 1)
                        '.Add(m_strDevice, "_SYS", "TARE_WEIGHT_B", String.Empty, m_strDevice & ".MD138", 1)
                        '.Add(m_strDevice, "_SYS", "CURRENT_WEIGHT_B", String.Empty, m_strDevice & ".MD139", 1)
                        '.Add(m_strDevice, "_SYS", "SWITCH_WEIGHT_B", String.Empty, m_strDevice & ".MX4168", 1)
                        '.Add(m_strDevice, "_SYS", "OVERLOAD_B", String.Empty, m_strDevice & ".MX4130", 1)
                        '.Add(m_strDevice, "_SYS", "ZERO_PERMESSO_B", String.Empty, m_strDevice & ".MX4133", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_7_B", String.Empty, m_strDevice & ".MX4136", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_3_B", String.Empty, m_strDevice & ".MX4137", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_6_B", String.Empty, m_strDevice & ".MX4139", 1)
                        '.Add(m_strDevice, "_SYS", "ERROR_9_B", String.Empty, m_strDevice & ".MX4140", 1)
                        '.Add(m_strDevice, "_SYS", "PowerFail_B", String.Empty, m_strDevice & ".MX4146", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_PowerFail_B", String.Empty, m_strDevice & ".MX4213", 1)
                        '.Add(m_strDevice, "_SYS", "LAST_ERROR_B", String.Empty, m_strDevice & ".MB531", 1)
                        '.Add(m_strDevice, "_SYS", "RESET_ERROR_B", String.Empty, m_strDevice & ".MX4217", 1)


                        '.Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
                        '.Add(m_strDevice, "_SYS", "ST_WPA", String.Empty, m_strDevice & ".ST_WPA", 1)
                        '.Add(m_strDevice, "_SYS", "GROSS_WEIGHT", String.Empty, m_strDevice & ".MD8", 1)
                        '.Add(m_strDevice, "_SYS", "NET_WEIGHT", String.Empty, m_strDevice & ".MD9", 1)
                        '.Add(m_strDevice, "_SYS", "TARE_WEIGHT", String.Empty, m_strDevice & ".MD10", 1)
                        '.Add(m_strDevice, "_SYS", "LastGrossAlibi", String.Empty, m_strDevice & ".MD16", 1)
                        '.Add(m_strDevice, "_SYS", "LastNetAlibi", String.Empty, m_strDevice & ".MD17", 1)
                        '.Add(m_strDevice, "_SYS", "LastTareAlibi", String.Empty, m_strDevice & ".MD18", 1)
                        '.Add(m_strDevice, "_SYS", "AlibiNum", String.Empty, m_strDevice & ".MD19", 1)
                        '.Add(m_strDevice, "_SYS", "AlibiDate", String.Empty, m_strDevice & ".MD20", 1)
                        '.Add(m_strDevice, "_SYS", "AlibiTime", String.Empty, m_strDevice & ".MD21", 1)

                        '.Add(m_strDevice, "_SYS", "WEIGHT_UNIT", String.Empty, m_strDevice & ".MB17", 1)
                        '.Add(m_strDevice, "_SYS", "MAX_WEIGHT", String.Empty, m_strDevice & ".MD14", 1)
                        '.Add(m_strDevice, "_SYS", "MIN_WEIGHT", String.Empty, m_strDevice & ".MD15", 1)
                        '.Add(m_strDevice, "_SYS", "ScaleInterval", String.Empty, m_strDevice & ".MB18", 1)
                        '.Add(m_strDevice, "_SYS", "Exponent", String.Empty, m_strDevice & ".MB16", 1)
                        '.Add(m_strDevice, "_SYS", "WEIGHT_STABLE", String.Empty, m_strDevice & ".MX38", 1)
                        '.Add(m_strDevice, "_SYS", "SET_ZERO", String.Empty, m_strDevice & ".MX112", 1) '113
                        '.Add(m_strDevice, "_SYS", "PRINT", String.Empty, m_strDevice & ".MX120", 1) '113
                        '.Add(m_strDevice, "_SYS", "DB@status", String.Empty, m_strDevice & ".DB.@status", 1)
                        '.Add(Me.m_strDevice, "_SYS", "TEST", String.Empty, m_strDevice & ".mr388", 1)

                End Select



                OPCClient.NumberOfItems = .Count
            End With
            '    Case "Wgt_Combics"
            '    Case "Wgt_Maxxis"



            'End Select

            'OPCItemDefList.Add(New OPCItemDef(Device + ".MD8", True, 100, Type.GetType("System.Void")))
            'OPCItemDefList.Add(New OPCItemDef(Device + ".MB17", True, 101, Type.GetType("System.Void")))  'Weight Unit

            'OPCItemDefList.Add(New OPCItemDef(Device + ".MX38", True, 102, Type.GetType("System.Void")))  'Weight is stable
            'OPCItemDefList.Add(New OPCItemDef(Device + ".MX112", True, 103, Type.GetType("System.Void")))  'SET ZERO

            ' MSG.LocalMl = OPCClient.LocalML
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsX4.Init() Err:" & Ex.Message)
        End Try
    End Sub

    'Public Sub InitDevice(ByVal theDevice As String)
    '    Try

    '        With OPCClient.LocalML
    '            .Clear()
    '            .Add(theDevice, "_SYS", "ST_COMM_OK", String.Empty, m_strDevice & ".ST_COMM_OK", 1)
    '            .Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
    '            '.Add(m_strDevice, "_SYS", "ST_WPA", String.Empty, m_strDevice & ".ST_WPA", 1)
    '            .Add(theDevice, "_SYS", "DB@status", String.Empty, m_strDevice & ".DB.@status", 1)
    '            '.Add(Me.m_strDevice, "_SYS", "TEST", String.Empty, m_strDevice & ".mr388", 1)
    '            OPCClient.NumberOfItems = .Count
    '        End With
    '        ' MSG.LocalMl = OPCClient.LocalML
    '    Catch Ex As Exception
    '        Lg.WriteErr(m_strDevice, "@ Error in clsX4.Init() Err:" & Ex.Message)
    '    End Try
    'End Sub


    Shared WGT As Decimal


    Private Sub OPCClient_theGrp_DataChange(ByVal source As Object, ByVal e As OPCDA.NET.DataChangeEventArgs) Handles OPCClient.theGrp_DataChange
        RaiseEvent theGrp_DataChange(source, e)

        'Dim s As OPC.Data.OPCItemState
        'For Each s In e.sts
        '    If s.Error Then
        '        'Debug.Print("  Handle:" + s.HandleClient.ToString() + " !ERROR:0x" + s.Error.ToString("X"))
        '    Else
        '        Try
        '            OPCClient.LocalML.Item(s.HandleClient).Value = s.DataValue
        '        Catch ex As Exception
        '            Lg.WriteErr(m_strDevice, "@ Error in Msg.OPCClient_theGrp_DataChange(" & OPCClient.LocalML.Item(s.HandleClient).Name & ") Err:" & ex.Message, clsLog.eLogLevel.Debug)
        '        End Try
        '    End If
        'Next

    End Sub

    Private Sub OPCClient_theGrp_ReadComplete(ByVal source As Object, ByVal e As OPCDA.NET.ReadCompleteEventArgs) Handles OPCClient.theGrp_ReadComplete
        RaiseEvent theGrp_ReadComplete(source, e)

        'Dim s As OPC.Data.OPCItemState
        'For Each s In e.sts
        '    If s.Error Then
        '        'Debug.Print("  Handle:" + s.HandleClient.ToString() + " !ERROR:0x" + s.Error.ToString("X"))
        '    Else
        '        Try
        '            OPCClient.LocalML.Item(s.HandleClient).Value = s.DataValue.ToString()
        '        Catch ex As Exception
        '            Lg.WriteErr(m_strDevice, "@ Error in Msg.OPCClient_theGrp_ReadComplete(" & OPCClient.LocalML.Item(s.HandleClient).Name & ") Err:" & ex.Message, clsLog.eLogLevel.Debug)
        '        End Try
        '    End If
        'Next
    End Sub

    Private Sub OPCClient_theGrp_WriteComplete(ByVal source As Object, ByVal e As OPCDA.NET.WriteCompleteEventArgs) Handles OPCClient.theGrp_WriteComplete
        RaiseEvent theGrp_WriteComplete(source, e)
    End Sub


End Class



Public Class clsX4Message





End Class

