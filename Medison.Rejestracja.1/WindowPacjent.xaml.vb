Public Class WindowPacjent
    Dim ds As New DataSet1
    Public wybrany As Boolean = False
    Public p As DataSet1.PacjenciRow
    Public pm As Pacjenci

    Public db As medisonEntities

    Public imie As String = ""
    Public nazwisko As String = ""
    Public pesel As String = ""
    Public wyszukaj As Boolean = False

    Public Sub TextBox1_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles TextBox1.KeyDown
        Try
            If If(e Is Nothing, True, e.Key = Key.Enter) = True Then
                If Me.TextBox1.Text.Length > 0 Then
                    Dim nn As Boolean = False

                    If Me.TextBox1.Text.ToLower.EndsWith("x") Then
                        nn = True
                        Me.TextBox1.Text = Me.TextBox1.Text.Substring(0, Me.TextBox1.Text.Length - 1)
                    End If

                    If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                        ds = New DataSet1
                        TableAdaptery.Pacjeci.FillBySzukaj(ds.Pacjenci, Me.TextBox1.Text, Me.TextBox1.Text)
                        For Each r As DataSet1.PacjenciRow In Me.ds.Pacjenci.Rows
                            If r.IsdataUrodzeniaNull = False Then r.aDataUrodzenia = r.dataUrodzenia
                        Next
                        Me.DataContext = ds.Pacjenci


                        If ds.Pacjenci.Rows.Count = 0 Or nn = True Then
                            If MsgBox("Czy dodać nowego pacjenta?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                Dim p1 = ds.Pacjenci.NewRow
                                p1("Nazwisko") = Me.TextBox1.Text.ToUpper.Substring(0, 1) + Me.TextBox1.Text.ToLower.Substring(1)
                                p1("Imie") = ""
                                p1("Pesel") = ""
                                p1("Plec") = ""
                                p1("Nazwisko2") = ""
                                p1("Imie2") = ""
                                p1("dataUrodzenia") = New Date(1900, 1, 1)
                                p1("kod") = "66-400"
                                p1("Miejscowosc") = "Gorzów Wlkp."
                                p1("Adres") = ""
                                p1("NrDomu") = ""
                                p1("NrLokalu") = ""
                                p1("Telefon") = ""
                                p1("Pesel") = ""
                                p1("kod1") = "66-400"
                                Dim w As New WindowSzczegolyPacjentaKrotkie(p1)
                                w.ShowDialog()
                                If w.zapisz = True Then
                                    ds.Pacjenci.AddPacjenciRow(p1)
                                    TableAdaptery.Pacjeci.Update(ds.Pacjenci)
                                    Me.DataContext = ds.Pacjenci.OrderBy(Function(o) o.Nazwisko + " " + o.Imie)
                                    ' ''For Each o In Me.DataGrid1.Items
                                    ' ''    If o("Imie") = p1("Imie") And o("Nazwisko") = p1("Nazwisko") Then
                                    ' ''        Me.DataGrid1.ItemsSource = o
                                    ' ''    End If
                                    ' ''Next
                                    Try
                                        p1("aDataUrodzenia") = p1("dataUrodzenia")
                                    Catch ex As Exception

                                    End Try
                                    Me.DataGrid1.SelectedItem = p1

                                End If
                            End If
                        End If

                    Else
                        Dim p2 As Pacjenci
                        Dim temp = (From a In db.Pacjenci Where a.Nazwisko.Contains(Me.TextBox1.Text) Or a.Pesel.Contains(Me.TextBox1.Text) Order By a.Nazwisko, a.Imie).ToList
                        If temp.Count = 0 Or nn = True Then
                            If MsgBox("Czy dodać nowego pacjenta?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                Dim p1 As New Pacjenci
                                p1.Nazwisko = Me.TextBox1.Text.ToUpper.Substring(0, 1) + Me.TextBox1.Text.ToLower.Substring(1)
                                p1.Imie = ""
                                p1.Pesel = ""
                                p1.Plec = ""
                                p1.Nazwisko2 = ""
                                p1.Imie2 = ""
                                p1.DataUrodzenia = New Date(1900, 1, 1)
                                p1.Kod = "66-400"
                                p1.Miejscowosc = "Gorzów Wlkp."
                                p1.Adres = ""
                                p1.NrDomu = ""
                                p1.NrLokalu = ""
                                p1.Telefon = ""
                                p1.Pesel = ""
                                Dim w As New WindowSzczegolyPacjentaKrotkie(p1)
                                w.ShowDialog()
                                If w.zapisz = True Then
                                    temp.Add(p1)
                                    db.Pacjenci.AddObject(p1)
                                    db.SaveChanges()
                                End If
                                p2 = p1

                            End If
                        End If
                        Me.DataContext = (From o In temp Order By o.Nazwisko, o.Imie)
                        If p2 IsNot Nothing Then Me.DataGrid1.SelectedItem = p2
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCr + ex.StackTrace)
        End Try

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.DataContext = ds.Pacjenci
        Me.TextBox1.Focus()
        If Me.TextBox1.Text <> "" Then
            Me.TextBox1_KeyDown(Nothing, Nothing)
           
        End If


    End Sub

    Private Sub Wybierz(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Module1.komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
            Try
                TableAdaptery.Pacjeci.Update(ds.Pacjenci)
            Catch
            End Try
            Dim i As Long = sender.tag("NrP")
            If i = -1 Then
                i = TableAdaptery.Pacjeci.ScalarQuery
                sender.tag("NrP") = i
            End If

            For Each dr In ds.Pacjenci
                If dr.NrP = i Then
                    p = dr
                End If
            Next



        Else
            pm = sender.tag
        End If
        wybrany = True
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)

    End Sub

    Private Sub Edytuj(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim zamknij As Boolean = False
            If komunikacja.typPolaczenia = PolaczenieInfo.typKomunikacji.Lokalne Then
                Dim w As New WindowSzczegolyPacjentaKrotkie(CType(sender.tag, System.Data.DataRowView))
                w.ShowDialog()
                If w.zapisz = True Then zamknij = True
                w = Nothing
            Else
                Dim w As New WindowSzczegolyPacjentaKrotkie(CType(sender.tag, Pacjenci))
                w.ShowDialog()
                If w.zapisz = True Then zamknij = True
                w = Nothing
            End If

            If zamknij = True Then
                Wybierz(sender, e)
            End If
        Catch ex As Exception

        End Try
    End Sub

    

    Private Sub DataGrid1_LoadingRow(sender As System.Object, e As System.Windows.Controls.DataGridRowEventArgs) Handles DataGrid1.LoadingRow
        Try
            Dim o = e.Row.Item
            If Me.wyszukaj = True Then
                If o.ToString = "Medison.Rejestracja._1.Pacjenci" Then
                    If o.aImie = imie And o.aNazwisko = nazwisko And o.aPesel = pesel Then
                        Me.DataGrid1.SelectedItem = o
                        wyszukaj = False
                    End If
                Else
                    If o("Imie") = imie And o("Nazwisko") = nazwisko And o("Pesel") = pesel Then
                        Me.DataGrid1.SelectedItem = o
                        wyszukaj = False
                    End If
                End If

            End If
        Catch
        End Try
    End Sub

End Class
