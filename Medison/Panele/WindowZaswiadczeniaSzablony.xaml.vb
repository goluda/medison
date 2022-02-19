Public Class WindowZaswiadczeniaSzablony

    Dim db As New medisonEntities
    Public wybrana As CelSzablony

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            db.SaveChanges()
            MsgBox("Zapisano")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.celSzablony = db.CelSzablony.ToList
        Me.DataContext = Me.celSzablony

    End Sub

    Private Sub Wstaw(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            db.SaveChanges()
            wybrana = sender.tag
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
    Dim celSzablony As New List(Of CelSzablony)
    Private Sub ButtonDodaj_Click(sender As Object, e As RoutedEventArgs) Handles ButtonDodaj.Click
        Dim l = New CelSzablony

        db.CelSzablony.Add(l)
        Me.celSzablony.Add(l)
        Me.DataGrid2.Items.Refresh()
        Me.DataGrid2.SelectedItem = l
    End Sub
End Class
