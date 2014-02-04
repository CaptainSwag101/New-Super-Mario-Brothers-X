Public Class Gameplay
    Inherits BaseScreen

    Public Shared Map As MapBase
    Public Shared TileSize As Integer = 32
    Public Shared MapX As Integer = 0
    Public Shared MapY As Integer = 0
    Public Shared MoveToX As Integer
    Public Shared MoveToY As Integer
    Private mapWidth As Integer = 75
    Private mapHeight As Integer = 60
    Private moveTime As Integer = 0
    Private animateFrame As Integer = 0
    Private animateTime As Integer = 0
    Private animateDir As Integer = 0
    Private mapBounds As New Rectangle(4, 30, 672, 416)
    Private minimapBounds As New Rectangle(680, 30, 116, 93)
    Private minimapBB As RenderTarget2D
    Private updateMinimap As Boolean = False

    Private pathStart As Vector2
    Private pathEnd As Vector2
    Private pathing As Boolean = False
    Private path As Pathing

    Public Sub New()
        Name = "Gameplay"
        TransitionTime = 0
        minimapBB = New RenderTarget2D(Globals.Graphics.GraphicsDevice, mapWidth * 2, mapHeight * 2, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
        Map = New Map(mapWidth, mapHeight)
        updateMinimap = True
    End Sub

    Public Overrides Sub HandleInput()
        If Focused = True And State = ScreenState.Active Then
            If Input.KeyboardInput.KeyPress(Keys.F2) Then
                Map = New Map(mapWidth, mapHeight)
                updateMinimap = True
            End If
            If mapBounds.Contains(Input.MouseInput.Position) Then
                If Input.MouseInput.MiddlePressed Then
                    MoveToX = MathHelper.Clamp(Math.Floor((Input.MouseInput.Position.X - mapBounds.X) / TileSize) + MapX - 10, 0, mapWidth - 20)
                    MoveToY = MathHelper.Clamp(Math.Floor((Input.MouseInput.Position.Y - mapBounds.Y) / TileSize) + MapY - 6, 0, mapHeight - 12)
                ElseIf Input.MouseInput.LeftPressed Then
                    'Map.TileList(Input.MouseInput.Position.X / TileSize + MapX, Input.MouseInput.Position.Y / TileSize + MapY).Fog = False
                    If pathing Then
                        pathing = False
                        pathEnd.X = Math.Floor((Input.MouseInput.Position.X - mapBounds.X) / TileSize) + MapX
                        pathEnd.Y = Math.Floor((Input.MouseInput.Position.Y - mapBounds.Y) / TileSize) + MapY
                        If path Is Nothing OrElse path.EndPoint <> pathEnd Then path = New Pathing(pathStart, pathEnd)
                    Else
                        path = Nothing
                        pathing = True
                        pathStart.X = Math.Floor((Input.MouseInput.Position.X - mapBounds.X) / TileSize) + MapX
                        pathStart.Y = Math.Floor((Input.MouseInput.Position.Y - mapBounds.Y) / TileSize) + MapY
                    End If
                End If
            End If
            If minimapBounds.Contains(Input.MouseInput.Position) Then
                If Input.MouseInput.LeftDown Then
                    Gameplay.MapX = MathHelper.Clamp((Input.MouseInput.Position.X - minimapBounds.X) / (minimapBounds.Width / mapWidth) - 10, 0, mapWidth - 20)
                    Gameplay.MoveToX = MathHelper.Clamp((Input.MouseInput.Position.X - minimapBounds.X) / (minimapBounds.Width / mapWidth) - 10, 0, mapWidth - 20)
                    Gameplay.MapY = MathHelper.Clamp((Input.MouseInput.Position.Y - minimapBounds.Y) / (minimapBounds.Height / mapHeight) - 6, 0, mapHeight - 12)
                    Gameplay.MoveToY = MathHelper.Clamp((Input.MouseInput.Position.Y - minimapBounds.Y) / (minimapBounds.Height / mapHeight) - 6, 0, mapHeight - 12)
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        If path IsNot Nothing AndAlso path.PathFound = False Then path.FindPath()
        If moveTime > 1 Then
            If MapX < MoveToX Then MapX += 1
            If MapX > MoveToX Then MapX -= 1
            If MapY < MoveToY Then MapY += 1
            If MapY > MoveToY Then MapY -= 1
            moveTime = 0
        Else
            moveTime += 1
        End If
        UpdateFog()
        animateTime += Globals.GameTime.ElapsedGameTime.TotalMilliseconds
        If animateTime > 1500 Then
            If animateDir = 0 Then
                animateFrame += 1
            Else
                animateFrame -= 1
            End If
            If animateFrame >= 2 Then
                animateFrame = 2
                animateDir = 1
            ElseIf animateFrame <= 0 Then
                animateFrame = 0
                animateDir = 0
            End If
            animateTime = 0
        End If
        If updateMinimap Then RenderMinimap()
    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin()
        For drawY = 0 To 12
            For drawX = 0 To 20
                Dim x As Integer = drawX + MapX
                Dim y As Integer = drawY + MapY
                Dim DrawTexture As Texture2D = Nothing
                Select Case Map.TileList(x, y).Type
                    Case TileType.Grass
                        DrawTexture = Globals.Textures.Grass
                    Case TileType.Water
                        DrawTexture = Globals.Textures.Water
                    Case TileType.Mountain
                        DrawTexture = Globals.Textures.Mountain
                    Case TileType.Forest
                        DrawTexture = Globals.Textures.Forest
                End Select
                Dim offset As Integer = 0
                If Map.TileList(x, y).Type = TileType.Water Then offset = animateFrame * TileSize
                Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(0, offset, 32, 32), Color.White * Alpha)
                If Map.TileList(x, y).Index > 0 Then
                    Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(Map.TileList(x, y).Index * 32, offset, 32, 32), Color.White * Alpha)
                End If
                If Map.TileList(x, y).Index2 > 16 Then
                    Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(Map.TileList(x, y).Index2 * 32, offset, 32, 32), Color.White * Alpha)
                End If
                Globals.SpriteBatch.Draw(Globals.Textures.Fog, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(Map.TileList(x, y).FogIndex * 32, 0, 32, 32), Color.White * Alpha)
                Globals.SpriteBatch.Draw(Globals.Textures.Fog, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(Map.TileList(x, y).FogIndex2 * 32, 0, 32, 32), Color.White * Alpha)
                If path IsNot Nothing AndAlso path.PathFound Then
                    For Each pathPoint As Pathing.PathPoint In path.FoundPath
                        If x = pathPoint.Position.X And y = pathPoint.Position.Y Then
                            Globals.SpriteBatch.Draw(Globals.Textures.Path, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(0, 0, 32, 32), Color.Blue * Alpha * 0.7)
                        End If
                    Next
                    If x = pathStart.X And y = pathStart.Y Then
                        Globals.SpriteBatch.Draw(Globals.Textures.Path, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(0, 0, 32, 32), Color.Green * Alpha * 0.7)
                    ElseIf x = pathEnd.X And y = pathEnd.Y Then
                        Globals.SpriteBatch.Draw(Globals.Textures.Path, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(0, 0, 32, 32), Color.Red * Alpha * 0.7)
                    End If
                End If
                If x = pathStart.X And y = pathStart.Y And pathing Then
                    Globals.SpriteBatch.Draw(Globals.Textures.Path, New Rectangle(drawX * TileSize + mapBounds.X, drawY * TileSize + mapBounds.Y, TileSize, TileSize), New Rectangle(0, 0, 32, 32), Color.Green * Alpha * 0.7)
             End If
            Next
        Next
        Globals.SpriteBatch.Draw(minimapBB, minimapBounds, Color.White)
        Globals.SpriteBatch.Draw(Globals.Textures.MinimapBox, New Rectangle(Gameplay.MapX * minimapBounds.Width / mapWidth + minimapBounds.X, Gameplay.MapY * minimapBounds.Height / mapHeight + minimapBounds.Y, 21 * minimapBounds.Width / mapWidth - 1, 13 * minimapBounds.Height / mapHeight - 1), Color.White)
        Globals.SpriteBatch.End()
    End Sub

    Private Sub RenderMinimap()
        Globals.Graphics.GraphicsDevice.SetRenderTarget(minimapBB)
        Globals.SpriteBatch.Begin()
        For Y = 0 To mapHeight
            For X = 0 To mapWidth
                Dim drawX As Integer = X * 2
                Dim drawY As Integer = Y * 2
                Dim DrawTexture As Texture2D = Nothing
                Select Case Gameplay.Map.TileList(X, Y).Type
                    Case TileType.Grass
                        DrawTexture = Globals.Textures.Grass
                    Case TileType.Water
                        DrawTexture = Globals.Textures.Water
                    Case TileType.Mountain
                        DrawTexture = Globals.Textures.Mountain
                    Case TileType.Forest
                        DrawTexture = Globals.Textures.Forest
                End Select
                Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX, drawY, 2, 2), New Rectangle(0, 0, 32, 32), Color.White * Alpha)
                If Gameplay.Map.TileList(X, Y).Index > 0 Then
                    Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX, drawY, 2, 2), New Rectangle(Gameplay.Map.TileList(X, Y).Index * 32, 0, 32, 32), Color.White * Alpha)
                End If
                If Gameplay.Map.TileList(X, Y).Index2 > 16 Then
                    Globals.SpriteBatch.Draw(DrawTexture, New Rectangle(drawX, drawY, 2, 2), New Rectangle(Gameplay.Map.TileList(X, Y).Index2 * 32, 0, 32, 32), Color.White * Alpha)
                End If
                Globals.SpriteBatch.Draw(Globals.Textures.Fog, New Rectangle(drawX, drawY, 2, 2), New Rectangle(Gameplay.Map.TileList(X, Y).FogIndex * 32, 0, 32, 32), Color.White * Alpha)
                Globals.SpriteBatch.Draw(Globals.Textures.Fog, New Rectangle(drawX, drawY, 2, 2), New Rectangle(Gameplay.Map.TileList(X, Y).FogIndex2 * 32, 0, 32, 32), Color.White * Alpha)
            Next
        Next
        Globals.SpriteBatch.End()
        Globals.Graphics.GraphicsDevice.SetRenderTarget(Nothing)
        updateMinimap = False
    End Sub

    Private Sub UpdateFog()
        For y = 0 To mapHeight
            For x = 0 To mapWidth
                ' Diagonal Tile Check
                If Map.TileList(x, y).Fog = False Then
                    Dim border As Integer = 0
                    If x > 0 And y > 0 Then
                        If Map.TileList(x - 1, y - 1).Fog = True And Map.TileList(x - 1, y).Fog = False And Map.TileList(x, y - 1).Fog = False Then border += 1
                    End If
                    If x < mapWidth And y > 0 Then
                        If Map.TileList(x + 1, y - 1).Fog = True And Map.TileList(x, y - 1).Fog = False And Map.TileList(x + 1, y).Fog = False Then border += 2
                    End If
                    If x > 0 And y < mapHeight Then
                        If Map.TileList(x - 1, y + 1).Fog = True And Map.TileList(x - 1, y).Fog = False And Map.TileList(x, y + 1).Fog = False Then border += 4
                    End If
                    If x < mapWidth And y < mapHeight Then
                        If Map.TileList(x + 1, y + 1).Fog = True And Map.TileList(x + 1, y).Fog = False And Map.TileList(x, y + 1).Fog = False Then border += 8
                    End If
                    If border = 0 Then border = 16
                    Map.TileList(x, y).FogIndex = border
                    ' Adjacent Tile Check
                    border = 16
                    If y > 0 Then
                        If Map.TileList(x, y - 1).Fog = True Then border += 1
                    End If
                    If x > 0 Then
                        If Map.TileList(x - 1, y).Fog = True Then border += 2
                    End If
                    If x < mapWidth Then
                        If Map.TileList(x + 1, y).Fog = True Then border += 4
                    End If
                    If y < mapHeight Then
                        If Map.TileList(x, y + 1).Fog = True Then border += 8
                    End If
                    Map.TileList(x, y).FogIndex2 = border
                End If
            Next
        Next
    End Sub
End Class
