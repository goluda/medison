Public Class UserControlRozstrzelenie

    Public WriteOnly Property text As String
        Set(ByVal value As String)
            Me.StackPanelControl.Children.Clear()
            Dim t1 As String = If(value IsNot Nothing, value, " ")
            For Each a In t1.ToCharArray
                Dim b As New TextBlock
                b.Text = a
                b.Padding = New Thickness(0, 0, 10, 0)
                Me.StackPanelControl.Children.Add(b)
            Next
        End Set
    End Property
    Public Shared ReadOnly StateProperty As DependencyProperty = DependencyProperty.Register("text", GetType(String), GetType(UserControlRozstrzelenie), New PropertyMetadata(False))

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class
