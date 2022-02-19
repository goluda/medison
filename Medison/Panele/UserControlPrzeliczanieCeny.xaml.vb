Public Class UserControlPrzeliczanieCeny


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = New przelicznik
    End Sub

    Class przelicznik
        Implements ComponentModel.INotifyPropertyChanged


        Private _brutto As Decimal = 0
        Public Property brutto() As Decimal
            Get
                Return _brutto
            End Get
            Set(ByVal value As Decimal)
                _brutto = value
                notify("brutto")
                notify("netto")
            End Set
        End Property

        Private _stawka As Decimal = 22
        Public Property stawka() As Decimal
            Get
                Return _stawka
            End Get
            Set(ByVal value As Decimal)
                _stawka = value
                notify("stawka")
                notify("netto")
            End Set
        End Property

        Public ReadOnly Property netto As Decimal
            Get
                Return Math.Round(brutto / (1 + stawka / 100), 2)
            End Get
        End Property


        Sub notify(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyName))
        End Sub
        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
End Class
