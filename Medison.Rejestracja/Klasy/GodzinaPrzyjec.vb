Public Class GodzinaPrzyjec
    Implements System.ComponentModel.INotifyPropertyChanged

    Private _dzienTygodnia As String
    Public Property DzienTygodnia() As String
        Get
            Return _dzienTygodnia
        End Get
        Set(ByVal value As String)
            _dzienTygodnia = value
            notify("DzienTygodnia")
        End Set
    End Property
    Private _godzinaOd As String
    Public Property GodzinaOd() As String
        Get
            Return _godzinaOd
        End Get
        Set(ByVal value As String)
            _godzinaOd = value
            notify("GodzinaOd")
        End Set
    End Property
    Private _godzinaDo As String
    Public Property GodzinaDo() As String
        Get
            Return _godzinaDo
        End Get
        Set(ByVal value As String)
            _godzinaDo = value
            notify("GodzinaDo")
        End Set
    End Property


    Public Function getXml() As XElement
        Dim xx As New XElement("Godziny")
        serializator.serializeObjectToXelement(xx, Me)
        Return xx
    End Function

    Public Sub readFromXml(x As XElement)
        serializator.DeserializeObjectToXelement(x, Me)
    End Sub


    Public Sub notify(propertyName As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
