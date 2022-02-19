Public Class WindowNrChoroby

    Public zapisz As Boolean = False


    Sub New(ByVal _db As medisonEntities, ByVal _choroba As StatystycznaKlasyfikacjaChorob)
        InitializeComponent()
        Me.ListBoxChoroby.ItemsSource = _db.StatystycznaKlasyfikacjaChorob.OrderBy(Function(o) o.NrStatystyczny)
        Try

            Me.ListBoxChoroby.ScrollIntoView(_choroba)
            Me.ListBoxChoroby.SelectedItem = _choroba
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub ListBoxChoroby_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ListBoxChoroby.MouseDoubleClick
        zapisz = True
        Me.Close()
    End Sub
End Class
