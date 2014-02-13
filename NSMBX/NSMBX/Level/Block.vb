Public Class Block
    'Public Property ImageAsset As String = ""
    Public Property ImageAsset As Integer
    'Public Property Name As String = ""
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
    Public IsAir As Boolean = True
    Public IsHitTrigger As Boolean = False
    Public IsLayerDestroyTrigger As Boolean = False
    Public TriggerScript As String = False

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
