Public Class WindowPodgladWydrukuWizyt

    Dim pac As Pacjenci
    Sub New(p As Pacjenci)
        InitializeComponent()
        pac = p
    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataGridWizyty.ItemsSource = pac.Wizyty.Where(Function(o) If(o.CzyDrukowac IsNot Nothing, o.CzyDrukowac, False) = True)
    End Sub
End Class
