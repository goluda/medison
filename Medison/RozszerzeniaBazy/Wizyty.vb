Partial Public Class Wizyty
    Implements System.ComponentModel.INotifyPropertyChanged
    
    Public ReadOnly Property ImieINazwiskoLekarza As String
        Get
            Return If(Me.NazwiskoLekarza IsNot Nothing, Me.NazwiskoLekarza, "") + " " + If(Me.ImieLekarza IsNot Nothing, Me.ImieLekarza, "")
        End Get
    End Property
    Public ReadOnly Property L4Info As String
        Get
            If If(Me.ZwolnienieDni IsNot Nothing, Me.ZwolnienieDni, 0) > 0 Then
                Return Me.ZwolnienieDni.ToString + " L4"
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property blok As String
        Get
            If If(Me.L4 IsNot Nothing, Me.L4, False) = True Then
                Return "BLOK"
            Else
                Return ""
            End If
        End Get
    End Property

    Private _fvOryginal As String = "Oryginał"
    Public Property FvOryginal() As String
        Get
            Return _fvOryginal
        End Get
        Set(ByVal value As String)
            _fvOryginal = value
        End Set
    End Property


    Public ReadOnly Property roznicaCzasu As String
        Get
            If wyszedl IsNot Nothing Then
                Dim godz As Integer = GodzWizyty.Value.Hour * 100 + GodzWizyty.Value.Minute
                Dim t As Integer = Now.Hour * 100 + Now.Minute
                If godz < t Then
                    Dim test As Integer = godz - t
                    Dim h As Integer = Int(test / 100)
                    Dim m As Integer = test - h
                    Return New DateTime(1900, 1, 1, h, m, 0).ToShortTimeString
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        End Get
    End Property

    Public Property GodzWizytyToString As String
        Get
            Try
                Return GodzWizyty.Value.ToShortTimeString
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(ByVal value As String)
            Try
                If value.Contains(":") = True Then
                    Dim a As String() = value.Split(":")
                    GodzWizyty = New Date(1900, 1, 1, Convert.ToInt32(a(0)), Convert.ToInt32(a(1)), 0)
                Else
                    Dim minuty = Right(value, 2)
                    Dim godziny = value.Substring(0, value.Length - 2)
                    GodzWizyty = New Date(1900, 1, 1, Convert.ToInt32(godziny), Convert.ToInt32(minuty), 0)
                End If


            Catch ex As Exception

            End Try
            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("GodzWizytyToString"))
            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("GodzWizyty"))
        End Set
    End Property

    Public ReadOnly Property WszedlToString As String
        Get
            Try

                Return wszedl.Value.ToShortTimeString
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public ReadOnly Property TerazTenString As String
        Get
            Return If(If(Me.TerazTen IsNot Nothing, Me.TerazTen, False) = True, "O", "")
        End Get
        
    End Property

    Public Sub setTerazTen()
        If Me.TerazTen = True Then
            Me.TerazTen = False
        Else
            Me.TerazTen = True
        End If

        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("TerazTenString"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("TerazTen"))
    End Sub

    Public ReadOnly Property BackgroundColor As Windows.Media.SolidColorBrush
        Get
            If Me.DataWizyty.Value.Date = Now.Date Then
                Return New Windows.Media.SolidColorBrush(Windows.Media.Colors.Yellow)
            Else
                Return New Windows.Media.SolidColorBrush(Windows.Media.Colors.White)
            End If
        End Get
    End Property

    Public Sub odswierz()

        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Pacjenci"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("StatystycznaKlasyfikacjaChorob"))

        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Badania"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Rozpoznanie"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("usg"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Wywiad"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Zalecenia"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("NIPzakladu"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("NazwaZakladuPracy"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Wszedl"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("WszedlToString"))

        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("NrStatystycznyChoroby"))

        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("CzyDrukowac"))

    End Sub

    Public ReadOnly Property poprzednieL4 As List(Of Wizyty)
        Get
            Dim tt = (From a In Me.Pacjenci.Wizyty Where _
                    If(a.ZwolnienieDni IsNot Nothing, a.ZwolnienieDni, 0) > 0 _
                    Order By a.ZwolnienieOd Descending).ToList
            Return tt
        End Get
    End Property


    Public Sub odswierzDaty()
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWizyty"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOd"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOdToString"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieDo"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieDoToString"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieDni"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("WystawionoDnia"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("WystawionoDniaToString"))
        'RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWizyty"))
    End Sub

    Public Property DataWizytyToString As String
        Get
            If Me.DataWizyty IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.DataWizyty.Value.Day, Me.DataWizyty.Value.Month, Me.DataWizyty.Value.Year)
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Try
                Dim rok As Integer = Convert.ToInt32(value.Substring(4, 4))
                Dim miesiac As Integer = Convert.ToInt32(value.Substring(2, 2))
                Dim dzien As Integer = Convert.ToInt32(value.Substring(0, 2))
                Me.DataWizyty = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWizyty"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataWizytyToString"))
            Catch
            End Try
        End Set
    End Property

    'Public Property ZwolnienieOdToString As String
    '    Get
    '        If Me.ZwolnienieOd IsNot Nothing Then
    '            Return String.Format("{0:00}-{1:00}-{2:00}", Me.ZwolnienieOd.Value.Day, Me.ZwolnienieOd.Value.Month, Me.ZwolnienieOd.Value.Year)
    '        Else
    '            Return ""
    '        End If
    '    End Get
    '    Set(ByVal value As String)
    '        Try
    '            value = value.Replace("-", "").Replace(".", "")
    '            Dim rok As Integer = Convert.ToInt32(value.Substring(4, 4))
    '            Dim miesiac As Integer = Convert.ToInt32(value.Substring(2, 2))
    '            Dim dzien As Integer = Convert.ToInt32(value.Substring(0, 2))
    '            Me.ZwolnienieOd = New DateTime(rok, miesiac, dzien)
    '            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOd"))
    '            RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOdToString"))
    '        Catch
    '        End Try
    '    End Set
    'End Property

    Public Property ZwolnienieOdToString As String
        Get
            If Me.ZwolnienieOd IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.ZwolnienieOd.Value.Day, Me.ZwolnienieOd.Value.Month, Me.ZwolnienieOd.Value.Year)
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
                Me.ZwolnienieOd = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOd"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieOdToString"))
            Catch
            End Try
        End Set
    End Property

    Public Property DataUrodzeniaOsobyPodOpToString As String
        Get
            If Me.Pacjenci.DataUrodzeniaOsobyPodOp IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.Pacjenci.DataUrodzeniaOsobyPodOp.Value.Day, Me.Pacjenci.DataUrodzeniaOsobyPodOp.Value.Month, Me.Pacjenci.DataUrodzeniaOsobyPodOp.Value.Year)
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
                Me.Pacjenci.DataUrodzeniaOsobyPodOp = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Pacjenci.DataUrodzeniaOsobyPodOpToString"))

            Catch
            End Try
        End Set
    End Property

    Public Property ZwolnienieDoToString As String
        Get
            If ZwolnienieDo IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.ZwolnienieDo.Value.Day, Me.ZwolnienieDo.Value.Month, Me.ZwolnienieDo.Value.Year)
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
                Me.ZwolnienieDo = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieDo"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("ZwolnienieDoToString"))
            Catch
            End Try
        End Set
    End Property

    Public Property WystawionoDniaToString As String
        Get
            If WystawionoDnia IsNot Nothing Then
                Return String.Format("{0:00}-{1:00}-{2:00}", Me.WystawionoDnia.Value.Day, Me.WystawionoDnia.Value.Month, Me.WystawionoDnia.Value.Year)
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
                Me.WystawionoDnia = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("WystawionoDnia"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("WystawionoDniaToString"))
            Catch
            End Try
        End Set
    End Property

#Region "Dane do L4"
    Public Function rozstrzel(ByVal t As String) As String

        Dim temp As String = ""
        Dim t1 As String = If(t IsNot Nothing, t, " ")
        For Each a In t1.ToArray
            temp += a + " "
        Next
        Return temp
    End Function

    Public ReadOnly Property L4Pesel As String
        Get
            Return rozstrzel(Me.Pacjenci.Pesel)
        End Get
    End Property

    Public ReadOnly Property L4Imie As String
        Get
            Return rozstrzel(Me.Pacjenci.Imie).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4Nazwisko As String
        Get
            Return rozstrzel(Me.Pacjenci.Nazwisko).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4TypDokumentu As String
        Get
            Return rozstrzel(Me.TypyDokumentow.NazwaDokumentu).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4NumerDokumentu As String
        Get
            Select Case Me.NrTypuDokumentu
                Case 1
                    Return rozstrzel(Me.Pacjenci.NIP)
                Case 2
                    Return rozstrzel(Me.Pacjenci.Dowod)
                Case 3
                    Return rozstrzel(Me.Pacjenci.Paszport)
                Case Else
                    Return " "
            End Select
        End Get
    End Property

    Public ReadOnly Property L4ZwolnienieOd As String
        Get
            Try
                Return rozstrzel(String.Format("{0:00}{1:00}{2:0000}", ZwolnienieOd.Value.Day, ZwolnienieOd.Value.Month, ZwolnienieOd.Value.Year))
            Catch
                Return " "
            End Try
        End Get
    End Property

    Public ReadOnly Property L4ZwolnienieDo As String
        Get
            Try

                Return rozstrzel(String.Format("{0:00}{1:00}{2:0000}", ZwolnienieDo.Value.Day, ZwolnienieDo.Value.Month, ZwolnienieDo.Value.Year))
            Catch
                Return " "
            End Try
        End Get
    End Property

    Public ReadOnly Property L4ZwolnienieDni As String
        Get
            Return rozstrzel(Me.ZwolnienieDni.ToString).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4DataUrodzenia As String
        Get
            Try
                Return rozstrzel(String.Format("{0:00}{1:00}{2:0000}", Pacjenci.DataUrodzenia.Value.Day, Pacjenci.DataUrodzenia.Value.Month, Pacjenci.DataUrodzenia.Value.Year))
            Catch
                Return " "
            End Try
        End Get
    End Property

    Public ReadOnly Property L4NrStatystycznyChoroby As String
        Get
            Return rozstrzel(Me.NrStatystycznyChoroby).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4KodPokrewienstwa As String
        Get
            If If(Me.KodPokrewienstwa IsNot Nothing, Me.KodPokrewienstwa, 0) <> 0 Then
                Return Me.KodPokrewienstwa
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property L4KodPokrewienstwaData As String
        Get
            If If(Me.KodPokrewienstwa IsNot Nothing, Me.KodPokrewienstwa, 0) <> 0 Then
                Return rozstrzel(String.Format("{0:00}{1:00}{2:0000}", Pacjenci.DataUrodzeniaOsobyPodOp.Value.Day, Pacjenci.DataUrodzeniaOsobyPodOp.Value.Month, Pacjenci.DataUrodzeniaOsobyPodOp.Value.Year))
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property L4ImieLekarza As String
        Get
            Return rozstrzel(Me.ImieLekarza).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4NazwiskoLekarza As String
        Get
            Return rozstrzel(Me.NazwiskoLekarza).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4NrLekarza As String
        Get
            Try
                Return rozstrzel(Me.NrLekarza.Value.ToString).ToUpper
            Catch
            End Try
        End Get
    End Property

    Public ReadOnly Property L4WystawionoDnia As String
        Get
            Return rozstrzel(String.Format("{0:00}{1:00}{2:0000}", WystawionoDnia.Value.Day, WystawionoDnia.Value.Month, WystawionoDnia.Value.Year))
        End Get
    End Property

    Public ReadOnly Property L4NIPLekarza As String
        Get
            Try
                Return rozstrzel(Me.NIPLekarza.Replace("-", "")).ToUpper
            Catch
                Return ""
            End Try
        End Get
    End Property

    Public ReadOnly Property L4NIPZakladu As String
        Get
            Return rozstrzel(Me.NIPzakladu.Replace("-", "")).ToUpper
        End Get
    End Property

    Public ReadOnly Property L4NazwaZakladu As String
        Get
            Return Me.NazwaZakladuPracy.ToUpper
        End Get
    End Property

    Public ReadOnly Property L4PacjentKodPocztowy As String
        Get
            Try
                Return rozstrzel(Me.Pacjenci.Kod).Replace("-", "")
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property L4PacjentMiejscowosc As String
        Get
            Try
                Return rozstrzel(Me.Pacjenci.Miejscowosc).ToUpper
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property L4PacjentAdres As String
        Get
            Try
                Return rozstrzel(Me.Pacjenci.Adres.ToUpper.Replace("UL.", "").Trim)
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property L4PacjentNrDomu As String
        Get
            Try
                Return rozstrzel(Me.Pacjenci.NrDomu).ToUpper
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property L4PacjentNrLokalu As String
        Get
            Try
                Return rozstrzel(Me.Pacjenci.NrLokalu).ToUpper
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
#End Region


    Public ReadOnly Property isReadOnly As Boolean
        Get
            Return Module1.isReadonly
        End Get
    End Property

    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
