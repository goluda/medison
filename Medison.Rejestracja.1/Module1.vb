Module Module1
    Public kolory As New List(Of Windows.Media.Brush)

    Public komunikacja As New PolaczenieInfo
    
    Public ostatniaDataWizyty As DateTime = Now.Date


    Public Sub utworzKolory()
        kolory.Clear()
        kolory.Add(Windows.Media.Brushes.DarkBlue)
        kolory.Add(Windows.Media.Brushes.DarkRed)
        kolory.Add(Windows.Media.Brushes.DarkGreen)
        kolory.Add(Windows.Media.Brushes.DarkGray)
        kolory.Add(Windows.Media.Brushes.DarkOrange)
        kolory.Add(Windows.Media.Brushes.DarkViolet)
        kolory.Add(Windows.Media.Brushes.DarkGoldenrod)
        kolory.Add(Windows.Media.Brushes.DarkGray)
    End Sub

    Public ulice As New List(Of String)
    Public Sub utworzUlice()
        For Each u In My.Resources.ulice.Split(vbCrLf)
            u = u.Replace(Chr(10), "").Replace(Chr(13), "").Trim
            ulice.Add(u)
        Next
    End Sub

End Module
