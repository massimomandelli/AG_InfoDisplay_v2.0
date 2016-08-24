Imports System
Imports System.Runtime.InteropServices
Imports System.Threading


Imports OPC.Common
Imports OPC.Data.Interface
Imports OPC.Data
Imports OPCDA.NET


Public Class ClsMemoryLocations
    '=================================================================================================
    '
    ' Collection of Class clsMemoryLocation
    '
    '=================================================================================================
    ' Copyright © 1999-2020 MA AUTOMAZIONE of Massimo Mandelli
    ' by Mandelli M.     Date creation: 25/10/2004   Time: 15.00                         (°°°)
    '=================================================================================================
    ' Revision
    '
    '
    '=================================================================================================


    '<+>============================================================<->
    '  variables for opc operations

    Private m_strDevice As String = String.Empty
    Public mCol As Collection
    Public Fields_Key As Collection
    Public Fields_List As Collection
    Public Fields_DES As Collection
    Public Fields_DEF_VAL As Collection

    Public mvarFormToCall As String = String.Empty
    Private mvarSql_Def As String = String.Empty
    Private mvarSql_Def_Where As String = String.Empty
    Private mvarTableName As String = String.Empty
    Private mvarTitle As String = String.Empty

    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False



    Dim mvarmasterQuality As Integer
    Public Property masterQuality() As Integer
        Get
            Return mvarmasterQuality
        End Get
        Set(ByVal Value As Integer)
            mvarmasterQuality = Value
        End Set
    End Property

    Dim mvarmasterError As Integer
    Public Property masterError() As Integer
        Get
            Return mvarmasterError
        End Get
        Set(ByVal Value As Integer)
            mvarmasterError = Value
        End Set
    End Property
    Dim mvargroupHandleClient As Integer
    Public Property groupHandleClient() As Integer
        Get
            Return mvargroupHandleClient
        End Get
        Set(ByVal Value As Integer)
            mvargroupHandleClient = Value
        End Set
    End Property

    '--------------------------------------------------
    ' FormToCall
    '--------------------------------------------------
    Public Property FormToCall() As String
        Get
            FormToCall = mvarFormToCall
        End Get
        Set(ByVal Value As String)
            mvarFormToCall = Value
        End Set
    End Property

    '--------------------------------------------------
    ' TableName
    '--------------------------------------------------
    Public Property TableName() As String
        Get
            TableName = mvarTableName
        End Get
        Set(ByVal Value As String)
            mvarTableName = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Sql_DEF
    '--------------------------------------------------
    Public Property Sql_DEF() As String
        Get
            Sql_DEF = mvarSql_Def
        End Get
        Set(ByVal Value As String)
            mvarSql_Def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Sql_Def_Where
    '--------------------------------------------------
    Public Property Sql_Def_Where() As String
        Get
            Sql_Def_Where = mvarSql_Def_Where
        End Get
        Set(ByVal Value As String)
            mvarSql_Def_Where = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Title
    '--------------------------------------------------
    Public Property Title() As String
        Get
            Title = mvarTitle
        End Get
        Set(ByVal Value As String)
            mvarTitle = Value
        End Set
    End Property

    Public Function Add(ByVal Device As String, ByVal Group As String, ByVal Name As String, ByVal Address As String, ByVal Cmd As String, ByVal State As Integer, Optional ByVal sKey As String = "") As ClsMemoryLocation
        Try
            'Add = Nothing
            'create a new object
            Dim objNewMember As ClsMemoryLocation
            objNewMember = New ClsMemoryLocation
            With objNewMember
                .Device = Device
                .Group = Group
                .Name = Name
                .Address = Address
                .Cmd = Cmd
                .State = State
                .ClassKey = Device & "_" & Group & "_" & Name


                If Len(sKey) = 0 Then
                    mCol.Add(objNewMember, .ClassKey)
                Else
                    mCol.Add(objNewMember, sKey)
                End If
            End With



            Add = objNewMember
            objNewMember = Nothing
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocation.Add () (value:" & Device & "_" & Group & "_" & Name & "  Err:" & Ex.Message)
            Return Nothing
        End Try

    End Function

    Public Function Exist(ByVal vntIndexKey As Object) As Boolean

        Try
            mCol(vntIndexKey).Device()
            Return True
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.Exist() Err:" & Ex.Message)
        End Try
    End Function

    Public Function Item(ByVal vntIndexKey As Object) As ClsMemoryLocation
        Try
            Item = mCol.Item(vntIndexKey)
        Catch Ex As Exception
            LG.WriteLog(m_strDevice, "Item non valido ! <" & vntIndexKey & "> Errore" & Ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function Count() As Long
        Try
            Count = mCol.Count
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.Count() Err:" & Ex.Message)
        End Try


    End Function

    Public Sub Remove(ByVal vntIndexKey As Object)
        Try
            mCol.Remove(vntIndexKey)
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.Remove() Err:" & Ex.Message)
        End Try

    End Sub

    Public Sub New()
        'Fields_Key = New Collection
        'Fields_List = New Collection
        'Fields_DES = New Collection

        'Fields_DEF_VAL = New Collection

        mCol = New Collection

        'Fields_Key.Add("Device", "Device")
        'Fields_List.Add("Device", "Device")
        'Fields_DES.Add("Device", "Device")
        'Fields_DEF_VAL.Add(String.Empty, "Device")
        ''-----------------------------------------------------------------
        'Fields_Key.Add("Name", "Name")
        'Fields_List.Add("Name", "Name")
        'Fields_DES.Add("Name", "Name")
        'Fields_DEF_VAL.Add(String.Empty, "Name")
        ''-----------------------------------------------------------------
        'Fields_List.Add("Address", "Address")
        'Fields_DES.Add("Address", "Address")
        'Fields_DEF_VAL.Add(String.Empty, "Address")
        ''-----------------------------------------------------------------
        'Fields_List.Add("Cmd", "Cmd")
        'Fields_DES.Add("Cmd", "Cmd")
        'Fields_DEF_VAL.Add(String.Empty, "Cmd")
        ''-----------------------------------------------------------------
        'Fields_List.Add("State", "State")
        'Fields_DES.Add("State", "State")
        'Fields_DEF_VAL.Add(0, "State")
        '-----------------------------------------------------------------


        mvarSql_Def = "SELECT [Device],[Name],[Address],[Cmd],[State] FROM A_X4MemoryLocation  " & mvarSql_Def_Where & " ORDER BY [Device],[Name]"
        mvarTableName = "A_X4MemoryLocation"
        mvarTitle = "Tabella : A_X4MemoryLocation"
    End Sub





    ' This sub remove all items from the collection.
    Sub Clear()
        Try
            mCol = New Collection
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.Clear() Err:" & Ex.Message)
        End Try

    End Sub

    Public Sub CopyToComboBox(ByRef theComboBox As Object)
        theComboBox.Clear()
        Dim x As Object
        For Each x In mCol
            theComboBox.AddItem(x.Device)
        Next
    End Sub

    Public Function InsertAllItemsDb() As String
        Dim x As Object
        InsertAllItemsDb = String.Empty
        For Each x In mCol
            InsertAllItemsDb = InsertAllItemsDb & x.Insert
        Next
    End Function

    Public Function UpdateAllItemsDb() As String
        Dim x As Object
        UpdateAllItemsDb = String.Empty
        For Each x In mCol
            UpdateAllItemsDb = UpdateAllItemsDb & x.Update
        Next
    End Function
    '--------------------------------------------------
    ' DELETE 
    '--------------------------------------------------
    'Public Function DeleteRecord(ByVal theDevice As String, ByVal theName As String) As String
    '    Dim Err$ = String.Empty
    '    Cn.RunSQL("DELETE * FROM A_X4MemoryLocation WHERE [Device]=" & Chr(39) & theDevice & Chr(39) & " AND [Name]=" & Chr(39) & theName & Chr(39) & String.Empty, , Err$)
    '    DeleteRecord = Err$
    'End Function

    '=================================================================================================
    ' Populate Collection from DataBase
    '=================================================================================================
    Public Function Load(ByVal theDevice As String, Optional ByVal theGroup As String = "", Optional ByVal theName As String = "") As Boolean
        'Dim SQL As String
        'Dim Rs As ADODB.Recordset
        'Dim objNewMember As ClsMemoryLocation
        'Dim TempWhere$ = String.Empty

        'TempWhere$ = TempWhere$ & IIf(theDevice <> String.Empty, IIf(TempWhere$ = String.Empty, " WHERE ", " AND ") & "[Device] =  '" & theDevice & "'", String.Empty)
        'TempWhere$ = TempWhere$ & IIf(theGroup <> String.Empty, IIf(TempWhere$ = String.Empty, " WHERE ", " AND ") & "[GROUP] =  '" & theGroup & "'", String.Empty)
        'TempWhere$ = TempWhere$ & IIf(theName <> String.Empty, IIf(TempWhere$ = String.Empty, " WHERE ", " AND ") & "[Name] =  '" & theName & "'", String.Empty)

        'SQL = "SELECT * FROM A_X4MemoryLocation " & TempWhere$ & " ORDER BY [Device],[Name]"
        'Rs = Cn.RunSQL(SQL)
        ''<+>============================================================<->
        '' clear all data
        'Clear()
        ''    Add  "<none>" ,......
        'If Not Rs Is Nothing Then
        '    With Rs
        '        Do While Not .EOF
        '            objNewMember = New ClsMemoryLocation
        '            With objNewMember

        '                .Device = Trim(String.Empty & Rs("Device").Value)
        '                .Group = Trim(String.Empty & Rs("Group").Value)
        '                .Name = String.Empty & Rs("Name").Value
        '                .Address = String.Empty & Rs("Address").Value
        '                .Cmd = String.Empty & Rs("Cmd").Value
        '                .State = IIf(IsDBNull(Rs("State").Value), -1, Rs("State").Value)

        '                .Device_LENGTH = Rs.Fields("Device").DefinedSize
        '                .Name_LENGTH = Rs.Fields("Name").DefinedSize
        '                .Address_LENGTH = Rs.Fields("Address").DefinedSize
        '                .Cmd_LENGTH = Rs.Fields("Cmd").DefinedSize
        '                .State_LENGTH = (Rs.Fields("State").DefinedSize * 3) - 1
        '                .ClassKey = Trim(.Device) & "_" & Trim(.Group) & "_" & Trim(.Name)

        '                mCol.Add(objNewMember, .ClassKey)

        '            End With


        '            objNewMember = Nothing

        '            .MoveNext()
        '        Loop
        '        Load = True
        '        .Close()
        '    End With

        'End If
        'Rs = Nothing
    End Function
    Public Function KeyTag(ByVal theName As String) As ClsMemoryLocation
        Try
            KeyTag = mCol(theName)
        Catch Ex As Exception
            'Lg.WriteLog(theDevice, "@ Error in KeyItem : Key=" & theDevice & "_" & theGroup & "_" & theName & " Err:" & Ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function KeyItem(ByVal theDevice As String, ByVal theGroup As String, ByVal theName As String) As ClsMemoryLocation
        Try
            KeyItem = mCol(theDevice & "_" & theGroup & "_" & theName)
        Catch Ex As Exception
            LG.WriteLog(theDevice, "@ Error in KeyItem : Key=" & theDevice & "_" & theGroup & "_" & theName & " Err:" & Ex.Message)
            Return Nothing
        End Try
    End Function


    Public Sub KeyRemove(ByVal theDevice As String, ByVal theGroup As String, ByVal theName As String)
        Try
            mCol.Remove(theDevice & "_" & theGroup & "_" & theName)
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.KeyRemove() Err:" & Ex.Message)
        End Try

    End Sub


    Public Function KeyMake(ByVal theDevice As String, ByVal theGroup As String, ByVal theName As String) As String 'ClsMemoryLocation 'Object
        Try
            KeyMake = theDevice & "_" & theGroup & "_" & theName
        Catch ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.KeyMake() Err:" & ex.Message)
            Return String.Empty
        End Try

    End Function



    Protected Overrides Sub Finalize()
        Try
            Fields_Key = Nothing
            Fields_List = Nothing
            Fields_DES = Nothing
            Fields_DEF_VAL = Nothing
            mCol = Nothing
        Catch Ex As Exception
            LG.WriteErr(m_strDevice, "@ Error in clsMemoryLocations.Finalize() Err:" & Ex.Message)
        End Try



        MyBase.Finalize()

    End Sub

    'Public Property OPCUpdateRate() As Integer
    '    Get
    '        Return m_intOPCUpdateRate
    '    End Get
    '    Set(ByVal Value As Integer)
    '        m_intOPCUpdateRate = Value
    '    End Set
    'End Property


    'Public Property EventsActive() As Boolean
    '    Get
    '        Return m_bEventsActive
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_bEventsActive = Value
    '    End Set
    'End Property

End Class

Public Class ClsMemoryLocation

    '=================================================================================================
    '
    ' Class clsMemoryLocation (Table: A_X4MemoryLocation)
    '
    '=================================================================================================
    ' Copyright © 1999-2020 MA AUTOMAZIONE of Massimo Mandelli
    ' by Mandelli M.     Date creation: 25/10/2004   Time: 15.00                         (°°°)
    '=================================================================================================
    ' Revision
    '
    '
    '=================================================================================================

    Public TheGrp As OpcGroup
    Private m_strGroup As String = String.Empty
    Private m_intHandleServer As Integer = 0
    Private m_objValue As Object = -1   '99
    Private LG As New clsLog
    Private EXIT_PROCESS As Boolean = False


    Private mvarClassKey As String = String.Empty
    Private mvarDevice As String = String.Empty
    Private mvarDevice_def As String = String.Empty
    Private mvarName As String = String.Empty
    Private mvarName_def As String = String.Empty
    Private mvarAddress As String = String.Empty
    Private mvarAddress_def As String = String.Empty
    Private mvarCmd As String = String.Empty
    Private mvarCmd_def As String = String.Empty
    Private mvarState As Integer = 0
    Private mvarState_def As Integer = 0

    '------------------------------------------------------------
    Private mvarDevice_LENGTH As Double = 0
    Private mvarDevice_TYP As String = String.Empty
    Private mvarDevice_FORMAT As String = String.Empty
    Private mvarName_LENGTH As Double = 0
    Private mvarName_TYP As String = String.Empty
    Private mvarName_FORMAT As String = String.Empty
    Private mvarAddress_LENGTH As Double = 0
    Private mvarAddress_TYP As String = String.Empty
    Private mvarAddress_FORMAT As String = String.Empty
    Private mvarCmd_LENGTH As Double = 0
    Private mvarCmd_TYP As String = String.Empty
    Private mvarCmd_FORMAT As String = String.Empty
    Private mvarState_LENGTH As Double = 0
    Private mvarState_TYP As String = String.Empty
    Private mvarState_FORMAT As String = String.Empty




    Public Function GetStrValue() As String
        Return String.Format("OPC:{0}:{1}", Name, Value)
    End Function



    Dim mvarDataValue As Object



    Public Property DataValue() As Object
        Get
            Return mvarDataValue
        End Get
        Set(ByVal Value As Object)
            mvarDataValue = Value
        End Set
    End Property

    Dim mvarQuality As Short
    Public Property Quality() As Short
        Get
            Return mvarQuality
        End Get
        Set(ByVal Value As Short)
            mvarQuality = Value
        End Set
    End Property

    Dim mvarError As Integer
    Public Property [Error]() As Integer
        Get
            Return mvarError
        End Get
        Set(ByVal Value As Integer)
            mvarError = Value
        End Set
    End Property
    Dim mvarHandleClient As Integer
    Public Property HandleClient() As Integer
        Get
            Return mvarHandleClient
        End Get
        Set(ByVal Value As Integer)
            mvarHandleClient = Value
        End Set
    End Property
    Dim mvarTimeStamp As Long
    Public Property TimeStamp() As Long
        Get
            Return mvarTimeStamp
        End Get
        Set(ByVal Value As Long)
            mvarTimeStamp = Value
        End Set
    End Property

    'Dim mvarDataType As Object
    'Public Property DataType() As Object
    '    Get
    '        Return mvarDataType
    '    End Get
    '    Set(ByVal Value As Object)
    '        mvarDataType = Value
    '    End Set
    'End Property

    '--------------------------------------------------
    ' ClassKey
    '--------------------------------------------------
    Public Property ClassKey() As String
        Get
            ClassKey = mvarClassKey
        End Get
        Set(ByVal Value As String)
            mvarClassKey = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Device_def
    '--------------------------------------------------
    Public Property Device_def() As String
        Get
            Device_def = mvarDevice_def
        End Get
        Set(ByVal Value As String)
            mvarDevice_def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Device_LENGTH
    '--------------------------------------------------
    Public Property Device_LENGTH() As Double
        Get
            Device_LENGTH = mvarDevice_LENGTH
        End Get
        Set(ByVal Value As Double)
            mvarDevice_LENGTH = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Device_TYP
    '--------------------------------------------------
    Public Property Device_TYP() As String
        Get
            Device_TYP = mvarDevice_TYP
        End Get
        Set(ByVal Value As String)
            mvarDevice_TYP = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Device_FORMAT
    '--------------------------------------------------
    Public Property Device_FORMAT() As String
        Get
            Device_FORMAT = mvarDevice_FORMAT
        End Get
        Set(ByVal Value As String)
            mvarDevice_FORMAT = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Name_def
    '--------------------------------------------------
    Public Property Name_def() As String
        Get
            Name_def = mvarName_def
        End Get
        Set(ByVal Value As String)
            mvarName_def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Name_LENGTH
    '--------------------------------------------------
    Public Property Name_LENGTH() As Double
        Get
            Name_LENGTH = mvarName_LENGTH
        End Get
        Set(ByVal Value As Double)
            mvarName_LENGTH = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Name_TYP
    '--------------------------------------------------
    Public Property Name_TYP() As String
        Get
            Name_TYP = mvarName_TYP
        End Get
        Set(ByVal Value As String)
            mvarName_TYP = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Name_FORMAT
    '--------------------------------------------------
    Public Property Name_FORMAT() As String
        Get
            Name_FORMAT = mvarName_FORMAT
        End Get
        Set(ByVal Value As String)
            mvarName_FORMAT = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Address_def
    '--------------------------------------------------
    Public Property Address_def() As String
        Get
            Address_def = mvarAddress_def
        End Get
        Set(ByVal Value As String)
            mvarAddress_def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Address_LENGTH
    '--------------------------------------------------
    Public Property Address_LENGTH() As Double
        Get
            Address_LENGTH = mvarAddress_LENGTH
        End Get
        Set(ByVal Value As Double)
            mvarAddress_LENGTH = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Address_TYP
    '--------------------------------------------------
    Public Property Address_TYP() As String
        Get
            Address_TYP = mvarAddress_TYP
        End Get
        Set(ByVal Value As String)
            mvarAddress_TYP = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Address_FORMAT
    '--------------------------------------------------
    Public Property Address_FORMAT() As String
        Get
            Address_FORMAT = mvarAddress_FORMAT
        End Get
        Set(ByVal Value As String)
            mvarAddress_FORMAT = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Cmd_def
    '--------------------------------------------------
    Public Property Cmd_def() As String
        Get
            Cmd_def = mvarCmd_def
        End Get
        Set(ByVal Value As String)
            mvarCmd_def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Cmd_LENGTH
    '--------------------------------------------------
    Public Property Cmd_LENGTH() As Double
        Get
            Cmd_LENGTH = mvarCmd_LENGTH
        End Get
        Set(ByVal Value As Double)
            mvarCmd_LENGTH = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Cmd_TYP
    '--------------------------------------------------
    Public Property Cmd_TYP() As String
        Get
            Cmd_TYP = mvarCmd_TYP
        End Get
        Set(ByVal Value As String)
            mvarCmd_TYP = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Cmd_FORMAT
    '--------------------------------------------------
    Public Property Cmd_FORMAT() As String
        Get
            Cmd_FORMAT = mvarCmd_FORMAT
        End Get
        Set(ByVal Value As String)
            mvarCmd_FORMAT = Value
        End Set
    End Property

    '--------------------------------------------------
    ' State_def
    '--------------------------------------------------
    Public Property State_def() As Integer
        Get
            State_def = mvarState_def
        End Get
        Set(ByVal Value As Integer)
            mvarState_def = Value
        End Set
    End Property

    '--------------------------------------------------
    ' State_LENGTH
    '--------------------------------------------------
    Public Property State_LENGTH() As Double
        Get
            State_LENGTH = mvarState_LENGTH
        End Get
        Set(ByVal Value As Double)
            mvarState_LENGTH = Value
        End Set
    End Property

    '--------------------------------------------------
    ' State_TYP
    '--------------------------------------------------
    Public Property State_TYP() As String
        Get
            State_TYP = mvarState_TYP
        End Get
        Set(ByVal Value As String)
            mvarState_TYP = Value
        End Set
    End Property

    '--------------------------------------------------
    ' State_FORMAT
    '--------------------------------------------------
    Public Property State_FORMAT() As String
        Get
            State_FORMAT = mvarState_FORMAT
        End Get
        Set(ByVal Value As String)
            mvarState_FORMAT = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Device
    '--------------------------------------------------
    Public Property Device() As String
        Get
            Device = mvarDevice
        End Get
        Set(ByVal Value As String)
            mvarDevice = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Name
    '--------------------------------------------------
    Public Property Name() As String
        Get
            Name = mvarName
        End Get
        Set(ByVal Value As String)
            mvarName = Value
        End Set
    End Property
    Dim mvarFullName As String

    Public Property FullName() As String
        Get
            mvarFullName = mvarDevice & "." & mvarName
            FullName = mvarFullName
        End Get
        Set(ByVal Value As String)
            mvarFullName = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Address
    '--------------------------------------------------
    Public Property Address() As String
        Get
            Address = mvarAddress
        End Get
        Set(ByVal Value As String)
            mvarAddress = Value
        End Set
    End Property

    '--------------------------------------------------
    ' Cmd
    '--------------------------------------------------
    Public Property Cmd() As String
        Get
            Cmd = mvarCmd
        End Get
        Set(ByVal Value As String)
            mvarCmd = Value
        End Set
    End Property

    '--------------------------------------------------
    ' State
    '--------------------------------------------------
    Public Property State() As Integer
        Get
            State = mvarState
        End Get
        Set(ByVal Value As Integer)
            mvarState = Value
        End Set
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        'RemoveHandler TheGrp.ReadCompleted, AddressOf theGrp_ReadComplete
    End Sub

    Public Sub New()
        mvarDevice_LENGTH = 20
        mvarDevice_TYP = "T"
        mvarDevice_FORMAT = "!"
        mvarName_LENGTH = 30
        mvarName_TYP = "T"
        mvarName_FORMAT = "!"
        mvarAddress_LENGTH = 30
        mvarAddress_TYP = "T"
        mvarAddress_FORMAT = "!"
        mvarCmd_LENGTH = 20
        mvarCmd_TYP = "T"
        mvarCmd_FORMAT = "!"
        mvarState_LENGTH = 11                                       ' ([len] * 3 (255))-1 
        mvarState_TYP = "N"
        mvarState_FORMAT = "##########0"

        'AddHandler TheGrp.ReadCompleted, AddressOf theGrp_ReadComplete
        '------ Default setting -------  
    End Sub

    Sub theGrp_ReadComplete(ByVal source As Object, ByVal e As ReadCompleteEventArgs)
        ' Debug.print("ReadComplete event: " + e.transactionID.ToString())
        Try
            Dim s As OPCItemState

            'lg.WriteLog("theGrp_ReadComplete : " & m_strOPCGroup)
            For Each s In e.sts
                If s.Error Then
                    Debug.Print("  Handle:" + s.HandleClient.ToString() + " !ERROR:0x" + s.Error.ToString("X"))
                Else
                    'mCol.Item(s.HandleClient).VALUE = s.DataValue.ToString()
                    'Debug.print("  Handle:" + s.HandleClient.ToString() + " Value:" + s.DataValue.ToString())
                End If
            Next
        Catch Ex As Exception
            Debug.Print("@ Error in theGrp_ReadComplete() Err:" & Ex.Message)
        End Try

    End Sub

    '--------------------------------------------------
    ' DELETE 
    '--------------------------------------------------
    Public Function Delete() As String
        'Dim Err$ = String.empty
        'Cn.RunSQL("DELETE * FROM A_X4MemoryLocation WHERE [Device]=" & Chr(39) & mvarDevice & Chr(39) & " AND [Name]=" & Chr(39) & mvarName & Chr(39) & String.empty, , Err$)
        'Delete = Err$
    End Function

    '--------------------------------------------------
    ' UPDATE 
    '--------------------------------------------------
    Public Function Update() As String
        'Dim Err$ = String.empty
        'Cn.RunSQL("UPDATE A_X4MemoryLocation  SET [Address]=" & Chr(39) & mvarAddress & Chr(39) & ",[Cmd]=" & Chr(39) & mvarCmd & Chr(39) & ",[State]=" & Str(mvarState) & " WHERE [Device]=" & Chr(39) & mvarDevice & Chr(39) & " AND [Name]=" & Chr(39) & mvarName & Chr(39) & String.empty, , Err$)
        'Update = Err$
    End Function

    '--------------------------------------------------
    ' INSERT 
    '--------------------------------------------------
    Public Function Insert() As String
        'Dim Err$ = String.empty
        'Cn.RunSQL("INSERT INTO A_X4MemoryLocation ([Device],[Name],[Address],[Cmd],[State]) VALUES (" & Chr(39) & mvarDevice & Chr(39) & "," & Chr(39) & mvarName & Chr(39) & "," & Chr(39) & mvarAddress & Chr(39) & "," & Chr(39) & mvarCmd & Chr(39) & "," & Str(mvarState) & ")", , Err)
        'Insert = Err$
    End Function

    '--------------------------------------------------
    ' LoadFromDB
    '--------------------------------------------------
    Public Function LoadFromDB() As Boolean
        'Dim Rs As ADODB.Recordset, SQL$
        'SQL = "SELECT * FROM A_X4MemoryLocation WHERE [Device]=" & Chr(39) & mvarDevice & Chr(39) & " AND [Name]=" & Chr(39) & mvarName & Chr(39) & String.empty

        'Rs = Cn.RunSQL(SQL)

        'If Not Rs Is Nothing Then
        '    With Rs
        '        If .RecordCount > 0 Then
        '            LoadFromDB = True
        '            mvarDevice = String.empty & String.empty & .Fields("Device").Value
        '            mvarName = String.empty & String.empty & .Fields("Name").Value
        '            mvarAddress = String.empty & String.empty & .Fields("Address").Value
        '            mvarCmd = String.empty & String.empty & .Fields("Cmd").Value
        '            mvarState = IIf(IsDBNull(.Fields("State").Value), -1, .Fields("State").Value)

        '        End If
        '        .Close()
        '    End With
        'End If
        'Rs = Nothing
    End Function

    Public Function Exist() As Boolean
        'Try
        '    Dim Rs As ADODB.Recordset, SQL$
        '    SQL = "select * from A_X4MemoryLocation WHERE [Device]=" & Chr(39) & mvarDevice & Chr(39) & " AND [Name]=" & Chr(39) & mvarName & Chr(39) & String.empty
        '    Rs = Cn.RunSQL(SQL)
        '    If Not Rs Is Nothing Then
        '        With Rs
        '            If .RecordCount > 0 Then
        '                Exist = True
        '            End If
        '            .Close()
        '        End With
        '    End If
        '    Rs = Nothing
        'Catch Ex As Exception
        'End Try
    End Function

    Public Property Value() As Object
        Get
            Return m_objValue
        End Get
        Set(ByVal theValue As Object)
            Try
                m_objValue = theValue
            Catch Ex As Exception
                LG.WriteErr(m_strGroup, "@ Error in clsMemoryLocation.Value() Err:" & Ex.Message)
                ' m_objValue =Object
            End Try

        End Set
    End Property


    Dim mvarDataType As Object
    Public Property OPCDataType() As Object
        Get
            Return mvarDataType
        End Get
        Set(ByVal Value As Object)
            mvarDataType = Value
        End Set
    End Property


    Public Function Write(ByVal theValue As Object) As Boolean
        Try
            Debug.Print("Write :" & mvarName & ":")
            Debug.Print(theValue)
            If theValue Is Nothing Then
                'Lg.WriteLog(m_strGroup, "Group: " & m_strGroup & " OPC Write Error! Name:<" & mvarName & "> the parameter <theValue> IS NULL")
                Exit Function
            End If
            Randomize() ' Initialize random-number generator.

            Dim IdTransaction As Integer = CInt(Int((55667788 * Rnd()) + 1))
            Dim tmpItemValues(0) As Object
            Dim tmpHandlesSrv(0) As Integer
            '     Debug.Print(OPCDataType)
            '    Debug.Print(mvarName)

            '  Debug.Print TypeOf(theValue) is
            Select Case TypeName(theValue)
                'Case "Boolean"
                'Case "Byte"
                'Case "Char"
                'Case "Date"
                'Case "DBNull"
                Case "Decimal", "Double", "Long", "Short", "Single", "UInteger", "ULong", "UShort"

                    ' Dim t As String = theValue.ToString
                    '  t = t.Replace(".", ",")
                    ' tmpItemValues(0) = CType(t, Decimal)
                    tmpItemValues(0) = CDec(theValue)
                    'Case "Double"
                    '    'Case "Integer"
                    '    'Case "Object"
                    'Case "Long"
                    '    'Case "Nothing"
                    '    'Case "SByte"
                    'Case "Short"
                    'Case "Single"
                    '    'Case "String"
                    'Case "UInteger"
                    'Case "ULong"
                    'Case "UShort"
                Case Else
                    tmpItemValues(0) = (theValue)
            End Select
            ' Debug.Print(OPCDataType)
            'DirectCast (

            tmpHandlesSrv(0) = m_intHandleServer

            Dim aError() As Integer = {0}
            Dim tmpCancelID As Integer = 0



            '<+>============================================================<->
            '  aggiunta perchè opc troppo lento...speriamo bene...
            ' m_objValue = theValue   ' <.> M.M. 06/03/2005 18:09:38  
            ' m_objValue = Convert.ChangeType(theValue, Type.GetTypeFromHandle(Type.GetTypeHandle(theValue)))

            'Debug.print("Group: " & m_strGroup & " Write <" & mvarName & " : cmd " & mvarCmd & "> Value: " & theValue)
            If TheGrp.Write(tmpHandlesSrv, tmpItemValues, IdTransaction, tmpCancelID, aError) = False Then
                'MsgBox("TheGrp.Write FALSE", MsgBoxStyle.Critical)
                Return False
            Else

                Debug.Print("")
                Return True
            End If


        Catch Ex As Exception
            Debug.Print(Ex.Message)

            'Lg.WriteLog(m_strGroup, "Group: " & m_strGroup & " OPC Write Error! Name:<" & mvarName & "> Value:<" & theValue & "> Err:" & Ex.Message)
            Return False
        End Try
    End Function

    Public Sub Read() 'As Object
        Try
            Randomize() ' Initialize random-number generator.

            Dim IdTransaction As Integer = CInt(Int((55667788 * Rnd()) + 1))
            Dim tmpItemValues(0) As Object
            Dim tmpHandlesSrv(0) As Integer

            tmpHandlesSrv(0) = m_intHandleServer

            Dim aError() As Integer = {0}
            Dim tmpCancelID As Integer


            'Debug.print("Group: " & m_strGroup & " Read <" & mvarName & " : cmd " & mvarCmd & "> Value: ")
            Debug.Print(TheGrp.Read(tmpHandlesSrv, IdTransaction, tmpCancelID, aError))

        Catch Ex As Exception
            LG.WriteLog(m_strGroup, "Group: " & m_strGroup & " OPC Read Error! <" & mvarName & "> : cmd " & mvarCmd & " Err:" & Ex.Message)
            'Return Nothing
        End Try
    End Sub


    Public Property HandleServer() As Integer
        Get
            Return m_intHandleServer
        End Get
        Set(ByVal Value As Integer)
            Try
                m_intHandleServer = Value
            Catch Ex As Exception
                LG.WriteLog(m_strGroup, "@ Error in HandleServer()  Err:" & Ex.Message)
            End Try

        End Set
    End Property

    Public Property Group() As String
        Get
            Return m_strGroup
        End Get
        Set(ByVal Value As String)
            Try
                m_strGroup = Value
            Catch Ex As Exception
                LG.WriteLog(m_strGroup, "@ Error in Group()  Err:" & Ex.Message)
            End Try

        End Set
    End Property


End Class
