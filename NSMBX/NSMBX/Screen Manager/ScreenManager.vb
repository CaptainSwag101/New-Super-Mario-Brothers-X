Public Enum ScreenState
    Active
    Shutdown
    Hidden
End Enum

Public Class ScreenManager
    Private Shared Screens As New List(Of BaseScreen)
    Private Shared NewScreens As New List(Of BaseScreen)
    ' CREATE DEBUG SCREEN
    Private DebugScreen As New Debug()

    Public Sub New()
        AddScreen(DebugScreen)
    End Sub

    Public Sub Update()

        DebugScreen.Screens = "Screens: "

        ' GENERATE LIST OF DEAD SCREENS FOR REMOVAL
        Dim RemoveScreens As New List(Of BaseScreen)

        For Each screen As BaseScreen In Screens
            If screen.State = ScreenState.Shutdown Then
                RemoveScreens.Add(screen)
            Else
                DebugScreen.Screens += screen.Name + ", "
                screen.Focused = False
            End If
        Next

        ' REMOVE DEAD SCREENS
        For Each screen As BaseScreen In RemoveScreens
            Screens.Remove(screen)
        Next

        ' ADD NEW SCREENS TO MANAGER LIST
        For Each screen As BaseScreen In NewScreens
            Screens.Add(screen)
        Next
        NewScreens.Clear()

        ' RESET DEBUG SCREEN TO TOP OF THE LIST
        Screens.Remove(DebugScreen)
        Screens.Add(DebugScreen)

        ' CHECK SCREEN FOCUS
        If Screens.Count > 0 Then
            For i = screens.Count - 1 To 0 Step -1
                If Screens(i).GrabFocus Then
                    Screens(i).Focused = True
                    DebugScreen.FocusScreen = "Focused Screen: " + Screens(i).Name
                    Exit For
                End If
            Next
        Else
            DebugScreen.FocusScreen = "Focused Screen: "
        End If

        ' HANDLE INPUT FOR FOCUSED SCREEN
        For Each screen As BaseScreen In Screens
            If Globals.WindowFocused Then ' MAKING SURE THE GAME WINDOW HAS FOCUS
                screen.HandleInput()
            End If
            screen.Update()
        Next
    End Sub

    Public Sub Draw()
        For Each screen As BaseScreen In Screens
            If screen.State = ScreenState.Active Then
                screen.Draw()
            End If
        Next
    End Sub

    Public Shared Sub AddScreen(screen As BaseScreen)
        NewScreens.Add(screen)
    End Sub

    Public Shared Sub UnloadScreen(screen As String)
        For Each FoundScreen As BaseScreen In Screens
            If FoundScreen.Name = screen Then
                FoundScreen.Unload()
                Exit For
            End If
        Next
    End Sub
End Class
