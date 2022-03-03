Partial Public Class Pacjenci
    Implements System.ComponentModel.INotifyPropertyChanged

    Private _refundacja As Boolean = False
    Public Property refundacja() As Boolean
        Get
            Return _refundacja
        End Get
        Set(ByVal value As Boolean)
            _refundacja = value

        End Set
    End Property
    Public ReadOnly Property refundacjaVisible As Windows.Visibility
        Get
            Return If(refundacja = True, Windows.Visibility.Visible, Windows.Visibility.Hidden)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return If(Me.Nazwisko IsNot Nothing, Me.Nazwisko, "") + " " + If(Me.Imie IsNot Nothing, Me.Imie, "")
    End Function
    Public ReadOnly Property latTekst As String
        Get
            If Me.DataUrodzenia IsNot Nothing Then
                Return "Lat"
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property wiek As Integer
        Get
            Try
                Return Now.Year - Me.DataUrodzenia.Value.Year
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property NazwiskoWielkimiLiterami As String
        Get
            Return Nazwisko.ToUpper
        End Get
    End Property

    Public ReadOnly Property NazwiskoImie As String
        Get
            Return If(Me.Nazwisko IsNot Nothing, Me.Nazwisko, "") + " " + If(Me.Imie IsNot Nothing, Me.Imie, "")
        End Get
    End Property


    Public ReadOnly Property Recepties As Collections.ObjectModel.ObservableCollection(Of Recepty)
        Get
            Dim o As New Collections.ObjectModel.ObservableCollection(Of Recepty)
            For Each w In Me.Wizyty
                For Each l In w.Recepty
                    o.Add(l)
                Next
            Next
            Return o
        End Get
    End Property

    Public ReadOnly Property Adres1 As String
        Get
            If Me.Adres Is Nothing Then
                Return ""
            End If
            Dim przyd As String = "ul. "
            If Adres.ToLower.StartsWith("al.") Then przyd = ""
            If Adres.ToLower.StartsWith("ul.") Then przyd = ""
            If Adres.ToLower.StartsWith("plac ") Then przyd = ""
            If Adres.ToLower.StartsWith("rondo ") Then przyd = ""

            Return przyd + Me.Adres
        End Get
    End Property

    Public ReadOnly Property PelnyAdresPacjenta As String
        Get
            Return If(Me.Kod IsNot Nothing, Me.Kod, "") + " " + If(Me.Miejscowosc IsNot Nothing, Me.Miejscowosc, "") + " " + If(Me.Adres1 IsNot Nothing, Me.Adres1, "") + " " + If(Me.NrDomu IsNot Nothing, Me.NrDomu, "") + If(Me.NrLokalu IsNot Nothing, "/" + Me.NrLokalu, "")
        End Get
    End Property
    Public ReadOnly Property PelnyAdresPacjenta2 As String
        Get
            Return If(Me.Kod IsNot Nothing, Me.Kod, "") + " " + If(Me.Miejscowosc IsNot Nothing, Me.Miejscowosc, "") + vbCrLf + If(Me.Adres1 IsNot Nothing, Me.Adres1, "") + " " + If(Me.NrDomu IsNot Nothing, Me.NrDomu, "") + If(Me.NrLokalu IsNot Nothing, "/" + Me.NrLokalu, "")
        End Get
    End Property

    Public Sub odswierzMiejscowosci()
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Kod"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Miejscowosc"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Pesel"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzenia"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzeniaToString"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("NIP"))

    End Sub

    Public ReadOnly Property isReadOnly As Boolean
        Get
            Return Module1.isReadonly
        End Get
    End Property

    Public Property DataUrodzeniaToString As String
        Get
            If DataUrodzenia IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.DataUrodzenia.Value.Day, Me.DataUrodzenia.Value.Month, Me.DataUrodzenia.Value.Year)
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
                Me.DataUrodzenia = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzenia"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzeniaToString"))
            Catch
            End Try
        End Set
    End Property

    Public Property DataUrodzeniaOsobyPodOpToString As String
        Get
            If Me.DataUrodzeniaOsobyPodOp IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.DataUrodzeniaOsobyPodOp.Value.Day, Me.DataUrodzeniaOsobyPodOp.Value.Month, Me.DataUrodzeniaOsobyPodOp.Value.Year)
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
                Me.DataUrodzeniaOsobyPodOp = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzeniaOsobyPodOpToString"))

            Catch
            End Try
        End Set
    End Property

    Property miejscowoscID As String
        Get
            Try
                Return Me.Miejscowosc + "|" + Me.Kod
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            Try
                Dim ala = value.Split("|")
                Me.Miejscowosc = ala(0)
                Me.Kod = ala(1)
            Catch ex As Exception

            End Try

            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("miejscowoscID"))
            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Miejscowosc"))
            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Kod"))
        End Set
    End Property


    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
