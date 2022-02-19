Public Class UserControlKalkulator182

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = New przelicznik
    End Sub

    Class przelicznik
        Implements ComponentModel.INotifyPropertyChanged



        Private _dataStart As DateTime = Now.Date
        Public Property dataStart() As DateTime
            Get
                Return _dataStart
            End Get
            Set(ByVal value As DateTime)
                _dataStart = value
                notify("dataStart")
                notify("dataKoniec")
            End Set
        End Property

        Public ReadOnly Property dataKoniec As Nullable(Of DateTime)
            Get
                Try
                    Return dataStart.AddDays(181)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property


        Sub notify(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyName))
        End Sub
        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
End Class
