Imports System.Data.Objects.DataClasses

Partial Public Class Towary
    Implements System.ComponentModel.INotifyPropertyChanged

    Public ReadOnly Property CenaSprzedazy As Decimal
        Get
            Try
                Return Me.CenaNettoSugerowana * (1 + Me.StawkaVAT)
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property



    Private Sub Towary_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged1
        Select Case e.PropertyName
            Case "CenaNettoSugerowana"
                ' Me.ReportPropertyChanged("CenaSprzedazy")
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("CenaSprzedazy"))

            Case "StawkaVAT"
                'Me.ReportPropertyChanged("CenaSprzedazy")
                RaiseEvent PropertyChanged1(Me, New System.ComponentModel.PropertyChangedEventArgs("CenaSprzedazy"))

        End Select
    End Sub

    Public ReadOnly Property opis As String
        Get
            Return Me.Nazwa + " [" + String.Format("{0:c}", Me.CenaSprzedazy) + "]"
        End Get
    End Property

    Public Event PropertyChanged1(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
