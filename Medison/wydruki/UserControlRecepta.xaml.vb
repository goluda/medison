Public Class UserControlRecepta



    Sub New(w As Wizyty, opusc As Integer, nic As Object)
        InitializeComponent()
        Me.ImageBackground.Visibility = Windows.Visibility.Collapsed

        Me.DataContext = w
        Dim leki = w.Recepty.Where(Function(o) If(o.indywidualna IsNot Nothing, o.indywidualna, False) = False).Skip(opusc).ToList
        Dim przewl As String = "X"
        For Each l In leki
            If l.ReceptaChorobyPrzewlekle <> "X" Then
                przewl = l.ReceptaChorobyPrzewlekle
            End If
        Next
        Me.LabelChorobyPrzewlekle.Content = przewl
        If leki.Count <= 5 Then
            Me.ListView1.ItemsSource = leki
        Else
            Me.ListView1.ItemsSource = leki.Take(5)
        End If
    End Sub

    Sub New(w As Wizyty, idRecepty As Integer)
        InitializeComponent()
        Me.ImageBackground.Visibility = Windows.Visibility.Collapsed

        Me.DataContext = w
        Dim l As New List(Of Recepty)
        Dim r = w.Recepty.Where(Function(o) o.NrRecepty = idRecepty).ToList
        Me.LabelChorobyPrzewlekle.Content = r.First.ReceptaChorobyPrzewlekle
        l.AddRange(r)
        Me.ListView1.ItemsSource = l
    End Sub




    Private Sub UserControl_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub


End Class
