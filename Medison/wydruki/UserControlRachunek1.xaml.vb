Public Class UserControlRachunek1
    Sub New(ByVal r As Rachunki)
        InitializeComponent()
        Me.DataContext = r
        GridRachunek.SelectedIndex = -1
        GridFaktura.SelectedIndex = -1
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
