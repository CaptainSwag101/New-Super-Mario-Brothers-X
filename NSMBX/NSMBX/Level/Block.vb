Public Class Block
    'Public Property ImageAsset As String = ""
    Public Property ImageAsset As Integer
    'Public Property Name As String = ""
    Public Property SrcRect As Rectangle
    Public Property BlockAngle As Byte = 0  ' CAN BE EITHER 0, 12.25, 22.5, OR 45 DEGREES
    Public Property SizeX As Integer = 32
    Public Property SizeY As Integer = 32
    
    ' TILE ANIMATION
    Public IsAnimated As Boolean = False
    Public AniFrames As Integer = 0
    Public AniFrame As Integer = 0
    Public AniSpeed As Integer = 0
    Public AniBase As Integer
    Public Animator As New Animation(Me)
    
    ' TILE ACTION
    Public IsActivated As Boolean = False
    Public IsAir As Boolean = False
    Public IsHitTrigger As Boolean = False
    Public IsLayerDestroyTrigger As Boolean = False
    Public TriggerScript As String = False
    
    ' ADDITIONAL PROPERTIES
    Public Property Tint As Color
    
    Public Sub Animate(X As Integer, Y As Integer, Width As Integer, Height As Integer)
        If AniFrame = 0 Then
            SrcRect = New Rectangle(AniBase, SizeY, Width, Height) ' CAPTURE BLOCK SOURCE SIZE!!!
        Else
            SrcRect = New Rectangle(X + (SizeX * AniFrame), SizeY, Width, Height) ' CAPTURE BLOCK SOURCE SIZE!!!
        End If
    End Sub
End Class
