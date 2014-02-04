Public Class Tile
    Public Property ImageAsset As String = ""
    Public Property Name As String = ""
    Public Property SrcRect As Rectangle

    ' TILE ANIMATION
    Public IsAnimated As Boolean = False
    Public AniFrames As Integer = 0
    Public AniFrame As Integer = 0
    Public AniSpeed As Integer = 0
    Public AniBase As Integer
    Public Animator As New Animation(Me)

    ' TILE ACTION
    Public IsActivated As Boolean = False
    Public IsBlocked As Boolean = False
    Public IsTouchTrigger As Boolean = False
    Public IsStepTrigger As Boolean = False
    Public TriggerScript As String = False

    ' TILE ENTITY
    'Public Property Entity As NPC

    ' ADDITIONAL PROPERTIES
    Public Property Tint As Color

    Public Sub Animate(X As Integer, Y As Integer, Width As Integer, Height As Integer)
        'MsgBox("Test")
        If AniFrame = 0 Then
            SrcRect = New Rectangle(AniBase, Y, Width, Height) ' CAPTURE TILE SOURCE SIZE!!!
        Else
            SrcRect = New Rectangle(X + (16 * AniFrame), Y, Width, Height) ' CAPTURE TILE SOURCE SIZE!!!
        End If
    End Sub
End Class
