' Rename Class
Public Class ScreenTemplate
    Inherits BaseScreen

    Public Sub New()
        ' Add Name
        Name = ""
        ' Default Transition Time is 750 ms
        'TransitionTime = 750
        'HasInput = False
    End Sub

    Public Overrides Sub HandleInput()

    End Sub

    Public Overrides Sub Update()

    End Sub

    Public Overrides Sub Draw()
        MyBase.Draw()
        Globals.SpriteBatch.Begin()

        Globals.SpriteBatch.End()
    End Sub
End Class
