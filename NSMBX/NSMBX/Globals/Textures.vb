Public Class Textures
    ' IMAGE ASSETS
    'Public Shared ImageAssets As New List(Of ImageAsset)

    Public Shared RadAvatar As Texture2D
    Public Shared Menu1 As Texture2D
    Public Shared Title As Texture2D

    ' INVENTORY
    Public Shared Inventory As Texture2D
    Public Shared Items As Texture2D

    ' TOONS
    Public Shared RadMarvin As Texture2D
    Public Shared Player As Texture2D
    Public Shared Tomato As Texture2D

    Public Shared Sub Load()
        RadAvatar = Globals.Content.Load(Of Texture2D)("GFX\radface")
        Menu1 = Globals.Content.Load(Of Texture2D)("GFX\menu1")
        Title = Globals.Content.Load(Of Texture2D)("GFX\Title")
        Inventory = Globals.Content.Load(Of Texture2D)("GFX\INVBG")
        Items = Globals.Content.Load(Of Texture2D)("GFX\items")

        ' TOONS
        'RadMarvin = Globals.Content.Load(Of Texture)("GFX\Char\rad")
        Player = Globals.Content.Load(Of Texture2D)("GFX\Char\BlendaOverworld")
        Tomato = Globals.Content.Load(Of Texture2D)("GFX\Char\Tomato")

        ' TEMP IMPORTS
        'Dim IA As New ImageAsset
        'With IA
        '   .Image = Items
        '   .Name = "Items"
        'End With
        'ImageAssets.Add(IA)

    End Sub

    ' LOAD TEXTURE FROM IMAGE ASSETS
    'Public Shared Function FetchImage(AssetId As String) As Texture2D
    'For Each Asset As ImageAsset In Textures.ImageAssets
    'If Asset.Name = AssetId Then
    'Return Asset.Image
    'End If
    'Next

    'Return Textures.Menu1  ' Return gradient if asset does not exist
    'End Function
End Class
