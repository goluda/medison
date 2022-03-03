Public Class UserControlUSGZaMiesiac
    Implements MyUserControl

    Dim db As New medisonEntities

   

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        setInfo("Ładowanie listy lekarzy, proszę czekać...")

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
        setInfo("Ładowanie danych")
        Dim miesiac As Integer = 0
        Dim rok As Integer = 0
        miesiac = dane.data.Month
        rok = dane.data.Year

        Dim usg1 = (From a In db.usg Where a.CzyjeUsgSkrot <> "x").ToList
        Dim czUsg = (From a In usg1 Select a.NrUsg).ToArray

        Dim temp = (From a In db.Wizyty _
                   Join l In db.DaneLekarza On a.NrGabinetu Equals l.NrGabinetu _
                   Where a.DataWizyty.Value.Year = rok And a.DataWizyty.Value.Month = miesiac And a.NrUsg And czUsg.Contains(a.NrUsg) _
                   Select a.DataWizyty.Value.Month, a.DataWizyty.Value.Year, a.Usg1.CzyjeUsg, a.Usg1.KwotaUsg, lekarz = l.Nazwisko + " " + l.Imie).ToList

        Dim temp1 = From a In temp Group a By a.lekarz, a.Month, a.Year Into g = Group _
                  Select Year, Month, lekarz, _
                  AniaIlosc = g.Where(Function(o) o.CzyjeUsg = "Ania").Count, _
                  AniaKwota = g.Where(Function(o) o.CzyjeUsg = "Ania").Sum(Function(o) o.KwotaUsg), _
                  AniaIloscDzieci = g.Where(Function(o) o.CzyjeUsg = "Ania dzieci").Count, _
                  AniaKwotaDzieci = g.Where(Function(o) o.CzyjeUsg = "Ania dzieci").Sum(Function(o) o.KwotaUsg), _
                  KrzysiekIlosc = g.Where(Function(o) o.CzyjeUsg = "Krzysiek").Count, _
                  KrzysiekKwota = g.Where(Function(o) o.CzyjeUsg = "Krzysiek").Sum(Function(o) o.KwotaUsg), _
                  KrzysiekIloscDzieci = g.Where(Function(o) o.CzyjeUsg = "Krzysiek dzieci").Count, _
                  KrzysiekKwotaDzieci = g.Where(Function(o) o.CzyjeUsg = "Krzysiek dzieci").Sum(Function(o) o.KwotaUsg), _
                  suma = g.Where(Function(o) o.CzyjeUsg = "Ania").Sum(Function(o) o.KwotaUsg) + _
                  g.Where(Function(o) o.CzyjeUsg = "Ania dzieci").Sum(Function(o) o.KwotaUsg) + _
                  g.Where(Function(o) o.CzyjeUsg = "Krzysiek").Sum(Function(o) o.KwotaUsg) + _
                  g.Where(Function(o) o.CzyjeUsg = "Krzysiek dzieci").Sum(Function(o) o.KwotaUsg)



        Me.LabelData.Content = Now.Date.ToLongDateString
        Me.DataContext = temp1
        setInfo("Pobrane dane")
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim b As Button = sender
        b.Visibility = Windows.Visibility.Hidden
        Module1.drukujVisual(Me, "Rozliczenie usg", 1000, 1000)

        b.Visibility = Windows.Visibility.Visible
    End Sub
End Class
