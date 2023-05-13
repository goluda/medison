Public Class WindowFV
    Dim db As medisonEntities
    Dim w As Wizyty

    Sub New(ByVal _w As Wizyty, ByVal _db As medisonEntities)
        InitializeComponent()
        db = _db
        w = _w


        Me.ComboBoxTypRachunku.ItemsSource = New String() {"Rachunek", "Faktura", "Faktura VAT"}
        Me.ComboBoxSposobZaplaty.ItemsSource = New String() {"gotówka", "przelew"}
        Me.ComboTowary.ItemsSource = db.Towary.OrderBy(Function(o) o.IndeksT).ToList
        Me.ListBoxTowary.ItemsSource = db.Towary.OrderBy(Function(o) o.IndeksT).Take(8).ToList

    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.ComboBox1.Items.Add("Oryginał")
        Me.ComboBox1.Items.Add("Kopia")
        Me.ComboBox1.Items.Add("Oryginał/Kopia")
        Me.ComboBox1.Items.Add(" ")


        Me.DataContext = w
        ladujRachunki()
        Dim MedisonEntities As Medison.medisonEntities = New Medison.medisonEntities()
        'Load data into Towaries. You can modify this code as needed.
        'Dim TowariesViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TowariesViewSource"), System.Windows.Data.CollectionViewSource)
        'Dim TowariesQuery As System.Data.Objects.ObjectQuery(Of Medison.Towary) = Me.GetTowariesQuery(MedisonEntities)
        'TowariesViewSource.Source = TowariesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly)
    End Sub

    Sub ladujRachunki()
        Me.Rachunek.DataContext = (From a In w.Pacjenci.Rachunki Where a.NrGabinetu = dane.WybranyLekarz.NrGabinetu Order By a.DataWystawienia Descending).ToList
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            w.odswierz()

            db.SaveChanges()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub WybierzFirme(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'db.SaveChanges()

            Dim z As Zaklady = sender.tag

            w.Pacjenci.NIPzakladu = z.NIPzakladu
            w.Pacjenci.NazwaZakladuPracy = z.NazwaZakladuPracy
            w.Pacjenci.AdresZakladuPracy = z.AdresZakladuPracy
            w.Pacjenci.KodZakladuPracy = z.KodZakladuPracy
            w.Pacjenci.MiastoZakladuPracy = z.MiastoZakladuPracy
            w.odswierz()
        Catch
        End Try
    End Sub

    Private Sub SzukajFirmy(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Enter Then
            Dim t As TextBox = sender
            If t.Text = "" Then Exit Sub
            Me.DataGridFirmy.ItemsSource = From a In db.Zaklady Where a.NIPzakladu.Contains(t.Text) Or a.NazwaZakladuPracy.Contains(t.Text) Order By a.NazwaZakladuPracy
        End If
    End Sub

    Sub przepiszLekarza(ByVal n As Rachunki)
        Dim lekarz = db.DaneLekarza.Where(Function(o) o.NrGabinetu = w.NrGabinetu).First
        n.DataWystawienia = Now.Date
        n.NrGabinetu = lekarz.NrGabinetu
        n.Pacjenci = w.Pacjenci
        n.NIPFV = lekarz.NIPlekarza
        n.NazwaFV = lekarz.Nazwa
        n.AdresFV = lekarz.Adres
        n.KodFV = lekarz.Kod
        n.MiastoFV = lekarz.Miasto
        n.BankFV = lekarz.Bank
        n.NrKontaFV = lekarz.NrKonta
        n.Wystawil = lekarz.Imie + " " + lekarz.Nazwisko
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim n As New Rachunki

        Try
            Dim rok As Integer = Now.Year
            Dim temp = From a In db.Rachunki Where a.NrGabinetu = w.NrGabinetu And a.DataWystawienia.Value.Year = rok Order By a.Numerrachunku Descending

            n.Numerrachunku = 1
            If temp.Count > 0 Then
                n.Numerrachunku = temp.First.Numerrachunku + 1
            End If


        Catch ex As Exception

        End Try

        przepiszLekarza(n)

        w.Pacjenci.Rachunki.Add(n)

        ladujRachunki()
        Me.ListBox1.SelectedItem = n
        db.SaveChanges()
    End Sub

    Sub wyswietlRachunek(ByVal n As Rachunki)
        Me.GridRach.Children.Clear()
        Dim r As New UserControlRachunekSzczegoly(db, n, Nothing, False)
        r.ButtonWroc.Visibility = Windows.Visibility.Hidden
        r.Margin = New System.Windows.Thickness(0)
        Me.GridRach.Children.Add(r)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            Dim r As Rachunki = Me.GridRach.DataContext
            r.NaKogoNIP = If(w.Pacjenci.NIP IsNot Nothing, w.Pacjenci.NIP, "")
            r.NaKogoAdres = If(w.Pacjenci.Miejscowosc IsNot Nothing, w.Pacjenci.Miejscowosc, "") + " " + If(w.Pacjenci.Adres IsNot Nothing, w.Pacjenci.Adres, "") + " " + If(w.Pacjenci.NrDomu IsNot Nothing, w.Pacjenci.NrDomu, "") + " " + If(w.Pacjenci.NrLokalu IsNot Nothing, w.Pacjenci.NrLokalu, "")
            r.NaKogoKod = If(w.Pacjenci.Kod IsNot Nothing, w.Pacjenci.Kod, "")
            r.NaKogo = w.Pacjenci.ToString
            r.odswierz()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            Dim r As Rachunki = Me.GridRach.DataContext
            r.NaKogoNIP = If(w.Pacjenci.NIPzakladu IsNot Nothing, w.Pacjenci.NIPzakladu, "")
            r.NaKogoAdres = If(w.Pacjenci.MiastoZakladuPracy IsNot Nothing, w.Pacjenci.MiastoZakladuPracy, "") + " " + If(w.Pacjenci.AdresZakladuPracy IsNot Nothing, w.Pacjenci.AdresZakladuPracy, "")
            r.NaKogoKod = If(w.Pacjenci.KodZakladuPracy IsNot Nothing, w.Pacjenci.KodZakladuPracy, "")
            r.NaKogo = w.Pacjenci.NazwaZakladuPracy
            r.odswierz()
        Catch
        End Try
    End Sub

    Private Sub zmianaTowaru(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim o = sender.tag

        If o IsNot Nothing Then o.przepiszTowar()
    End Sub

    Private Function GetTowariesQuery(ByVal MedisonEntities As Medison.medisonEntities) As List(Of Medison.Towary)

        Return MedisonEntities.Towary.ToList
    End Function

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        db.SaveChanges()

        Dim r As Rachunki = Me.ListBox1.SelectedItem

        If If(r.Numerrachunku IsNot Nothing, r.Numerrachunku, 0) = 0 Then
            MsgBox("Wprowadź numer rachunku")
            Exit Sub
        End If

        If If(r.NaKogo IsNot Nothing, r.NaKogo, "") = "" Then
            MsgBox("Wpisz dane pacjenta lub dane zakładu!!")
            Exit Sub
        End If
        r.FvOryginal = w.FvOryginal
        'If r.RachunekTyp.Contains("VAT") Then
        r.Info = "UWAGA: Podatnik zwolniony z podatku VAT na podstawie art. 113 ust. 1 ustawy o podatku od towaru i usług z dnia 11.03.2004r."
        If r.NazwaFV.Contains("Partnerska") Then
            r.Info = "Podatnik zwolniony od podatku na podstawie art. 113 ust.1 ustawy o podatku od towarów i usług z dn. 11.03.2004"
        Else
            r.Info = "Podatnik zwolniony od podatku na podstawie art. 43 ust. 1 pkt. 19 ustawy o podatku od towarów i usług z dn. 11.03.2004"
        End If
        'End If
        Dim rr As New UserControlRachunek1(r)
        drukujVisual(rr, "Rach", Module1.printDial.PrintableAreaWidth - 25, Module1.printDial.PrintableAreaHeight)
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub
    Private Sub DodajDoFaktury(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim t As Towary = sender.tag
            Dim o As New OpisyRachunku
            o.NrTowaru = t.NrTowaru
            o.Towary = t
            o.SWWKWiU = t.SWWKWiU
            o.CenaNetto = t.CenaNettoSugerowana
            o.Ilosc = 1
            o.JedMiary = t.JedMiary
            o.WartoscNetto = o.Ilosc * o.CenaNetto
            o.StawkaVAT = t.StawkaVAT
            o.WartoscVAT = Math.Round(o.WartoscNetto.Value * o.StawkaVAT.Value, 2)
            o.WartoscBrutto = o.WartoscNetto + o.WartoscVAT

            Dim r As Rachunki = Me.ListBox1.SelectedItem
            r.OpisyRachunku.Add(o)
            db.SaveChanges()
            r.odswierz()
            r.obliczBruttoNetto()

        Catch
        End Try
        Me.GridOpisyRachunku.Items.Refresh()

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button6.Click
        If MsgBox("Czy napewno chcesz usunąć ten rachunek?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                Dim r As Rachunki = Me.ListBox1.SelectedItem

                db.Rachunki.Remove(r)
                db.SaveChanges()
            Catch
            End Try
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim opis As OpisyRachunku = Me.GridOpisyRachunku.SelectedItem

            If MessageBox.Show("Czy napewno usunąć pozycję " + opis.NazwaTowaru, "Pytanie", vbYesNo) <> MsgBoxResult.Yes Then
                Return
            End If

            Dim r As Rachunki = Me.ListBox1.SelectedItem
            r.OpisyRachunku.Remove(opis)
            db.OpisyRachunku.Remove(opis)
            db.SaveChanges()
            r.odswierz()
            r.obliczBruttoNetto()
            Me.GridOpisyRachunku.Items.Refresh()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)

        Try
            Dim w = New WindowWybierzMagazyn(db)
            w.ShowDialog()

            Dim t As Towary = w.t
            w = Nothing

            Dim o As New OpisyRachunku
            o.NrTowaru = t.NrTowaru
            o.Towary = t
            o.SWWKWiU = t.SWWKWiU
            o.CenaNetto = t.CenaNettoSugerowana
            o.Ilosc = 1
            o.JedMiary = t.JedMiary
            o.WartoscNetto = o.Ilosc * o.CenaNetto
            o.StawkaVAT = t.StawkaVAT
            o.WartoscVAT = Math.Round(o.WartoscNetto.Value * o.StawkaVAT.Value, 2)
            o.WartoscBrutto = o.WartoscNetto + o.WartoscVAT

            Dim r As Rachunki = Me.ListBox1.SelectedItem
            r.OpisyRachunku.Add(o)
            db.SaveChanges()
            r.odswierz()
            r.obliczBruttoNetto()


        Catch
        End Try
        Me.GridOpisyRachunku.Items.Refresh()

    End Sub

    Private Sub ZmianaIlosci(sender As Object, e As RoutedEventArgs)
        Dim o As OpisyRachunku
        Dim b As Button = sender
        o = b.Tag

        Dim ilosc = InputBox("Podaj ilosc", "pytanie", o.Ilosc)
        If ilosc.ToString = "" Then Exit Sub
        If ilosc = 0 Then Exit Sub
        o.Ilosc = ilosc

        o.WartoscNetto = o.Ilosc * o.CenaNetto
        o.WartoscVAT = Math.Round(o.WartoscNetto.Value * o.StawkaVAT.Value, 2)
        o.WartoscBrutto = o.WartoscNetto + o.WartoscVAT

        db.SaveChanges()
        Dim r As Rachunki = Me.ListBox1.SelectedItem
        r.odswierz()
        r.obliczBruttoNetto()
        Me.GridOpisyRachunku.Items.Refresh()


    End Sub
End Class
