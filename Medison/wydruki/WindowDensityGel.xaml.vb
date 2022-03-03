Public Class WindowDensityGel
    Public Property wizyta As Wizyty
        Get
            Return Me.DataContext
        End Get
        Set(ByVal value As Wizyty)
            DataContext = value
        End Set
    End Property

    Dim db As medisonEntities


    Sub New(w As Wizyty, _db As medisonEntities)
        InitializeComponent()
        If w.DensityGel Is Nothing Then w.DensityGel = "prawego stawu kolanowego"
        If w.DensityGelOpis Is Nothing Then w.DensityGelOpis = "Zmiany zwyrodnieniowe kolana prawego"
        wizyta = w
        db = _db
        'w.Pacjenci.Adres1




    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim print As New PrintDialog
        print.ShowDialog()
        'Wizyta1.tlo.Visibility = Windows.Visibility.Hidden
        dokumentL4.PageWidth = print.PrintableAreaWidth
        dokumentL4.PageHeight = print.PrintableAreaHeight
        dokumentL4.ColumnWidth = print.PrintableAreaWidth
        Dim doc As IDocumentPaginatorSource = dokumentL4
        print.PrintDocument(doc.DocumentPaginator, "L4")
    End Sub
End Class
