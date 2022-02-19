Public Class WindowPodgladDokumentu

    Dim dok As New FlowDocument

    Sub New(ByVal _dok As FlowDocument)
        InitializeComponent()
        dok = _dok

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Dim pagSource As IDocumentPaginatorSource = dok
        Dim pr As New PrintDialog
        pr.PrintDocument(pagSource.DocumentPaginator, "Wydruk")
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Dim pagSource As IDocumentPaginatorSource = dok
        Me.DocumentViewer1.Document = pagSource.DocumentPaginator.GetPage(0)
        
    End Sub
End Class
