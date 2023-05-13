Class MainWindow 

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim db As New medisonEntities1

        For Each s As DataSetMed.SzablonyRow In New DataSetMedTableAdapters.SzablonyTableAdapter().GetData
            Dim s1 As New Szablony
            Try
                s1.Kolejnosc = s.Kolejnosc
            Catch : End Try : Try
                s1.NrGabinetu = s.NrGabinetu
            Catch : End Try : Try
                s1.NrGrSzablon = s.NrGrSzablon
            Catch : End Try : Try
                s1.NrRodzSzab = s.NrRodzSzab
            Catch : End Try : Try
                s1.NrSzablonu = s.NrSzablonu
            Catch : End Try : Try
                s1.OpisSz = s.OpisSz
            Catch : End Try : Try
                s1.Skrot = s.Skrot
            Catch : End Try
            Try
                db.Szablonies.AddObject(s1)
                db.SaveChanges()
            Catch ex As Exception

            End Try
        Next
        db.SaveChanges()
        MsgBox("gotowe")
    End Sub
End Class
