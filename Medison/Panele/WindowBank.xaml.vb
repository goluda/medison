Public Class WindowBank
    Dim db As medisonEntities
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        db = New medisonEntities



        Dim dtOd1 = dpOd.SelectedDate
        Dim dtDo1 = dpDo.SelectedDate

        poprawBank(dtOd1, dtDo1)

        Dim temp = db.Wizyty.Where(Function(o) o.DataWizyty >= dtOd1 _
                                       And o.DataWizyty <= dtDo1 _
                                       And o.karta = True) _
                                       .GroupBy(Function(o) o.ImieLekarza + " " + o.NazwiskoLekarza) _
                                       .Select(Function(o) New With {.Lekarze = o.Key, .ilosc = o.Count(), .suma = o.Sum(Function(o2) o2.WplataKarta)}) _
                                       .ToList
        Me.dataGrid1.ItemsSource = temp


    End Sub

    Private Sub poprawBank(dtOd1 As Date?, dtDo1 As Date?)
        Dim temp = db.Wizyty.Where(Function(o) o.DataWizyty >= dtOd1 _
                                       And o.DataWizyty <= dtDo1 _
                                       And o.karta Is Nothing).ToList
        For Each t In temp
            t.karta = False
        Next
        db.SaveChanges()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.dpDo.SelectedDate = Today
        Me.dpOd.SelectedDate = Today

    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        db = New medisonEntities

        Dim dtOd1 = dpOd.SelectedDate
        Dim dtDo1 = dpDo.SelectedDate

        poprawBank(dtOd1, dtDo1)

        Dim temp = db.Wizyty.Where(Function(o) o.DataWizyty >= dtOd1 _
                                       And o.DataWizyty <= dtDo1 _
                                       And o.karta <> True) _
                                       .GroupBy(Function(o) o.ImieLekarza + " " + o.NazwiskoLekarza) _
                                       .Select(Function(o) New With {.Lekarze = o.Key, .ilosc = o.Count(), .suma = o.Sum(Function(o2) o2.Wplata)}) _
                                       .ToList
        Me.dataGrid1.ItemsSource = temp
    End Sub
End Class
