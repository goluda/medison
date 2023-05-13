Public Class WindowRejestracja
    Dim ds As New DataSet1

    Dim wizyty As New List(Of TempRejestracja)
    Dim wizyty1 As New List(Of TempRejestracja)
    Dim wizyty2 As New List(Of TempRejestracja)

    Dim wycieta As TempRejestracja

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'Me.Top = 1
        'Me.Left = 1
        'Me.Height = My.Computer.Screen.WorkingArea.Height - 300
        'Me.Width = My.Computer.Screen.WorkingArea.Width - 100


        Me.Calendar1.SelectedDate = Now.Date
        TableAdaptery.DniTygodnia.Fill(ds.dniTygodnia)
        TableAdaptery.DaneLekarza.Fill(ds.DaneLekarza)
        Me.ListBoxLekarz.ItemsSource = Me.ds.DaneLekarza
        'Me.ListBoxRejestracja.DataContext = wizyty

    End Sub

    Sub obliczGodziny()
        Dim dt = szukajNastepnegoDnia(Me.Calendar1.SelectedDate.Value.Date.AddDays(-1))
        'obliczGodzinyDlaDaty(dt, Me.ListBoxRejestracja, wizyty, LabelData)
        obliczGodzinyDlaDaty(dt, New ListBox, wizyty, LabelData)
        'dt = szukajNastepnegoDnia(dt)
        'obliczGodzinyDlaDaty(dt, Me.ListBoxRejestracja1, wizyty1, LabelData1)
        'dt = szukajNastepnegoDnia(dt)
        'obliczGodzinyDlaDaty(dt, Me.ListBoxRejestracja2, wizyty2, LabelData2)
    End Sub

    Function szukajNastepnegoDnia(data As DateTime) As DateTime
        Try
            Dim i As Integer = 0
            Dim godziny As DataSet1.GodzinyPrzyjecDataTable
            Dim lek = Me.ListBoxLekarz.SelectedValue
            Dim nrGabinetu = lek("NrGabinetu")
            Do
                i += 1
                data = data.AddDays(1)
                godziny = TableAdaptery.GodzinyPrzyjec.GetDataByNrGabinetuDzien(nrGabinetu, data.DayOfWeek)
            Loop While godziny.Rows.Count = 0 And i <= 6
            Return data
        Catch
            Return data.AddDays(1)
        End Try
    End Function


    Sub obliczGodzinyDlaDaty(data As DateTime, list As ListBox, ByRef w As List(Of TempRejestracja), dlab As Label)
        Try
            Me.WrapPanelListaPacjentow.Children.Clear()

            w.Clear()

            System.Windows.Forms.Application.DoEvents()

            dlab.Content = data.ToLongDateString + " " + dzienTygodnia(data.DayOfWeek)

            Dim dzien As Integer = data.DayOfWeek

            Dim lek = Me.ListBoxLekarz.SelectedValue
            Dim nrGabinetu = lek("NrGabinetu")

            If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                Dim wizytyDzien = TableAdaptery.Wizyta.GetDataByDaty1(data, data.AddDays(1), nrGabinetu)

                Dim i As Integer = 0
                For Each dr In TableAdaptery.GodzinyPrzyjec.GetDataByNrGabinetuDzien(nrGabinetu, dzien)
                    Dim data1 = data.AddHours(dr.dodzinaOd).AddMinutes(dr.minutaOd)
                    Dim data2 = data.AddHours(dr.godzinaDo).AddMinutes(dr.minutaDo)
                    Do While data1 < data2
                        Dim d As New TempRejestracja
                        d.Data = data1
                        d.nrGabinetu = nrGabinetu
                        d.nowa = False
                        d.minut = dr.skokMinut
                        d.godzinaBrush = Module1.kolory(i)


                        Dim temp1 = wizytyDzien.Where(Function(o) o.data = data1)
                        If temp1.Count > 0 Then
                            d.WizytaZBazy = temp1(0)
                        End If

                        w.Add(d)
                        data1 = data1.AddMinutes(dr.skokMinut)
                    Loop
                    i += 1
                Next

            Else
                Dim db As New medisonEntities
                Dim d1 As DateTime = data.AddDays(1)
                Dim d2 As DateTime = data
                Dim nr As Integer = nrGabinetu
                Dim wizytyDzien = (From a In db.Wizyty Where a.NrGabinetu = nr _
                                  And a.DataWizyty >= d2 And a.DataWizyty < d1).ToList

                For Each o In wizytyDzien.Where(Function(o1) o1.GodzWizyty Is Nothing)
                    o.GodzWizyty = New DateTime(1900, 1, 1)
                Next

                Dim i As Integer = 0
                For Each dr In TableAdaptery.GodzinyPrzyjec.GetDataByNrGabinetuDzien(nrGabinetu, dzien)
                    Dim data1 = data.AddHours(dr.dodzinaOd).AddMinutes(dr.minutaOd)
                    Dim data2 = data.AddHours(dr.godzinaDo).AddMinutes(dr.minutaDo)
                    Do While data1 < data2
                        Dim d As New TempRejestracja
                        d.Data = data1
                        d.nrGabinetu = nrGabinetu
                        d.nowa = False
                        d.minut = dr.skokMinut
                        d.godzinaBrush = Module1.kolory(i)


                        Dim temp1 = wizytyDzien.Where(Function(o) o.DataWizyty.Value.Date = data1.Date And o.GodzWizyty.Value.Hour = data1.Hour And o.GodzWizyty.Value.Minute = data1.Minute)
                        If temp1.Count > 0 Then
                            d.WizytaMedison = temp1(0)
                        End If

                        w.Add(d)
                        data1 = data1.AddMinutes(dr.skokMinut)
                    Loop
                    i += 1
                Next

            End If

        Catch ex As Exception
            Dim stoptu = 1
        End Try
        list.ItemsSource = w
        list.Items.Refresh()
        list.Focus()


        generujWrapPanel(w)

    End Sub

    Sub generujWrapPanel(ByRef w As List(Of TempRejestracja))
        Me.WrapPanelListaPacjentow.Children.Clear()
        For Each i In w
            Dim p = New UserControlPozycjaKalendarza(i)
            AddHandler p.SwipeStartEvent, AddressOf SwipeStart
            AddHandler p.SwipeEndEvent, AddressOf SwipeEnd
            AddHandler p.WklejEvent, AddressOf Wklej
            AddHandler p.DodajPacjentaEvent, AddressOf DodajPacjenta
            AddHandler p.ZmienPacjentaEvent, AddressOf ZmienPacjenta
            AddHandler p.PokazEvent, AddressOf Pokaz
            AddHandler p.WytnijEvent, AddressOf Wytnij
            AddHandler p.ZadzwonioneEvent, AddressOf Zadzwonione
            AddHandler p.UsunPacjentaEvent, AddressOf UsunPacjenta
            AddHandler p.UstawInfoEvent, AddressOf UstawInfo

            Me.WrapPanelListaPacjentow.Children.Add(p)
        Next
    End Sub



    Public Function dzienTygodnia(d As System.DayOfWeek) As String
        Select Case d
            Case DayOfWeek.Sunday
                Return "Niedziela"
            Case DayOfWeek.Monday
                Return "Poniedziałek"
            Case DayOfWeek.Tuesday
                Return "Wtorek"
            Case DayOfWeek.Wednesday
                Return "Środa"
            Case DayOfWeek.Thursday
                Return "Czwartek"
            Case DayOfWeek.Friday
                Return "Piątek"
            Case DayOfWeek.Saturday
                Return "Sobota"

            Case Else
                Return ""
        End Select
    End Function

    Private Sub ListBoxLekarz_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListBoxLekarz.SelectionChanged
        obliczGodziny()
    End Sub

    Private Sub Calendar1_SelectedDatesChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles Calendar1.SelectedDatesChanged
        obliczGodziny()
    End Sub

    Private Sub DodajPacjenta(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim t As TempRejestracja
            t = sender.tag
            't.WizytaZBazy = New Wizyty


            Dim w As New WindowPacjent
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then

                w.ShowDialog()
                If w.wybrany = True Then

                    TableAdaptery.Wizyta.Insert(t.nrGabinetu, _
                                                t.Data, _
                                                "", _
                                                If(w.p.IsNazwiskoNull = False, w.p.Nazwisko, Nothing), _
                                                If(w.p.IsNazwisko2Null = False, w.p.Nazwisko2, Nothing), _
                                                If(w.p.IsImieNull = False, w.p.Imie, Nothing), _
                                                If(w.p.IsImie2Null = False, w.p.Imie2, Nothing), _
                                                If(w.p.IsPlecNull = False, w.p.Plec, Nothing), _
                                                If(w.p.IsdataUrodzeniaNull = False, w.p.dataUrodzenia, Nothing), _
                                                If(w.p.IskodNull = False, w.p.kod, Nothing), _
                                                If(w.p.IsMiejscowoscNull = False, w.p.Miejscowosc, Nothing), _
                                                If(w.p.IsAdresNull = False, w.p.Adres, Nothing), _
                                                If(w.p.IsNrDomuNull = False, w.p.NrDomu, Nothing), _
                                                If(w.p.IsNrLokaluNull = Nothing, w.p.NrLokalu, Nothing), _
                                                If(w.p.IsTelefonNull = False, w.p.Telefon, Nothing), _
                                                If(w.p.IsPeselNull = Nothing, w.p.Pesel, Nothing), _
                                                False, 0, True)
                End If


            Else

                Dim db As New medisonEntities
                w.db = db
                w.ShowDialog()
                Dim l = db.DaneLekarza.Where(Function(o) o.NrGabinetu = t.nrGabinetu).First
                If w.wybrany = True Then
                    Dim wm As New Wizyty
                    wm.NrGabinetu = t.nrGabinetu
                    wm.DataWizyty = t.Data.Date
                    wm.GodzWizyty = New DateTime(1900, 1, 1, t.Data.Hour, t.Data.Minute, 0)
                    wm.potwierdzona = False
                    wm.Pacjenci = w.pm
                    wm.TerazTen = False
                    wm.CzyDrukowac = False
                    wm.L4 = False
                    wm.Wplata = 0
                    wm.ImieLekarza = l.Imie
                    wm.NazwiskoLekarza = l.Nazwisko
                    wm.NrLekarza = l.NrLekarza
                    wm.NIPLekarza = l.NIPlekarza
                    db.SaveChanges()
                End If

            End If
            Module1.ostatniaDataWizyty = t.Data
            obliczGodziny()

            w = Nothing


        Catch ex As Exception

        End Try


    End Sub


    Private Sub UsunPacjenta(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If MsgBox("Czy usunąć tą rejestrację?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            Dim t As TempRejestracja
            t = sender.tag
            Module1.ostatniaDataWizyty = Now.Date

            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                
                TableAdaptery.Wizyta.DeleteQuery(t.WizytaZBazy.Identyfikator)
            Else
                Dim db As New medisonEntities
                Dim temp = db.Wizyty.Where(Function(o) o.NrWizyty = t.WizytaMedison.NrWizyty)
                If temp.Count > 0 Then
                    db.DeleteObject(temp.First)
                End If
                db.SaveChanges()

            End If
            obliczGodziny()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UstawInfo(sender As System.Object, e As System.Windows.RoutedEventArgs)

        Try
            Dim t As TempRejestracja = sender.tag
            Dim temp = InputBox("Wprowadź uwagi:", "Uwagi", t.aUwagi)

            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then

                TableAdaptery.Wizyta.UpdateQueryUstawUwagi(temp, t.WizytaZBazy.Identyfikator)
            Else
                Dim db As New medisonEntities
                Dim temp1 = db.Wizyty.Where(Function(o) o.NrWizyty = t.WizytaMedison.NrWizyty)
                If temp1.Count > 0 Then
                    temp1.First.Uwagi = temp
                End If
                db.SaveChanges()

            End If

            obliczGodziny()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Zadzwonione(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim a = MsgBox("Czy pacjent potwierdził wizytę?", MsgBoxStyle.YesNo)
        Try
            Dim t As TempRejestracja = sender.tag


            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                TableAdaptery.Wizyta.UpdateQueryPotwierdz(If(a = MsgBoxResult.Yes, True, False), t.WizytaZBazy.Identyfikator)

            Else
                Dim db As New medisonEntities
                Dim temp = db.Wizyty.Where(Function(o) o.NrWizyty = t.WizytaMedison.NrWizyty)
                If temp.Count > 0 Then
                    temp.First.potwierdzona = If(a = MsgBoxResult.Yes, True, False)
                End If
                db.SaveChanges()

            End If

            obliczGodziny()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Wytnij(sender As System.Object, e As System.Windows.RoutedEventArgs)
        'If MsgBox("Czy chcesz wyciąć tą wizytę?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
        Try
            Dim t As TempRejestracja = sender.tag
            wycieta = t
            'MsgBox("Wybierz inną datę wizyty i kliknij wklej")
        Catch ex As Exception

        End Try
        'End If
    End Sub

    Private Sub Wklej(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If wycieta IsNot Nothing Then
            'If MsgBox("Czy chcesz tutaj wkleić skopiowaną wizytę?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                Dim t As TempRejestracja = sender.tag
                Module1.ostatniaDataWizyty = t.Data

                If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                    TableAdaptery.Wizyta.UpdateQueryWklej(t.nrGabinetu, t.Data, wycieta.WizytaZBazy.Identyfikator)

                Else
                    Dim db As New medisonEntities
                    Dim temp1 = db.Wizyty.Where(Function(o) o.NrWizyty = wycieta.WizytaMedison.NrWizyty)
                    If temp1.Count > 0 Then
                        temp1.First.DataWizyty = t.Data.Date
                        temp1.First.GodzWizyty = New DateTime(1900, 1, 1, t.Data.Hour, t.Data.Minute, 0)
                    End If
                    db.SaveChanges()

                End If
                obliczGodziny()
                wycieta = Nothing
            Catch ex As Exception

            End Try
            'End If
        Else
        MsgBox("Najpier wytnij wizytę")
        End If
    End Sub

    Private Sub Pokaz(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim t As TempRejestracja = sender.tag
            If t.szczegoly = Windows.Visibility.Collapsed Then
                t.szczegoly = Windows.Visibility.Visible
            Else
                t.szczegoly = Windows.Visibility.Collapsed
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ZmienPacjenta(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim t As TempRejestracja
            t = sender.tag
            't.WizytaZBazy = New Wizyty
            Dim w As New WindowPacjent
            w.imie = t.aImie
            w.nazwisko = t.aNazwisko
            w.pesel = t.aPesel
            w.wyszukaj = True
            w.TextBox1.Text = t.aNazwisko
            'w.TextBox1_KeyDown(Nothing, Nothing)


            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then

                w.ShowDialog()
                If w.wybrany = True Then
                    TableAdaptery.Wizyta.UpdateQueryZmianaPacjenta(If(w.p.IsNazwiskoNull = False, w.p.Nazwisko, Nothing), _
                                                If(w.p.IsNazwisko2Null = False, w.p.Nazwisko2, Nothing), _
                                                If(w.p.IsImieNull = False, w.p.Imie, Nothing), _
                                                If(w.p.IsImie2Null = False, w.p.Imie2, Nothing), _
                                                If(w.p.IsPlecNull = False, w.p.Plec, Nothing), _
                                                If(w.p.IsdataUrodzeniaNull = False, w.p.dataUrodzenia, Nothing), _
                                                If(w.p.IskodNull = False, w.p.kod, Nothing), _
                                                If(w.p.IsMiejscowoscNull = False, w.p.Miejscowosc, Nothing), _
                                                If(w.p.IsAdresNull = False, w.p.Adres, Nothing), _
                                                If(w.p.IsNrDomuNull = False, w.p.NrDomu, Nothing), _
                                                If(w.p.IsNrLokaluNull = Nothing, w.p.NrLokalu, Nothing), _
                                                If(w.p.IsTelefonNull = False, w.p.Telefon, Nothing), _
                                                If(w.p.IsPeselNull = Nothing, w.p.Pesel, Nothing), _
                                                t.WizytaZBazy.Identyfikator)


                End If

            Else
                Dim db As New medisonEntities
                Dim temp1 = db.Wizyty.Where(Function(o) o.NrWizyty = t.WizytaMedison.NrWizyty)
                If temp1.Count > 0 Then

                    w.db = db
                    w.ShowDialog()

                    If w.wybrany = True Then
                        temp1.First.Pacjenci = w.pm
                    End If


                End If
                db.SaveChanges()

            End If

            w = Nothing
            Module1.ostatniaDataWizyty = t.Data
            obliczGodziny()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim ff As New FlowDocument
        ff.FontFamily = New Windows.Media.FontFamily("Arial")
        ff.FontSize = 10
        'ff.PageWidth = 700
        ff.PageWidth = 700
        ff.PageHeight = 700
        ff.PagePadding = New Thickness(50)
        ff.ColumnGap = 0
        ff.ColumnWidth = 700

        Dim l = Me.ListBoxLekarz.SelectedItem
        Dim p As New Paragraph(New Bold(New Run(l(1) + " " + l(2))))
        p.FontSize = 18
        p.SetTextAlignment(p, TextAlignment.Center)
        ff.Blocks.Add(p)

        'generujWydrukDla(Me.LabelData.Content, Me.ListBoxRejestracja, ff)
        generujWydrukDla(Me.LabelData.Content, New ListBox, ff)
        'generujWydrukDla(Me.LabelData1.Content, Me.ListBoxRejestracja1, ff)
        'generujWydrukDla(Me.LabelData2.Content, Me.ListBoxRejestracja2, ff)


        Dim pr As New PrintDialog
        Dim pagSource As IDocumentPaginatorSource = ff
        pr.PrintDocument(pagSource.DocumentPaginator, "rejestracja")
    End Sub

    Public Sub generujWydrukDla(data As DateTime, l As ListBox, ByRef ff As FlowDocument)

        ff.Blocks.Add(New Paragraph(New Bold(New Run("Wizyty na dzień: " + data.ToShortDateString))))
        For Each o As TempRejestracja In l.Items
            If o.wolna = False Then
                Dim p1 As New Paragraph
                p1.FontSize = 16
                p1.Inlines.Add(New Run(o.godzina + " "))
                p1.Inlines.Add(New Bold(New Run(o.aNazwisko + " " + o.aImie + ", tel. " + o.aTelefon)))
                'p1.Inlines.Add(New Run(" ur.: " + o.aDataUrodzenia + " PESEL: " + o.aPesel))
                'p1.Inlines.Add(New Run(" Adres: " + o.aMiejscowosc + " " + o.aAdres + " tel. " + o.aTelefon))
                ff.Blocks.Add(p1)
            End If
        Next

    End Sub

    Dim sx = 0
    Dim sy = 0

    Private Sub SwipeStart(sender As Object, e As MouseButtonEventArgs)
        Dim p = e.GetPosition(Me)
        sx = p.X
        sy = p.Y
    End Sub

    Private Sub SwipeEnd(sender As Object, e As MouseButtonEventArgs)
        Dim p = e.GetPosition(Me)
        Dim y = sy - p.Y
        Dim x = sx - p.X


        If y > -20 And y < 20 Then
            If x > 20 Then
                'MsgBox("swipe left")
                PoprzedniDzien()

            End If
            If x < -20 Then
                'MsgBox("swipe right")
                NastepnyDzien()
            End If
        End If
    End Sub
    Public Sub NastepnyDzien()
        Dim dt = Calendar1.SelectedDate.Value.Date
        dt = dt.AddDays(1)
        Calendar1.SelectedDate = dt
        obliczGodziny()
    End Sub
    Public Sub PoprzedniDzien()
        Dim dt = Calendar1.SelectedDate.Value.Date
        dt = dt.AddDays(-1)
        Calendar1.SelectedDate = dt
        obliczGodziny()
    End Sub

    Private Sub Window_SizeChanged(sender As Object, e As SizeChangedEventArgs)

    End Sub

    Private Sub window_Loaded_1(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
Class converterDBNUll
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Try
            Return value
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Try
            Return value
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
