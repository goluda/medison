Partial Public Class Recepty
    Implements System.ComponentModel.INotifyPropertyChanged


    Public ReadOnly Property pokazPrzyciskWydruku As Windows.Visibility
        Get
            If If(Me.indywidualna IsNot Nothing, Me.indywidualna, False) = True Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Get
    End Property

    Public ReadOnly Property doWydruku As String
        Get
            Try
                Return _
                    If(Me.Dawka IsNot Nothing, Me.Dawka + " ", "") + _
                    If(Me.Postac IsNot Nothing, Me.Postac + " ", "") + _
                    If(Me.Ilosc IsNot Nothing, Me.Ilosc + " ", "")

            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public Sub odswierzProperty()
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("pokazPrzyciskWydruku"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("indywidualna"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Dawka"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Postac"))
        RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("Dawkowanie"))
    End Sub

    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
