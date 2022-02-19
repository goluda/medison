Public Class UserControlRachunekSzczegoly
    Implements MyUserControl

    Dim db As medisonEntities
    Dim WithEvents p As Rachunki
    Dim lp As UserControlListaRachunkow
    Dim nowy As Boolean

    Sub New(ByVal _db As medisonEntities, ByVal _p As Rachunki, ByVal _lp As UserControlListaRachunkow, ByVal _nowy As Boolean)
        InitializeComponent()


        db = _db
        p = _p
        lp = _lp
        Me.DataContext = p


    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub

    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza

    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose

        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza

    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)

    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ButtonWroc.Click

        If nowy = True Then
            db.Rachunki.Add(p)
        End If
        Try
            db.SaveChanges()
            loadModule(lp)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Class
