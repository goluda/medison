Public Class WindowNowaRezerwacja

    Dim db As medisonEntities
    Dim w As Wizyty

    Dim oldUSG As Decimal = 0
    Dim n As Boolean

    Sub New(ByVal _rezerwacja As Wizyty, ByVal _db As medisonEntities, ByVal nowa As Boolean)
        InitializeComponent()
        db = _db
        Me.ComboUSG.ItemsSource = db.Usg.ToList

        w = _rezerwacja
        n = nowa

        Try

            oldUSG = db.Usg.Where(Function(o) o.NrUsg = w.NrUsg).First.KwotaUsg
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = w
        If n = True Then

            Me.TextBoxSzukajPacjenta.Focus()
            Me.ZaznaczWszystko(Me.TextBoxSzukajPacjenta, Nothing)
        Else
            Me.TextBoxGodzina.Focus()
            Me.ZaznaczWszystko(Me.TextBoxGodzina, Nothing)
        End If
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Me.TextBoxSzukajPacjenta.Text = "" Then Exit Sub
        Dim szu As String = Me.TextBoxSzukajPacjenta.Text
        Dim temp = (From a In db.Pacjenci Where
                 a.Nazwisko.StartsWith(szu) Or
                 a.Pesel.StartsWith(szu)
                    Order By a.Nazwisko, a.Imie).ToList

        Me.DataGridSzukajPacjentow.ItemsSource = temp


        If k = Key.Enter Then
            If temp.Count = 0 Then
                If (MsgBox("Dodać nowego pacjenta?", MsgBoxStyle.YesNo)) = MsgBoxResult.Yes Then
                    Dim p As New Pacjenci
                    p.Nazwisko = Me.TextBoxSzukajPacjenta.Text
                    If p.Nazwisko.EndsWith("x") Then p.Nazwisko = p.Nazwisko.Substring(0, p.Nazwisko.Length - 1)
                    If p.Nazwisko.EndsWith("X") Then p.Nazwisko = p.Nazwisko.Substring(0, p.Nazwisko.Length - 1)
                    p.Kod = "66-400"
                    p.Miejscowosc = "Gorzów Wlkp."
                    Dim w1 As New WindowSzczegolyPacjentaKrotkie(p)
                    w1.ShowDialog()
                    db.Pacjenci.Add(p)
                    db.SaveChanges()
                    Dim szu1 As String = p.Nazwisko
                    Dim temp1 = (From a In db.Pacjenci Where
                             a.Nazwisko.StartsWith(szu1)
                                 Order By a.Nazwisko, a.Imie).ToList
                    Me.DataGridSzukajPacjentow.ItemsSource = temp1
                    w.Pacjenci = p
                    w.Nrp = p.Nrp
                    w.odswierz()
                    Me.TextBoxGodzina.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub WybierzPacjenta(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        w.Pacjenci = sender.tag
        w.odswierz()
        w.Nrp = w.Pacjenci.Nrp
        TextBoxGodzina.Focus()
    End Sub

    Public Event CloseWizyta()


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If w.Nrp Is Nothing Then
            MsgBox("Wybierz pacjenta")
            Exit Sub
        End If

        Dim newusg As Decimal = 0
        Try
            Try
                If db.Usg.Where(Function(o) o.NrUsg = w.NrUsg).First.CzyjeUsgSkrot = "x" Then
                    w.NrUsg = Nothing
                End If
            Catch ex As Exception

            End Try

            newusg = db.Usg.Where(Function(o) o.NrUsg = w.NrUsg).First.KwotaUsg
        Catch ex As Exception

        End Try
        If w.Wplata Is Nothing Then w.Wplata = 0
        w.Wplata = w.Wplata - oldUSG + newusg
        w.CzyDrukowac = False
        If n = True Then db.Wizyty.Add(w)
        db.SaveChanges()
        RaiseEvent CloseWizyta()
    End Sub

    Dim k As System.Windows.Input.Key

    Private Sub TextBoxSzukajPacjenta_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBoxSzukajPacjenta.KeyDown
        k = e.Key
        If e.Key = Key.Enter Then
            Me.Button1_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub ZaznaczWszystko(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim tb As TextBox = sender
        tb.SelectionStart = 0
        tb.SelectionLength = tb.Text.Length
    End Sub

    Private Sub TextBoxSzukajPacjenta_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBoxSzukajPacjenta.TextChanged
        'k = Nothing
        If TextBoxSzukajPacjenta.Text.Length >= 3 Then
            Me.Button1_Click(Me, e)
        End If
    End Sub

    Sub ShowDialog()
        Throw New NotImplementedException
    End Sub

End Class
