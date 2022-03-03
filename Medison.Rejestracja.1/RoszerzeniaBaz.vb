Public Class RoszerzeniaBaz

End Class


Partial Public Class Pacjenci
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public Property DataUrodzeniaToString As String
        Get
            If DataUrodzenia IsNot Nothing Then
                If DataUrodzenia.Value.Year > 1900 Then
                    Return String.Format("{0:00}-{1:00}-{2:00}", Me.DataUrodzenia.Value.Day, Me.DataUrodzenia.Value.Month, Me.DataUrodzenia.Value.Year)
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Try
                value = value.Replace("-", "").Replace(".", "")
                Dim rok As Integer = Convert.ToInt32(value.Substring(4, 4))
                Dim miesiac As Integer = Convert.ToInt32(value.Substring(2, 2))
                Dim dzien As Integer = Convert.ToInt32(value.Substring(0, 2))
                Me.DataUrodzenia = New DateTime(rok, miesiac, dzien)
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzenia"))
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("DataUrodzeniaToString"))
            Catch
            End Try
        End Set
    End Property
End Class





