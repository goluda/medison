Public Class UserControlRejestracja
    Implements MyUserControl

    Dim db As medisonEntities

    Dim wysokosc As Integer = 0

    Dim WithEvents tim As New System.Windows.Forms.Timer With {.Interval = 3000, .Enabled = False}

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Module1.isReadonly = False
        Me.GridInfo.DataContext = Module1.dane
        ladujDane()
        tim.Enabled = True
    End Sub

    Sub zapisz()

        db.SaveChanges()
    End Sub

    Sub ladujDane()
        Try

            setInfo("Ładuję dane dla: " + Module1.dane.WybranyLekarz.ToString + " dla dnia: " + Module1.dane.data.ToShortDateString)
            If db IsNot Nothing Then db.Dispose()
            db = New medisonEntities
            Me.ComboUSG.ItemsSource = db.Usg.ToList


            Dim dane1 = (From a In db.Wizyty.Include("Pacjenci") Where
                             a.DataWizyty = Module1.dane.data _
                             And a.NrGabinetu = dane.WybranyLekarz.NrGabinetu
                         Order By a.GodzWizyty.Value.Hour, a.GodzWizyty.Value.Minute).ToList
            For Each d In dane1
                If d.Wplata = 0 Then d.Wplata = Nothing
                If d.WplataKarta = 0 Then d.WplataKarta = Nothing

            Next
            Me.DataContext = dane1
            Try
                Me.LabelKwota.Content = String.Format("{0:c}", dane1.Sum(Function(o) o.Wplata))
            Catch ex As Exception
                Me.LabelKwota.Content = ""
            End Try

            If wybranaWizyta IsNot Nothing Then
                For Each o As Wizyty In DataGrid1.Items
                    If wybranaWizyta = o.NrWizyty Then Me.DataGrid1.SelectedItem = o : GoTo 1

                Next
            End If
1:
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ustawWielkoscGrida()

    End Sub
    Sub ustawWielkoscGrida()
        If wysokosc <> 0 Then

            Dim headerHeight As Integer = 26
            Dim nw As Integer = wysokosc - headerHeight - 70 - R1.Height.Value
            Try
                Dim rowHeight As Decimal = 25
                Dim iloscWierszy As Integer = nw / rowHeight
                'Dim iloscWierszy As Integer = 16
                Dim nw1 = (iloscWierszy - 1) * rowHeight + headerHeight

                If nw1 <> Me.DataGrid1.Height Then Me.DataGrid1.Height = nw1
            Catch ex As Exception

            End Try
        End If
    End Sub
    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose
        'db.Dispose()
        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza

        tim.Enabled = False
    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)
        'MsgBox(nowaData)
        ladujDane()
    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)
        ladujDane()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ladujDane()
    End Sub

    Sub pokazEdycja(ByVal r As Wizyty, ByVal nowa As Boolean)
        R1.Height = New Windows.GridLength(255)
        GridEdycja.Children.Clear()
        Dim c As New WindowNowaRezerwacja(r, db, nowa)
        AddHandler c.CloseWizyta, Sub()
                                      'If nowa = True Then
                                      '    db.Wizyty.Add(r)
                                      '    Me.DataGrid1.SelectedItem = r

                                      'End If

                                      'db.SaveChanges()
                                      wybranaWizyta = r.NrWizyty
                                      tim.Enabled = True

                                      R1.Height = New Windows.GridLength(1)
                                      GridEdycja.Children.Clear()
                                      ladujDane()


1:
                                  End Sub

        GridEdycja.Children.Add(c)
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        tim.Enabled = False
        Dim r As New Wizyty
        r.DataWizyty = dane.data.Date
        r.ImieLekarza = dane.WybranyLekarz.Imie
        r.NazwiskoLekarza = dane.WybranyLekarz.Nazwisko
        r.NIPLekarza = dane.WybranyLekarz.NIPlekarza
        r.NrGabinetu = dane.WybranyLekarz.NrGabinetu
        r.NrLekarza = dane.WybranyLekarz.NrLekarza
        r.TerazTen = False
        r.karta = False
        r.WplataKarta = 0
        r.L4 = False
        Dim w As New WindowNowaRezerwacja(r, db, True)
        'w.ShowDialog()

        'db.Wizyties.Add(r)
        'db.SaveChanges()

        'ladujDane()
        'tim.Enabled = True
        pokazEdycja(r, True)
        ustawWielkoscGrida()
    End Sub

    Private Sub SzczegolyPacjenta(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tim.Enabled = False
        Dim w As Wizyty = sender.tag
        wybranaWizyta = w.NrWizyty
        '        loadModule(New UserControlSzczegolyPacjenta(db, w.Pacjenci, Me, False))
        tim.Enabled = False
        Dim ww As New WindowSzczegolyPacjentaKrotkie(w.Pacjenci)
        ww.ShowDialog()
        db.SaveChanges()
        tim.Enabled = True
    End Sub

    Private Sub SzczegolyWizyty(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim w As Wizyty = sender.tag
        wybranaWizyta = w.NrWizyty
        If If(w.L4 IsNot Nothing, w.L4, False) = True Then
            MsgBox("Pacjent jest u lekarza")
            Exit Sub
        End If

        loadModule(New UserControlWizyta(db, w, Me))
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub

    Private Sub DrukujKwit(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            tim.Enabled = False
            Dim w As Wizyty = sender.tag
            If w.karta Is Nothing Then w.karta = False
            wybranaWizyta = w.NrWizyty
            If If(w.L4 IsNot Nothing, w.L4, False) = True Then
                MsgBox("Pacjent jest u lekarza")
                Exit Sub
            End If

            Dim test = If(w.Wplata IsNot Nothing, w.Wplata, 0) + If(w.WplataKarta IsNot Nothing, w.WplataKarta, 0)

            ' If If(w.Wplata IsNot Nothing, w.Wplata, 0) = 0 Then
            If test = 0 Then
                'w.Wplata = 150
                'w.Wplata = 200
                w.Wplata = 250

                db.SaveChanges()

                'Dim db1 As New medisonEntities
                'Try
                '    Dim ala = db1.Wizyty.Where(Function(o) o.NrWizyty = w.NrWizyty).First

                '    If ala.Wplata <> 120 Then ala.Wplata = 120

                '    db1.SaveChanges()
                'Catch ex As Exception

                'End Try
            End If

            Dim u As New UserControlWydrukKwit(w)
            drukujVisual(u, "Kwit: " + w.Pacjenci.ToString, 800, 400)
            ladujDane()
        Catch ex As Exception

        End Try
        tim.Enabled = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click

        db.SaveChanges()
        db = New medisonEntities
        ladujDane()
    End Sub

    Private Sub ZmienTerazTen(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim w As Wizyty = sender.tag
            wybranaWizyta = w.NrWizyty
            If If(w.L4 IsNot Nothing, w.L4, False) = True Then
                MsgBox("Pacjent jest u lekarza")
                Exit Sub
            End If

            w.setTerazTen()
            db.SaveChanges()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub L4(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            tim.Enabled = False
            Dim w As Wizyty = sender.tag
            wybranaWizyta = w.NrWizyty
            If If(w.L4 IsNot Nothing, w.L4, False) = True Then
                MsgBox("Pacjent jest u lekarza")
                Exit Sub
            End If
            Dim ww As New WindowL4(w, db)
            ww.ShowDialog()
            db.SaveChanges()
            tim.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BankWizyta(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim w As Wizyty = sender.tag
            If w.karta Is Nothing Then w.karta = False
            If w.karta = False Then
                w.karta = True
                w.WplataKarta = w.Wplata
                w.Wplata = 0
            Else
                w.karta = False
                w.Wplata = w.WplataKarta
                w.WplataKarta = 0
            End If
            db.SaveChanges()
            ladujDane()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub tim_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tim.Tick
        Try
            tim.Enabled = False
            db.SaveChanges()
            Dim p As Integer = DataGrid1.SelectedIndex
            db = New medisonEntities
            ladujDane()
            Try
                DataGrid1.SelectedIndex = p
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
        tim.Enabled = True
    End Sub

    Private Sub Edytuj(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        tim.Enabled = False
        Dim r As Wizyty = sender.tag
        wybranaWizyta = r.NrWizyty
        If If(r.L4 IsNot Nothing, r.L4, False) = True Then
            MsgBox("Pacjent jest u lekarza")
            Exit Sub
        End If
        'Dim w As New WindowNowaRezerwacja(r, db, False)
        'w.ShowDialog()
        'db.SaveChanges()
        'tim.Enabled = True
        pokazEdycja(r, False)
        ustawWielkoscGrida()
    End Sub

    'Private Sub Usun(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
    Private Sub Usun(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim w As Wizyty = sender.tag
            wybranaWizyta = Nothing
            tim.Enabled = False
            If MsgBox("Czy usunąć tą wizytę?", vbYesNo) = vbYes Then
                If If(w.ZwolnienieDni IsNot Nothing, w.ZwolnienieDni, 0) > 0 Then
                    If MsgBox("Wizyta ma L4 czy napewno chcesz ją usunąć?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        Exit Sub
                    End If
                End If
                db.Wizyty.Remove(w)
                db.SaveChanges()
                ladujDane()

            End If
            tim.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PokazHistorie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim w As Wizyty = sender.tag
            wybranaWizyta = w.NrWizyty
            tim.Enabled = False
            Dim uu As New UserControlSzczegolyPacjenta(db, w.Pacjenci, Me, False)
            loadModule(uu)


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.DataGrid1.SelectedItems.Count > 0 Then
            MsgBox("Musisz najpierw zaznaczyć wizyty, które chcesz przenieść")
        End If

        Dim w As New WindowData
        w.Calendar1.SelectedDate = dane.data
        w.ShowDialog()
        If w.wybranaData = True Then
            For Each ww As Wizyty In Me.DataGrid1.SelectedItems
                ww.DataWizyty = w.Calendar1.SelectedDate
            Next
            db.SaveChanges()
            ladujDane()
        End If
    End Sub

    Private Sub PrzeniesWizyte(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            tim.Enabled = False
            Dim ww As Wizyty = sender.tag
            wybranaWizyta = ww.NrWizyty

            Dim w As New WindowData
            w.Calendar1.SelectedDate = dane.data
            w.ShowDialog()
            If w.wybranaData = True Then

                ww.DataWizyty = w.Calendar1.SelectedDate

                db.SaveChanges()
                ladujDane()
            End If
        Catch ex As Exception
        End Try
        tim.Enabled = True
    End Sub

    Private Sub FV(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tim.Enabled = False
        Try
            Dim w As Wizyty = sender.tag
            wybranaWizyta = w.NrWizyty
            Dim ww As New WindowFV(w, db)

            ww.ShowDialog()
        Catch ex As Exception

        End Try
        tim.Enabled = True
    End Sub



    Private Sub UserControl_SizeChanged(ByVal sender As System.Object, ByVal e As System.Windows.SizeChangedEventArgs) Handles MyBase.SizeChanged
        Try
            Me.wysokosc = e.NewSize.Height
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        If MsgBox("Czy napewno chcesz wyzerować płatności z tego dnia?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
            Exit Sub
        End If
        tim.Enabled = False
        Dim dane1 = From a In db.Wizyty Where _
                             a.DataWizyty = Module1.dane.data _
                             And a.NrGabinetu = dane.WybranyLekarz.NrGabinetu _
                             Order By a.GodzWizyty.Value.Hour, a.GodzWizyty.Value.Minute

        For Each w In dane1
            w.Wplata = 0
        Next
        db.SaveChanges()
        ladujDane()
        tim.Enabled = True
    End Sub
End Class
