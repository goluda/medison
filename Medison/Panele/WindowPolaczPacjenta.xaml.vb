Public Class WindowPolaczPacjenta

    Dim db As medisonEntities
    Dim p As Pacjenci
    Sub New(ByVal _p As Pacjenci, ByVal _db As medisonEntities)
        InitializeComponent()
        db = _db
        p = _p
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = p

    End Sub


    Private Sub szukaj()
        Dim danePacjentow = (From a In db.Pacjenci Order By a.Nazwisko, a.Imie Where
                                                                         If(Me.TextBoxImie.Text <> "", a.Imie.StartsWith(Me.TextBoxImie.Text), 1 = 1) _
                                                                         And If(Me.TextBoxNazwisko.Text <> "", a.Nazwisko.StartsWith(Me.TextBoxNazwisko.Text), 1 = 1) _
                                                                         And a.Nrp <> p.Nrp).ToList
        DataContext = danePacjentow
    End Sub

    Private Sub PolaczPacjentow(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim i As Integer = Me.DataGrid1.SelectedIndex

            If MsgBox("Czy napewno chcesz połączyć tego pacjenta z wybranym pacjentem?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            Dim p1 As Pacjenci = sender.tag

            For Each w In p1.Wizyty.ToList
                w.Pacjenci = p
            Next
            For Each w In p1.Zaswiadczenia.ToList
                w.Pacjenci = p
            Next
            For Each w In p1.Skierowania.ToList
                w.Pacjenci = p

            Next
            For Each w In p1.Rachunki.ToList
                w.Pacjenci = p
            Next
            db.Pacjenci.Remove(p1)
            db.SaveChanges()
            szukaj()

            Try
                Me.DataGrid1.SelectedIndex = i
            Catch
            End Try
            MsgBox("Gotowe połączono pacjentów i usunięto wybranego")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        szukaj()
    End Sub

    Private Sub DataGrid1_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles DataGrid1.SelectionChanged

    End Sub
End Class
