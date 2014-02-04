Class TestScreen
    Inherits BaseScreen

    Private TestText As String = "WE CAN HAVE NICE THINGS."
    Private TextPos As New Vector2(20, 195)
    Private IsAlive As Boolean = True

    Private LifeSpan As Integer = 0

    Public Sub New()
        Name = "TestScreen"
    End Sub

    Public Overrides Sub HandleInput()

    End Sub

    Public Overrides Sub Update()
        If LifeSpan < 5000 Then
            LifeSpan += Globals.GameTime.ElapsedGameTime.TotalMilliseconds
        Else
            IsAlive = False
        End If

        If IsAlive = False Then
            Me.State = ScreenState.Shutdown
        End If
    End Sub

    Public Overrides Sub Draw()
        Globals.SpriteBatch.Begin()
        Globals.SpriteBatch.Draw(Textures.RadAvatar, New Rectangle(0, 0, Globals.GameSize.X, Globals.GameSize.Y), New Rectangle(16, 16, 1, 1), Color.White)
        Globals.SpriteBatch.DrawString(Fonts.Georgia_16, TestText, TextPos, Color.Red)
        Globals.SpriteBatch.End()
    End Sub

End Class
