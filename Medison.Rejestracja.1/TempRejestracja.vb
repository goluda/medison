Public Class TempRejestracja
    Implements System.ComponentModel.INotifyPropertyChanged

    Private _data As DateTime
    Public Property Data() As DateTime
        Get
            Return _data
        End Get
        Set(ByVal value As DateTime)
            _data = value
            notifyAll()
        End Set
    End Property
    Private _nrGabinetu As Integer
    Public Property nrGabinetu() As Integer
        Get
            Return _nrGabinetu
        End Get
        Set(ByVal value As Integer)
            _nrGabinetu = value
            notifyAll()
        End Set
    End Property
    Private _minut As Integer
    Public Property minut() As Integer
        Get
            Return _minut
        End Get
        Set(ByVal value As Integer)
            _minut = value
            notifyAll()
        End Set
    End Property

    Private _szczegoly As Windows.Visibility = Visibility.Collapsed
    Public Property szczegoly() As Windows.Visibility
        Get
            Return _szczegoly
        End Get
        Set(ByVal value As Windows.Visibility)
            _szczegoly = value
            notify("szczegoly")
        End Set
    End Property


    Public ReadOnly Property wysokosc As Integer
        Get
            Return 55
            'Return minut * 3
        End Get
    End Property

    Public ReadOnly Property godzina As String
        Get
            Return String.Format("{0:00}:{1:00}", Data.Hour, Data.Minute)

        End Get
    End Property

    Public ReadOnly Property kolor As Windows.Media.Brush
        Get
            If Module1.ostatniaDataWizyty = Me.Data Then
                Return New SolidColorBrush(Windows.Media.Colors.Orange)
            End If
            If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If Me.WizytaZBazy IsNot Nothing Then
                    If Me.WizytaZBazy.IsPotwierdzonaNull = True Then
                        Return New SolidColorBrush(Windows.Media.Colors.LightBlue)
                    Else
                        If Me.WizytaZBazy.Potwierdzona = False Then
                            Return New SolidColorBrush(Windows.Media.Colors.LightBlue)
                        Else
                            Return New SolidColorBrush(Windows.Media.Colors.LightGreen)
                        End If
                    End If
                Else
                    Return New SolidColorBrush(Windows.Media.Colors.White)
                End If
            Else
                If Me.WizytaMedison IsNot Nothing Then
                    If Me.WizytaMedison.potwierdzona Is Nothing Then
                        Return New SolidColorBrush(Windows.Media.Colors.LightBlue)
                    Else
                        If Me.WizytaMedison.potwierdzona = False Then
                            Return New SolidColorBrush(Windows.Media.Colors.LightBlue)
                        Else
                            Return New SolidColorBrush(Windows.Media.Colors.LightGreen)
                        End If
                    End If
                Else
                    Return New SolidColorBrush(Windows.Media.Colors.White)
                End If

            End If

        End Get
    End Property

    Private _nowa As Boolean
    Public Property nowa() As Boolean
        Get
            Return _nowa
        End Get
        Set(ByVal value As Boolean)
            _nowa = value
            notifyAll()
        End Set
    End Property

    Private _wizytaZBazy As DataSet1.WizytaRow
    Public Property WizytaZBazy() As DataSet1.WizytaRow
        Get
            Return _wizytaZBazy
        End Get
        Set(ByVal value As DataSet1.WizytaRow)
            _wizytaZBazy = value
            notifyAll()
        End Set
    End Property

    Private _WizytaMedison As Wizyty
    Public Property WizytaMedison() As Wizyty
        Get
            Return _WizytaMedison
        End Get
        Set(ByVal value As Wizyty)
            _WizytaMedison = value
            notifyAll()
        End Set
    End Property


    Public ReadOnly Property wolna As Boolean
        Get
            If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                If WizytaMedison Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property pokazWolna As Windows.Visibility
        Get
            If wolna = True Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Get
    End Property
    Public ReadOnly Property pokazZajeta As Windows.Visibility
        Get
            If wolna = False Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Get
    End Property

    Public Sub notifyAll()
        notify("Data")
        notify("nrGabinetu")
        notify("godzina")
        notify("minut")
        notify("nowa")
        notify("WizytaZBazy")
        notify("wysokosc")
        notify("wolna")
        notify("pokazWolna")
        notify("pokazZajeta")
    End Sub

    Private _godzinaBrush As Windows.Media.Brush
    Public Property godzinaBrush() As Windows.Media.Brush
        Get
            Return _godzinaBrush
        End Get
        Set(ByVal value As Windows.Media.Brush)
            _godzinaBrush = value
            notify("godzinaBrush")
        End Set
    End Property

    Public ReadOnly Property aNazwisko As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aNazwisko
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.Nazwisko
                Else
                    Return ""
                End If
            End If
            
        End Get
    End Property
    Public ReadOnly Property aImie As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aImie
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.Imie
                Else
                    Return ""
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property aImie2 As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aImie2
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.Imie2
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aPesel As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aPesel
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return If(WizytaMedison.Pacjenci.Pesel IsNot Nothing, WizytaMedison.Pacjenci.Pesel, "")
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aDataUrodzenia As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aDataUrodzenia
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return If(WizytaMedison.Pacjenci.DataUrodzenia IsNot Nothing, WizytaMedison.Pacjenci.DataUrodzenia.Value.ToShortDateString, "")
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aMiejscowosc As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aMiejscowosc
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.Miejscowosc
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aNrdomu As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aNrdomu
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.NrDomu
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aNrLokalu As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aNrLokalu
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.NrLokalu
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aTelefon As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aTelefon
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Pacjenci.Telefon
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aAdres As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aAdres
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return If(WizytaMedison.Pacjenci.Adres IsNot Nothing, WizytaMedison.Pacjenci.Adres + " ", "") + _
                        If(WizytaMedison.Pacjenci.NrDomu IsNot Nothing, WizytaMedison.Pacjenci.NrDomu + "/", "") + _
                        If(WizytaMedison.Pacjenci.NrLokalu IsNot Nothing, WizytaMedison.Pacjenci.NrLokalu, "")
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property aUwagi As String
        Get
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                If WizytaZBazy IsNot Nothing Then
                    Return WizytaZBazy.aUwagi
                Else
                    Return ""
                End If
            Else
                If WizytaMedison IsNot Nothing Then
                    Return WizytaMedison.Uwagi
                Else
                    Return ""
                End If
            End If
        End Get
    End Property

    Sub notify(propertyName As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
