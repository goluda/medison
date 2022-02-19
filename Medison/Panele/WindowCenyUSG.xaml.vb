Public Class WindowCenyUSG

    Dim db As New medisonEntities

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataGrid1.ItemsSource = db.Usg.ToList

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            db.SaveChanges()
            MsgBox("Zapisano")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
