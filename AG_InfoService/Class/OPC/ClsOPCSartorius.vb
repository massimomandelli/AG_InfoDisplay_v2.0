Imports System
Imports System.Runtime.InteropServices
Imports System.Threading

Imports OPC.Common
'Imports OPC.Data.Interface
'Imports OPC.Data
Imports OPCDA.Interface

Imports OPCDA
Imports OPCDA.NET
Imports OPCDA.NET.OpcGroup



Public Class ClsOPCSartorius
    '=================================================================================================
    '
    ' OPC Sartorius 
    '
    '=================================================================================================
    ' Copyright © 1999-2020 MA AUTOMAZIONE of Massimo Mandelli
    ' by Mandelli M.     Date creation: 25/02/2005   Time: 14:59:32                         (°°°)
    '=================================================================================================
    ' Revision
    '
    '
    '=================================================================================================

    'Friend LocalML As New ClsMemoryLocations
    Friend LocalML As ClsMemoryLocations

    '----------------- giu-2007 ---------------------------------------------------------------------
    Public Event theGrp_ReadComplete(ByVal source As Object, ByVal e As ReadCompleteEventArgs)
    Public Event theGrp_WriteComplete(ByVal source As Object, ByVal e As WriteCompleteEventArgs)
    Public Event theGrp_DataChange(ByVal source As Object, ByVal e As DataChangeEventArgs)
    '----------------- giu-2007 ---------------------------------------------------------------------

    Private m_strOPCServerName As String = "GWT.OPCServer.1"    'GWT Default OPC Server name
    Private m_strOPCGroup As String
    Public TheSrv As OpcServer
    'Private WithEvents TheGrp As OpcGroup
    Public TheGrp As OpcGroup
    Public ItemDefs() As OPCItemDef
    Friend HandlesSrv() As Integer

    '<+>============================================================<->
    '  variables for opc operations
    Public CancelID As Integer
    Public aE(1) As Integer
    Public ItemValues() As Object
    Public rItm() As OPCItemResult
    Private m_strDevice As String
    Private m_intOPCUpdateRate As Integer = 50
    Private m_intNumberOfItems As Integer = 0
    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False
    '--------------------------------------------------
    ' Device
    '--------------------------------------------------
    Public Property Device() As String
        Get
            'OPC.Common.HRESULTS      
            Return m_strDevice
        End Get
        Set(ByVal Value As String)
            Try
                m_strDevice = Value
                m_strOPCGroup = Value
            Catch ex As Exception

            End Try
            
        End Set
    End Property

    Public Property OPCUpdateRate() As Integer
        Get
            Return m_intOPCUpdateRate
        End Get
        Set(ByVal Value As Integer)
            Try
                m_intOPCUpdateRate = Value
            Catch ex As Exception

            End Try

        End Set
    End Property


    Public Property NumberOfItems() As Integer
        Get
            Return m_intNumberOfItems
        End Get
        Set(ByVal Value As Integer)
            Try
                m_intNumberOfItems = Value
            Catch ex As Exception

            End Try

        End Set
    End Property

    Public Property GroupName() As String
        Get
            Return m_strOPCGroup
        End Get
        Set(ByVal Value As String)
            Try
                m_strOPCGroup = Value
            Catch ex As Exception

            End Try

        End Set
    End Property

    Public Sub New()
        LocalML = New ClsMemoryLocations
    End Sub

    Protected Overrides Sub Finalize()
        Try
            LocalML = Nothing
        Catch ex As Exception
            Lg.WriteErr("debug", "Error xxx() Err:" & ex.Message)
        End Try

        MyBase.Finalize()
    End Sub
    '=================================================================================================
    ' OPC Connect
    '=================================================================================================
    Public Function OPCServerConnect(Optional ByVal theServerName$ = "", _
                                     Optional ByVal theGroupName$ = "", _
                                     Optional ByVal theUpdateRate As Integer = -1) As Boolean
        Try
            If theServerName$ <> String.Empty Then
                m_strOPCServerName = theServerName
            End If

            If theGroupName$ <> String.Empty Then
                m_strOPCGroup = theGroupName$
            End If

            If theUpdateRate <> -1 Then
                m_intOPCUpdateRate = theUpdateRate
            End If

            TheSrv = New OpcServer()
            TheSrv.Connect(m_strOPCServerName)
            Thread.Sleep(500)   ' come da esempio

            '<+>============================================================<->
            ' we are faster then some servers!

            Dim OpcSts As Boolean '= False

            Dim mRetry As Integer = 200
            Do Until OpcSts = True
                OpcSts = GetOPCServerStatus()
                '            If OpcSts = True Then Exit Do
                mRetry = mRetry - 1
                If mRetry < 0 Then Exit Do
            Loop

            'Thread.Sleep(1000) ' giu-2007

            '<+>============================================================<->
            '  add our only working group

            TheGrp = TheSrv.AddGroup(m_strOPCGroup, False, m_intOPCUpdateRate, 1)
            OPCServerConnect = OpcSts
            Lg.WriteLog(m_strDevice, "OPCServerConnect() result:" & IIf(OpcSts, "Connected", "Disconnected"))

        Catch EX1 As ExternalException
            Lg.WriteErr(m_strDevice, "@ Error in OPCServerConnect()[ExternalException] Err:" & EX1.Message())
            Return False
        Catch Ex As Exception
            Lg.WriteLog(m_strDevice, "@ Error in OPCServerConnect() Err:" & Ex.Message)
            Return False
        End Try

    End Function

    Public Function GetOPCServerStatus() As Boolean
        Try


            Dim theOpcSrvSts As OPCDA.SERVERSTATUS = Nothing

            TheSrv.GetStatus(theOpcSrvSts)

            With theOpcSrvSts
                ' .eServerState()

                Lg.WriteLog(m_strDevice, "OPCServer:" & .szVendorInfo & " State:" & .eServerState.ToString, 4)
                If .eServerState = OPCDA.OpcServerState.Running Then GetOPCServerStatus = True
            End With

        Catch ex As Exception
            Lg.WriteErr(m_strDevice, "@ Error in GetOPCServerStatus()[ExternalException] Err:" & ex.Message())
        End Try
    End Function

    Private m_StartDisconnect As Boolean = False
    Public Function Disconnect() As Boolean
        m_StartDisconnect = True

        'Dim aError(1) As Integer
        'Try
        '    Lg.WriteLog(m_strDevice, "Start clsOPCSartorius.Disconnect()")

        '    'test magg.2006 provo a toglire la rimozione del gruppo

        '    With TheGrp
        '        .Active = False
        '        .SetEnable(False)
        '        ' test feb 2007
        '        Dim tmpObj As Object
        '        For Each tmpObj In ItemDefs
        '            tmpObj.active = False
        '            'tmpObj = Nothing
        '        Next
        '        'ItemDefs = Nothing
        '        Lg.WriteLog(m_strDevice, "      clsOPCSartorius.Disconnect(Remove ItemDefs)")
        '        'Thread.Sleep(200)
        '        Lg.WriteLog(m_strDevice, "      clsOPCSartorius.Disconnect(RemoveItems(HandlesSrv, aError))")

        '        Try
        '            .RemoveItems(HandlesSrv, aError)
        '        Catch ex As Exception
        '            Lg.WriteErr(m_strDevice, "@ Error in Disconnect.RemoveItems()[ExternalException] Err:" & ex.Message())
        '        End Try

        '    End With




        'Lg.WriteLog(m_strDevice, "      clsOPCSartorius.Disconnect(TheGrp = Nothing)")

        'Return True

        'Catch EX1 As ExternalException
        '    Lg.WriteErr(m_strDevice, "@ Error in Disconnect()[ExternalException] Err:" & EX1.Message())
        '    Return False
        'Catch Ex As Exception

        '    Lg.WriteErr(m_strDevice, "@ Error in Disconnect()Err:" & Ex.Message())
        '    Return False

        'End Try

        RemoveHandlers()

        Dim aEE(m_intNumberOfItems) As Integer
        TheGrp.RemoveItems(HandlesSrv, aEE)
        TheGrp.Remove(False)

        Threading.Thread.Sleep(200)

        TheSrv.Disconnect()
        TheGrp = Nothing
        TheSrv = Nothing

    End Function




    Public Function StopOpcProcess() As Boolean
        Exit Function
        KillProcess("ewdrv_01")
        KillProcess("PR1792")



        'Dim myProcesses() As Process
        'Dim myProcess As Process

        'Try
        '    Lg.WriteLog(m_strDevice, "OPC StopOpcProcess() Kill process PR1792"
        '    myProcesses = Process.GetProcessesByName("PR1792")
        '    For Each myProcess In myProcesses
        '        myProcess.Kill()
        '    Next
        '    Lg.WriteLog(m_strDevice, "OPC StopOpcProcess() tart Kill PR1792"

        '    myProcesses = Process.GetProcessesByName("ewdrv_01")
        '    For Each myProcess In myProcesses
        '        myProcess.Kill()
        '    Next
        '    Return True
        'Catch Ex As Exception
        'End Try
    End Function
    Private Sub KillProcess(ByVal ProcessName$)
        Dim myProcesses() As Process
        Dim myProcess As Process
        Try
            Lg.WriteLog(m_strDevice, "Start Kill Process: " & ProcessName)
            myProcesses = Process.GetProcessesByName(ProcessName)

            For Each myProcess In myProcesses
                myProcess.Kill()
                Lg.WriteLog(m_strDevice, "Process: " & ProcessName & " Killed")
            Next

        Catch Ex As Exception
            Lg.WriteLog(m_strDevice, "@ Error in  KillProcess(" & ProcessName & ")")
        End Try
    End Sub


    'Public Sub AddOpcItem(ByVal theItemName As String)
    '    Try
    '        '.Add(m_strDevice, "A", "ST_COMM_OK", String.Empty, "ST_COMM_OK", 1)
    '        LocalML.Add(m_strDevice, m_strOPCGroup, theItemName, String.Empty, theItemName, 1)

    '        Dim i As Integer = 0
    '        Dim NumOfItems As Integer = LocalML.Count - 1
    '        Dim ItemName As String = String.Empty

    '        ReDim ItemDefs(NumOfItems)
    '        ReDim HandlesSrv(NumOfItems)
    '        ReDim aE(NumOfItems)
    '        'ReDim rItm(NumOfItems)

    '        '<+>============================================================<->
    '        '  add items and save server handles
    '        For i = 0 To NumOfItems
    '            ItemName = Trim(m_strOPCGroup & "." & LocalML.Item(i + 1).Cmd)
    '            ItemDefs(i) = New OPCItemDef(ItemName, True, i + 1, VarEnum.VT_EMPTY)
    '        Next

    '        TheGrp.AddItems(ItemDefs, rItm)

    '        If rItm Is Nothing Then Exit Sub

    '        For i = 0 To UBound(rItm)
    '            HandlesSrv(i) = rItm(i).HandleServer
    '            With LocalML.Item(i + 1)
    '                .TheGrp = TheGrp
    '                .Group = m_strOPCGroup
    '                .HandleServer = HandlesSrv(i)
    '                .Device = m_strDevice
    '            End With
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Public Sub AddOpcItemsFromCollection()
        Try
            Dim i As Integer = 0
            Dim NumOfItems As Integer = LocalML.Count - 1
            Dim ItemName As String = String.Empty

            ReDim ItemDefs(NumOfItems)
            ReDim HandlesSrv(NumOfItems)
            ReDim aE(NumOfItems)
            'ReDim rItm(NumOfItems)

            '<+>============================================================<->
            '  add items and save server handles
            For i = 0 To NumOfItems
                ItemName = Trim(LocalML.Item(i + 1).Cmd)
                'ItemName = Trim(m_strOPCGroup & "." & LocalML.Item(i + 1).Cmd)
                ItemDefs(i) = New OPCItemDef(ItemName, True, i + 1, VarEnum.VT_EMPTY)
            Next



            'TheGrp.SetEnable(False)
            'TheGrp.Active = False


            Me.TheGrp.AddItems(ItemDefs, rItm)

            If rItm Is Nothing Then Exit Sub

            For i = 0 To UBound(rItm)
                HandlesSrv(i) = rItm(i).HandleServer
                With Me.LocalML.Item(i + 1)
                    .TheGrp = TheGrp
                    '.Group = m_strOPCGroup
                    .HandleServer = HandlesSrv(i)
                    '.Device = m_strDevice

                End With
            Next


            '<+>============================================================<->
            '  asynch read our items
            ' TheGrp.RemoveItems()
            Me.TheGrp.SetEnable(True)
            Me.TheGrp.Active = True

            'AddHandlers()


            '<+>============================================================<->
            '  if EventsActive is true the system active the group events
            'If m_bEventsActive = True Then
            '    '<+>============================================================<->
            '    '  Events
            '    AddHandler TheGrp.ReadCompleted, AddressOf theGrp_ReadComplete
            '    AddHandler TheGrp.DataChanged, AddressOf theGrp_DataChange
            '    AddHandler TheGrp.WriteCompleted, AddressOf theGrp_WriteComplete

            'End If
        Catch Ex As Exception
            Debug.print("AddOpcItemsFromCollection ERR." & Ex.Message)
        End Try

    End Sub

    'Private Sub TheSrv_ShutdownRequested(ByVal sender As Object, ByVal e As OPC.Data.ShutdownRequestEventArgs) Handles TheSrv.ShutdownRequested
    '    Debug.print(string.empty)
    'End Sub

    'Private Sub TheGrp_CancelCompleted(ByVal sender As Object, ByVal e As OPC.Data.CancelCompleteEventArgs) Handles TheGrp.CancelCompleted
    '    Debug.print(string.empty)
    'End Sub

    ' ------------------------------ events -----------------------------

    Public Function AddHandlers() As Boolean
        Try
            AddHandler TheGrp.DataChanged, AddressOf TheGrp_DataChanged
            AddHandler TheGrp.ReadCompleted, AddressOf TheGrp_ReadCompleted
            AddHandler TheGrp.WriteCompleted, AddressOf TheGrp_WriteCompleted

            TheGrp.AdviseIOPCDataCallback() 'm.m.


            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function RemoveHandlers() As Boolean
        Try
            RemoveHandler TheGrp.DataChanged, AddressOf TheGrp_DataChanged
            RemoveHandler TheGrp.ReadCompleted, AddressOf TheGrp_ReadCompleted
            RemoveHandler TheGrp.WriteCompleted, AddressOf TheGrp_WriteCompleted
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub TheGrp_DataChanged(ByVal sender As Object, ByVal e As OPCDA.NET.DataChangeEventArgs) 'Handles TheGrp.DataChanged
        If m_StartDisconnect = False Then
            RaiseEvent theGrp_DataChange(sender, e)
        End If
    End Sub
    '  ByVal sender As Object, ByVal e As DataChangeEventArgs

    Private Sub TheGrp_ReadCompleted(ByVal sender As Object, ByVal e As OPCDA.NET.ReadCompleteEventArgs) ' Handles TheGrp.ReadCompleted
        If m_StartDisconnect = False Then
            RaiseEvent theGrp_ReadComplete(sender, e)
        End If
    End Sub

    Private Sub TheGrp_WriteCompleted(ByVal sender As Object, ByVal e As OPCDA.NET.WriteCompleteEventArgs) 'Handles TheGrp.WriteCompleted
        If m_StartDisconnect = False Then
            RaiseEvent theGrp_WriteComplete(sender, e)
        End If
    End Sub
End Class
