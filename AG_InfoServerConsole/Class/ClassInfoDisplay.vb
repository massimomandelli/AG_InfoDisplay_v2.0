Public Class ClassInfoDisplay
    Const Prefix As String = "#"
    Const EOT As String = "@"
    Const Sep As String = "§"

    Public Sub New(ByVal Id As Integer)
        Try
            Me.Id = Id
        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Property Id As Integer

    Public Property SetPoint As Decimal = 0
    Public Property Weight As Decimal = 0

    Public Property Trigger As Decimal = 90
    Public Property Status As Int16 = 0
    Public Property Badge As String = ""
    Public Property Targa As String = ""
    Public Property Autista As String = ""
    Public Property Message As String = ""


    Public Property NET_WEIGHT_A As Decimal = 0
    Public Property NET_WEIGHT_B As Decimal = 0
    Public Property NET_WEIGHT_C As Decimal = 0
    Public Property SETPOINT_A As Decimal = 0
    Public Property SETPOINT_B As Decimal = 0
    Public Property SETPOINT_C As Decimal = 0
    Public Property TruckType As Integer = 0
    Public Property TruckPhase As Integer = 0
    Public Property ST_COMM_OK As Boolean

    Public Function GetMessage() As String
        Try
            Return Prefix + Id.ToString.Trim + Sep + SetPoint.ToString.Trim + Sep + Weight.ToString.Trim + Sep + Trigger.ToString.Trim + Sep + Badge + Sep + Targa + Sep + Autista + Sep + Message + EOT

        Catch ex As Exception

        End Try
    End Function

    Public Sub DecodeMessage(ByVal Message As String)
        Try
            If Message.Length > 2 And Message.Substring(0) = Prefix And Message.Substring(Message.Length) = EOT Then

                Dim s() As Object = Message.Split(Sep)

                Debug.Print("")

            End If


        Catch ex As Exception

        End Try
    End Sub

End Class
