Partial Public Class Leki
    Public ReadOnly Property NazwaLekuZRefundacja As String
        Get
            Return Me.NazwaLeku + " " + If(Me.refundacja IsNot Nothing, "ref. " + Me.refundacja, "")
        End Get
    End Property

    Public ReadOnly Property refundacja1 As String
        Get
            Return If(Me.refundacja IsNot Nothing, "ref. " + Me.refundacja, "")
        End Get
    End Property
End Class
