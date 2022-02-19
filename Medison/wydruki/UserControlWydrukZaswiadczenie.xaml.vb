Public Class UserControlWydrukZaswiadczenie

    Sub New(ByVal z As Zaswiadczenia)
        InitializeComponent()
        Me.DataContext = z
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
