Public Class Background
    Inherits BaseScreen

    Public Sub New()
        Name = "BackGround"
    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin()

        Globals.SpriteBatch.End()
    End Sub
End Class