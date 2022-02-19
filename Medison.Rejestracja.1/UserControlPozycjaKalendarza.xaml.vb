Public Class UserControlPozycjaKalendarza

    Dim r As TempRejestracja
    Public Event SwipeStartEvent(sender As Object, e As MouseButtonEventArgs)
    Public Event SwipeEndEvent(sender As Object, e As MouseButtonEventArgs)
    Public Event WklejEvent(sender As Object, e As RoutedEventArgs)
    Public Event DodajPacjentaEvent(sender As Object, e As RoutedEventArgs)
    Public Event ZmienPacjentaEvent(sender As Object, e As RoutedEventArgs)
    Public Event PokazEvent(sender As Object, e As RoutedEventArgs)
    Public Event WytnijEvent(sender As Object, e As RoutedEventArgs)
    Public Event ZadzwonioneEvent(sender As Object, e As RoutedEventArgs)
    Public Event UsunPacjentaEvent(sender As Object, e As RoutedEventArgs)
    Public Event UstawInfoEvent(sender As Object, e As RoutedEventArgs)

    Sub New(_r As TempRejestracja)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        r = _r
        Me.DataContext = r
    End Sub

    Private Sub SwipeStart(sender As Object, e As MouseButtonEventArgs)
        RaiseEvent SwipeStartEvent(sender, e)
    End Sub

    Private Sub SwipeEnd(sender As Object, e As MouseButtonEventArgs)
        RaiseEvent SwipeEndEvent(sender, e)
    End Sub

    Private Sub Wklej(sender As Object, e As RoutedEventArgs)
        RaiseEvent WklejEvent(sender, e)

    End Sub

    Private Sub DodajPacjenta(sender As Object, e As RoutedEventArgs)
        RaiseEvent DodajPacjentaEvent(sender, e)
    End Sub

    Private Sub ZmienPacjenta(sender As Object, e As RoutedEventArgs)
        RaiseEvent ZmienPacjentaEvent(sender, e)
    End Sub

    Private Sub Pokaz(sender As Object, e As RoutedEventArgs)
        RaiseEvent PokazEvent(sender, e)
    End Sub

    Private Sub Wytnij(sender As Object, e As RoutedEventArgs)
        RaiseEvent WytnijEvent(sender, e)
    End Sub

    Private Sub Zadzwonione(sender As Object, e As RoutedEventArgs)
        RaiseEvent ZadzwonioneEvent(sender, e)
    End Sub

    Private Sub UsunPacjenta(sender As Object, e As RoutedEventArgs)
        RaiseEvent UsunPacjentaEvent(sender, e)
    End Sub

    Private Sub UstawInfo(sender As Object, e As RoutedEventArgs)
        RaiseEvent UstawInfoEvent(sender, e)
    End Sub
End Class
