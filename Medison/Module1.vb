Module Module1
    Public dane As New DaneUzytkowe

    Public printDial As New PrintDialog

    Public mainForm As MainWindow

    Sub setInfo(ByVal t As String)
        mainForm.LabelInfo.Content = t
        'System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub loadModule(ByVal c As MyUserControl)
        mainForm.loadModule(c)
    End Sub

    Public Sub zablokujPrzyciski()
        mainForm.panelPrzyciski.IsEnabled = False
    End Sub
    Public Sub odblokujPrzyciski()
        mainForm.panelPrzyciski.IsEnabled = True
    End Sub

    Public Sub drukujVisual(ByVal control As UserControl, ByVal nazwa As String, ByVal szerokosc As Integer, ByVal wysokosc As Integer)
        control.Measure(New Size(szerokosc, wysokosc))
        control.Arrange(New Rect(New Windows.Size(szerokosc, wysokosc)))
        control.UpdateLayout()

        printDial.PrintVisual(control, nazwa)

    End Sub

    Public wybranaWizyta As Nullable(Of Long)

    Public Sub podgladVisual()

    End Sub

    Public isReadonly As Boolean = True

    Public ulice As New List(Of String)
    Public Sub utworzUlice()
        For Each u In My.Resources.ulice.Split(vbCrLf)
            u = u.Replace(Chr(10), "").Replace(Chr(13), "").Trim
            ulice.Add(u)
        Next
    End Sub
End Module
