Public Class DaneUzytkowe
    Implements ComponentModel.INotifyPropertyChanged


    Private _wybranyLekarz As DaneLekarza
    Public Property WybranyLekarz() As DaneLekarza
        Get
            Return _wybranyLekarz
        End Get
        Set(ByVal value As DaneLekarza)
            _wybranyLekarz = value
            Changed("WybranyLekarz")
            RaiseEvent ZmianaLekarza(_wybranyLekarz)
            Module1.setInfo("Zmieniono lekarza na: " + WybranyLekarz.ToString)
        End Set
    End Property


    Private _data As DateTime = Now.Date
    Public Property data() As DateTime
        Get
            Return _data
        End Get
        Set(ByVal value As DateTime)
            _data = value
            Changed("data")
            RaiseEvent ZmienaDaty(_data)
            Module1.setInfo("Zmieniono datę na: " + data.ToShortDateString)
        End Set
    End Property

    Public Event ZmienaDaty(ByVal nowaData As Date)
    Public Event ZmianaLekarza(ByVal nowyLekarz As DaneLekarza)

    Sub Changed(ByVal propetyname As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propetyname))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
