Public Class Item
    ' TEXTURE SOURCE
    Public ImageAsset As String = "Items"
    Public sRect As Rectangle

    Public Item As ItemClass
    Public Name As String
    Public Description As String
    Public IsStackable As Boolean
    Public Count As Integer
    Public IsOneUse As Boolean = True


End Class

Public Enum ItemClass
    Weapon
    Useable
    Special
End Enum