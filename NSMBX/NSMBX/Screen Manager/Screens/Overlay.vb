Public Class Overlay
    Inherits BaseScreen

    Public Sub New()
        Name = "Overlay"
        TransitionTime = 0
        HasInput = False
    End Sub

    Public Overrides Sub HandleInput()
        '-170, - 75, -90
        ' Save these numbers, I need them for something!
    End Sub

    Public Overrides Sub Update()

    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin()
        Globals.SpriteBatch.Draw(Globals.Textures.Overlay, New Rectangle(0, 0, 800, 450), Color.White * Alpha)
        Globals.SpriteBatch.End()
    End Sub
End Class
