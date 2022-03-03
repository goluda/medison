Public Class UserControlListaRachunkow
    Implements MyUserControl

    Dim db As New medisonEntities

    Dim danePacjentow As Windows.Data.BindingListCollectionView

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        setInfo("Ładowanie listy pacjentów, proszę czekać...")

        'danePacjentow.SortDescriptions.Add(New System.ComponentModel.SortDescription("Nazwisko", ComponentModel.ListSortDirection.Ascending))
        'danePacjentow.SortDescriptions.Add(New System.ComponentModel.SortDescription("Imie", ComponentModel.ListSortDirection.Ascending))
        szukaj()

    End Sub

    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose
        'db.Dispose()
        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)
        'MsgBox(nowaData)
    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)
        szukaj()
    End Sub

   

    Private Sub szukaj()
        setInfo("Ładowanie listy rachunków")
        Dim rach = (From o In db.Rachunki Where o.NrGabinetu = dane.WybranyLekarz.NrGabinetu).ToList
        Me.DataContext = From o In rach
                         Order By o.rocznik Descending, Convert.ToInt32(o.Numerrachunku) Descending
        setInfo("Lista rachunków załadowana")
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

    
    
    Private Sub WyswietlRachunek(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim r As Rachunki = sender.tag
        loadModule(New UserControlRachunekSzczegoly(db, r, Me, False))
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub

    Private Sub Usun(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim w As Rachunki = sender.tag

            If MsgBox("Czy usunąć tą wizytę?", vbYesNo) = vbYes Then

                db.Rachunki.Remove(w)
                db.SaveChanges()
                szukaj()
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
