Public Class ClassWP

    Public Property Weight As Decimal = 0

    Public WEIGHT_STABLE As Boolean = False

    Public Property MinWeight As Decimal = 0
    Public Property MaxWeight As Decimal = 0
    Public Property Precision As Decimal = 0
    Public Property Unit As String = "kg"
    Public Property WP As String = ""
    Public Property IP As String = ""
    Public Property Port As Integer = 0

    Public Property ST_WGT As Object
    Public Property SET_ZERO As Object
    Public Property SET_TARE As Object
    Public Property RESET_TARE As Object
    Public Property SET_FIX_TARE As Object
    Public Property VALUE_FIX_TARE As Object
    Public Property CALIBRATION_ACTIVE As Object
    Public Property DEVICE_IS_TARED As Object

    Public Property ScaleInterval As Object
    Public Property Exponent As Object
    Public Property Serialnumber As Object
    Public Property GROSS_WEIGHT As Object
    Public Property NET_WEIGHT As Object
    Public Property TARE_WEIGHT As Object
    Public Property CURRENT_WEIGHT As Object
    Public Property SWITCH_WEIGHT As Object


    Public Property OVERLOAD As String
    Public Property ERROR_7 As String
    Public Property ERROR_6 As String
    Public Property ERROR_9 As String
    Public Property ERROR_3 As String
    Public Property PowerFail As String


    '.Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A", 1)
    '.Add(m_strDevice, "_SYS", "ST_WGT_A", String.Empty, m_strDevice & ".ST_WGT_A_GROSS_STANDSTILL", 1)
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
    '.Add(m_strDevice, "_SYS", "Serialnumber_A", String.Empty, m_strDevice & ".MD6", 1)

    '.Add(m_strDevice, "_SYS", "GROSS_WEIGHT_A", String.Empty, m_strDevice & ".MD8", 1)
    '.Add(m_strDevice, "_SYS", "NET_WEIGHT_A", String.Empty, m_strDevice & ".MD9", 1)
    '.Add(m_strDevice, "_SYS", "TARE_WEIGHT_A", String.Empty, m_strDevice & ".MD10", 1)
    '.Add(m_strDevice, "_SYS", "CURRENT_WEIGHT_A", String.Empty, m_strDevice & ".MD11", 1)

    '.Add(m_strDevice, "_SYS", "SWITCH_WEIGHT_A", String.Empty, m_strDevice & ".MX72", 1)

    Public Function IsStable() As String
        Return IIf(WEIGHT_STABLE = True, "1", "0")
    End Function

    Public Function WGT_STR()
        Return String.Format("{0} {1}", Weight, Unit)
    End Function

    Public Function WGT_STR_OK()
        Try
            Debug.Print(Exponent)

            Dim Div As Integer = 10 ^ Exponent

            '  Dim tmpWgt As String = (CURRENT_WEIGHT / Div).ToString.Replace(",", ".")

            If DEVICE_IS_TARED = True Then
                Return (NET_WEIGHT / Div).ToString.Replace(",", ".")

            Else

                Return Weight.ToString.Replace(",", ".")
            End If
        Catch ex As Exception

        End Try


    End Function

    Public Function WGT_TARE()

        Debug.Print(Exponent)

        Dim Div As Integer = 10 ^ Exponent

        'Dim tmpWgt As String = (CURRENT_WEIGHT / Div).ToString.Replace(",", ".")

        If DEVICE_IS_TARED = True Then
            Return (TARE_WEIGHT / Div).ToString.Replace(",", ".")

        Else

            Return Weight.ToString.Replace(",", ".")
        End If
    End Function


    Public Function WGT_GROSS()

        Debug.Print(Exponent)

        Dim Div As Integer = 10 ^ Exponent

        Dim tmpWgt As String = (CURRENT_WEIGHT / Div).ToString.Replace(",", ".")


        Return Weight.ToString.Replace(",", ".")

    End Function
    Public Function get_Error() As String
        Dim S As String = ""
        S = OVERLOAD
        S = S + IIf(ERROR_7 = "", "", "+" + ERROR_7)
        S = S + IIf(ERROR_6 = "", "", "+" + ERROR_6)
        S = S + IIf(ERROR_9 = "", "", "+" + ERROR_9)
        S = S + IIf(ERROR_3 = "", "", "+" + ERROR_3)
        S = S + IIf(PowerFail = "", "", "+" + PowerFail)
        Return S
    End Function



End Class
