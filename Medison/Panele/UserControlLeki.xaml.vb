Public Class UserControlLeki
    Implements MyUserControl

    Dim db As New medisonEntities

   

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        Dim postaciLeku As String() = {"tab.", "amp.", "czop.", "maść", "żel", "krople", "syrop", "sasz.", "zioła"}
        Me.ComboPostacLeku.ItemsSource = postaciLeku

        setInfo("Ładowanie listy leków, proszę czekać...")

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

    End Sub



    Private Sub szukaj()
        setInfo("Ładowanie listy lekow")
        Me.Leki = db.Leki.OrderBy(Function(o) o.NazwaLeku).ToList
        Me.DataContext = Leki
        setInfo("Lista leków załadowana")
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub
    Dim Leki As New List(Of Leki)
    Private Sub ButtonDodaj_Click(sender As Object, e As RoutedEventArgs) Handles ButtonDodaj.Click
        Dim l = New Leki
        l.Wycofany = False
        db.Leki.Add(l)
        Me.Leki.Add(l)
        Me.DataGrid1.Items.Refresh()
        Me.DataGrid1.SelectedItem = l
    End Sub






    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            Dim a = db.SaveChanges()
            MsgBox("Zapisano zmiany")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

   
End Class
