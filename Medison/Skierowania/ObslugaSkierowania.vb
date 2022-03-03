Module ObslugaSkierowania
    Public Function ZapiszSkierowanie(fl As FlowDocument, typ As String) As XDocument
        Dim xml As New XDocument
        Dim doc As New XElement("Skierowanie")
        doc.Add(New XAttribute("typ", typ))
        For Each o In fl.Blocks
            Dim ola = o.ToString
            If ola.Contains("Table") Then
                Dim t As Table = o
                For Each r In t.RowGroups
                    For Each r1 In r.Rows
                        For Each c In r1.Cells
                            For Each bl1 In c.Blocks
                                Dim ola1 = bl1.ToString
                                If ola1.Contains("BlockUIContainer") Then
                                    Dim u As BlockUIContainer = bl1
                                    Dim o1 As Object = u.Child
                                    parse(o1, doc)
                                End If
                            Next
                        Next
                    Next
                Next
            End If
            If ola.Contains("BlockUIContainer") Then
                Dim u As BlockUIContainer = o
                Dim o1 As Object = u.Child
                parse(o1, doc)
            End If


        Next
        xml.Add(doc)
        Return xml
    End Function

    Public Function WczytajSkierownanie(fl As FlowDocument, x As String)
        Try
            Dim xml As XDocument = XDocument.Parse(x)
            For Each o In fl.Blocks
                Dim ola = o.ToString

                If ola.Contains("Table") Then
                    Dim t As Table = o
                    For Each r In t.RowGroups
                        For Each r1 In r.Rows
                            For Each c In r1.Cells
                                For Each bl1 In c.Blocks
                                    Dim ola1 = bl1.ToString
                                    If ola1.Contains("BlockUIContainer") Then
                                        Dim u As BlockUIContainer = bl1
                                        Dim o1 As Object = u.Child
                                        restore(o1, xml)
                                    End If
                                Next
                            Next
                        Next
                    Next
                End If

                If ola.Contains("BlockUIContainer") Then
                    Dim u As BlockUIContainer = o
                    Dim o1 As Object = u.Child
                    restore(o1, xml)
                End If

            Next

        Catch
        End Try
    End Function

    Sub parse(o1 As Object, x As XElement)
        Dim ala = o1.ToString

        If ala.Contains("TextBox") Then
            Dim o2 As TextBox = o1
            x.Add(New XElement(o2.Name, o2.Text))
        End If
        If ala.Contains("Label") Then
            Dim o2 As Label = o1
            If o2.Name <> "" Then
                x.Add(New XElement(o2.Name, o2.Content))
            End If
        End If
        If ala.Contains("StackPanel") Then
            Dim o2 As StackPanel = o1
            For Each o3 In o2.Children
                parse(o3, x)
            Next
        End If
    End Sub

    Sub restore(o1 As Object, x As XDocument)
        Dim ala = o1.ToString

        If ala.Contains("TextBox") Then
            Dim o2 As TextBox = o1
            o2.Text = PobierzWartosc(o2.Name, x)
        End If
        If ala.Contains("Label") Then
            Dim o2 As Label = o1
            If o2.Name <> "" Then
                o2.Content = PobierzWartosc(o2.Name, x)
            End If
        End If
        If ala.Contains("StackPanel") Then
            Dim o2 As StackPanel = o1
            For Each o3 In o2.Children
                restore(o3, x)
            Next
        End If
    End Sub

    Public Function PobierzWartosc(nazwa As String, x As XDocument) As String
        Try
            Return x.Descendants(nazwa).First.Value
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Module
