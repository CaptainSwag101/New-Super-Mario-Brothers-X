Class Debug
    Inherits BaseScreen

    Public Screens As String = ""
    Public FocusScreen As String = ""

    Private fps As Integer
    Private fpsCounter As Integer
    Private fpsTimer As Double
    Private fpsText As String = ""

    Private BGRect As Rectangle

    Private KeyDown As Boolean = False

    Public Sub New()
        Name = "Debug"
        State = ScreenState.Hidden
        GrabFocus = False
    End Sub

    Public Overrides Sub HandleInput()
        MyBase.HandleInput()

        If Input.KeyPressed(Keys.F1) Then
            If State = ScreenState.Active Then
                State = ScreenState.Hidden
            ElseIf State = ScreenState.Hidden Then
                State = ScreenState.Active
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        MyBase.Update()
        If Screens.Length > 0 Then
            Screens = Screens.Substring(0, Screens.Length - 2)
        End If

        Dim txtWidth As Integer = 0
        Dim txtHeight As Integer = 0
        If Fonts.Verdana_8.MeasureString(Screens).X > txtWidth Then
            txtWidth = Fonts.Verdana_8.MeasureString(Screens).X
        End If
        If Fonts.Verdana_8.MeasureString(FocusScreen).X > txtWidth Then
            txtWidth = Fonts.Verdana_8.MeasureString(FocusScreen).X
        End If
        txtHeight = Fonts.Verdana_8.MeasureString(fpsText).Y * 3
        BGRect = New Rectangle(0, 0, txtWidth + 20, txtHeight + 20)
    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()

        If Globals.GameTime.TotalGameTime.TotalMilliseconds > fpsTimer Then
            fps = fpsCounter
            fpsTimer = Globals.GameTime.TotalGameTime.TotalMilliseconds + 1000
            fpsCounter = 1
            fpsText = "FPS: " & fps
        Else
            fpsCounter += 1
        End If

        Globals.SpriteBatch.Begin()
        Globals.SpriteBatch.Draw(Textures.RadAvatar, BGRect, Color.Black * 0.6F)
        Globals.SpriteBatch.DrawString(Fonts.Verdana_8, fpsText, New Vector2(10, 10), Color.White)
        Globals.SpriteBatch.DrawString(Fonts.Verdana_8, Screens, New Vector2(10, 22), Color.White)
        Globals.SpriteBatch.DrawString(Fonts.Verdana_8, FocusScreen, New Vector2(10, 34), Color.White)
        Globals.SpriteBatch.End()
    End Sub
End Class
