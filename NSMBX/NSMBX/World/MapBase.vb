Public Class MapBase
    ' MAP LISTS
    Public TileList(0, 0) As Tile

    Public MapName As String = ""
    Public MapWidth As Integer = 0
    Public MapHeight As Integer = 0

    ' PLAYER START LOCATION
    Public StartLocation As Vector2

    Public Sub New(Name As String, width As Integer, height As Integer, start As Vector2)
        ReDim TileList(width, height)
        MapWidth = width
        MapHeight = height
        MapName = Name

        StartLocation = start
    End Sub

End Class
