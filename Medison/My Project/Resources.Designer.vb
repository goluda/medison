﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Medison.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Density_Plateled_Gel() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Density_Plateled_Gel", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 11 Listopada
        '''30 Stycznia 
        '''9 Maja 
        '''Akacjowa 
        '''Alabastrowa 
        '''Aleja Konstytucji 3 Maja 
        '''Aleja Odrodzenia Polski 
        '''Aleje 11 Listopada 
        '''Ametystowa 
        '''Andersa 
        '''Ar. Krajowej 
        '''Ar. Ludowej 
        '''Ar. Polskiej 
        '''Arciszewskiego 
        '''Ariańska 
        '''Artylerzystów 
        '''Asnyka 
        '''Ateńska 
        '''Baczewskiego 
        '''Baczyńskiego 
        '''Bajana 
        '''Barytowa 
        '''Bat. Chłopskich 
        '''Batorego 
        '''Bazaltowa 
        '''Bema 
        '''Berlinga 
        '''Berlińska 
        '''Berylowa 
        '''Bierzarina 
        '''Billewiczówny 
        '''Bindera 
        '''Biwakowa 
        '''Bledzewska 
        '''Błotna 
        '''Bociania 
        '''Bogusławskiego 
        '''Boh. Lenino 
        ''' [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ulice() As String
            Get
                Return ResourceManager.GetString("ulice", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
