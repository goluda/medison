Partial Public Class Rachunki
    Implements System.ComponentModel.INotifyPropertyChanged

    Sub obliczBruttoNetto()
        Me.Netto = Me.OpisyRachunku.Sum(Function(o) o.WartoscNetto)
        Me.Brutto = Me.OpisyRachunku.Sum(Function(o) o.WartoscBrutto)

        notify("Brutto")
        notify("brutto1")
        notify("Netto")
    End Sub

    Public Sub odswierz()
        notify("NaKogoNIP")
        notify("NaKogoAdres")
        notify("NaKogoKod")
        notify("NaKogo")

    End Sub

    Private _fvOryginal As String = "Oryginał"
    Public Property FvOryginal() As String
        Get
            Return _fvOryginal
        End Get
        Set(ByVal value As String)
            _fvOryginal = value
        End Set
    End Property


    Private _info As String = ""
    Public Property Info() As String
        Get
            Return _info
        End Get
        Set(ByVal value As String)
            _info = value
        End Set
    End Property

    Public ReadOnly Property rocznik As Integer
        Get
            Try
                Return Me.DataWystawienia.Value.Year
            Catch
                Return 0


            End Try
        End Get
    End Property

    Public ReadOnly Property nrRachunku1 As String
        Get
            Return If(Numerrachunku IsNot Nothing, Numerrachunku.ToString, "") + If(EndFixNrRachunku IsNot Nothing, EndFixNrRachunku, "")
        End Get
    End Property
    Public ReadOnly Property Wystawil1 As String
        Get
            Return "Dodać kto wystawił!!!"
        End Get
    End Property
    Public ReadOnly Property brutto1 As Decimal
        Get
            Return Me.OpisyRachunku.Sum(Function(o) o.WartoscBrutto)
        End Get
    End Property

    Public ReadOnly Property RachunekTyp As String
        Get
            Select Case Me.TypRachunku
                Case "Rachunek"
                    Return "Rachunek nr:"
                Case "Faktura VAT"
                    Return "Faktura VAT nr:"
                Case "Faktura"
                    Return "Faktura nr:"

            End Select
            'Return If(Me.TypRachunku = "Rachunek", "Rachunek nr:", "Faktura VAT nr")
        End Get
    End Property
    Public ReadOnly Property RachunekPlatnosc As String
        Get
            'Return If(Me.TypRachunku = "Rachunek", "gotówką", If(Me.SposobZap IsNot Nothing, Me.SposobZap, "gotówką"))
            Return If(Me.SposobZap IsNot Nothing, Me.SposobZap, "gotówką")
        End Get
    End Property

    Public ReadOnly Property czyRachunek As Windows.Visibility
        Get
            Select Case Me.TypRachunku
                Case "Rachunek"
                    Return Windows.Visibility.Visible
                Case "Faktura VAT"
                    Return Windows.Visibility.Collapsed
                Case "Faktura"
                    Return Windows.Visibility.Visible

            End Select
            'Return If(Me.TypRachunku = "Rachunek", Windows.Visibility.Visible, Windows.Visibility.Collapsed)
        End Get
    End Property
    Public ReadOnly Property czyFaktura As Windows.Visibility
        Get
            Select Case Me.TypRachunku
                Case "Rachunek"
                    Return Windows.Visibility.Collapsed
                Case "Faktura VAT"
                    Return Windows.Visibility.Visible
                Case "Faktura"
                    Return Windows.Visibility.Collapsed

            End Select
            'Return If(Me.TypRachunku = "Rachunek", Windows.Visibility.Collapsed, Windows.Visibility.Visible)
        End Get
    End Property

    Public ReadOnly Property NumerrachunkuOpis As String
        Get
            Return If(Me.Numerrachunku IsNot Nothing, Numerrachunku.ToString, "") + If(EndFixNrRachunku IsNot Nothing, EndFixNrRachunku, "") + "/" + Me.DataWystawienia.Value.Year.ToString
        End Get
    End Property
    Public ReadOnly Property MiesiacSprzedazy As String
        Get
            Dim mies As String() = {"", "styczeń", "luty", "marzec", "kwiecień", "maj", "czerwiec", "lipiec", "sierpień", "wrzesień", "październik", "listopad", "grudzień"}
            Try
                Return mies(Me.DataWystawienia.Value.Month)
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Sub notify(ByVal PropertyName As String)
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub

    Public ReadOnly Property podsumowanieVAT As List(Of RachunkiPodsumowanie)
        Get
            Dim dd As New List(Of RachunkiPodsumowanie)

            Try
                dd = (From a In Me.OpisyRachunku Group a By a.StawkaVatString Into g = Group _
                    Select New RachunkiPodsumowanie With _
                           {.stawka = StawkaVatString, _
                            .wartoscNetto = g.Sum(Function(o) o.WartoscNetto), _
                            .wartoscVat = g.Sum(Function(o) o.WartoscVAT), _
                            .wartoscBrutto = g.Sum(Function(o) o.WartoscBrutto)}).ToList
            Catch ex As Exception

            End Try

            Return dd
        End Get
    End Property


    Public Property DataWystawieniaToString As String
        Get
            If DataWystawienia IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.DataWystawienia.Value.Day, Me.DataWystawienia.Value.Month, Me.DataWystawienia.Value.Year)
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Try
                value = value.Replace("-", "").Replace(".", "")
                Dim rok As Integer = Convert.ToInt32(value.Substring(4, 4))
                Dim miesiac As Integer = Convert.ToInt32(value.Substring(2, 2))
                Dim dzien As Integer = Convert.ToInt32(value.Substring(0, 2))
                Me.DataWystawienia = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWystawienia"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWystawieniaToString"))
            Catch
            End Try
        End Set
    End Property

    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
