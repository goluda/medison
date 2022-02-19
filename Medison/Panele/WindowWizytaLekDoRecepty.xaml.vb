Public Class WindowWizytaLekDoRecepty

    Public gotowa As Boolean = False

    Dim db As medisonEntities
    Dim r As Recepty

    Sub New(_db As medisonEntities, _r As Recepty)
        InitializeComponent()
        db = _db
        r = _r
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.ComboBoxChorobyPrzewlekle.ItemsSource = New String() {"", "X", "P"}

        Me.ComboBoxLek.ItemsSource = db.Leki.OrderBy(Function(o) o.NazwaLeku).ToList
        Me.ComboBoxPostac.ItemsSource = New String() {"tab.", "amp.", "czop.", "maść", "żel", "krople", "syrop", "sasz.", "zioła"}
        Me.DataContext = r
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If Me.TextBox1.Text = "" Then
            MsgBox("Podaj ilość")
            Exit Sub
        End If
        If Me.TextBox2.Text = "" Then
            MsgBox("Podaj dawkowanie")
            Exit Sub
        End If
        If Me.ComboBoxLek.SelectedIndex = -1 Then
            Dim nazwaLeku As String = Me.ComboBoxLek.Text
            If MsgBox("Nie ma leku " + nazwaLeku + " czy chcesz go dodać do bazy danych?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
                Exit Sub
            End If

            r.Leki = New Leki With {.NazwaLeku = nazwaLeku}

        End If
        gotowa = True
        Me.Close()
    End Sub

    Private Sub ComboBoxLek_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles ComboBoxLek.SelectionChanged
        Try
            Dim l As Leki = Me.ComboBoxLek.SelectedItem
            r.Postac = l.PostacLeku
            r.Dawkowanie = l.Dawkowanie
            r.odswierzProperty()
        Catch ex As Exception

        End Try
    End Sub
End Class
