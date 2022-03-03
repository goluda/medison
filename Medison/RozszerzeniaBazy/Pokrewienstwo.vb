Partial Public Class Pokrewienstwo
    Public ReadOnly Property opis As String
        Get
            Return Me.KodPokrew.ToString + " " + Me.OpisPokrewienstwa
        End Get
    End Property
End Class
