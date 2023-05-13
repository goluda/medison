Public Class UserControlWydrukKartaPacjenta

    Sub New(ByVal p As Pacjenci)
        InitializeComponent()
        DataContext = p
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
