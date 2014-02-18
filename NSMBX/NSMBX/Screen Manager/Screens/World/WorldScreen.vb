Public Class WorldScreen
    Inherits BaseScreen

    ' MAP DIMENSIONS
    'Public Map As MapBase
    'Public MapWidth As Integer = 0
    'Public MapHeight As Integer = 0
    'Public TileSize As Integer = 32

    ' CURRENT COORDINATES
    'Public MapX As Integer = 20 ' Map X Coordinate
    'Public MapY As Integer = 19 ' Map Y Coordinate

    ' SPRITE SOURCES
    'Private sRect As Rectangle

    ' TRIGGER PROCESSING 
    Private TriggerActivated As Boolean = False

    ' MAP DIMENSIONS
    Public Shared Map As MapBase
    Public MapWidth As Integer = 256
    Public MapHeight As Integer = 256
    Public TileSize As Integer = 32

    ' CURRENT COORDINATES
    Public Shared MapX As Integer = 20 ' Map X Coordinate
    Public Shared MapY As Integer = 19 ' Map Y Coordinate

    ' SPRITE SOURCES
    Private sRect As Rectangle
    Private SelectedSrcX As Integer = 0
    Private SelectedSrcY As Integer = 0

    ' CLICKED TILE PROPERTIES
    Private SelectedBlocked As Boolean = False
    Private SelectedStepTrigger As Boolean = False
    Private SelectedTouchTrigger As Boolean = False
    Private SelectedOnetimeTrigger As Boolean = False
    Private SelectedActivated As Boolean = False

    Dim IsSaving As Boolean = False

    Public Sub New()
        Name = "WorldScreen"
        GrabFocus = True

        ' CREATE A NEW BLANK MAP
        Map = New MapBase(MapWidth, MapHeight, New Vector2(0, 0))
        MapX = 0
        MapY = 0
        For X = 0 To MapWidth
            For Y = 0 To MapHeight
                Map.TileList(X, Y).SrcRect = New Rectangle(0, 0, 16, 16)
                Map.TileList(X, Y).IsBlocked = False
                Map.TileList(X, Y).IsStepTrigger = False
                Map.TileList(X, Y).IsTouchTrigger = False
                Map.TileList(X, Y).IsOnetimeTrigger = False
                Map.TileList(X, Y).IsActivated = False
            Next
        Next
    End Sub

    Public Overrides Sub HandleInput()
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

        If Input.KeyPressed(Keys.NumPad1) Then
            SelectedSrcX = 0
            SelectedSrcY = 0
        ElseIf Input.KeyPressed(Keys.NumPad2) Then
            SelectedSrcX = 0
            SelectedSrcY = 16
        ElseIf Input.KeyPressed(Keys.NumPad3) Then
            SelectedSrcX = 0
            SelectedSrcY = 80
        ElseIf Input.KeyPressed(Keys.NumPad4) Then
            SelectedSrcX = 32
            SelectedSrcY = 48
        ElseIf Input.KeyPressed(Keys.NumPad5) Then
            SelectedSrcX = 16
            SelectedSrcY = 0
        ElseIf Input.KeyPressed(Keys.NumPad6) Then
            SelectedSrcX = 32
            SelectedSrcY = 0
        ElseIf Input.KeyPressed(Keys.NumPad7) Then
            SelectedSrcX = 48
            SelectedSrcY = 0
        ElseIf Input.KeyPressed((Keys.NumPad8)) Then
			SelectedBlocked = Not SelectedBlocked
        ElseIf Input.KeyPressed((Keys.NumPad9)) Then
			SelectedActivated = Not SelectedActivated
        End If

        If Input.MouseClick() = MouseButton.Left Then
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).SrcRect = New Rectangle(SelectedSrcX, SelectedSrcY, 16, 16)
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).IsBlocked = SelectedBlocked
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).IsStepTrigger = False
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).IsTouchTrigger = False
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).IsOnetimeTrigger = False
            Map.TileList((CInt(Math.Floor(Mouse.GetState.X / 32))) + MapX, (CInt(Math.Floor(Mouse.GetState.Y / 32) + MapY))).IsActivated = SelectedActivated
        End If
		
        If Input.KeyPressed(Keys.S) And IsSaving = False Then
            IsSaving = True
            Dim MH As New MapHandler
            MH.SaveMap(Map)
            IsSaving = False
        End If
		
		If (Input.KeyPressed(Keys.LeftControl) Or Input.KeyPressed(Keys.RightControl)) And Input.KeyPressed(Keys.G) Then
            Globals.DebugEnabled = Not Globals.DebugEnabled
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
                    Globals.SpriteBatch.Draw(Textures.World, New Rectangle(DrawX * TileSize + PlayerOffsetX, DrawY * TileSize + PlayerOffsetY, TileSize, TileSize), Map.TileList(x, y).SrcRect, Color.White)
                    
					' VIEW COORDINATES ON TILE
                    If Globals.DebugEnabled = True Then
                        Globals.SpriteBatch.DrawString(Fonts.Arial_8, "X:" & x & vbCrLf & "Y:" & y, New Vector2(DrawX * TileSize, DrawY * TileSize), Color.White)
                    End If
					
					' VIEW IF A TILE IS BLOCKED
                    If Map.TileList(x, y).IsBlocked = True And Globals.DebugEnabled = False Then
                        Globals.SpriteBatch.DrawString(Fonts.Arial_8, "Block", New Vector2(DrawX * TileSize, DrawY * TileSize), Color.White)
                    End If
                End If
            Next
        Next

		' DRAW INVISIBLE PLAYER SPRITE
        Globals.SpriteBatch.Draw(Textures.Player, New Rectangle(PlayerScreenX * 32, PlayerScreenY * 32, 32, 32), FetchPlayerSrc(LastDir), Color.White * 0.0F)
		
		' DRAW MOUSE TOOLTIP
        Globals.SpriteBatch.Draw(Textures.World, New Rectangle(CInt(Math.Floor(Mouse.GetState.X / 32)) * 32, CInt(Math.Floor(Mouse.GetState.Y / 32)) * 32, 32, 32), New Rectangle(SelectedSrcX, SelectedSrcY, 16, 16), Color.White)
        If SelectedBlocked = True And Globals.DebugEnabled = False Then
            Globals.SpriteBatch.DrawString(Fonts.Arial_8, "Block", New Vector2(CInt(Math.Floor(Mouse.GetState.X / 32)) * 32, CInt(Math.Floor(Mouse.GetState.Y / 32)) * 32), Color.White)
        End If

        Globals.SpriteBatch.End()
    End Sub

    Public Sub PlaceTile()

    End Sub
End Class
