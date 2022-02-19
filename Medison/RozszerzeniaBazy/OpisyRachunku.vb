Partial Public Class OpisyRachunku
    Implements System.ComponentModel.INotifyPropertyChanged

    Sub propChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged1 ', Me.PropertyChanged1
        Select Case e.PropertyName
            Case "Towary"
                przepiszTowar()
            Case "CenaNetto"
                przepiszCene()
            Case "Ilosc"
                przepiszCene()

            Case "NrTowaru"
                przepiszTowar()
        End Select
    End Sub





    Sub przepiszCene()
        If Me.Ilosc Is Nothing Then Ilosc = 0

        Try
            Dim WarNetto As Decimal = Me.CenaNetto * Me.Ilosc
            'Dim RoznicaNetto = WarNetto * 100 - Int(WarNetto * 100)
            'If RoznicaNetto < 1 / 2 Then
            '    Me.WartoscNetto = (Int(WarNetto * 100)) / 100
            'Else
            '    Me.WartoscNetto = ((Int(WarNetto * 100) + 1) / 100)
            'End If
            Me.WartoscNetto = WarNetto
            Dim WarVAT As Decimal = Me.WartoscNetto * Me.StawkaVAT
            'Dim RoznicaVAT = WarVAT * 100 - Int(WarVAT * 100)
            'If RoznicaVAT < 1 / 2 Then
            '    Me.WartoscVAT = (Int(WarVAT * 100)) / 100
            'Else
            '    Me.WartoscVAT = ((Int(WarVAT * 100) + 1) / 100)
            'End If
            Me.WartoscVAT = WarVAT
        Catch
        End Try
        Me.WartoscBrutto = Me.WartoscNetto + Me.WartoscVAT
        notify("CenaNetto")
        notify("Ilosc")
        notify("WartoscVAT")
        notify("WartoscNetto")
        notify("WartoscBrutto")
        notify("StawkaVatString")
        If Me.Rachunki IsNot Nothing Then Me.Rachunki.obliczBruttoNetto()
        'Me.Rachunki.obliczBruttoNetto()
    End Sub

    Public ReadOnly Property StawkaVatString As String
        Get
            If Me.ZwolnienieVAT = True Then
                Return "zw"
            Else
                Return String.Format("{0:P}", Me.StawkaVAT)
            End If
        End Get
    End Property

    Public Sub przepiszTowar()
        If Me.Towary IsNot Nothing Then
            Try
                Me.NazwaTowaru = Me.Towary.Nazwa + " " + Me.Towary.Wymiary
                Try
                    Me.ZwolnienieVAT = Me.Towary.ZwolnienieVAT
                Catch
                End Try
                Me.SWWKWiU = Me.Towary.SWWKWiU
                Me.StawkaVAT = Me.Towary.StawkaVAT
                Me.JedMiary = Me.Towary.JedMiary
                Me.CenaNetto = Me.Towary.CenaNettoSugerowana
                Me.ZwolnienieVAT = Me.Towary.ZwolnienieVAT
                If Me.Ilosc Is Nothing Then Me.Ilosc = 1
                If Me.Ilosc = 0 Then Me.Ilosc = 1

            Catch ex As Exception

            End Try
        End If

        notify("NrTowaru")
        notify("NazwaTowaru")
        notify("StawkaVAT")
        notify("ZwolnienieVAT")
        notify("SWWKWiU")
        notify("JedMiary")
        notify("CenaNetto")
        notify("Ilosc")
        notify("WartoscVAT")
        notify("WartoscNetto")
        notify("WartoscBrutto")
        notify("StawkaVatString")
    End Sub

    Sub notify(ByVal PropertyName As String)
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
