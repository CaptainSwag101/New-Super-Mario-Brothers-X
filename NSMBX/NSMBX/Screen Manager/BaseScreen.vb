Public MustInherit Class BaseScreen
    Public Name As String = ""
    Public State As ScreenState = ScreenState.Active
    Public Alpha As Single
    Public Position As Single
    Public Focused As Boolean = False
    Public GrabFocus As Boolean = True

    Public Overridable Sub HandleInput()

    End Sub

    Public Overridable Sub Update()

    End Sub

    Public Overridable Sub Draw()

    End Sub

    Public Overridable Sub Unload()
        State = ScreenState.Shutdown
    End Sub
End Class
