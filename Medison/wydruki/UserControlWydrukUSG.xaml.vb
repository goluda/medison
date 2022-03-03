Public Class UserControlWydrukUSG

    Sub New(ByVal w As Wizyty)
        InitializeComponent()
        Me.DataContext = w

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
