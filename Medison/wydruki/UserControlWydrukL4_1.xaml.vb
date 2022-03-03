Public Class UserControlWydrukL4_1
    Public Property wizyta As Wizyty
        Get
            Return Me.DataContext
        End Get
        Set(ByVal value As Wizyty)
            DataContext = value
        End Set
    End Property


    Private Sub UserControl_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
