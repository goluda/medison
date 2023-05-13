Public Class UserControlSzczegolyPacjenta
    Implements MyUserControl
    Dim db As medisonEntities
    Dim WithEvents p As Pacjenci
    Dim lp As MyUserControl
    Dim nowy As Boolean
    Dim pr As New PrintDialog

    Dim printDialog As New Windows.Controls.PrintDialog()

    Public wizytaGlowna As UserControlWizyta

    Sub New(ByVal _db As medisonEntities, ByVal _p As Pacjenci, ByVal _lp As MyUserControl, ByVal _nowy As Boolean)
        InitializeComponent()
        Me.ComboBoxGrupaKrwi.Items.Add("A")
        Me.ComboBoxGrupaKrwi.Items.Add("B")
        Me.ComboBoxGrupaKrwi.Items.Add("AB")
        Me.ComboBoxGrupaKrwi.Items.Add("0")

        Me.ComboBoxRh.Items.Add("+")
        Me.ComboBoxRh.Items.Add("-")

        Me.ComboBoxPlec.Items.Add("n")
        Me.ComboBoxPlec.Items.Add("K")
        Me.ComboBoxPlec.Items.Add("M")
        Me.ComboBoxPlec.Items.Add("x")

        db = _db
        p = _p
        lp = _lp
        nowy = _nowy
        Me.DataContext = p
        Me.ListViewWizyty.ItemsSource = p.Wizyty.OrderByDescending(Function(o) o.DataWizyty)

        OdswiezListy()


    End Sub

    Sub OdswiezListy()
        Me.ListBoxZaswiadczenia.ItemsSource = p.Zaswiadczenia.OrderByDescending(Function(o) o.DataZaswiad)
        Me.ListBoxSkierowania.ItemsSource = p.Skierowania.OrderByDescending(Function(o) o.DataSkierow)
    End Sub

    Sub info()

        setInfo("Załadowano dane pacjenta: " + If(p.Nazwisko IsNot Nothing, p.Nazwisko, "") + " " + If(p.Imie IsNot Nothing, p.Imie, ""))
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        info()
    End Sub

    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
        AddHandler p.PropertyChanged1, AddressOf zmianaDanychPacjenta
    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose

        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
        RemoveHandler p.PropertyChanged1, AddressOf zmianaDanychPacjenta
    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)

    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            If wizytaGlowna IsNot Nothing Then
                wizytaGlowna.Button1_Click(sender, e)
                Exit Sub
            End If
            '======================
            If nowy = True Then
                db.Pacjenci.Add(p)
            End If

            Dim a = p.Wizyty.OrderByDescending(Function(o) o.DataWizyty)
            If a.Count > 0 Then
                a.First.L4 = False
            End If

            db.SaveChanges()
            System.Threading.Thread.Sleep(1000)
            System.Windows.Forms.Application.DoEvents()

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

    Private Sub zmianaDanychPacjenta(ByVal sender As Object, ByVal e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName = "Nazwisko" Or e.PropertyName = "Imie" Then info()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim w As New WindowFirma(db)
        w.ShowDialog()
        If w.wybranaFirma IsNot Nothing Then
            p.Zaklady = w.wybranaFirma
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click

        Dim z As Zaswiadczenia = New Zaswiadczenia With {.DataZaswiad = Now.Date}
        p.Zaswiadczenia.Add(z)
        OdswiezListy()

        Me.ListBoxZaswiadczenia.ScrollIntoView(z)
        Me.ListBoxZaswiadczenia.SelectedItem = z
        Me.ListBoxZaswiadczenia.Items.Refresh()
    End Sub

  
    Private Sub ButtonNoweSkierowanie_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ButtonNoweSkierowanie.Click
        Dim s As New Skierowania With {.DataSkierow = Now.Date}
        p.Skierowania.Add(s)
        OdswiezListy()

        Me.ListBoxSkierowania.ScrollIntoView(s)
        Me.ListBoxSkierowania.SelectedValue = s
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

   

    Public Function generujKarte() As FlowDocument
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

        For Each w2 In p.Wizyty
            If w2.CzyDrukowac Is Nothing Then w2.CzyDrukowac = False
        Next

        For Each w In p.Wizyty.Where(Function(o) o.CzyDrukowac = True).OrderBy(Function(o) o.DataWizyty)
            Dim parW As New Paragraph With {.FontSize = 12, .FontFamily = New Windows.Media.FontFamily("Arial")}
            parW.Margin = New Thickness(130, 0, 0, 0)
            ff.Blocks.Add(parW)
            
            Dim zw As String = ""
            If w.ZwolnienieOd IsNot Nothing Then
                zw = "Zwol. od: " + w.ZwolnienieOd.Value.ToShortDateString + " "
                If w.ZwolnienieDo IsNot Nothing Then
                    zw += "Zwol. do: " + w.ZwolnienieDo.Value.ToShortDateString
                End If
            End If
            parW.Inlines.Add(vbCr + w.DataWizyty.Value.ToShortDateString + " " + vbCr + zw + " " + If(w.ZwolnienieDni IsNot Nothing, If(w.ZwolnienieDni <> 0, w.ZwolnienieDni.ToString, ""), "") + vbCr)
            If w.NazwiskoLekarza IsNot Nothing Then
                parW.Inlines.Add(New Bold(New Run("Lekarz: ")))
                parW.Inlines.Add(w.NazwiskoLekarza + " " + If(w.ImieLekarza IsNot Nothing, w.ImieLekarza, "") + vbCr + vbCr)
            End If
            Try
                Dim chor As String = w.NrStatystycznyChoroby
                Try
                    chor += " " + w.StatystycznaKlasyfikacjaChorob.opis
                Catch ex As Exception

                End Try
                parW.Inlines.Add(vbCr + "ICD-10: " + chor + vbCr)
            Catch ex As Exception

            End Try
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
                    Try
                        parRec.Inlines.Add(If(l.Leki.NazwaLeku IsNot Nothing, l.Leki.NazwaLeku, "") + " " + If(l.Postac IsNot Nothing, l.Postac, "") + " " + If(l.Ilosc IsNot Nothing, l.Ilosc, "") + " " + If(l.Dawkowanie IsNot Nothing, l.Dawkowanie, "") + vbCr)
                    Catch
                    End Try
                Next
                ff.Blocks.Add(parRec)
            End If

        Next

        Return ff
    End Function

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Try
            db.SaveChanges()
            Dim ff = generujKarte()
            'Dim ff = New UserControlWydrukHistoria(p)


            Dim pagSource As IDocumentPaginatorSource = ff
            pr.PrintDocument(pagSource.DocumentPaginator, "Historia: " + p.ToString)
            'Dim podlad As New WindowPodgladDokumentu(ff)
            'podlad.ShowDialog()
            'podlad = Nothing
        Catch ex As Exception
            MsgBox(ex.Message + vbCr + ex.StackTrace)
        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            db.SaveChanges()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DrukujZaswiadczenie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        db.SaveChanges()
        Dim z As Zaswiadczenia = sender.tag
        Module1.drukujVisual(New UserControlWydrukZaswiadczenie(z), "Zaświadczenie", 800, 800)
    End Sub

    Private Sub DrukujSkierowanie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        db.SaveChanges()
        Dim z As Skierowania = sender.tag
        If z.xml Is Nothing Then
            Module1.drukujVisual(New UserControlWydrukSkierowania(z), "Skierowanie", 800, 800)
        Else
            If z.DoKogo = "Na zabiegi" Then
                Dim w As New WindowSkierowanieNaZabiegi(p, z)
                w.ShowDialog()
                w = Nothing
                db.SaveChanges()
            End If
            If z.DoKogo = "KT" Then
                Dim w As New WindowSkierownieKT(p, z)
                w.ShowDialog()
                w = Nothing
                db.SaveChanges()
            End If
            If z.DoKogo = "Do pracowni rezonansu magnetycznego" Then
                Dim w As New WindowSkierowanieDoPracowniRezonansuMagnetycznego(p, z)
                w.ShowDialog()
                w = Nothing
                db.SaveChanges()
            End If
        End If
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

    Private Sub PokazWizyte(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim ala As String = lp.ToString

        Dim w As New UserControlWizyta(db, sender.tag, If(ala = "Medison.UserControlListaPacjentow", Me, lp))
        If Me.wizytaGlowna IsNot Nothing Then
            For Each p1 In Me.wizytaGlowna.poprzednie
                w.poprzednie.Add(p1)
            Next
            w.poprzednie.Add(wizytaGlowna.w)
        End If
        Module1.loadModule(w)
    End Sub

    Private Sub UsunSkierowanie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If MsgBox("Czy usunąć to skierowanie?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                Dim s As Skierowania = sender.tag
                db.Skierowania.Remove(s)
                db.SaveChanges()
            Catch
            End Try
        End If
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

    Private Sub UsunZaswiadczenie(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If MsgBox("Czy usunąć to zaświadczenie?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                Dim s As Zaswiadczenia = sender.tag
                db.Zaswiadczenia.Remove(s)
                db.SaveChanges()
            Catch
            End Try
        End If
        Me.ListBoxZaswiadczenia.Items.Refresh()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button4.Click
        For Each w In p.Wizyty
            w.CzyDrukowac = True
            w.odswierz()

        Next
    End Sub

    Private Sub Button6_Click_1(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button6.Click
        For Each w In p.Wizyty
            w.CzyDrukowac = False
            w.odswierz()
        Next
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button7.Click
        Dim w As New WindowPodgladWydrukuWizyt(p)
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub SkierowanieSzablon(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim w As New WindowSkierowaniaSzablony
        w.ShowDialog()
        If w.wybrana IsNot Nothing Then
            Dim s As Skierowania = sender.tag
            s.Cel = w.wybrana.cel
            s.DoKogo = w.wybrana.SkierowaniaPoradnie.NazwaPoradni
        End If
        w = Nothing
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

    Private Sub ZaswiadczenieSzablon(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim w As New WindowZaswiadczeniaSzablony
        w.ShowDialog()
        If w.wybrana IsNot Nothing Then
            Dim s As Zaswiadczenia = sender.tag
            s.Cel = w.wybrana.cel
        End If
        w = Nothing
        Me.ListBoxZaswiadczenia.Items.Refresh()
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button8.Click
        Dim s As New Skierowania
        s.DataSkierow = Now.Date
        Dim w As New WindowSkierowanieDoPracowniRezonansuMagnetycznego(p, s)
        w.ShowDialog()
        w = Nothing
        If MsgBox("Zapisać?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            p.Skierowania.Add(s)
            db.SaveChanges()
        End If
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button9.Click
        Dim s As New Skierowania
        s.DataSkierow = Now.Date
        Dim w As New WindowSkierownieKT(p, s)
        w.ShowDialog()
        w = Nothing
        If MsgBox("Zapisać?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            p.Skierowania.Add(s)
            db.SaveChanges()
        End If
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button10.Click
        Dim s As New Skierowania
        s.DataSkierow = Now.Date
        Dim w As New WindowSkierowanieNaZabiegi(p, s)
        w.ShowDialog()
        w = Nothing
        If MsgBox("Zapisać?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            p.Skierowania.Add(s)
            db.SaveChanges()
        End If
        Me.ListBoxSkierowania.Items.Refresh()
    End Sub
End Class
