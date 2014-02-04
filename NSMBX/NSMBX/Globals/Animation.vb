Public Class Animation
    Public Parent As Tile
    Public AniTimer As Double

    Public Sub New(TileObject As Tile)
        Parent = TileObject
        AniTimer = 0
    End Sub

    Public Sub Update()
        'MsgBox(AniTimer & ":" & Parent.AniSpeed & ":" & Parent.IsAnimated & ":" & Parent.AniFrames)
        If Parent.IsAnimated = True Then
            AniTimer += Globals.GameTime.ElapsedGameTime.TotalMilliseconds

            If AniTimer > Parent.AniSpeed Then

                If Parent.AniFrame < Parent.AniFrames Then
                    Parent.AniFrame += 1
                    Parent.Animate(Parent.SrcRect.X, Parent.SrcRect.Y, Parent.SrcRect.Width, Parent.SrcRect.Height)
                Else
                    Parent.AniFrame = 0
                    Parent.Animate(Parent.AniBase, Parent.SrcRect.Y, Parent.SrcRect.Width, Parent.SrcRect.Height)
                End If
                AniTimer = 0
            End If
        End If
    End Sub
End Class
