Public Class UserControlFirmy
    Implements MyUserControl

    Dim db As New medisonEntities



    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        
        '
        'szukaj()

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

    End Sub



    Private Sub szukaj()
        setInfo("Ładowanie listy firm")
        Dim szukanie As String = Me.TextBoxNazwisko.Text
        Me.Zaklady = (From a In db.Zaklady Where
                         If(szukanie <> "", a.NazwaZakladuPracy.Contains(szukanie), 1 = 1) _
                         Or If(szukanie <> "", a.NIPzakladu.Contains(szukanie), 1 = 1)
                      Order By a.NazwaZakladuPracy).ToList
        Me.DataContext = Me.Zaklady


        setInfo("Lista załadowana")
    End Sub

    Dim Zaklady As New List(Of Zaklady)
    Private Sub ButtonDodaj_Click(sender As Object, e As RoutedEventArgs) Handles ButtonDodaj.Click
        Dim l = New Zaklady

        db.Zaklady.Add(l)
        Me.Zaklady.Add(l)
        Me.DataGrid1.Items.Refresh()
        Me.DataGrid1.SelectedItem = l
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub






    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            Dim a = db.SaveChanges()
            MsgBox("Zapisano zmiany")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub TextBoxNazwisko_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBoxNazwisko.KeyDown
        If e.Key = Key.Enter Then
            szukaj()
        End If
    End Sub
End Class
