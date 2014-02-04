Public Class Globals
    Public Shared Content As ContentManager
    Public Shared Graphics As GraphicsDeviceManager
    Public Shared SpriteBatch As SpriteBatch
    Public Shared GameTime As GameTime
    Public Shared WindowFocused As Boolean
    Public Shared GameSize As Vector2
    Public Shared BackBuffer As RenderTarget2D
    Public Shared DebugEnabled As Boolean = False
    Public Shared InDialog As Boolean = False

    ' TILE DEFAULTS
    Public Shared TileSize As Integer = 16 ' SOURCE SIZE FOR TILE SET

    ' SETUP PLAYER
    Public Shared Player1 As New Player

    Public Shared GameRandomizer As Random
End Class
