Public Class WindowSkierowanieNaZabiegi
    Dim p As Pacjenci
    Dim s As Skierowania

    Sub New(_p As Pacjenci, _s As Skierowania)
        InitializeComponent()
        p = _p
        s = _s
    End Sub

    Private Sub WindowSkierowanieDoPracowniRezonansuMagnetycznego_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        s.xml = ObslugaSkierowania.ZapiszSkierowanie(Me.FlowDocument1, "Na zabiegi").ToString
        'MsgBox(s.xml)
    End Sub

    Private Sub WyborRozpoznania(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.F4 Then
            Dim w As New WindowSkierowanieWyborRozpoznania
            w.ShowDialog()
            If w.wybrane = True Then
                sender.text = w.rozpoznanie
            End If
            w = Nothing
        End If
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.LabelData.Content = Now.Date.ToShortDateString
        'Me.LabelDataUrodzenia.Content = p.DataUrodzenia.Value.ToShortDateString
        Me.LabelAdres.Content = p.PelnyAdresPacjenta
        Me.LabelNazwiskoiImie.Content = p.NazwiskoImie
        'Me.LabelWiek.Content = p.wiek
        Me.LabelPESEL.Content = p.Pesel
        Me.LabelDataUrodzenia.Content = p.DataUrodzenia.Value.ToShortDateString
        Me.LabelPlec.Content = p.Plec

        'Dim p1 As Paragraph = Me.test
        'Dim a = 1
        'myRun.Text = Now.ToString
        If s.xml Is Nothing Then
            s.xml = ""
            s.DoKogo = "Na zabiegi"
        Else
            ObslugaSkierowania.WczytajSkierownanie(Me.FlowDocument1, s.xml)
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim prnDlg As New PrintDialog
        'Me.FlowDocument1.Name = "Skierowanie do poradni rezonansu magnetycznego"
        Me.FlowDocument1.ColumnWidth = prnDlg.PrintableAreaWidth
        Me.FlowDocument1.PageWidth = prnDlg.PrintableAreaWidth
        Dim pag As IDocumentPaginatorSource = Me.FlowDocument1
        prnDlg.PrintDocument(pag.DocumentPaginator, "Skierowanie do poradni rezonansu magnetycznego")

    End Sub

    Private Sub Przekresl(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim l As Label = sender
            If l.Foreground.Equals(Windows.Media.Brushes.Black) Then
                l.Foreground = Windows.Media.Brushes.LightGray
            Else
                l.Foreground = Windows.Media.Brushes.Black
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
