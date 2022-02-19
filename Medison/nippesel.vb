Public Class nippesel
    Public Shared Function sprawdzPesel(ByVal pesel As String) As Boolean
        Dim dlugosc As Integer
        Dim i As Integer
        Dim test As Decimal
        Dim test1 As Decimal
        Dim kontrola As Integer
        Dim tekst As String
        tekst = pesel
        dlugosc = Len(tekst)
        If dlugosc = 11 Then
            kontrola = Val(Mid(tekst, 11, 1))
            test = Val(Mid(tekst, 1, 1)) * 1 + Val(Mid(tekst, 2, 1)) * 3 + Val(Mid(tekst, 3, 1)) * 7 + Val(Mid(tekst, 4, 1)) * 9 + Val(Mid(tekst, 5, 1)) * 1 + Val(Mid(tekst, 6, 1)) * 3 + Val(Mid(tekst, 7, 1)) * 7 + Val(Mid(tekst, 8, 1)) * 9 + Val(Mid(tekst, 9, 1)) * 1 + Val(Mid(tekst, 10, 1)) * 3
            test1 = Int(test / 10)
            Dim t1 As Integer = Convert.ToInt32(Right(Int(test).ToString, 1))
            Dim k1 = 10 - t1
            If k1 = 10 Then k1 = 0
            If k1 = kontrola Then
                Return True
            Else
                Return False
            End If


        Else
            Return False
        End If
    End Function

    Public Shared Function dataZPesel(ByVal pesel As String) As DateTime
        Try
            Dim rok As Integer = Convert.ToInt32(pesel.Substring(0, 2))
            Dim miesiac As Integer = Convert.ToInt32(pesel.Substring(2, 2))
            Dim dzien As Integer = Convert.ToInt32(pesel.Substring(4, 2))

            If miesiac > 80 Then
                rok = rok + 1800
                miesiac = miesiac - 80
            Else
                If miesiac > 20 Then
                    rok = rok + 2000
                    miesiac = miesiac - 20
                Else
                    rok = rok + 1900
                End If
            End If
            Return New DateTime(rok, miesiac, dzien)
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Shared Function sprawdzNIP(ByVal nip As String) As Boolean
        Dim dlugosc As Integer
        Dim i As Integer
        Dim test As Integer
        Dim test1 As Double
        Dim kontrola As Integer
        Dim tekst1
        Dim tekst2
        'usunięcie myślników
        nip = nip.Replace(" ", "").Replace("-", "")

        dlugosc = nip.Length
        If dlugosc = 10 Then
            kontrola = Val(Mid(nip, 10, 1))
            test = Val(Mid(nip, 1, 1)) * 6 + Val(Mid(nip, 2, 1)) * 5 + Val(Mid(nip, 3, 1)) * 7 + Val(Mid(nip, 4, 1)) * 2 + Val(Mid(nip, 5, 1)) * 3 + Val(Mid(nip, 6, 1)) * 4 + Val(Mid(nip, 7, 1)) * 5 + Val(Mid(nip, 8, 1)) * 6 + Val(Mid(nip, 9, 1)) * 7
            test1 = Int(test / 11)
            If test - test1 * 11 = kontrola Then
                Return True
            Else
                Return False
            End If


        Else
            Return False
        End If
    End Function
End Class
