Public Enum MouseButton
    Left
    Right
    None
End Enum

Public Class Input
    Shared CurrentKeyState As KeyboardState
    Shared LastKeyState As KeyboardState
    Public Shared CurrentMouseState As MouseState
    Shared LastMouseState As MouseState

    Public Shared Sub Update()
        LastKeyState = CurrentKeyState
        CurrentKeyState = Keyboard.GetState
        LastMouseState = CurrentMouseState
        CurrentMouseState = Mouse.GetState
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

    Public Shared Function MouseClick() As MouseButton
        If CurrentMouseState.LeftButton = ButtonState.Pressed And CurrentMouseState.RightButton = ButtonState.Released Then
            Return MouseButton.Left
        ElseIf CurrentMouseState.RightButton = ButtonState.Pressed And CurrentMouseState.LeftButton = ButtonState.Released Then
            Return MouseButton.Right
        Else
            Return MouseButton.None
        End If
    End Function
End Class
