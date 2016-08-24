Public Class ClsOPCConfiguration
    Public Property FileName As String = "PR1792.cfg"
    Public Property FilePath As String = "C:\Program Files (x86)\Sartorius\PR1792\"

    Public Function GetItems() As List(Of String)


        Try
            Dim mItems As New List(Of String)

            Dim fileContents As String
            fileContents = My.Computer.FileSystem.ReadAllText(FilePath & FileName)
            Dim F_END As Boolean = False
            Dim Idx As Integer = 0
            Dim IdxE As Integer = 0
            Dim S As String = ""
            Dim IdxR As Integer = 1

            'GWT_OPCSERVER
            Do Until F_END

                Idx = InStr(IdxR, fileContents, "[", CompareMethod.Text)
                If Idx > 0 Then
                    IdxE = InStr(Idx, fileContents, "]", CompareMethod.Text)

                    S = fileContents.Substring(Idx, IdxE - Idx - 1)
                    Debug.Print(S)
                    If S <> "GWT_OPCSERVER" Then
                        mItems.Add(S)

                    End If
                    IdxR = IdxE + 1
                Else
                    F_END = True
                End If


            Loop

            Return mItems


        Catch ex As Exception

        End Try
    End Function



End Class
