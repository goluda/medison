Public Class UserControlWydrukHistoria

    Sub New(ByVal p As Pacjenci)
        InitializeComponent()
        pacjent = p
    End Sub



    Private _pacjent As Pacjenci
    Public Property pacjent() As Pacjenci
        Get
            Return _pacjent
        End Get
        Set(ByVal value As Pacjenci)
            _pacjent = value
        End Set
    End Property

End Class
