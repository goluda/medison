Public Class UserControlWydrukL4


    Public Property wizyta As Wizyty
        Get
            Return Me.DataContext
        End Get
        Set(ByVal value As Wizyty)
            DataContext = value
        End Set
    End Property

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
