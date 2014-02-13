Public Class Player
    Public CurrentForm As Powerup
    Public LastForm As Powerup
    Public ReserveItem As NPC
    
    Public Sub New()
        If Globals.NewGame
    End Sub
    
End Class

Public Enum Powerup
    None
    Mushroom
    Fire
    Leaf
    Tanooki
    Statue
    Hammer
    Ice
End Enum
