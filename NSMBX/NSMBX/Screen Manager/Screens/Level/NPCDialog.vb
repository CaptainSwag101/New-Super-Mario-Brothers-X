Public Class NPCDialog
    Inherits BaseScreen

    Private MenuPos As New Vector2(10, 10)
    Private MenuSize As New Vector2(495, 100)

    Private strDialog As String

    Public Sub New(DialogText As String)
        Globals.InDialog = True
        Name = "NPCDialog"
        Focused = True
        GrabFocus = True

        strDialog = DialogText
    End Sub

    Public Overrides Sub HandleInput()
        If Input.KeyPressed(Keys.X) Then
            ScreenManager.UnloadScreen("NPCDialog")
            Globals.InDialog = False
        End If
    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin()

        MenuGFX.DrawMenuGFX(New Rectangle(MenuPos.X, MenuPos.Y, MenuSize.X, MenuSize.Y), Color.White, True, Globals.SpriteBatch, Textures.Menu1, 0.9)

        Globals.SpriteBatch.DrawString(Fonts.Georgia_16, strDialog, New Vector2(MenuPos.X + 20, MenuPos.Y + 10), Color.White)

        Globals.SpriteBatch.End()
    End Sub
End Class
