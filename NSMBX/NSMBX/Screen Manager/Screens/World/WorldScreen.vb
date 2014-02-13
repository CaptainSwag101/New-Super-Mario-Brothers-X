Public Class WorldScreen
    Inherits BaseScreen

    ' MAP DIMENSIONS
    Public Map As MapBase
    Public MapWidth As Integer = 0
    Public MapHeight As Integer = 0
    Public TileSize As Integer = 32

    ' CURRENT COORDINATES
    Public MapX As Integer = 20 ' Map X Coordinate
    Public MapY As Integer = 19 ' Map Y Coordinate

    ' SPRITE SOURCES
    Private sRect As Rectangle

    ' TRIGGER PROCESSING 
    Private TriggerActivated As Boolean = False

    Public Sub New(mapname As String)
        Name = "WorldScreen"
        'Map = New MapBase(MapWidth, MapHeight, New Vector2(0, 0))
        Dim MH As New MapHandler
        Map = MH.LoadMap(mapname, Me)
        MapWidth = Map.MapWidth
        MapHeight = Map.MapHeight

        MapX = Map.StartLocation.X
        MapY = Map.StartLocation.Y
    End Sub

    Public Overrides Sub HandleInput()
        If Focused = True Then

            If PlayerOffsetX = 0 And PlayerOffsetY = 0 And PlayerMoving = False Then
                If Input.KeyDown(Keys.Down) Then
                    MovePlayer(1, PlayerX, PlayerY + 1)
                    LastDir = 1
                ElseIf Input.KeyDown(Keys.Up) Then
                    MovePlayer(4, PlayerX, PlayerY - 1)
                    LastDir = 4
                ElseIf Input.KeyDown(Keys.Left) Then
                    MovePlayer(2, PlayerX - 1, PlayerY)
                    LastDir = 2
                ElseIf Input.KeyDown(Keys.Right) Then
                    MovePlayer(3, PlayerX + 1, PlayerY)
                    LastDir = 3
                Else
                    MoveDir = 0
                End If
            End If

            ' INVOKE NPC DIALOG
            If Input.KeyPressed(Keys.X) Then
                NPCChat(LastDir)
            End If

            'If Input.KeyPressed(Keys.I) Then
            '   ScreenManager.AddScreen(New Inventory)
            'End If

        End If
    End Sub

    Public Overrides Sub Update()
        ' ************** CHARACTER MOVEMENT UPDATES ****************
        MoveTime += Globals.GameTime.ElapsedGameTime.TotalMilliseconds
        If MoveTime > 25 And PlayerMoving = True Then
            If MoveDir = 0 And (PlayerOffsetX <> 0 Or PlayerOffsetY <> 0) Then
                ' COMPLETE MOVE CYCLE BEFORE ACCEPTING NEW MOVEMENT
                Move(LastDir)
            Else
                Move(MoveDir)
            End If

            If PlayerOffsetX = 0 And PlayerOffsetY = 0 Then
                PlayerMoving = False
            End If

            MoveTime = 0
        End If

        ' UPDATE PLAYER COORDINATES
        PlayerX = MapX + PlayerScreenX
        PlayerY = MapY + PlayerScreenY

        ' ************* END CHARACTER MOVEMENT UPDATES ***************

        ' ********** WORLD UPDATES **********
        ' TILE ANIMATION
        ' UPDATE ANIMATIONS
        For Each T As Tile In Map.AnimationList
            T.Animator.Update()
        Next
    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone)

        ' DRAW TILE LAYER
        For DrawX = -1 To 16
            For DrawY = -1 To 15
                Dim x As Integer = DrawX + MapX
                Dim y As Integer = DrawY + MapY

                If x >= 0 And x <= MapWidth And y >= 0 And y <= MapHeight Then
                    ' ******* Create FetchTileSource() function first *******
                    'Globals.SpriteBatch.Draw(Textures.FetchImage(Map.TileList(x, y).ImageAsset), New Rectangle(DrawX * TileSize + PlayerOffsetX, DrawY * TileSize + PlayerOffsetY, TileSize, TileSize), Map.TileList(x, y).SrcRect, Color.White)
                    ' VIEW COORDINATES ON TILE
                    'Globals.SpriteBatch.DrawString(Fonts.Verdana_8, "X:" & x & vbCrLf & "Y:" & y, New Vector2(DrawX * TileSize, DrawY * TileSize), Color.Black)
                End If
            Next
        Next

        Globals.SpriteBatch.Draw(Textures.Player, New Rectangle(PlayerScreenX * 32, PlayerScreenY * 32, 32, 32), FetchPlayerSrc(LastDir), Color.White)

        Globals.SpriteBatch.End()
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
