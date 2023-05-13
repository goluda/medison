Partial Public Class UbezpieczonyW
    Public Overrides Function ToString() As String
        Return Me.NrUbezpieczonyW.ToString + " " + Me.OpisUbezpieczonyW
    End Function

    Public ReadOnly Property opis As String
        Get
            Return Me.ToString
        End Get
    End Property

End Class
