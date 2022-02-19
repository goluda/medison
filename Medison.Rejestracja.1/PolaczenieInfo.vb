Public Class PolaczenieInfo
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Sub notify(propertyName As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    End Sub
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public typDanych As typKomunikacji = typKomunikacji.Zdalne

    Public Enum typKomunikacji
        Lokalne
        Zdalne
    End Enum

    Public ReadOnly Property OpisTypuPolaczenia As String
        Get
            If typDanych = typKomunikacji.Lokalne Then
                Return "Lokalna baza na laptopie"
            Else
                Return "Baza na serwerze"
            End If
        End Get
    End Property

    Public Property typPolaczenia As typKomunikacji
        Get
            Return typDanych
        End Get
        Set(value As typKomunikacji)
            typDanych = value
            notify("typPolaczenia")
            notify("OpisTypuPolaczenia")
        End Set
    End Property

    Public Sub zmienTypPolaczenia()
        If Me.typPolaczenia = typKomunikacji.Lokalne Then
            Me.typPolaczenia = typKomunikacji.Zdalne
        Else
            Me.typPolaczenia = typKomunikacji.Lokalne
        End If
    End Sub
End Class
