Public Class WindowWydrukL4Test
    Sub New(ByVal w As Wizyty)
        InitializeComponent()
        Wizyta1.wizyta = w
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

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
