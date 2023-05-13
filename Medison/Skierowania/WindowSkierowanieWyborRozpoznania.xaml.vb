Public Class WindowSkierowanieWyborRozpoznania
    Public rozpoznanie As String = ""
    Public wybrane As Boolean = False

    Dim db As New medisonEntities

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.TextBox1.Focus()
    End Sub

    Private Sub WybierzPozycje(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim s As StatystycznaKlasyfikacjaChorob = sender.tag
            rozpoznanie = s.NrStatystyczny + "-" + s.Rozpoznanie
            wybrane = True
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox1_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles TextBox1.KeyDown
        If e.Key = Key.Enter Then
            If Me.TextBox1.Text <> "" Then
                Dim aa As String = Me.TextBox1.Text.ToString.ToUpper
                Me.DataGrid1.ItemsSource = db.StatystycznaKlasyfikacjaChorob.Where(Function(o) o.NrStatystyczny.StartsWith(aa)).ToList
            End If
        End If
        If e.Key = Key.Escape Then
            Me.Close()
        End If
    End Sub
End Class
