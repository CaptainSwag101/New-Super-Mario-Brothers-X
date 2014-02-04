Public Class Game1
    Inherits Microsoft.Xna.Framework.Game
    Private ScreenManager As ScreenManager
    Private SoundManager As SoundManager


    Public Sub New()
        Globals.Graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"

        Globals.GameRandomizer = New Random

#If CONFIG = "Debug" Then
        Globals.DebugEnabled = True
#End If

#If CONFIG = "Release" Then
        Globals.DebugEnabled = False
#End If

    End Sub

    Protected Overrides Sub Initialize()
        MyBase.Initialize()

        Me.IsMouseVisible = True

        Window.AllowUserResizing = True

        ' SET GAME RESOLUTION (SNES)
        Globals.GameSize = New Vector2(512, 448)
        Globals.Graphics.PreferredBackBufferWidth = Globals.GameSize.X
        Globals.Graphics.PreferredBackBufferHeight = Globals.GameSize.Y
        'Globals.Graphics.SynchronizeWithVerticalRetrace = False ' ***** REMOVE THIS LINE TO LOCK FPS *****
        Globals.Graphics.ApplyChanges()
        'Me.IsFixedTimeStep = False ' ***** REMOVE THIS LINE TO LOCK FPS *****

        ' MAKE BACKBUFFER A RENDER TARGET
        Globals.BackBuffer = New RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.GameSize.X, Globals.GameSize.Y, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)


    End Sub

    Protected Overrides Sub LoadContent()
        Globals.SpriteBatch = New SpriteBatch(GraphicsDevice)
        Globals.Content = Me.Content
        ' LOAD TEXTURES, FONTS, AND SOUNDS HERE
        Textures.Load()
        Fonts.Load()
        'Sounds.Load()

        ' ADD DEFAULT SCREENS
        ScreenManager = New ScreenManager()
        ScreenManager.AddScreen(New TitleScreen())
        ScreenManager.AddScreen(New MainMenu())


    End Sub

    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        MyBase.Update(gameTime)

        Globals.WindowFocused = Me.IsActive
        Globals.GameTime = gameTime

        ' UPDATE OUR SCREENS
        ScreenManager.Update()

        ' CHECK INPUT
        Input.Update()
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        ' SET THE RENDER TARGET TO THE BACKBUFFER
        Globals.Graphics.GraphicsDevice.SetRenderTarget(Globals.BackBuffer)
        GraphicsDevice.Clear(Color.Black)
        MyBase.Draw(gameTime)

        'DRAW SCREEN MANABGER CONTENTS
        ScreenManager.Draw()
        Globals.Graphics.GraphicsDevice.SetRenderTarget(Nothing)

        ' DRAW BACKBUFFER TO SCREEN
        Globals.SpriteBatch.Begin()
        Globals.SpriteBatch.Draw(Globals.BackBuffer, New Rectangle(0, 0, Globals.Graphics.GraphicsDevice.Viewport.Width, Globals.Graphics.GraphicsDevice.Viewport.Height), Color.White)

        'TEST SPRITEFONT
        'Globals.SpriteBatch.DrawString(Fonts.Georgia_16, "HELLO WORLD", New Vector2(20, 75), Color.White)
        'Globals.SpriteBatch.DrawString(Fonts.Verdana_8, "HELLO WORLD", New Vector2(20, 100), Color.White)

        ' TEST SPRITE DRAWING
        'Globals.SpriteBatch.Draw(Textures.RadAvatar, New Rectangle(20, 150, 32, 32), New Rectangle(0, 0, 32, 32), Color.White)

        Globals.SpriteBatch.End()
    End Sub

End Class
