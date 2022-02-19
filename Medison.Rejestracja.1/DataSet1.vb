Partial Class DataSet1
    Partial Class PacjenciDataTable

        Private Sub PacjenciDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.NazwiskoColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class PacjenciRow



        Public Property DataUrodzeniaToString As String
            Get
                If IsdataUrodzeniaNull() = False Then
                    If dataUrodzenia.Year > 1900 Then
                        Return String.Format("{0:00}-{1:00}-{2:00}", Me.dataUrodzenia.Day, Me.dataUrodzenia.Month, Me.dataUrodzenia.Year)
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                Try
                    value = value.Replace("-", "").Replace(".", "")
                    Dim rok As Integer = Convert.ToInt32(value.Substring(4, 4))
                    Dim miesiac As Integer = Convert.ToInt32(value.Substring(2, 2))
                    Dim dzien As Integer = Convert.ToInt32(value.Substring(0, 2))
                    dataUrodzenia = New DateTime(rok, miesiac, dzien)
                    
                Catch
                End Try
            End Set
        End Property

    End Class

    Partial Class DaneLekarzaDataTable

        Private Sub DaneLekarzaDataTable_DaneLekarzaRowChanging(sender As System.Object, e As DaneLekarzaRowChangeEvent) Handles Me.DaneLekarzaRowChanging

        End Sub

    End Class



End Class
