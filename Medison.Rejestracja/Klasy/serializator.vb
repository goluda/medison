Public Class serializator

    Public Shared Function serializeObjectToXelement(ByRef x As XElement, o As Object) As XElement
        For Each p In o.GetType.GetProperties

            Dim typ As String = p.PropertyType.ToString

            If typ = "System.String" Or typ = "System.Integer" Or typ = "System.DateTime" Or typ = "System.Decimal" Then
                x.Add(New XAttribute(p.Name, p.GetValue(o, Nothing)))
            End If

        Next
        Return x
    End Function


    Public Shared Function DeserializeObjectToXelement(ByRef x As XElement, ByRef o As Object) As XElement
        For Each a In x.Attributes
            Try
                Dim p = o.GetType.GetProperty(a.Name.LocalName)
                p.SetValue(o, a.Value, Nothing)
            Catch ex As Exception
            End Try
        Next
        Return x
    End Function
End Class
