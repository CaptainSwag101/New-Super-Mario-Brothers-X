Partial Public Class WorldScreen
    ' SCREEN POSITION
    Public PlayerScreenX As Integer = 8
    Public PlayerScreenY As Integer = 6

    ' DA Player'S MAP COORDINATES
    Public PlayerX As Integer = 0
    Public PlayerY As Integer = 0

    ' DA Player'S MAP OFFSET - SMOOTH WALKING
    Public PlayerOffsetX As Integer = 0
    Public PlayerOffsetY As Integer = 0

    ' MOVEMENT
    Public PlayerMoving As Boolean = True
    Public MoveTime As Double = 0
    Public MoveSpeed As Integer = 3
    Public MoveDir As Integer = 0
    Public LastDir As Integer = 1

    Private PlayerFrame As Integer = 0

    ' SCROLLS THE MAP IN THE UPDATE CYCLE
    Public Sub Move(dir As Integer)
        MoveDir = dir

        Select Case dir
            Case 1 ' DOWN
                PlayerOffsetY -= MoveSpeed

                If PlayerOffsetY <= -32 Then
                    MapY += 1
                    PlayerOffsetY = 0
                End If
            Case 2 ' LEFT
                PlayerOffsetX += MoveSpeed

                If PlayerOffsetX >= 32 Then
                    MapX -= 1
                    PlayerOffsetX = 0
                End If
            Case 3 ' RIGHT
                PlayerOffsetX -= MoveSpeed

                If PlayerOffsetX <= -32 Then
                    MapX += 1
                    PlayerOffsetX = 0
                End If
            Case 4 ' UP
                PlayerOffsetY += MoveSpeed

                If PlayerOffsetY >= 32 Then
                    MapY -= 1
                    PlayerOffsetY = 0
                End If
        End Select

        If PlayerOffsetX <> 0 Then
            PlayerFrame = Math.Floor(Math.Abs(PlayerOffsetX) / 32 * 3)
        ElseIf PlayerOffsetY <> 0 Then
            PlayerFrame = Math.Floor(Math.Abs(PlayerOffsetY) / 32 * 3)
        Else
            PlayerFrame = 0
        End If

        If MoveDir <> 0 Then
            LastDir = dir
        End If
    End Sub

    Public Sub MovePlayer(dir As Integer, Playerx As Integer, Playery As Integer)
        If Map.TileList(Playerx, Playery).IsBlocked = False Then
            MoveDir = dir
            PlayerMoving = True
        End If
    End Sub

    Private Function FetchPlayerSrc(dir As Integer) As Rectangle
        Select Case dir
            Case 1 ' DOWN
                sRect = New Rectangle(0, 32, 16, 16)
            Case 2 ' LEFT
                sRect = New Rectangle(0, 16, 16, 16)
            Case 3 ' RIGHT
                sRect = New Rectangle(0, 0, 16, 16)
            Case 4 ' UP
                sRect = New Rectangle(0, 48, 16, 16)
        End Select
        Return sRect
    End Function

End Class
