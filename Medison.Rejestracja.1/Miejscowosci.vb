Public Class Miejscowosci
    Property kod As String
    Property miejscowosc As String
    Public ReadOnly Property widok As String
        Get
            Return miejscowosc + " [" + kod + "]"
        End Get
    End Property
    Public ReadOnly Property id As String
        Get
            Return miejscowosc + "|" + kod
        End Get
    End Property
End Class
