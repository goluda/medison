Public Class WindowSkierowaniaSzablony

    Dim db As New medisonEntities
    Public wybrana As SkierowaniaCele

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            db.SaveChanges()
            MsgBox("Zapisano")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = db.SkierowaniaPoradnie.ToList
    End Sub

    Private Sub Wstaw(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            db.SaveChanges()
            wybrana = sender.tag
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class
