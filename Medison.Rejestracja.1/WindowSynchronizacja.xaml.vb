Public Class WindowSynchronizacja

    Sub log(t As String)
        t = Now.ToString + "   " + t
        ListBox1.Items.Add(t)
        ListBox1.SelectedItem = t
        ListBox1.ScrollIntoView(t)
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try


            log("Wybieranie wizyt do przesłania do bazy")
            Dim ta As New DataSet1TableAdapters.WizytaTableAdapter
            Dim taP As New DataSet1TableAdapters.PacjenciTableAdapter
            Dim taL As New DataSet1TableAdapters.DaneLekarzaTableAdapter
            Dim doSynchronizacji = ta.GetDataByDoSynchronizacji
            log("Znaleziono " + doSynchronizacji.Rows.Count.ToString + " wizyt")
            Dim db As New medisonEntities
            For Each r In doSynchronizacji
                log(r.data.ToString + " " + If(r.IsPeselNull = False, r.Pesel, "") + " " + r.Nazwisko + " " + If(r.IsImieNull = False, r.Imie, ""))
                If r.Nazwisko.Contains("Dziuba") Then
                    Dim stophere = 1
                End If
                Dim w As Wizyty
                If If(r.IsoryginalIDNull = False, r.oryginalID, 0) = 0 Then
                    w = New Wizyty
                    w.L4 = False
                    w.TerazTen = False
                    w.CzyDrukowac = False
                    w.Wplata = 0
                    db.Wizyty.AddObject(w)
                Else
                    Dim temp = db.Wizyty.Where(Function(o) o.NrWizyty = r.oryginalID)
                    If temp.Count > 0 Then
                        w = temp.First
                    Else
                        GoTo 1
                    End If
                End If

                Dim pesel1 = If(r.IsPeselNull = False, r.Pesel, "")

                Dim tempP = db.Pacjenci.Where(Function(o) o.Pesel = pesel1 And o.Nazwisko = r.Nazwisko And o.Imie = r.Imie)
                Dim p As Pacjenci
                If tempP.Count = 0 Then
                    p = New Pacjenci With {.Pesel = pesel1, .Nazwisko = If(r.IsNazwiskoNull = False, r.Nazwisko, ""), _
                                                             .Nazwisko2 = If(r.IsNazwisko2Null = False, r.IsNazwisko2Null, ""), _
                                                             .Imie = If(r.IsImieNull = False, r.Imie, ""), _
                                                             .Imie2 = If(r.IsImie2Null = False, r.Imie2, ""), _
                                                             .Plec = If(r.IsPlecNull = False, r.Plec, ""), _
                                                             .DataUrodzenia = If(r.IsdataUrodzeniaNull = False, r.dataUrodzenia, Nothing), _
                                                             .Kod = If(r.IskodNull = False, r.kod, ""), _
                                                             .Miejscowosc = If(r.IsMiejscowoscNull = False, r.Miejscowosc, ""), _
                                                             .Adres = If(r.IsAdresNull = False, r.Adres, ""), _
                                                             .NrDomu = If(r.IsNrDomuNull = False, r.NrDomu, ""), _
                                                             .NrLokalu = If(r.IsNrLokaluNull = False, r.NrLokalu, ""), _
                                                             .Telefon = If(r.IsTelefonNull = False, r.Telefon, "")}
                    If r.IsdataUrodzeniaNull = True Then
                        p.DataUrodzenia = Nothing
                    End If
                    db.Pacjenci.AddObject(p)
                Else
                    p = tempP.First
                End If

                Dim tempoG = db.DaneLekarza.Where(Function(o) o.NrGabinetu = r.nrGabinetu)
                If tempoG.Count > 0 Then
                    w.NazwiskoLekarza = tempoG.First.Nazwisko
                    w.ImieLekarza = tempoG.First.Imie
                    w.NrLekarza = tempoG.First.NrLekarza
                    w.NIPLekarza = tempoG.First.NIPlekarza
                End If


                w.Pacjenci = p
                w.NrGabinetu = r.nrGabinetu
                w.DataWizyty = New DateTime(r.data.Year, r.data.Month, r.data.Day)
                w.GodzWizyty = New DateTime(1900, 1, 1, r.data.Hour, r.data.Minute, 0)
                w.Uwagi = If(r.IsuwagiNull = False, r.uwagi, "")
                w.potwierdzona = If(r.IsPotwierdzonaNull = False, r.Potwierdzona, False)
                'w.TerazTen = False
                'w.CzyDrukowac = False
                'w.L4=False 

                db.SaveChanges()
1:
            Next

            log("Wizyty przekazane")

            log("Usuwanie wizyt")
            For Each l In taL.GetData().Where(Function(o) o.synchronizacja = True)
                ta.DeleteQueryCzyszczenieBazy(l.NrGabinetu)
            Next
            log("Baza oczyszczona")
            log("Importowanie nowej listy wizyt")
            Dim dt1 = Now.Date
            For Each l In taL.GetData().Where(Function(o) o.synchronizacja = True)
                Dim nowe = (From a In db.Wizyty Where a.DataWizyty >= dt1 And a.GodzWizyty IsNot Nothing And a.NrGabinetu = l.NrGabinetu).ToList
                For Each n In nowe
                    log(n.DataWizyty.ToString + " " + If(n.Pacjenci.Nazwisko IsNot Nothing, n.Pacjenci.Nazwisko, "") + " " + If(n.Pacjenci.Imie IsNot Nothing, n.Pacjenci.Imie, ""))
                    Dim dt As DateTime

                    dt = New DateTime(n.DataWizyty.Value.Year, n.DataWizyty.Value.Month, n.DataWizyty.Value.Day, n.GodzWizyty.Value.Hour, n.GodzWizyty.Value.Minute, 0)

                    ta.Insert(n.NrGabinetu, dt, _
                              If(n.Uwagi IsNot Nothing, n.Uwagi, ""), _
                              If(n.Pacjenci.Nazwisko IsNot Nothing, n.Pacjenci.Nazwisko, ""), _
                              If(n.Pacjenci.Nazwisko2 IsNot Nothing, n.Pacjenci.Nazwisko2, ""), _
                              If(n.Pacjenci.Imie IsNot Nothing, n.Pacjenci.Imie, ""), _
                              If(n.Pacjenci.Imie2 IsNot Nothing, n.Pacjenci.Imie2, ""), _
                              If(n.Pacjenci.Plec IsNot Nothing, n.Pacjenci.Plec, ""), _
                              If(n.Pacjenci.DataUrodzenia IsNot Nothing, n.Pacjenci.DataUrodzenia, Nothing), _
                              If(n.Pacjenci.Kod IsNot Nothing, n.Pacjenci.Kod, ""), _
                              If(n.Pacjenci.Miejscowosc IsNot Nothing, n.Pacjenci.Miejscowosc, ""), _
                              If(n.Pacjenci.Adres IsNot Nothing, n.Pacjenci.Adres, ""), _
                              If(n.Pacjenci.NrDomu IsNot Nothing, n.Pacjenci.NrDomu, ""), _
                              If(n.Pacjenci.NrLokalu IsNot Nothing, n.Pacjenci.NrLokalu, ""), _
                              If(n.Pacjenci.Telefon IsNot Nothing, n.Pacjenci.Telefon, ""), _
                              If(n.Pacjenci.Pesel IsNot Nothing, n.Pacjenci.Pesel, ""), _
                              If(n.potwierdzona IsNot Nothing, n.potwierdzona, False), _
                              n.NrWizyty, False)

                    Dim tempPacj = taP.GetDataBy1(If(n.Pacjenci.Nazwisko IsNot Nothing, n.Pacjenci.Nazwisko, ""), _
                                                If(n.Pacjenci.Imie IsNot Nothing, n.Pacjenci.Imie, ""), _
                                                If(n.Pacjenci.Pesel IsNot Nothing, n.Pacjenci.Pesel, ""))
                    If tempPacj.Rows.Count > 0 Then
                        taP.UpdateQuery(If(n.Pacjenci.Nazwisko2 IsNot Nothing, n.Pacjenci.Nazwisko2, ""), _
                                        If(n.Pacjenci.Imie2 IsNot Nothing, n.Pacjenci.Imie2, ""), _
                                        If(n.Pacjenci.Plec IsNot Nothing, n.Pacjenci.Plec, ""), _
                                        If(n.Pacjenci.DataUrodzenia IsNot Nothing, n.Pacjenci.DataUrodzenia, Nothing), _
                                        If(n.Pacjenci.Kod IsNot Nothing, n.Pacjenci.Kod, ""), _
                                        If(n.Pacjenci.Miejscowosc IsNot Nothing, n.Pacjenci.Miejscowosc, ""), _
                                        If(n.Pacjenci.Adres IsNot Nothing, n.Pacjenci.Adres, ""), _
                                        If(n.Pacjenci.NrDomu IsNot Nothing, n.Pacjenci.NrDomu, ""), _
                                        If(n.Pacjenci.NrLokalu IsNot Nothing, n.Pacjenci.NrLokalu, ""), _
                                        If(n.Pacjenci.Telefon IsNot Nothing, n.Pacjenci.Telefon, ""), _
                                        If(n.Pacjenci.Nazwisko IsNot Nothing, n.Pacjenci.Nazwisko, ""), _
                                        If(n.Pacjenci.Imie IsNot Nothing, n.Pacjenci.Imie, ""), _
                                        If(n.Pacjenci.Pesel IsNot Nothing, n.Pacjenci.Pesel, ""))

                    Else
                        taP.Insert(If(n.Pacjenci.Nazwisko IsNot Nothing, n.Pacjenci.Nazwisko, ""), _
                                   If(n.Pacjenci.Nazwisko2 IsNot Nothing, n.Pacjenci.Nazwisko2, ""), _
                                   If(n.Pacjenci.Imie IsNot Nothing, n.Pacjenci.Imie, ""), _
                                   If(n.Pacjenci.Imie2 IsNot Nothing, n.Pacjenci.Imie2, ""), _
                                   If(n.Pacjenci.Plec IsNot Nothing, n.Pacjenci.Plec, ""), _
                                    If(n.Pacjenci.DataUrodzenia IsNot Nothing, n.Pacjenci.DataUrodzenia, Nothing), _
                                    If(n.Pacjenci.Kod IsNot Nothing, n.Pacjenci.Kod, ""), _
                                    If(n.Pacjenci.Miejscowosc IsNot Nothing, n.Pacjenci.Miejscowosc, ""), _
                                    If(n.Pacjenci.Adres IsNot Nothing, n.Pacjenci.Adres, ""), _
                                    If(n.Pacjenci.NrDomu IsNot Nothing, n.Pacjenci.NrDomu, ""), _
                                    If(n.Pacjenci.NrLokalu IsNot Nothing, n.Pacjenci.NrLokalu, ""), _
                                    If(n.Pacjenci.Telefon IsNot Nothing, n.Pacjenci.Telefon, ""), _
                                    If(n.Pacjenci.Pesel IsNot Nothing, n.Pacjenci.Pesel, ""))
                    End If

                Next
            Next
            log("Nowe wizyty zaimportowano")

            MsgBox("Koniec")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
