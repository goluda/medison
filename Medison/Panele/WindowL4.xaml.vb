Public Class WindowL4

    Dim db As medisonEntities
    Dim w As Wizyty

    Sub New(ByVal _w As Wizyty, ByVal _db As medisonEntities)
        InitializeComponent()
        db = _db
        w = _w

        Me.ListBoxUbezpieczonyW.ItemsSource = db.UbezpieczonyW.ToList
        Me.ListBoxChodziLezy.ItemsSource = db.ChodziLezy.ToList
        Me.ListBoxTypDokumentu.ItemsSource = db.TypyDokumentow.ToList
        'Me.ComboBoxNrStatystyczny.ItemsSource = db.StatystycznaKlasyfikacjaChorob.OrderBy(Function(o) o.kod)

        Me.ComboBoxA.ItemsSource = New String() {"", "A", "B", "C", "D", "E"}
        Me.ComboBoxB.ItemsSource = New String() {"", "A", "B", "C", "D", "E"}
        Me.ComboBoxC.ItemsSource = New String() {"", "A", "B", "C", "D", "E"}
        Me.ComboBoxD.ItemsSource = New String() {"", "A", "B", "C", "D", "E"}

        Me.ListBoxPokrewienstwo.ItemsSource = db.Pokrewienstwo.ToList
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = w
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

            w.NIPzakladu = z.NIPzakladu
            w.NazwaZakladuPracy = z.NazwaZakladuPracy
            w.AdresZakladuPracy = z.AdresZakladuPracy
            w.KodZakladuPracy = z.KodZakladuPracy
            w.MiastoZakladuPracy = z.MiastoZakladuPracy
            w.odswierz()
        Catch
        End Try
    End Sub

    Private Sub SzukajFirmy(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Enter Then

            Dim t As TextBox = sender
            t.Text = t.Text.Replace("-", "")

            If t.Text = "" Then Exit Sub
            If nippesel.sprawdzNIP(t.Text) = False Then
                MsgBox("Błędny NIP")
                Exit Sub
            End If
            Dim firmy = (From a In db.Zaklady Where a.NIPzakladu.Replace("-", "") = t.Text Order By a.NazwaZakladuPracy).ToList
            'Me.DataGridFirmy.ItemsSource = firmy

            If firmy.Count > 0 Then
                Dim z = firmy.First
                w.NIPzakladu = z.NIPzakladu
                w.NazwaZakladuPracy = z.NazwaZakladuPracy
                w.AdresZakladuPracy = "" 'z.AdresZakladuPracy
                w.KodZakladuPracy = "" 'z.KodZakladuPracy
                w.MiastoZakladuPracy = "" 'z.MiastoZakladuPracy
                w.odswierz()
            End If

        End If
    End Sub
    Private Sub SzukajFirmyNazwa(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Enter Then

            Dim t As TextBox = sender


            If t.Text = "" Then Exit Sub

            Dim firmy = (From a In db.Zaklady Where a.NazwaZakladuPracy.ToLower.Contains(t.Text.ToLower) Order By a.NazwaZakladuPracy).ToList
            'Me.DataGridFirmy.ItemsSource = firmy

            If firmy.Count > 0 Then
                Dim z = firmy.First
                w.NIPzakladu = z.NIPzakladu
                w.NazwaZakladuPracy = z.NazwaZakladuPracy
                w.AdresZakladuPracy = "" 'z.AdresZakladuPracy
                w.KodZakladuPracy = "" 'z.KodZakladuPracy
                w.MiastoZakladuPracy = "" 'z.MiastoZakladuPracy
                w.odswierz()
            End If

        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try

      
        w.odswierz()
        db.SaveChanges()

        If w.ZwolnienieOd Is Nothing Then
            MsgBox("Wpisz datę od kiedy zwolnienie")
            Exit Sub
        End If

        If w.ZwolnienieDo Is Nothing Then
            MsgBox("Wpisz datę do kiedy zwolnienie")
            Exit Sub
        End If
        If w.ZwolnienieDni Is Nothing Then
            MsgBox("Wpisz ilość dni zwolnienia")
            Exit Sub
        End If

        If w.L4DataUrodzenia = "" Then
            MsgBox("Wpisz datę urodzenia")
            Exit Sub
        End If

        'If w.KodABC1 Is Nothing Then
        '    MsgBox("Wpisz Kod ABC")
        '    Exit Sub
        'End If
        'If w.KodABC2 Is Nothing Then
        '    MsgBox("Wpisz Kod ABC")
        '    Exit Sub
        'End If
        'If w.KodABC3 Is Nothing Then
        '    MsgBox("Wpisz Kod ABC")
        '    Exit Sub
        'End If
        'If w.KodABC4 Is Nothing Then
        '    MsgBox("Wpisz Kod ABC")
        '    Exit Sub
        'End If

        If w.WystawionoDnia Is Nothing Then
            MsgBox("Wpisz datę wystawienia")
            Exit Sub
        End If

        If w.TypyDokumentow Is Nothing Then
            MsgBox("Wybierz typ dokumentu i wpisz numer")
            Exit Sub
        End If

        If w.NrTypuDokumentu = 1 And If(w.Pacjenci.NIP IsNot Nothing, w.Pacjenci.NIP, "") = "" Then
            MsgBox("Wpisz nip pacjenta")
            Exit Sub
        End If

        If w.NrTypuDokumentu = 2 And If(w.Pacjenci.Dowod IsNot Nothing, w.Pacjenci.Dowod, "") = "" Then
            MsgBox("Wpisz nr dowodu pacjenta")
            Exit Sub
        End If

        If w.NrTypuDokumentu = 3 And If(w.Pacjenci.Paszport IsNot Nothing, w.Pacjenci.Paszport, "") = "" Then
            MsgBox("Wpisz nr paszportu pacjenta")
            Exit Sub
        End If

        If If(w.Pacjenci.Pesel IsNot Nothing, w.Pacjenci.Pesel, "") = "" Then
            MsgBox("Wpisz pesel pacjenta")
            Exit Sub
        End If

        If If(w.NIPzakladu IsNot Nothing, w.NIPzakladu, "") = "" Then
            MsgBox("Wpisz NIP zakładu")
            Exit Sub
        End If

        If If(w.NazwaZakladuPracy IsNot Nothing, w.NazwaZakladuPracy, "") = "" Then
            MsgBox("Wpisz NIP zakładu")
            Exit Sub
        End If


        Dim w1 As New WindowWydrukL4_2(w)
            w1.ShowDialog()
        Catch ex As Exception
            Dim err As String = ex.Message + vbCrLf
            If ex.InnerException IsNot Nothing Then
                err += ex.InnerException.Message
            End If
            MsgBox(err)
        End Try
    End Sub

    Private Sub TextBox1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TextBox1.MouseDoubleClick
        w.ZwolnienieOd = Now.Date
        w.odswierzDaty()
    End Sub

    Private Sub TextBox2_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TextBox2.MouseDoubleClick
        w.WystawionoDnia = Now.Date
        w.odswierzDaty()
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox3.TextChanged
        Try
            w.ZwolnienieDni = Me.TextBox3.Text
            w.ZwolnienieDo = w.ZwolnienieOd.Value.AddDays(w.ZwolnienieDni - 1)
        Catch ex As Exception

        End Try
        w.odswierzDaty()
    End Sub

    Private Sub ListBoxTypDokumentu_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListBoxTypDokumentu.SelectionChanged
        Try
            Select Case ListBoxTypDokumentu.SelectedIndex
                Case 0
                    Me.TextBoxNIP.Focus()
                    Me.TextBoxNIP.SelectionStart = 0
                    Me.TextBoxNIP.SelectionLength = Me.TextBoxNIP.Text.Length

                Case 1
                    Me.TextBoxDowod.Focus()
                    Me.TextBoxDowod.SelectionStart = 0
                    Me.TextBoxDowod.SelectionLength = Me.TextBoxDowod.Text.Length
                Case 2
                    Me.TextBoxPaszport.Focus()
                    Me.TextBoxPaszport.SelectionStart = 0
                    Me.TextBoxPaszport.SelectionLength = Me.TextBoxPaszport.Text.Length
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox1.LostFocus
        w.ZwolnienieOdToString = Me.TextBox1.Text
        Me.TextBox3_TextChanged(sender, Nothing)
    End Sub

    Private Sub PrzepiszFirme(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim stara As Wizyty = sender.tag
            w.NIPzakladu = stara.NIPzakladu
            w.NazwaZakladuPracy = stara.NazwaZakladuPracy
            w.AdresZakladuPracy = "" 'stara.NazwaZakladuPracy
            w.KodZakladuPracy = "" 'stara.KodZakladuPracy
            w.MiastoZakladuPracy = "" ' stara.MiastoZakladuPracy
            w.odswierz()
        Catch
        End Try
    End Sub

    Private Sub DodajNowaFirme(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Enter Then
            If Me.TextBoxNIPFirmy.Text <> "" Then
                If db.Zaklady.Where(Function(o) o.NIPzakladu = Me.TextBoxNIPFirmy.Text).Count = 0 Then
                    Dim z As New Zaklady
                    z.NIPzakladu = Me.TextBoxNIPFirmy.Text
                    z.NazwaZakladuPracy = Me.TextBoxNazwaFirmy.Text
                    z.KodZakladuPracy = ""
                    z.AdresZakladuPracy = ""
                    z.MiastoZakladuPracy = ""
                    db.Zaklady.Add(z)
                    db.SaveChanges()

                    MsgBox("Dodano firme")
                    Dim firmy = From a In db.Zaklady Where a.NIPzakladu = Me.TextBoxNIPFirmy.Text Order By a.NazwaZakladuPracy
                    'Me.DataGridFirmy.ItemsSource = firmy
                End If
            Else
                Dim t As TextBox = sender


                If t.Text = "" Then Exit Sub

                Dim firmy = From a In db.Zaklady Where a.NazwaZakladuPracy.ToLower.Contains(t.Text.ToLower) Order By a.NazwaZakladuPracy
                'Me.DataGridFirmy.ItemsSource = firmy

                If firmy.Count > 0 Then
                    Dim z = firmy.First
                    w.NIPzakladu = z.NIPzakladu
                    w.NazwaZakladuPracy = z.NazwaZakladuPracy
                    w.AdresZakladuPracy = "" 'z.AdresZakladuPracy
                    w.KodZakladuPracy = "" 'z.KodZakladuPracy
                    w.MiastoZakladuPracy = "" 'z.MiastoZakladuPracy
                    w.odswierz()
                End If
            End If

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            db.SaveChanges()
            Dim ww As New WindowNrChoroby(db, w.StatystycznaKlasyfikacjaChorob)
            ww.ShowDialog()
            If ww.zapisz = True Then
                Dim chor As StatystycznaKlasyfikacjaChorob = ww.ListBoxChoroby.SelectedItem
                w.StatystycznaKlasyfikacjaChorob = chor
                w.NrStatystycznyChoroby = chor.NrStatystyczny

            End If
            db.SaveChanges()
            w.odswierz()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub OdswierzChorobe(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        w.NrStatystycznyChoroby = sender.text.ToString.ToUpper
        w.odswierz()

        If db.StatystycznaKlasyfikacjaChorob.Where(Function(o) o.NrStatystyczny = w.NrStatystycznyChoroby).Count = 0 Then MsgBox("Podany numer statystyczny jest błędny")
        'db.SaveChanges()
        'w.odswierz()

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        If MsgBox("Czy napewno chcesz usunąć to L4", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        w.ZwolnienieDni = Nothing
        w.ZwolnienieDo = Nothing
        w.ZwolnienieOd = Nothing
        w.WystawionoDnia = Nothing
        w.NIPzakladu = Nothing
        w.AdresZakladuPracy = Nothing
        w.NazwaZakladuPracy = Nothing
        w.odswierz()
        w.odswierzDaty()
    End Sub

    Private Sub TextBox4_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox4.LostFocus
        If Me.TextBox4.Text <> "" Then
            If nippesel.sprawdzPesel(Me.TextBox4.Text) = True Then
                w.Pacjenci.Pesel = Me.TextBox4.Text
                w.Pacjenci.DataUrodzenia = nippesel.dataZPesel(w.Pacjenci.Pesel)
                w.Pacjenci.odswierzMiejscowosci()
            Else
                MsgBox("Uwaga!!! Błędny PESEL")
            End If
        End If
    End Sub

   

    
    Private Sub TextBoxNIP_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBoxNIP.LostFocus
        If Me.TextBoxNIP.Text <> "" Then
            If nippesel.sprawdzNIP(Me.TextBoxNIP.Text) = True Then
                w.Pacjenci.NIP = Me.TextBoxNIP.Text

                w.Pacjenci.odswierzMiejscowosci()
            Else
                MsgBox("Uwaga!!! Błędny NIP")
            End If
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Try
            w.odswierz()
            db.SaveChanges()

            If w.ZwolnienieOd Is Nothing Then
                MsgBox("Wpisz datę od kiedy zwolnienie")
                Exit Sub
            End If

            If w.ZwolnienieDo Is Nothing Then
                MsgBox("Wpisz datę do kiedy zwolnienie")
                Exit Sub
            End If
            If w.ZwolnienieDni Is Nothing Then
                MsgBox("Wpisz ilość dni zwolnienia")
                Exit Sub
            End If

            If w.L4DataUrodzenia = "" Then
                MsgBox("Wpisz datę urodzenia")
                Exit Sub
            End If

            'If w.KodABC1 Is Nothing Then
            '    MsgBox("Wpisz Kod ABC")
            '    Exit Sub
            'End If
            'If w.KodABC2 Is Nothing Then
            '    MsgBox("Wpisz Kod ABC")
            '    Exit Sub
            'End If
            'If w.KodABC3 Is Nothing Then
            '    MsgBox("Wpisz Kod ABC")
            '    Exit Sub
            'End If
            'If w.KodABC4 Is Nothing Then
            '    MsgBox("Wpisz Kod ABC")
            '    Exit Sub
            'End If

            If w.WystawionoDnia Is Nothing Then
                MsgBox("Wpisz datę wystawienia")
                Exit Sub
            End If

            If If(w.NrStatystycznyChoroby IsNot Nothing, w.NrStatystycznyChoroby, "") = "" Then
                MsgBox("Wpisz numer statystyczny choroby")
                Exit Sub
            End If

            If w.TypyDokumentow Is Nothing Then
                MsgBox("Wybierz typ dokumentu i wpisz numer")
                Exit Sub
            End If

            If w.NrTypuDokumentu = 1 And If(w.Pacjenci.NIP IsNot Nothing, w.Pacjenci.NIP, "") = "" Then
                MsgBox("Wpisz nip pacjenta")
                Exit Sub
            End If

            If w.NrTypuDokumentu = 2 And If(w.Pacjenci.Dowod IsNot Nothing, w.Pacjenci.Dowod, "") = "" Then
                MsgBox("Wpisz nr dowodu pacjenta")
                Exit Sub
            End If

            If w.NrTypuDokumentu = 3 And If(w.Pacjenci.Paszport IsNot Nothing, w.Pacjenci.Paszport, "") = "" Then
                MsgBox("Wpisz nr paszportu pacjenta")
                Exit Sub
            End If

            If If(w.Pacjenci.Pesel IsNot Nothing, w.Pacjenci.Pesel, "") = "" Then
                MsgBox("Wpisz pesel pacjenta")
                Exit Sub
            End If

            If If(w.NIPzakladu IsNot Nothing, w.NIPzakladu, "") = "" Then
                MsgBox("Wpisz NIP zakładu")
                Exit Sub
            End If

            If If(w.NazwaZakladuPracy IsNot Nothing, w.NazwaZakladuPracy, "") = "" Then
                MsgBox("Wpisz NIP zakładu")
                Exit Sub
            End If


            Dim w1 As New WindowWydrukL4_1(w)
            w1.ShowDialog()
        Catch ex As Exception
            Dim err As String = ex.Message + vbCrLf
            If ex.InnerException IsNot Nothing Then
                err += ex.InnerException.Message
            End If
            MsgBox(err)
        End Try
    End Sub
End Class
