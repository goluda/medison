Public Class UserControlWizyta
    Implements MyUserControl
    Dim db As medisonEntities
    Public WithEvents w As Wizyty
    Dim lp As MyUserControl

    Public isReadonly As Boolean = False

    Dim wybranySzablon As typSzablonu = typSzablonu.Wywiad

    Enum typSzablonu
        Wywiad = 1
        Zalecenia = 4
        USG = 0
        Rozpoznanie = 2
        Badania = 3
    End Enum

    Sub New(ByVal _db As medisonEntities, ByVal _w As Wizyty, ByVal _lp As MyUserControl)
        InitializeComponent()
        Me.ComboBoxGrupaKrwi.Items.Add("A")
        Me.ComboBoxGrupaKrwi.Items.Add("B")
        Me.ComboBoxGrupaKrwi.Items.Add("AB")
        Me.ComboBoxGrupaKrwi.Items.Add("0")

        Me.ComboBoxRh.Items.Add("+")
        Me.ComboBoxRh.Items.Add("-")

        Me.ComboBoxUprawnienia.ItemsSource = New String() {"", "X", "IB", "IW", "ZK"}
        'Me.ComboBoxChorobyPrzewlekle.ItemsSource = New String() {"X", "P"}


        'Me.ComboPostac.ItemsSource = New String() {"tab.", "amp.", "czop.", "maść", "żel", "krople", "syrop", "sasz.", "zioła"}

        db = _db
        '        Me.ComboBoxNrStatystyczny.ItemsSource = db.StatystycznaKlasyfikacjaChorob.OrderBy(Function(o) o.NrStatystyczny)
        w = _w
        lp = _lp
        Me.DataContext = w

        If w.Pacjenci.KasaChorych Is Nothing Then w.Pacjenci.KasaChorych = "04"
        If w.Pacjenci.KasaChorych = "04R" Then w.Pacjenci.KasaChorych = "04"
        If w.Pacjenci.ReceptaUprawnienia Is Nothing Then w.Pacjenci.ReceptaUprawnienia = ""
        If w.Pacjenci.ReceptaChorobyPrzewlekle Is Nothing Then w.Pacjenci.ReceptaChorobyPrzewlekle = "X"

        'Me.ComboLeki.ItemsSource = db.Leki.OrderBy(Function(o) o.NazwaLeku)
        Me.ComboBoxGrupaSzablonow.ItemsSource = db.SzabGrupy.ToList
        Me.ComboBoxGrupaSzablonow.SelectedIndex = 0

    End Sub

    Sub info()

        setInfo("Załadowano dane wizyty: " + w.NrWizyty.ToString)
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        info()
        If Me.isReadonly = True Then Me.Background = Windows.Media.Brushes.DarkRed

        ladujRozpoznanie()
        Dim h As New UserControlSzczegolyPacjenta(db, w.Pacjenci, lp, False)
        h.wizytaGlowna = Me
        'h.Button1.Visibility = Windows.Visibility.Collapsed
        Me.GridHistoria.Children.Add(h)
        Me.GridRecepty.DataContext = w
        Me.Stack1.DataContext = w
        Me.Stack2.DataContext = w
    End Sub

    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose
        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)

    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)

    End Sub

    Public poprzednie As New List(Of Wizyty)
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click

        Try
            If isReadonly = False Then
                w.L4 = False
                For Each p In poprzednie
                    p.L4 = False
                Next
                db.SaveChanges()
                System.Threading.Thread.Sleep(1000)
                System.Windows.Forms.Application.DoEvents()
            End If
            Module1.odblokujPrzyciski()
            loadModule(lp)
        Catch ex As Exception
            Dim err As String = ex.Message + vbCrLf
            If ex.InnerException IsNot Nothing Then
                err += ex.InnerException.Message
            End If
            MsgBox(err)
        End Try

    End Sub


    Private Sub OdswierzNumerStat(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        w.odswierz()
    End Sub

    Private Sub rowne(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim s As Szablony = sender.tag

        Select Case wybranySzablon
            Case typSzablonu.Badania
                w.Badania = s.OpisSz


            Case typSzablonu.Rozpoznanie
                w.Rozpoznanie = s.OpisSz

            Case typSzablonu.USG
                w.usg = s.OpisSz

            Case typSzablonu.Wywiad
                w.Wywiad = s.OpisSz

            Case typSzablonu.Zalecenia
                w.Zalecenia = s.OpisSz



        End Select

        w.odswierz()

    End Sub

    Private Sub dodaj(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim s As Szablony = sender.tag

        Select Case wybranySzablon
            Case typSzablonu.Badania
                w.Badania += " " + s.OpisSz


            Case typSzablonu.Rozpoznanie
                w.Rozpoznanie += " " + s.OpisSz

            Case typSzablonu.USG
                w.usg += " " + s.OpisSz

            Case typSzablonu.Wywiad
                w.Wywiad += " " + s.OpisSz

            Case typSzablonu.Zalecenia
                w.Zalecenia += " " + s.OpisSz



        End Select

        w.odswierz()
    End Sub

    Sub ladujRozpoznanie()
        Dim tt As Integer = wybranySzablon
        Dim gr As SzabGrupy = Me.ComboBoxGrupaSzablonow.SelectedItem
        Me.GridSzablon.DataContext = (From a In db.Szablony Where
                                   a.NrGabinetu = Module1.dane.WybranyLekarz.NrGabinetu _
                                   And a.NrGrSzablon = gr.NrGrSzablon _
                                   And a.NrRodzSzab = tt
                                      Order By a.Kolejnosc).ToList

    End Sub

    Private Sub WybierzWywiad(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        wybranySzablon = typSzablonu.Wywiad
        ladujRozpoznanie()
    End Sub

    Private Sub WybierzBadania(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        wybranySzablon = typSzablonu.Badania
        ladujRozpoznanie()
    End Sub

    Private Sub WybierzUSG(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        wybranySzablon = typSzablonu.USG
        ladujRozpoznanie()
    End Sub

    Private Sub WybierzRozpznanie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        wybranySzablon = typSzablonu.Rozpoznanie
        ladujRozpoznanie()
    End Sub

    Private Sub WybierzZalecenia(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        wybranySzablon = typSzablonu.Zalecenia
        ladujRozpoznanie()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim ww As New WindowL4(w, db)
        ww.ShowDialog()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Dim ww As New WindowFV(w, db)
        ww.ShowDialog()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Dim c As New UserControlWydrukUSG(w)
        Module1.drukujVisual(c, "USG", 700, 1000)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Try
            db.SaveChanges()





            'Dim ww As New UserControlWydrukWizyty(w)
            'Module1.drukujVisual(ww, "Wizyta", 800, 400)

            'Exit Sub

            Dim p = w.Pacjenci

            Dim pr As New PrintDialog
            Dim ff As New FlowDocument
            ff.FontFamily = New Windows.Media.FontFamily("Arial")
            'ff.PageWidth = 700
            ff.PageWidth = 700
            ff.PageHeight = 700
            ff.PagePadding = New Thickness(50)
            ff.ColumnGap = 0
            ff.ColumnWidth = 700


            Dim par1 As New Paragraph()
            par1.Margin = New Thickness(370, 0, 0, 0)
            par1.TextAlignment = TextAlignment.Left
            par1.FontWeight = System.Windows.FontWeights.Bold
            par1.Inlines.Add(p.NazwiskoWielkimiLiterami + vbCr + p.Imie)
            ff.Blocks.Add(par1)

            Dim par2 As New Paragraph
            par2.Margin = New Thickness(130, 0, 0, 0)
            par2.TextAlignment = TextAlignment.Left

            par2.Inlines.Add(vbCr + vbCr + "Data ur: " + If(p.DataUrodzenia IsNot Nothing, p.DataUrodzenia.Value.ToShortDateString, "") + " " + If(p.Pesel IsNot Nothing, p.Pesel, "") + vbCr + p.PelnyAdresPacjenta)
            ff.Blocks.Add(par2)

            'For Each w In p.Wizyties.Where(Function(o) o.CzyDrukowac = True)
            Dim parW As New Paragraph With {.FontSize = 12, .FontFamily = New Windows.Media.FontFamily("Arial")}
            parW.Margin = New Thickness(130, 0, 0, 0)
            ff.Blocks.Add(parW)
            Try
                Dim chor As String = w.NrStatystycznyChoroby
                Try
                    chor += " " + w.StatystycznaKlasyfikacjaChorob.opis
                Catch ex As Exception

                End Try
                parW.Inlines.Add(vbCr + "Rozpoznanie: " + chor + vbCr)

            Catch ex As Exception

            End Try
            Dim zw As String = ""
            If w.ZwolnienieOd IsNot Nothing Then
                zw = "Zwol. od: " + w.ZwolnienieOd.Value.ToShortDateString + " "
                If w.ZwolnienieDo IsNot Nothing Then
                    zw += "Zwol. do: " + w.ZwolnienieDo.Value.ToShortDateString
                End If
            End If
            parW.Inlines.Add(vbCr + w.DataWizyty.Value.ToShortDateString + " " + zw + " " + If(w.ZwolnienieDni IsNot Nothing, If(w.ZwolnienieDni <> 0, w.ZwolnienieDni.ToString, ""), "") + vbCr)
            If w.NazwiskoLekarza IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Lekarz: ")))
                parW.Inlines.Add(w.NazwiskoLekarza + " " + If(w.ImieLekarza IsNot Nothing, w.ImieLekarza, "") + vbCr + vbCr)
            End If
            If w.Wywiad IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Wywiad: ")))
                parW.Inlines.Add(w.Wywiad + vbCr + vbCr)
            End If
            If w.Badania IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Badania: ")))
                parW.Inlines.Add(w.Badania + vbCr + vbCr)
            End If
            If w.Rozpoznanie IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Rozpoznanie: ")))
                parW.Inlines.Add(w.Rozpoznanie + vbCr + vbCr)
            End If
            If w.Zalecenia IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Zalecenia:  ")))
                parW.Inlines.Add(w.Zalecenia + vbCr + vbCr)
            End If
            parW.Inlines.Add(vbCr)
            'Next

            If w.Recepty.Count > 0 Then
                Dim parRec As New Paragraph With {.FontSize = 10, .FontFamily = New Windows.Media.FontFamily("Arial")}
                parRec.Margin = New Thickness(150, 0, 0, 0)
                parRec.Inlines.Add(New Bold(New Run("Leki" + vbCr)))
                For Each l In w.Recepty
                    parRec.Inlines.Add(l.Leki.NazwaLeku + " " + l.Postac + " " + l.Ilosc + " " + l.Dawkowanie + vbCr)
                Next
                ff.Blocks.Add(parRec)
            End If


            Dim pag As IDocumentPaginatorSource = ff
            pr.PrintDocument(pag.DocumentPaginator, "wizyta")

        Catch ex As Exception
            MsgBox(ex.Message + vbCr + ex.StackTrace)
        End Try
    End Sub

    Private Sub NowySzablon(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim gr As SzabGrupy = Me.ComboBoxGrupaSzablonow.SelectedItem
            Dim s As New Szablony
            s.NrGabinetu = dane.WybranyLekarz.NrGabinetu
            s.NrGrSzablon = gr.NrGrSzablon
            s.NrRodzSzab = wybranySzablon
            Dim ww As New WindowNowySzablon(s)
            ww.ShowDialog()
            db.Szablony.Add(s)
            db.SaveChanges()
            ladujRozpoznanie()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub EdytujSzablon(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim w As New WindowNowySzablon(sender.tag)
            w.ShowDialog()
            db.SaveChanges()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button6.Click
        db.SaveChanges()
        Try
            Dim ww As New WindowNrChoroby(db, w.StatystycznaKlasyfikacjaChorob)
            ww.ShowDialog()
            If ww.zapisz = True Then
                Dim chor As StatystycznaKlasyfikacjaChorob = ww.ListBoxChoroby.SelectedItem
                w.StatystycznaKlasyfikacjaChorob = chor
                w.NrStatystycznyChoroby = chor.NrStatystyczny

            End If
        Catch
        End Try
        db.SaveChanges()
        w.odswierz()
    End Sub

    Private Sub OdswierzChorobe(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        w.NrStatystycznyChoroby = sender.text.ToString.ToUpper
        w.odswierz()
        If db.StatystycznaKlasyfikacjaChorob.Where(Function(o) o.NrStatystyczny = w.NrStatystycznyChoroby).Count = 0 Then MsgBox("Podany numer statystyczny jest błędny")
        'db.SaveChanges()
        'w.odswierz()
    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub

    Private Sub UsunGodzine(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        w.wszedl = Nothing
        w.odswierz()
    End Sub

    Private Sub UsunLek(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            If MsgBox("Czy chcesz usunąć ten lek?", vbYesNo) = MsgBoxResult.Yes Then
                Dim l As Recepty = sender.tag
                w.Recepty.Remove(l)
                Try
                    db.Recepty.Remove(l)
                Catch ex As Exception

                End Try
                Me.GridRecepty.Items.Refresh()

            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DrukujRecepte(sender As System.Object, e As System.Windows.RoutedEventArgs)
        db.SaveChanges()
        If If(w.Pacjenci.Pesel IsNot Nothing, w.Pacjenci.Pesel.Trim, "") = "" Then
            MsgBox("Proszę podać numer PESEL")
            Exit Sub
        End If

        Dim temp = w.Recepty.Where(Function(o) If(o.indywidualna IsNot Nothing, o.indywidualna, False) = False).ToList()
        Dim opusc As Integer = 0
        If MsgBox("Drukować receptę dla Jarockich?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Do While opusc <= temp.Count
                Dim ww As New UserControlRecepta(w, opusc, Nothing)
                Module1.drukujVisual(ww, "Wizyta", 863, 761)
                opusc += 5
                If opusc <= temp.Count Then
                    MsgBox("włóż formularz kolejnej recepty")
                End If
            Loop
        Else
            Do While opusc < temp.Count
                Dim ww As New UserControlReceptaNowa(w, opusc, Nothing)
                Module1.drukujVisual(ww, "Wizyta", 863, 761)
                opusc += 5
                If opusc < temp.Count Then
                    MsgBox("włóż formularz kolejnej recepty")
                End If
            Loop
        End If
    End Sub

    Private Sub dodajLek(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try


            Dim r As New Recepty
            r.indywidualna = False
            r.ReceptaChorobyPrzewlekle = "X"
            Dim w1 As New WindowWizytaLekDoRecepty(db, r)
            w1.ShowDialog()
            If w1.gotowa = True Then
                w.Recepty.Add(r)
                Me.GridRecepty.Items.Refresh()

                db.SaveChanges()
            End If
            For Each r In w.Recepty
                r.odswierzProperty()
            Next
        Catch ex As Exception
            MsgBox(ex.Message + vbCr + ex.InnerException.Message)
        End Try
    End Sub

    Private Sub EdytujLek(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Try

            Dim r As Recepty = sender.tag
            Dim w1 As New WindowWizytaLekDoRecepty(db, r)
            w1.ShowDialog()
            If w1.gotowa = True Then
                w.Recepty.Add(r)
                db.SaveChanges()
            End If
            For Each r In w.Recepty
                r.odswierzProperty()
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.GridRecepty.Items.Refresh()

    End Sub


    Private Sub DrukujWizyte(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        db.SaveChanges()
        If If(w.Pacjenci.Pesel IsNot Nothing, w.Pacjenci.Pesel.Trim, "") = "" Then
            MsgBox("Proszę podać numer PESEL")
            Exit Sub
        End If
        Try
            Dim r As Recepty = sender.tag
            If MsgBox("Drukować receptę dla Jarockich?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Dim ww As New UserControlRecepta(w, r.NrRecepty)
                Module1.drukujVisual(ww, "Wizyta", 863, 761)
            Else
                Dim ww As New UserControlRecepta(w, r.NrRecepty)
                Module1.drukujVisual(ww, "Wizyta", 863, 761)
            End If
        Catch
        End Try
    End Sub

    Private Sub Button3_Copy_Click(sender As Object, e As RoutedEventArgs) Handles Button3_Copy.Click
        Dim ww As New WindowDensityGel(w, db)
        ww.ShowDialog()
        db.SaveChanges()

    End Sub
End Class
