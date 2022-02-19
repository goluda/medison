Public Class WindowPoprawianieMiejscowosci



    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        szukaj()
    End Sub

    Class poprawianieMiejscowosci
        Implements ComponentModel.INotifyPropertyChanged


        Private _miejsocowsc As String
        Public Property Miejscowosc() As String
            Get
                Return _miejsocowsc
            End Get
            Set(ByVal value As String)
                _miejsocowsc = value
                notify("Miejscowosc")
            End Set
        End Property

        Private _kodPocztowy As String
        Public Property KodPocztowy() As String
            Get
                Return _kodPocztowy
            End Get
            Set(ByVal value As String)
                _kodPocztowy = value
                notify("KodPocztowy")
            End Set
        End Property

        Private _nowaMiejscowosc As String
        Public Property NowaMiejscowosc() As String
            Get
                Return _nowaMiejscowosc
            End Get
            Set(ByVal value As String)
                _nowaMiejscowosc = value
                notify("NowaMiejscowosc")
            End Set
        End Property

        Private _nowyKodPocztowy As String
        Public Property NowyKodPocztowy() As String
            Get
                Return _nowyKodPocztowy
            End Get
            Set(ByVal value As String)
                _nowyKodPocztowy = value
                notify("NowyKodPocztowy")
            End Set
        End Property

        Sub popraw()
            Dim db As New medisonEntities
            Dim temp = (From a In db.Pacjenci Where _
                       a.Miejscowosc = Me.Miejscowosc And _
                       a.Kod = Me.KodPocztowy _
                       ).ToList

            For Each o In temp
                o.Miejscowosc = Me.NowaMiejscowosc
                o.Kod = NowyKodPocztowy
            Next

            db.SaveChanges()
        End Sub

        Sub notify(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyName))
        End Sub
        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

    Sub szukaj()
        Dim db As New medisonEntities

        For Each o In db.Pacjenci.Where(Function(o1) o1.Miejscowosc Is Nothing)
            o.Miejscowosc = ""
        Next
        For Each o In db.Pacjenci.Where(Function(o1) o1.Kod Is Nothing)
            o.Kod = ""
        Next
        db.SaveChanges()

        Dim temp = From a In db.Pacjenci Select _
                 a.Miejscowosc, a.Kod Distinct _
                 Order By Miejscowosc _
                 Select New poprawianieMiejscowosci With _
                        {.Miejscowosc = Miejscowosc, .NowaMiejscowosc = Miejscowosc, .KodPocztowy = Kod, .NowyKodPocztowy = Kod}

        Me.DataGrid1.ItemsSource = temp.ToList
    End Sub

    Private Sub Popraw(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        Try
            Dim o As poprawianieMiejscowosci = sender.tag
            o.popraw()
            szukaj()
        Catch ex As Exception

        End Try
    End Sub
End Class
