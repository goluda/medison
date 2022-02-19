Public Class WindowGodzinyPrzyjec

    Dim ds As New DataSet1

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        ds.EnforceConstraints = False
        TableAdaptery.DniTygodnia.Fill(ds.dniTygodnia)
        Me.ComboDni.ItemsSource = ds.dniTygodnia
        TableAdaptery.DaneLekarza.Fill(ds.DaneLekarza)
        TableAdaptery.GodzinyPrzyjec.Fill(ds.GodzinyPrzyjec)

        Me.DataContext = ds.DaneLekarza

        Me.ComboBoxLekarz2.ItemsSource = TableAdaptery.DaneLekarza.GetData


    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            TableAdaptery.GodzinyPrzyjec.Update(ds.GodzinyPrzyjec)
            MsgBox("Zapisane")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGrid1_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles DataGrid1.SelectionChanged

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim l1 As DataSet1.DaneLekarzaRow = Me.ListBox1.SelectedItem.row
        Dim nrgabinetu As Integer = Me.ComboBoxLekarz2.SelectedValue
        For Each g In l1.GetGodzinyPrzyjecRows
            Dim g1 As DataSet1.GodzinyPrzyjecRow = ds.GodzinyPrzyjec.NewRow
            g1.nrGabinetu = nrgabinetu
            g1.dzien = g.dzien
            g1.godzinaDo = g.godzinaDo
            g1.dodzinaOd = g.dodzinaOd
            g1.minutaDo = g.minutaDo
            g1.minutaOd = g.minutaOd
            g1.skokMinut = g.skokMinut
            ds.GodzinyPrzyjec.Rows.Add(g1)
        Next

    End Sub

    Private Sub Window_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If MsgBox("Zapisać?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Button1_Click(Nothing, Nothing)
        End If
    End Sub
End Class
