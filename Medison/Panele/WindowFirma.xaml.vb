Public Class WindowFirma
    Public wybranaFirma As Zaklady
    Public db As medisonEntities

    Sub New(ByVal _db As medisonEntities)
        InitializeComponent()
        db = _db
    End Sub

    Sub szukaj()
        Me.DataContext = (From a In db.Zaklady Where
                       If(Me.TextBox1.Text <> "", a.NIPzakladu.Contains(Me.TextBox1.Text), 1 = 1) _
                       Or If(Me.TextBox1.Text <> "", a.NazwaZakladuPracy.Contains(Me.TextBox1.Text), 1 = 1)).ToList
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.wybranaFirma = Nothing
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        szukaj()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox1.KeyDown
        If e.Key = Key.Enter Then
            szukaj()
        End If
    End Sub

    Private Sub Wybierz(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.wybranaFirma = sender.tag
        Me.Close()
    End Sub
End Class
