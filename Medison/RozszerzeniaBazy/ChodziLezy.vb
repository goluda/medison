Partial Public Class ChodziLezy

    Public ReadOnly Property opis As String
        Get
            Return Me.NrChodziLezy.ToString + " " + Me.OpisChodziLezy
        End Get
    End Property

End Class
