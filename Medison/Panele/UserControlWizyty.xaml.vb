Public Class UserControlWizyty
    Implements MyUserControl

    Dim db As medisonEntities

    Dim WithEvents tim As New System.Windows.Forms.Timer With {.Interval = 3000, .Enabled = False}

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

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
            Dim dane1 = (From a In db.Wizyty Where
                             a.DataWizyty = Module1.dane.data _
                             And a.NrGabinetu = dane.WybranyLekarz.NrGabinetu
                         Order By a.GodzWizyty.Value.Hour, a.GodzWizyty.Value.Minute).ToList
            Try
                Me.LabelKwota.Content = String.Format("{0:c}", dane1.Sum(Function(o) o.Wplata))
            Catch ex As Exception
                Me.LabelKwota.Content = ""
            End Try

            Me.DataContext = dane1
        Catch ex As Exception
        End Try


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

    Private Sub Edytuj(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim r As Wizyty = sender.tag
        'Dim w As New WindowNowaRezerwacja(r, db, False)
        'w.ShowDialog()
        'db.SaveChanges()

    End Sub

    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    Dim r As New Wizyty
    '    r.DataWizyty = dane.data.Date
    '    r.ImieLekarza = dane.WybranyLekarz.Imie
    '    r.NazwiskoLekarza = dane.WybranyLekarz.Nazwisko
    '    r.NIPLekarza = dane.WybranyLekarz.NIPlekarza
    '    r.NrGabinetu = dane.WybranyLekarz.NrGabinetu
    '    r.NrLekarza = dane.WybranyLekarz.NrLekarza
    '    r.TerazTen = False
    '    r.L4 = False
    '    Dim w As New WindowNowaRezerwacja(r, db, True)
    '    w.ShowDialog()
    '    If MsgBox("Zapisać nową wizytę?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '        db.Wizyties.Add(r)
    '        db.SaveChanges()
    '        MsgBox("Zapisano")

    '    End If
    '    ladujDane()
    'End Sub

    Private Sub SzczegolyPacjenta(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.tim.Enabled = False
        Dim w As Wizyty = sender.tag
        isReadonly = True

        loadModule(New UserControlSzczegolyPacjenta(db, w.Pacjenci, Me, False))
        'Dim ww As New WindowSzczegolyPacjentaKrotkie(w.Pacjenci)
        'ww.ShowDialog()
        'db.SaveChanges()
    End Sub

    Private Sub SzczegolyWizyty(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.tim.Enabled = False
        Dim w As Wizyty = sender.tag
        isReadonly = False
        loadModule(New UserControlWizyta(db, w, Me))
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub

    Private Sub DrukujKwit(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Me.tim.Enabled = False
            Dim w As Wizyty = sender.tag
            Dim u As New UserControlWydrukKwit(w)
            drukujVisual(u, "Kwit: " + w.Pacjenci.ToString, 800, 400)
        Catch ex As Exception

        End Try
        Me.tim.Enabled = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        tim.Enabled = False
        db.SaveChanges()
        db = New medisonEntities
        ladujDane()
        tim.Enabled = True
    End Sub

    Private Sub ZmienTerazTen(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Module1.isReadonly = False
            tim.Enabled = False
            Dim w As Wizyty = sender.tag
            w.wszedl = Now
            w.TerazTen = False
            w.L4 = True
            Module1.zablokujPrzyciski()
            db.SaveChanges()
            Dim u As New UserControlWizyta(db, w, Me)
            loadModule(u)
        Catch ex As Exception

        End Try
       
    End Sub

    Private Sub L4(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim w As Wizyty = sender.tag
            Dim ww As New WindowL4(w, db)
            ww.ShowDialog()
            db.SaveChanges()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tim_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tim.Tick
        Try
            db.SaveChanges()
            db.Dispose()
            db = New medisonEntities
            ladujDane()
        Catch ex As Exception

        End Try
    End Sub
End Class
