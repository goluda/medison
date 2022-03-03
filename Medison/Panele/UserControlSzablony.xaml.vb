Public Class UserControlSzablony
    Implements MyUserControl

    Dim db As New medisonEntities
    Dim WithEvents szablony As List(Of Szablony)
    Dim WithEvents szablonyGrupy As List(Of SzabGrupy)



    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        'AddHandler CType(Me.Resources("Grupy"), Windows.Data.CollectionViewSource)., AddressOf grupy_CurrentChanged
        '
        'szukaj()

        'Do not load your data at design time.
        'If Not (System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me)) Then
        '	'Load your data here and assign the result to the CollectionViewSource.
        '	Dim myCollectionViewSource As System.Windows.Data.CollectionViewSource = CType(Me.Resources("Resource Key for CollectionViewSource"), System.Windows.Data.CollectionViewSource)
        '	myCollectionViewSource.Source = your data
        'End If

        szukaj()
    End Sub

    Public Sub createHandlers() Implements MyUserControl.createHandlers
        AddHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        AddHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub



    Public Sub Dispose() Implements MyUserControl.Dispose
        'db.Dispose()
        RemoveHandler Module1.dane.ZmienaDaty, AddressOf zmianaDaty
        RemoveHandler Module1.dane.ZmianaLekarza, AddressOf zmianaLekarza
    End Sub

    Private Sub zmianaDaty(ByVal nowaData As Date)
        'MsgBox(nowaData)
    End Sub

    Private Sub zmianaLekarza(ByVal nowyLekarz As DaneLekarza)
        szukajSzablon()
    End Sub



    Private Sub szukaj()
        setInfo("Ładowanie danych")


        szablonyGrupy = (From a In db.SzabGrupy).ToList
        Me.DataGridSzablony.ItemsSource = szablonyGrupy
        setInfo("Lista załadowana")
        'Me.szablonyGrupy.MoveCurrentToLast()
        'Me.szablonyGrupy.MoveCurrentToFirst()
        szukajSzablon()

    End Sub

    Public Sub szukajSzablon()
        setInfo("Ładowanie szablonów")

        Dim grupa As SzabGrupy = Me.DataGridSzablony.CurrentItem
        Try
            setInfo(grupa.GrupaSz)

            Me.szablony = (From a In db.Szablony Where
                                                                         a.NrGabinetu = Module1.dane.WybranyLekarz.NrGabinetu _
                                                                         And a.NrGrSzablon = grupa.NrGrSzablon).ToList

            Me.DataContext = szablony
            setInfo("Załadowane")
        Catch
        End Try
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub






    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
       

        Try
            Dim a = db.SaveChanges()
            MsgBox("Zapisano zmiany")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub DataGridSzablony_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridSzablony.SelectionChanged
        szukajSzablon()
    End Sub

    Private Sub DataGridSzablony1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridSzablony1.SelectionChanged
        Return

        'Dim o As Windows.Data.BindingListCollectionView = sender
        Try
            If Me.DataGridSzablony1.SelectedItem IsNot Nothing Then
                Dim s As Szablony = New Szablony

                s.NrGabinetu = Module1.dane.WybranyLekarz.NrGabinetu
                s.NrGrSzablon = CType(Me.DataGridSzablony.SelectedItem, SzabGrupy).NrGrSzablon
                db.Szablony.Add(s)
                Me.szablony.Add(s)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub DataGridSzablony_RowEditEnding(sender As Object, e As DataGridRowEditEndingEventArgs) Handles DataGridSzablony.RowEditEnding

    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim g = New SzabGrupy()
        Me.szablonyGrupy.Add(g)
        db.SzabGrupy.Add(g)
        Me.DataGridSzablony1.Items.Refresh()
        Me.DataGridSzablony.Items.Refresh()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim s = New Szablony
        s.NrGabinetu = Module1.dane.WybranyLekarz.NrGabinetu
        s.NrGrSzablon = CType(Me.DataGridSzablony.SelectedItem, SzabGrupy).NrGrSzablon
        db.Szablony.Add(s)
        Me.szablony.Add(s)
        Me.DataGridSzablony1.Items.Refresh()
        Me.DataGridSzablony.Items.Refresh()
    End Sub
End Class
