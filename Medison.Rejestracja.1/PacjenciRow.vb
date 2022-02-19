Partial Public Class DataSet1
    Partial Public Class WizytaRow
        Public ReadOnly Property aNazwisko As String
            Get
                If Me.IsNazwiskoNull = False Then
                    Return Me.Nazwisko
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aImie As String
            Get
                If Me.IsImieNull = False Then
                    Return Me.Imie
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property aImie2 As String
            Get
                If Me.IsImie2Null = False Then
                    Return Me.Imie2
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aPesel As String
            Get
                If Me.IsPeselNull = False Then
                    Return Me.Pesel
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aDataUrodzenia As String
            Get
                If Me.IsdataUrodzeniaNull = False Then
                    Return Me.data.Date.ToShortDateString
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aMiejscowosc As String
            Get
                If Me.IsMiejscowoscNull = False Then
                    Return Me.Miejscowosc
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aNrdomu As String
            Get
                If Me.IsNrDomuNull = False Then
                    Return Me.NrDomu
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aNrLokalu As String
            Get
                If Me.IsNrLokaluNull = False Then
                    Return Me.NrLokalu
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aTelefon As String
            Get
                If Me.IsTelefonNull = False Then
                    Return Me.Telefon
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aAdres As String
            Get
                If Me.IsAdresNull = False Then
                    Return Me.Adres + " " + aNrdomu + "/" + aNrLokalu
                Else
                    Return "" + " " + aNrdomu + "/" + aNrLokalu
                End If
            End Get
        End Property
        Public ReadOnly Property aUwagi As String
            Get
                If Me.IsuwagiNull = False Then
                    Return Me.uwagi
                Else
                    Return ""
                End If
            End Get
        End Property

        Private _dataWizyty As DateTime
        Public Property DataWizyty() As DateTime
            Get
                Return _dataWizyty
            End Get
            Set(ByVal value As DateTime)
                _dataWizyty = value
            End Set

        End Property


        'Public Property kod1() As String
        '    Get
        '        Try
        '            Return Me.kod
        '        Catch ex As Exception
        '            Return ""
        '        End Try
        '    End Get
        '    Set(ByVal value As String)
        '        Me.kod = value
        '    End Set
        'End Property



    End Class
    Partial Public Class PacjenciRow
        Public ReadOnly Property aNazwisko As String
            Get
                If Me.IsNazwiskoNull = False Then
                    Return Me.Nazwisko
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aImie As String
            Get
                If Me.IsImieNull = False Then
                    Return Me.Imie
                Else
                    Return ""
                End If
            End Get
        End Property
        Public ReadOnly Property aPesel As String
            Get
                If Me.IsPeselNull = False Then
                    Return Me.Pesel
                Else
                    Return ""
                End If
            End Get
        End Property

    End Class
End Class
Partial Public Class Pacjenci
    Private _dataWizyty As DateTime
    Public Property DataWizyty() As DateTime
        Get
            Return _dataWizyty
        End Get
        Set(ByVal value As DateTime)
            _dataWizyty = value
        End Set

    End Property
    Public ReadOnly Property aImie As String
        Get
            If Me.Imie IsNot Nothing Then
                Return Me.Imie
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aNazwisko As String
        Get
            If Me.Nazwisko IsNot Nothing Then
                Return Me.Nazwisko
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aPesel As String
        Get
            If Me.Pesel IsNot Nothing Then
                Return Me.Pesel
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aDataUrodzenia As String
        Get
            If Me.DataUrodzenia IsNot Nothing Then
                Return Me.DataUrodzenia.Value.Date.ToShortDateString
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aMiejscowosc As String
        Get
            If Me.Miejscowosc IsNot Nothing Then
                Return Me.Miejscowosc
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aNrdomu As String
        Get
            If Me.NrDomu IsNot Nothing Then
                Return Me.NrDomu
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aNrLokalu As String
        Get
            If Me.NrLokalu IsNot Nothing Then
                Return Me.NrLokalu
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property aTelefon As String
        Get
            If Me.Telefon IsNot Nothing Then
                Return Me.Telefon
            Else
                Return ""
            End If
        End Get
    End Property

    Public Property kod1() As String
        Get
            Try
                Return Me.kod
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(ByVal value As String)
            Me.kod = value
        End Set
    End Property
End Class