Public Class WindowSzczegolyPacjentaKrotkie

    Dim miejsc As List(Of Miejscowosci)
    Dim pac As Pacjenci

    Sub New(ByVal p As Pacjenci)
        InitializeComponent()

        Dim db As New medisonEntities

        miejsc = (From a In db.Pacjenci Where a.Kod IsNot Nothing And a.Miejscowosc IsNot Nothing And a.Miejscowosc <> "" Select a.Kod, a.Miejscowosc Distinct Order By Miejscowosc Select New Miejscowosci With {.kod = Kod, .miejscowosc = Miejscowosc}).ToList
        Me.ComboBoxMiejscowosci.ItemsSource = miejsc

        Me.ComboBoxPlec.ItemsSource = New String() {"n", "K", "M", "x"}
        Me.DataContext = p
        pac = p
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.TextBoxAdres.ItemsSource = Module1.ulice
        Me.TextBox1.Focus()
        If pac.Imie Is Nothing Then
            Me.TextBox2.Focus()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox1.LostFocus
        Try
            Dim test = Me.TextBox1.Text.Split("-")
            Dim test1 As String = ""
            For Each a In test
                If a <> "" Then
                    test1 += a.Substring(0, 1).ToUpper + a.Substring(1).ToLower + "-"
                End If
            Next
            Me.TextBox1.Text = test1.Substring(0, test1.Length - 1)
        Catch
        End Try
    End Sub

    Private Sub TextBox2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox2.LostFocus
        Try
            Me.TextBox2.Text = Me.TextBox2.Text.Substring(0, 1).ToUpper + Me.TextBox2.Text.Substring(1).ToLower
        Catch
        End Try
    End Sub

   

    Private Sub TextBoxAdres_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBoxAdres.LostFocus
        Try
            If Me.TextBoxAdres.Text.Contains("ul. ") Then
                Dim a = Me.TextBoxAdres.Text.IndexOf("ul. ") + 4
                Me.TextBoxAdres.Text = Me.TextBoxAdres.Text.Substring(0, a) + Me.TextBoxAdres.Text.Substring(a, 1).ToUpper + Me.TextBoxAdres.Text.Substring(a + 1)
            End If
        Catch ex As Exception

        End Try
    End Sub

  
    Private Sub TextBox3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox3.LostFocus
        Try
            Dim kk As String = sender.text

            If kk = "66-400" Then
                pac.Kod = "66-400"
                pac.Miejscowosc = "Gorzów Wlkp."
                pac.odswierzMiejscowosci()
                Exit Sub
            End If

            Dim temp = From a In miejsc Where a.kod = kk

            If temp.Count > 0 Then
                pac.Miejscowosc = temp.First.miejscowosc
                pac.Kod = temp.First.kod
                pac.odswierzMiejscowosci()
            End If
        Catch
        End Try
    End Sub

    Private Sub TextBox4_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox4.LostFocus
        Try
            Dim kk As String = sender.text
            Dim temp = From a In miejsc Where a.miejscowosc = kk

            If temp.Count > 0 Then
                pac.Miejscowosc = temp.First.miejscowosc
                pac.Kod = temp.First.kod
                pac.odswierzMiejscowosci()
            End If
        Catch
        End Try
    End Sub

    Private Sub ComboBoxMiejscowosci_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ComboBoxMiejscowosci.SelectionChanged
        Try
            Dim m As Miejscowosci = ComboBoxMiejscowosci.SelectedItem
            pac.Miejscowosc = m.miejscowosc
            pac.Kod = m.kod
            pac.odswierzMiejscowosci()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox5_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TextBox5.LostFocus
        If Me.TextBox5.Text <> "" Then
            If nippesel.sprawdzPesel(Me.TextBox5.Text) = True Then
                pac.Pesel = Me.TextBox5.Text
                pac.DataUrodzenia = nippesel.dataZPesel(pac.Pesel)
                pac.odswierzMiejscowosci()
            Else
                MsgBox("Uwaga!!! Błędny PESEL")
            End If
        End If
    End Sub

    Private Sub szukajMiejsowosci(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)

        Me.ComboBoxMiejscowosci.ItemsSource = miejsc.Where(Function(o) o.miejscowosc.ToLower.StartsWith(Me.TextBoxSzukajMiejscowosci.Text.ToLower))
        Try
            If Me.TextBoxSzukajMiejscowosci.Text <> "" Then Me.ComboBoxMiejscowosci.SelectedIndex = 0
        Catch
        End Try
    End Sub
End Class
