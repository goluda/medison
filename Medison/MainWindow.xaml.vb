Class MainWindow 

    Dim db As New medisonEntities

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Module1.mainForm = Me
        Module1.utworzUlice()

        Try


            Me.GroupBox1.DataContext = Module1.dane
            Dim lek = (From a In db.DaneLekarza Where a.NrGabinetu <> 1 And a.NrGabinetu <> 11 And a.ZakonczonoDane Is Nothing).ToList
            Me.ComboBoxLekarze.ItemsSource = lek
            Module1.dane.WybranyLekarz = lek.First

            loadModule(New UserControlRejestracja)
        Catch ex As Exception
            Dim stophere = 1
        End Try
    End Sub

    Public Sub loadModule(ByVal c As MyUserControl)
        Try
            Dim c1 As MyUserControl = Me.MainPanel.Child

            c1.Dispose()
            c1 = Nothing


        Catch ex As Exception

        End Try
        Try
            c.createHandlers()

            Me.MainPanel.Child = c
        Catch
        End Try
    End Sub

    Private Sub ListaPacjentow(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlListaPacjentow)
    End Sub

    Private Sub Rejestracja(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlRejestracja)
    End Sub

    Private Sub ListaRachunkow(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlListaRachunkow)
    End Sub

    Private Sub Lekarze(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlLekarze(db))
    End Sub

    Private Sub Leki(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlLeki)
    End Sub

    Private Sub Towary(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlMagazyn)
    End Sub

    Private Sub Firmy(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        loadModule(New UserControlFirmy)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        loadModule(New UserControlSzablony)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        loadModule(New UserControlWizyty)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        loadModule(New UserControlUSGZaMiesiac)
    End Sub

    Private Sub PoprawianieMiejscowosci(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim w As New WindowPoprawianieMiejscowosci
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub CenyUSG(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim w As New WindowCenyUSG
        w.ShowDialog()
        w = Nothing
    End Sub

    Private Sub Window_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If MsgBox("Czy na pewno chcesz zamknąć program?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub BankRaport(sender As Object, e As RoutedEventArgs)
        Dim w As New WindowBank
        w.ShowDialog()
        w = Nothing
    End Sub
End Class
