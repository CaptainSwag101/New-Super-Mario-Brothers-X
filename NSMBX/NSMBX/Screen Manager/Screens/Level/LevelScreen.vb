Public Class LevelScreen
  Inherits BaseScreen
  
  Public TileList(x As Integer, y As Integer)
  Public NPCList As New List(Of NPC)
  
  Public Sub New()
    
  End Sub
  
  Private Sub NPCChat(Dir As Short)
        Dim V As New Vector2
        V = InvokeVector(Dir)

        'If Map.TileList(V.X, V.Y).Entity IsNot Nothing Then
        '   MsgBox(Map.TileList(V.X, V.Y).Entity.Dialog)
        '   ScreenManager.AddScreen(New NPCDialog(Map.TileList(V.X, V.Y).Entity.Dialog))
        'End If
    End Sub

    Public Function InvokeVector(Dir As Short) As Vector2
        Dim V As Vector2

        Select Case Dir
            Case 1 ' DOWN 
                V = New Vector2(PlayerX, PlayerY + 1)
            Case 2 ' LEFT
                V = New Vector2(PlayerX - 1, PlayerY)
            Case 3 ' RIGHT
                V = New Vector2(PlayerX + 1, PlayerY)
            Case 4 ' UP
                V = New Vector2(PlayerX, PlayerY - 1)
        End Select

        Return V
    End Function

    Private Sub ProcessTrigger(TriggerScript As String)
        ' Action|Value|Optional Params
        TriggerActivated = True

        Dim TriggerAction As String = Split(TriggerScript, "|")(0)
        Dim TriggerValue As String = Split(TriggerScript, "|")(1)
        Dim TriggerParams As String = ""

        If Split(TriggerScript, "|").Count > 2 Then
            TriggerParams = Split(TriggerScript, "|")(2)
        End If

        Select Case TriggerAction
            Case "LoadMap"
                Dim MH As New MapHandler
                Map = Nothing
                Map = MH.LoadMap(TriggerValue, Me)
                MapWidth = Map.MapWidth
                MapHeight = Map.MapHeight

                MapX = Map.StartLocation.X
                MapY = Map.StartLocation.Y

                ' OVERRIDE START COORDS
                If TriggerParams <> "" Then
                    MapX = Split(TriggerParams, ":")(0)
                    MapY = Split(TriggerParams, ":")(1)
                End If

                PlayerX = MapX + PlayerScreenX
                PlayerY = MapY + PlayerScreenY
            Case "Teleport"
                'TELEPORT DESTINATION
                If TriggerParams <> "" Then
                    MapX = Split(TriggerParams, ":")(0) - PlayerScreenX
                    MapY = Split(TriggerParams, ":")(1) - PlayerScreenY
                End If

                PlayerX = MapX + PlayerScreenX
                PlayerY = MapY + PlayerScreenY
        End Select
    End Sub
  
End Class
