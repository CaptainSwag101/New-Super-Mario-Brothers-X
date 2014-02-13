Public Class LevelBase
    ' LISTS OF BLOCKS, NPCs, ETC.
    Public BlockList(0, 0) As Block
    Public NPCList As New List(Of NPC)

    Public LevelName As String = ""
    Public LevelWidth As Integer = 0
    Public LevelHeight As Integer = 0

    ' PLAYER START LOCATION
    Public StartLocation As Vector2

    Public Sub New(Name As String, width As Integer, height As Integer, start As Vector2)
        ReDim BlockList(width, height)
        LevelWidth = width
        LevelHeight = height
        LevelName = Name

        StartLocation = start
    End Sub
End Class
