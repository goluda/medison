Partial Public Class TypyDokumentow
    Public ReadOnly Property opis As String
        Get
            Return Me.NrTypuDokumentu.ToString + " " + Me.NazwaDokumentu
        End Get
    End Property
End Class
