Class MainWindow 

    Private Sub button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles button1.Click
        Dim l As New Lekarz With {.NazwiskoLekarza = "Jaś Kowalski", .NrGabinetu = "13"}
        l.Godziny.Add(New GodzinaPrzyjec With {.DzienTygodnia = "Poniedzialek", .GodzinaDo = "14:00", .GodzinaOd = "8:00"})
        Dim aa = l.getXml
        Me.textBox1.Text = aa.ToString
        Me.DataContext = l
    End Sub

    Private Sub button2_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles button2.Click
        Dim xx As XElement
        xx = xx.Parse(Me.textBox1.Text)
        Dim l As Lekarz = Me.DataContext
        l.readFromXml(xx)
    End Sub
End Class
