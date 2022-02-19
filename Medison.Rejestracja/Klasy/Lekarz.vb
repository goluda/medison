Public Class Lekarz
    Implements System.ComponentModel.INotifyPropertyChanged

    Private _NazwiskoLekarza As String
    Public Property NazwiskoLekarza() As String
        Get
            Return _NazwiskoLekarza
        End Get
        Set(ByVal value As String)
            _NazwiskoLekarza = value
            notify("NazwiskoLekarza")
        End Set
    End Property
    Private _nrGabinetu As String
    Public Property NrGabinetu() As String
        Get
            Return _nrGabinetu
        End Get
        Set(ByVal value As String)
            _nrGabinetu = value
            notify("NrGabinetu")
        End Set
    End Property

    Private _godziny As New List(Of GodzinaPrzyjec)
    Public Property Godziny() As List(Of GodzinaPrzyjec)
        Get
            Return _godziny
        End Get
        Set(ByVal value As List(Of GodzinaPrzyjec))
            _godziny = value
            notify("Godziny")
        End Set
    End Property


    Public Function getXml() As XElement
        Dim xx As New XElement("Lekarz")
        serializator.serializeObjectToXelement(xx, Me)
        For Each l In Me.Godziny
            xx.Add(l.getXml)
        Next
        Return xx
    End Function

    Public Sub readFromXml(x As XElement)
        serializator.DeserializeObjectToXelement(x, Me)
        For Each xx In x.Descendants("Godziny")
            Dim g As New GodzinaPrzyjec
            g.readFromXml(xx)
        Next
    End Sub


    Public Sub notify(propertyName As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
