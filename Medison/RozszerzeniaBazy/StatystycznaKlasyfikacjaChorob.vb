Partial Public Class StatystycznaKlasyfikacjaChorob

    Public ReadOnly Property opis As String
        Get
            Return Me.NrStatystyczny + " " + Me.Rozpoznanie
        End Get
    End Property

End Class
