Public Class Layer
  ' THIS IS A STRIPPED-DOWN VERSION OF LEVELBASE WHICH ONLY HOLDS BLOCK DATA, AND CANNOT BE LOADED AS A STANDALONE LEVEL
  Inherits BaseScreen
  
  Public Name As String
  Public Visible As Boolean = True
  
  Public Sub New(name As String)
    Name = name
  End Sub
End Class
