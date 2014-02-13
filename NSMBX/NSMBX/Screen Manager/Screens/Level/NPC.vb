Public Class NPC
    Public ImageAlias As String

    Public sRect As Rectangle = New Rectangle(0, 0, 32, 32)
    Public AniFrame As Integer

    Public X As Integer
    Public Y As Integer

    Public OffsetX As Integer
    Public OffsetY As Integer

    Public IsMoving As Boolean = False
    
    Public LastDir As Integer = 1
    Public MoveDir As Integer = 1

    Public MaxHP As Integer
    Public HP As Integer

    Public MoveSpeed As Integer = 2
    Public MovePause As Integer = 1000

    Public Name As String
    Public Dialog As String
    Public NPCType As EntityType = EntityType.Peaceful

    Private tmrMove As Integer
    Private tmrMove2 As Integer

    Private rnd As New Random

    Public Function GetNPCTexture() As Texture2D
        For Each Asset As ImageAsset In Textures.ImageAssets
            If Asset.Name = ImageAlias Then
                Return Asset.Image
            End If
        Next

        Return Textures.Menu1
    End Function

    Public Function GetNPCSource() As Rectangle
        Select Case LastDir
            Case 1 ' DOWN
                sRect = New Rectangle(32 * AniFrame, 0, 32, 32)
            Case 2 ' UP
                sRect = New Rectangle(32 * AniFrame, 96, 32, 32)
            Case 3 ' RIGHT
                sRect = New Rectangle(32 * AniFrame, 64, 32, 32)
            Case 4 ' LEFT
                sRect = New Rectangle(32 * AniFrame, 32, 32, 32)
        End Select

        Return sRect
    End Function

    Private Sub MoveNPC(screen As WorldScreen, dir As Integer)
        LastDir = dir

        Select Case dir
            Case 1 ' DOWN
                If screen.Map.TileList(X, Y + 1).IsBlocked = False Then
                    OffsetY += MoveSpeed

                    If OffsetY >= 32 Then
                        Y += 1
                        OffsetY = 0
                    End If
                End If
            Case 2 ' UP
                If screen.Map.TileList(X, Y - 1).IsBlocked = False Then
                    OffsetY -= MoveSpeed

                    If OffsetY <= -32 Then
                        Y -= 1
                        OffsetY = 0
                    End If
                End If
            Case 3 ' RIGHT
                If screen.Map.TileList(X + 1, Y).IsBlocked = False Then
                    OffsetX += MoveSpeed

                    If OffsetX >= 32 Then
                        X += 1
                        OffsetX = 0
                    End If
                End If
            Case 4 ' LEFT
                If screen.Map.TileList(X - 1, Y).IsBlocked = False Then
                    OffsetX -= MoveSpeed

                    If OffsetX <= -32 Then
                        X -= 1
                        OffsetX = 0
                    End If
                End If
        End Select

        ' UPDATE ANIMATION FRAME
        If OffsetX <> 0 Then
            AniFrame = Math.Floor(Math.Abs(OffsetX) / 32 * 3)
        ElseIf OffsetY <> 0 Then
            AniFrame = Math.Floor(Math.Abs(OffsetY) / 32 * 3)
        Else
            AniFrame = 0
        End If
    End Sub

    Public Sub Chat()
        'ScreenManager.AddScreen(New TextProcessor(WindowStyle.Entity, Dialog, True))
    End Sub

    Public Sub Update(screen As WorldScreen)
        tmrMove += Globals.GameTime.ElapsedGameTime.TotalMilliseconds

        If tmrMove > 25 And IsMoving = True Then

            ' RESET PROPERTIES OF OPCCUPIED TILE
            screen.Map.TileList(X, Y).IsBlocked = False
            screen.Map.TileList(X, Y).Entity = Nothing

            MoveNPC(screen, LastDir)

            screen.Map.TileList(X, Y).IsBlocked = True
            screen.Map.TileList(X, Y).Entity = Me ' POPULATE TILE

            If OffsetX = 0 And OffsetY = 0 Then
                IsMoving = False
            End If

            tmrMove = 0
        End If

        ' SET BEHAVIOR CYCLE (TODO: CREATE DIFFERENT BEHAVIORS BASED ON NPCType (Enemy, Ally, etc.))

        ' TODO: DETERMINE THE CLOSEST TARGET THAT IS WITHIN AGGRO RANGE & SET ITS COORDS TO Target
        Select Case NPCType
            Case EntityType.Enemy
                
            Case EntityType.Peaceful
                
        End Select

        tmrMove += Globals.GameTime.ElapsedGameTime.TotalMilliseconds

        If tmrMove > 25 And IsMoving = True Then

            ' RESET PROPERTIES OF OPCCUPIED TILE
            screen.Map.TileList(X, Y).IsBlocked = False
            screen.Map.TileList(X, Y).Entity = Nothing

            MoveNPC(screen, LastDir)

            screen.Map.TileList(X, Y).IsBlocked = True
            screen.Map.TileList(X, Y).Entity = Me ' POPULATE TILE

            If OffsetX = 0 And OffsetY = 0 Then
                IsMoving = False
            End If

            tmrMove = 0
        End If

    End Sub
End Class

Public Enum EntityType
    None
    Ally
    Peaceful
    Enemy
End Enum
