Public Class UserControlLekarze
    Implements MyUserControl

    Dim db As New medisonEntities

    Sub New(ByVal _db As medisonEntities)
        InitializeComponent()
        db = _db
    End Sub

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
        setInfo("Ładowanie listy lekarzy")
        Me.Lekarza = db.DaneLekarza.OrderBy(Function(o) o.Nazwisko).ToList
        Me.DataContext = Me.Lekarza

        setInfo("Lista lekarzy załadowana")
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub



   


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            db.SaveChanges()
            MsgBox("Zapisano zmiany")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim l As DaneLekarza = sender.tag
        dane.WybranyLekarz = l
        loadModule(New UserControlRejestracja())
    End Sub
    Dim Lekarza As New List(Of DaneLekarza)

    Private Sub ButtonDodaj_Click(sender As Object, e As RoutedEventArgs) Handles ButtonDodaj.Click
        Dim l = New DaneLekarza
        db.DaneLekarza.Add(l)
        Me.Lekarza.Add(l)
        Me.DataGrid1.Items.Refresh()
        Me.DataGrid1.SelectedItem = l
    End Sub
End Class
