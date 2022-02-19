Public Class WindowWybierzMagazyn
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.ListBoxTowary.ItemsSource = db.Towary.OrderBy(Function(O) O.Nazwa).ToList

    End Sub

    Public t As Towary = Nothing


    Dim db As medisonEntities
    Sub New(ByVal _db As medisonEntities)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.db = _db


    End Sub

    Private Sub NajechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 1
    End Sub

    Private Sub OdjechaneMyszka(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        sender.opacity = 0.5
    End Sub
    Private Sub DodajDoFaktury(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Me.t = sender.tag
            Me.Close()

        Catch
        End Try


    End Sub
End Class
