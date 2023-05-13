Public Class UserControlListaPacjentow
    Implements MyUserControl

    Dim db As New medisonEntities


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        setInfo("Ładowanie listy pacjentów, proszę czekać...")

        'danePacjentow.SortDescriptions.Add(New System.ComponentModel.SortDescription("Nazwisko", ComponentModel.ListSortDirection.Ascending))
        'danePacjentow.SortDescriptions.Add(New System.ComponentModel.SortDescription("Imie", ComponentModel.ListSortDirection.Ascending))
        'Me.DataContext = danePacjentow
        setInfo("Lista pacjentów załadowana")
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

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        szukaj()
    End Sub

    Private Sub szukaj()
        Dim danePacjentow = ((From a In db.Pacjenci Order By a.Nazwisko, a.Imie Where
                                                                         If(Me.TextBoxImie.Text <> "", a.Imie.StartsWith(Me.TextBoxImie.Text), 1 = 1) _
                                                                         And If(Me.TextBoxNazwisko.Text <> "", a.Nazwisko.StartsWith(Me.TextBoxNazwisko.Text), 1 = 1)).ToList)
        DataContext = danePacjentow
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

    Private Sub SzczegolyPacjenta(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim p As Pacjenci = sender.tag
        Dim n As New UserControlSzczegolyPacjenta(db, p, Me, False)
        loadModule(n)
        'Dim w As New WindowSzczegolyPacjentaKrotkie(p)
        'w.ShowDialog()
        'db.SaveChanges()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim p As New Pacjenci
        'Dim n As New UserControlSzczegolyPacjenta(db, p, Me, True)
        'loadModule(n)
        Dim w As New WindowSzczegolyPacjentaKrotkie(p)
        w.ShowDialog()
        If MsgBox("Zapisać nowego pacjenta?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            db.Pacjenci.Add(p)
            db.SaveChanges()
        End If
    End Sub

    Private Sub TextBoxNazwisko_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBoxNazwisko.KeyDown, TextBoxImie.KeyDown
        If e.Key = Key.Enter Then
            szukaj()
        End If
    End Sub

    Private Sub DrukujKarte(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim p As Pacjenci = sender.tag
            'Dim a As New FixedDocument
            'Dim a1 As New Page
            'a.Pages.Add(p)
            'a1.
            Dim u As New UserControlWydrukKartaPacjenta(p)
            Module1.drukujVisual(u, "Wydruk karty: " + p.ToString, 500, 400)
            
        Catch
        End Try
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub

    Private Sub Usun(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If MsgBox("Czy usunąć pacjenta?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim p As Pacjenci = sender.tag
            If p.Wizyty.Count > 0 Then
                MsgBox("Pacjent ma już wizyty - nie można go usunąć")
                Exit Sub
            End If
            db.Pacjenci.Remove(p)
            db.SaveChanges()
            szukaj()
        End If
    End Sub

    Private Sub PolaczPacjentow(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim p As Pacjenci = sender.tag
            Dim w As New WindowPolaczPacjenta(p, db)
            w.ShowDialog()
            w = Nothing

        Catch ex As Exception

        End Try
    End Sub
End Class
