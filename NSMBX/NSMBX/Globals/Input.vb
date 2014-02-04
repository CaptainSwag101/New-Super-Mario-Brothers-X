Public Class Input
    Shared CurrentKeyState As KeyboardState
    Shared LastKeyState As KeyboardState

    Public Shared Sub Update()
        LastKeyState = CurrentKeyState
        CurrentKeyState = Keyboard.GetState
    End Sub

    Public Shared Function KeyDown(key As Keys) As Boolean
        Return CurrentKeyState.IsKeyDown(key)
    End Function

    Public Shared Function KeyPressed(key As Keys) As Boolean
        If CurrentKeyState.IsKeyDown(key) And LastKeyState.IsKeyUp(key) Then
            Return True
        End If
        Return False
    End Function

End Class

