Class MainWindow

    Sub log(t As String)
        Me.LabelInfo.Content = t
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        log("")
        Me.DataContext = Module1.komunikacja
        Module1.utworzKolory()
        Module1.utworzUlice()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Zdalne Then
            MsgBox("Ta funkcja działa tylko, gdy pracujesz z lokalną bazą danych")
            Exit Sub
        End If

        Try
            If MsgBox("Czy napewno chcesz pobrać listę lekarzy? Aktualna lista zostanie skasowana", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
                Exit Sub
            End If

            log("Pobieram dane pacjentów")
            Dim db As New medisonEntities
            Dim lista = db.Pacjenci.ToList()
            log("Znaleziono " + lista.Count.ToString + " pacjenótw")

            log("Usuwanie starych danych")
            Dim ta As New DataSet1TableAdapters.PacjenciTableAdapter
            ta.DeleteQuery()

            Dim i As Integer = 0
            Dim i1 As Integer = 0
            For Each o In lista
                i += 1
                i1 += 1
                If i1 = 100 Then
                    log(i.ToString + "/" + lista.Count.ToString + " " + o.Nazwisko + " " + o.Imie)
                    i1 = 0
                End If

                ta.Insert(o.Nazwisko, o.Nazwisko2, _
                          o.Imie, o.Imie2, o.Plec, o.DataUrodzenia, _
                          o.Kod, o.Miejscowosc, o.Adres, o.NrDomu, o.NrLokalu, _
                          o.Telefon, o.Pesel)


            Next
            log("")
            MsgBox("Dane pacjentów zaimportowane" + vbCr + vbCr + "Pamiętaj aby od czasu do czasu zrobić kompaktowanie bazy")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            If MsgBox("Czy napewno chcesz pobrać listę pacjętów? Aktualna lista zostanie skasowana", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
                Exit Sub
            End If

            log("Pobieram dane lekarzy")
            Dim db As New medisonEntities
            Dim lista = db.DaneLekarza.ToList()
            log("Znaleziono " + lista.Count.ToString + " lekarzy")

            log("Usuwanie starych danych")
            Dim ta As New DataSet1TableAdapters.DaneLekarzaTableAdapter
            ta.DeleteQuery()

            Dim i As Integer = 0
            Dim i1 As Integer = 0
            For Each o In lista
                i += 1
                i1 += 1

                log(i.ToString + "/" + lista.Count.ToString + " " + o.Nazwisko + " " + o.Imie)



                ta.Insert(o.NrGabinetu, o.Imie, o.Nazwisko, True)


            Next
            log("")
            MsgBox("Dane lekarzy zaimportowane" + vbCr + vbCr + "Pamiętaj aby od czasu do czasu zrobić kompaktowanie bazy")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Dim w As New WindowGodzinyPrzyjec
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim w As New WindowRejestracja
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button5.Click
        If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Zdalne Then
            MsgBox("Ta funkcja działa tylko, gdy pracujesz z lokalną bazą danych")
            Exit Sub
        End If
        Dim w As New WindowSynchronizacja
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button6.Click
        Module1.komunikacja.zmienTypPolaczenia()
    End Sub
End Class
