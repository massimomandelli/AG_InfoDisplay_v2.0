Imports System
Imports System.IO
Imports System.Threading

Public Class clsLog
    '=================================================================================================
    '
    ' File managment
    '
    '=================================================================================================
    ' Copyright © 1999-2020 MA AUTOMAZIONE of Massimo Mandelli
    ' by Mandelli M.     Date creation: 10/29/2004   Time: 17.15.09                         (°°°)
    '=================================================================================================
    ' Revision
    '
    '
    '=================================================================================================
    Const DATE_FORMAT As String = "dd-MM-yyyy HH:mm:ss.fff"
    '0	Developer                     	1
    '1	Admin                         	1
    '2	Super User                    	1
    '3	User Midle                    	1
    '4	User Low                      	1
    '5	Entry level                   	1
    Public Enum eLogLevel As Integer
        Debug = 0
        Internal = 0
        System = 1
        HightUserInformation = 2
        MiddleUserInformation = 3
        LowUserInformation = 4
        AllUserInformation = 5
        SecurityLevelInformation = 20
    End Enum

    Public Enum eMessageType As Integer
        InfoMessage = 0
        AllertMessage = 1
        ErrorMessage = 2
        LogMessage = 3
        AckMessage = 80
        DB = 4

        X4Msg = 5
        X4MsgChgSts = 51

        Pwd = 6
        Cfg = 7
        Info = 8
        Wgt = 9

        Security = 10
        Time = 11
        About = 29

        TruckInfo = 39
        LoadInfo = 47
        LoadStart = 40
        LoadEnd = 41

        UnloadStart = 42
        UnloadEnd = 43
        UnloadInfo = 44

        LoadEvNoMaterial = 45
        LoadEvMaterialOk = 46

        Antenna = 50
        AntennaError = 51

    End Enum

    'Public FORMATLOG As String = "_dd_MM"
    'Public FORMATLOG As String = "_MMM_dd"
    Public FORMATLOG As String = "_MM_dd"

    'Public LocalLogObj As ctlLog
    Public ComputerName As String = String.Empty
    Private LogPath$ = String.Empty
    Private LogFileNmae As String = String.Empty
    Private strDay As String = String.Empty
    Private strDayTomorrow As String = String.Empty
    Private FileName As File
    Private tmpFileToday$ = String.Empty
    Private tmpFileTomorrow$ = String.Empty
    Private _tCheckFileLog As New Thread(AddressOf _CheckFileLog)

    Private EXIT_PROCESS As Boolean = False
    Public Sub New()
        Try

            LogPath$ = String.Format("{0}\log\LOG_{1}", My.Application.Info.DirectoryPath.Trim, My.Application.Info.ProductName)
            'LogPath$ = Application.StartupPath.Trim & "\log\LOG_" & Application.ProductName.ToString 'VB6.GetEXEName()
            Debug.Print(LogPath$)
            StartMessageWriter()

            'Debug.print(Application.ProductName.Trim())
            'CheckFile()
            '_tCheckFileLog.Name = "_tCheckFileLog"
            '_tCheckFileLog.Start()

        Catch ex As Exception
            WriteErr("debug", "Error xxx() Err:" & ex.Message)
        End Try

    End Sub


    Protected Overrides Sub Finalize()
        Try
            StopMessageWriter()
            FLAG_Exit = True
            '_tCheckFileLog.Abort()
        Catch ex As Exception
            WriteErr("debug", "Error xxx() Err:" & ex.Message)
        End Try

        MyBase.Finalize()
    End Sub

    Dim FLAG_Exit As Boolean = False
    Private Sub _CheckFileLog()
        Try
            WriteLog("clsLog", "Thread _CheckFileLog() Started")
            Do
                CheckFile()
                If EXIT_PROCESS = True Then Exit Do
                If FLAG_Exit = True Then Exit Do
                Thread.Sleep(60000)
            Loop
            WriteLog("clsLog", "Thread _CheckFileLog() Terminato")
        Catch Ex As Exception
            Debug.print("@ Error in _CheckFileLog() Err:" & Ex.Message)
        End Try
    End Sub
    Private Sub CheckFile()
        Try
            strDay = Format(Now, FORMATLOG).ToString
            strDayTomorrow = Format(DateAdd("d", 1, Now), FORMATLOG).ToString

            '<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            '< Delete tomorrow log file
            '<
            tmpFileTomorrow$ = LogPath$ & strDayTomorrow & ".TXT"

            '<+>============================================================<->
            '  If exist the tomorrow log i delete it!
            If File.Exists(tmpFileTomorrow$) = True Then
                File.Delete(tmpFileTomorrow$)
            End If
            tmpFileToday$ = LogPath$ & strDay & ".TXT"
        Catch Ex As Exception
            Debug.print("@ Error in CheckFile() Err:" & Ex.Message)
        End Try
    End Sub



    Public Function WriteErr(ByVal From As String, ByVal theText As String, Optional ByVal theLevel As eLogLevel = eLogLevel.AllUserInformation, Optional ByVal IDMessage As eMessageType = eMessageType.ErrorMessage) As String

        Try
            Return iWriteLog(theText, From, theLevel, IDMessage)
        Catch ex As Exception
            Debug.print("@ Error in WriteErr() Err:" & ex.Message)
            Return String.Empty
        End Try
    End Function

    Public Function WriteAllert(ByVal From As String, ByVal theText As String, Optional ByVal theLevel As eLogLevel = eLogLevel.AllUserInformation, Optional ByVal IDMessage As eMessageType = eMessageType.AllertMessage) As String
        Try
            Return iWriteLog(theText, From, theLevel, IDMessage)
        Catch ex As Exception
            Debug.print("@ Error in WriteAllert() Err:" & ex.Message)
            Return String.Empty
        End Try

    End Function

    Public Function WriteUsrMsg(ByVal From As String, ByVal theText As String, Optional ByVal theLevel As eLogLevel = eLogLevel.AllUserInformation, Optional ByVal IDMessage As eMessageType = eMessageType.InfoMessage) As String
        Try
            Return iWriteLog(theText, From, theLevel, IDMessage)
        Catch ex As Exception
            Debug.print("@ Error in WriteUsrMsg() Err:" & ex.Message)
            Return String.Empty
        End Try

    End Function

    Public Overloads Function WriteLog(ByVal theText As String) As String
        Try
            Return iWriteLog(theText)
        Catch ex As Exception
            Debug.print("@ Error in WriteLog(only text) Err:" & ex.Message)
            Return String.empty
        End Try

    End Function

    Public Overloads Function WriteLog(ByVal From As String, ByVal theText As String, Optional ByVal theLevel As eLogLevel = eLogLevel.Debug, Optional ByVal theTypeMsgCode As eMessageType = eMessageType.LogMessage, Optional ByVal theMessageToSpeek As String = "") As String
        Try
            Return iWriteLog(theText, From, theLevel, theTypeMsgCode, theMessageToSpeek)
        Catch ex As Exception
            Debug.print("clsLog.WriteLog @ Error in WriteLog(1) Err:" & ex.Message)
            Return String.Empty
        End Try

    End Function

    Public Function WriteToFile(ByVal theMessage As String) As Boolean

        Try
            CheckFile()
            Dim sw As StreamWriter

            If File.Exists(tmpFileToday$) = False Then
                sw = File.CreateText(tmpFileToday$)
            Else
                sw = File.AppendText(tmpFileToday$)
            End If


            sw.WriteLine(theMessage$)
            sw.Flush()
            sw.Close()
            Return True

        Catch ex As Exception
            'Lg.WriteErr("debug", "Error xxx() Err:" & ex.Message )
            Return False
        End Try

    End Function

    Private Function iWriteLog(ByVal theText As String, _
                      Optional ByVal From As String = "Sys", _
                      Optional ByVal theLevel As eLogLevel = eLogLevel.AllUserInformation, _
                      Optional ByVal theTypeMsgCode As eMessageType = eMessageType.LogMessage, _
                      Optional ByVal theMessageToSpeek As String = "") As String




        ' Exit Function




        Try

            Dim sw As StreamWriter

            Dim theMessage$ = String.Empty
            Dim MyDate As Date = Now
            Dim MyDateStr$ = Format(MyDate, DATE_FORMAT)
            Debug.print(Environment.MachineName.Trim & " " & From & " " & Environment.UserName.Trim & " " & MyDate.ToShortDateString & " " & theTypeMsgCode.ToString & " " & theText.Trim & " " & theLevel.ToString)

            CheckFile()

            theMessage$ = MyDateStr & " [" & theLevel & "," & theTypeMsgCode & "] [" & From & "] " & theText

            If File.Exists(tmpFileToday$) = False Then
                sw = File.CreateText(tmpFileToday$)
            Else
                sw = File.AppendText(tmpFileToday$)
            End If


            sw.WriteLine(theMessage$)
            sw.Flush()
            sw.Close()

            iWriteLog = theMessage

            'LocalLogObj.WriteLog(theMessage)
            'Environment.UserName()
            'Environment.MachineName()

            'If From = string.empty Then
            '    From = "SYS"
            'End If

            Try
                'If Not (User Is Nothing) Then
                '    If theLevel >= User.UserInfo.Level Then
                '        LocalLogObj.WriteLog(MyDate, From, theLevel, theText, theTypeMsgCode, theMessageToSpeek)
                '    End If
                'End If
            Catch Ex As Exception
                WriteErr("iWriteLog", "clsLog.iWriteLog(0) @ Error iWriteLog() Err:" & Ex.Message)
            End Try


            Try


                'If theLevel > eLogLevel.Debug Then
                '    theText = Replace(theText.Trim, "'", "^")
                '    theText = Replace(theText.Trim, Chr(34), "^^")

                '    Dim SQL As String = String.Empty
                '    SQL = "INSERT INTO S_LOG (device,stationid,userid,[DATATIME],loglevel,des,idevent,state) VALUES ('" & From & "','" & Environment.MachineName & "','" & Environment.UserName.Trim & "', getdate()," & theLevel & ",'" & theText.Trim & "', " & theTypeMsgCode & ",1)"
                '    If myMsgCollection.Count > 1000 Then
                '        LocalLogObj.WriteLog(MyDate, "SYSTEM", 0, "Message > 1000", ctlLog.eMessageType.info, theMessageToSpeek)
                '    Else
                '        myMsgCollection.Add(SQL)
                '    End If
                'End If




                'Cn.SP_ToLog(Environment.MachineName.Trim, _
                '                   From, Environment.UserName.Trim, _
                '                   MyDate, _
                '                   CInt(theTypeMsgCode), _
                '                   theText.Trim, _
                '                     1, CInt(theLevel))
                'End If
            Catch ex As Exception
                iWriteLog = "clsLog.iWriteLog(1) @ Error in WriteLog: " & ex.Message
                Debug.print(iWriteLog)
            End Try

            'CREATE procedure dbo.insertlog
            '	@StationId  char(30),		
            '	@device  char(30),
            '	@userid  char(30),
            '	@datatime datetime,
            '	@idevent smallint,
            '	@des  char(200),
            '	@state int,
            '	@LogLevel int


            '' Debug.print(Environment.MachineName.Trim & " " & From & " " & Environment.UserName.Trim & " " & MyDate.ToShortDateString & " " & theTypeMsgCode.ToString & " " & theText.Trim & " " & theLevel.ToString)
        Catch Ex As Exception

            iWriteLog = "clsLog.iWriteLog(2) @ Error in WriteLog : " & ex.Message
            Debug.print(iWriteLog)
            'Lg.WriteErr("iWriteLog", "@ Error iWriteLog() Err:" & ex.Message )
            Return String.Empty
        End Try

    End Function
    Dim myMsgCollection As New Collection
    Dim _tMsgWriter As Thread


    Public Sub StartMessageWriter()
        Try

            If Not (_tMsgWriter Is Nothing) AndAlso _tMsgWriter.IsAlive = True Then Exit Sub

            _tMsgWriter = Nothing
            _tMsgWriter = New Thread(AddressOf MyMessageWriter)
            _tMsgWriter.Name = "_tMsgWriter"

            _tMsgWriter.Start()

            'Lg.WriteLog(m_strClassKey, "Start StartReadingProcess() OK")

        Catch Ex As Exception
            Debug.print("@ Error in StartMessageWriter() Err:" & Ex.Message)
        End Try
    End Sub
    Public Sub StopMessageWriter()

        Try
            bMyMessageWriter = True
            '_tMsgWriter.Abort()
            '_tMsgWriter.Join()
            '_tMsgWriter = Nothing
        Catch ex As Exception
            WriteErr("debug", "Error xxx() Err:" & ex.Message)
        End Try

    End Sub
    Dim bMyMessageWriter As Boolean = False
    Private Sub MyMessageWriter()

        'Try


        '    Dim tmpSql$ = String.Empty
        '    Do
        '        If FLAG_Exit = True Then Exit Do
        '        If bMyMessageWriter = True Then Exit Do

        '        If myMsgCollection.Count > 0 Then
        '            If Not (Cn Is Nothing) AndAlso Cn.State = ADODB.ObjectStateEnum.adStateOpen Then
        '                Do Until myMsgCollection.Count = 0
        '                    Try


        '                        tmpSql$ = tmpSql$ & myMsgCollection.Item(1) & ";" & vbCrLf

        '                        myMsgCollection.Remove(1)


        '                    Catch ex As Exception
        '                        Debug.Print("Error in tMessageWriter Err:" & ex.Message)
        '                    End Try
        '                Loop
        '            End If
        '            If tmpSql$ <> String.Empty Then
        '                'Cn.RunSQL(tmpSql$)
        '                SetDataSource(tmpSql$)
        '                Debug.Print(tmpSql)

        '            End If
        '            tmpSql$ = String.Empty

        '        End If
        '        Thread.Sleep(1000)   '200 12-06-08
        '    Loop
        'Catch ex As Exception
        '    Debug.print("Error in tMessageWriter Err:" & ex.Message)
        'End Try
        '_tMsgWriter = Nothing
    End Sub
    Private Sub MyMessageWriter_ok()

        'Try


        '    Dim tmpSql$ = String.Empty
        '    Do
        '        If FLAG_Exit = True Then Exit Do
        '        If bMyMessageWriter = True Then Exit Do

        '        If myMsgCollection.Count > 0 Then

        '            Try
        '                If Not (Cn Is Nothing) AndAlso Cn.State = ADODB.ObjectStateEnum.adStateOpen Then

        '                    tmpSql$ = myMsgCollection.Item(1)
        '                    If tmpSql$ <> String.Empty Then
        '                        'Cn.RunSQL(tmpSql$)
        '                        SetDataSource(tmpSql$)
        '                    End If
        '                    myMsgCollection.Remove(1)

        '                End If
        '            Catch ex As Exception
        '                Debug.Print("Error in tMessageWriter Err:" & ex.Message)
        '            End Try

        '        End If
        '        Thread.Sleep(250)   '200 12-06-08
        '    Loop
        'Catch ex As Exception
        '    Debug.Print("Error in tMessageWriter Err:" & ex.Message)
        'End Try
        '_tMsgWriter = Nothing
    End Sub
    Private Sub SetDataSource(ByVal query As String)
        'ByVal dataSet As DataSet, Optional ByVal nrCopy As Integer = 1
        'Try



        '    Const DATEFORMAT As String = "SET DATEFORMAT dmy" & Chr(13) & Chr(10)
        '    query = DATEFORMAT & query


        '    Dim oleConn As New OleDb.OleDbConnection(Cn.ConnectionString)
        '    Dim oleAdapter As New OleDb.OleDbDataAdapter
        '    'oleAdapter.SelectCommand = New OleDb.OleDbCommand(query, oleConn)

        '    oleConn.Open()
        '    oleAdapter.InsertCommand = New OleDb.OleDbCommand(query, oleConn)
        '    oleAdapter.InsertCommand.CommandType = CommandType.Text
        '    oleAdapter.InsertCommand.ExecuteNonQuery()
        '    oleConn.Close()

        'Catch Ex As Exception
        '    Lg.WriteErr("clsLog", "@ Error in Operation SetDataSource() Err:" & Ex.Message & " Source:" & Ex.Source)
        'End Try

    End Sub



    '    Public Function ToLog(ByVal Text As String)
    '        Dim strDAY$, strDayTomorrow$, FileName$

    '        On Error GoTo Errore

    '        '    If From = string.empty Then From = App.EXEName
    '        '    Dim Sql As String
    '        '    Text = Replace(Text, "'", "''")
    '        '    Sql = "insert into S_LOG (data,ID,MESSAGEFROM, MESSAGGIO,Stato,programname) VALUES (sysdate," & str$(IDMsg) & ",'" & From & "','" & Text & "',0,'" & GLB_NomeComputer & ":" & App.EXEName & "')"
    '        '    Call Cn.RunSQL(Sql)

    '    strDAY = Format(Date, "DD")
    '    strDayTomorrow = Format(DateAdd("d", 1, Date), "DD")

    '        '<^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    '        '< Delete tomorrow log file
    '        '<
    '        FileName$ = App.Path & "\log\LOG_" & App.EXEName & strDayTomorrow & ".TXT"

    '        If FileExists(FileName$) = True Then Kill(FileName)

    '        FileName$ = App.Path & "\log\LOG_" & App.EXEName & strDAY & ".TXT"

    'Retry:

    '    Open FileName$ For Append Shared As #1
    '        'Debug.Print Format(IDMsg, "@@@@@")
    '        'Print #1, Format(Date & " " & Time) & " " & GLB_NomeComputer & "  " & Format(IDMsg, "@@@@@") & " " & From & " " & Text
    '        Print #1, Format(Date & " " & Time) & " " & Text
    '    Close #1
    '        Exit Function
    'Errore:
    '        'Sleep 100
    '        'DoEvents
    'Debug.Print Err.Number; ex.Message  
    '        Select Case Err.Number
    '            Case 76 ' impossibile trovare il percorso
    '                Dim cmd$
    '                cmd = App.Path & "\log"
    '                MkDir(cmd)
    '            Case 55
    '        Close #1
    '        End Select

    '        Resume Retry
    '    End Function



End Class
