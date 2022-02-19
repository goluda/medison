Partial Public Class Zaswiadczenia
    Public ReadOnly Property BackgroundColor As Windows.Media.SolidColorBrush
        Get
            If Me.DataZaswiad.Value.Date = Now.Date Then
                Return New Windows.Media.SolidColorBrush(Windows.Media.Colors.Yellow)
            Else
                Return New Windows.Media.SolidColorBrush(Windows.Media.Colors.White)
            End If
        End Get
    End Property
End Class
