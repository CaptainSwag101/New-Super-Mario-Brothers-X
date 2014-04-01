Imports System.IO
Imports System.IO.Compression

Public Class LevelHandler
    Private NewLevel As LevelBase
    Private LevelName As String = ""
    'Private LevelAssets As New List(Of ImageAsset)

    Public Function LoadLevel(path As String, screen As LevelScreen) As LevelBase
        Using fStream As FileStream = New FileStream(path & ".lvl", FileMode.Open)
            Using zipStream As GZipStream = New GZipStream(fStream, CompressionMode.Decompress)
                Using reader As New BinaryReader(zipStream)
                    Dim Name As String = reader.ReadString
                    Dim mWidth As Integer = reader.ReadInt32
                    Dim mHeight As Integer = reader.ReadInt32
                    Dim xStart As Single = reader.ReadSingle
                    Dim yStart As Single = reader.ReadSingle
                    Dim NPCCount As Integer = reader.ReadInt32

                    NewLevel = New LevelBase(Name, mWidth, mHeight, New Vector2(xStart, yStart))
                    NewLevel.LevelName = Name
                    NewLevel.StartLocation.X = xStart
                    NewLevel.StartLocation.Y = yStart
                    NewLevel.NPCCount = NPCCount
                    'Game1.StartX = NewLevel.StartLocation.X + 8
                    'Game1.StartY = NewLevel.StartLocation.Y + 6

                    ' LOAD Level DATA
                    For X = 0 To NewLevel.LevelWidth
                        For Y = 0 To NewLevel.LevelHeight
                            NewLevel.BlockList(X, Y) = New Block
                            Dim srcBase As Integer = reader.ReadInt32
                            Dim SrcX As Integer = reader.ReadInt32
                            Dim srcY As Integer = reader.ReadInt32

                            With NewLevel.BlockList(X, Y)
                                .Name = reader.ReadString
                                .ImageAsset = reader.ReadString
                                .SrcRect = New Rectangle(srcBase, srcY, Globals.BlockSize, Globals.BlockSize)
                                .IsBlocked = reader.ReadBoolean
                                .IsStepTrigger = reader.ReadBoolean
                                .TriggerScript = reader.ReadString
                                .IsAnimated = reader.ReadBoolean
                                .AniFrames = reader.ReadInt32
                                .AniSpeed = reader.ReadInt32
                                .AniBase = srcBase
                            End With

                            ' LOAD ANIMATED BlockS
                            If NewLevel.BlockList(X, Y).IsAnimated = True Then
                                NewLevel.AnimationList.Add(NewLevel.BlockList(X, Y))
                            End If

                        Next
                    Next

                    ' LOAD NPC DATA
                    For n = 0 To NPCCount - 1
                        'Dim npc As New NPC
                        'With npc
                        '.ImageAlias = reader.ReadString
                        '.Name = reader.ReadString
                        '.Dialog = reader.ReadString
                        '.X = reader.ReadInt32
                        '.Y = reader.ReadInt32
                        '.IsVendor = reader.ReadBoolean
                        '.MoveSpeed = reader.ReadInt32
                        'End With
                        'NewLevel.BlockList(npc.X, npc.Y).Entity = npc
                        'NewLevel.NPCList.Add(npc)
                    Next

                    ' LOAD IMAGE ASSETS
                    ' RETRIEVE IMAGE COUNT
                    Dim ImageCount As Integer = reader.ReadInt32

                    For img = 0 To ImageCount - 1
                        'RETRIEVE ASSET NAME / ID
                        Dim AssetName As String = reader.ReadString
                        Dim AssetType As String = reader.ReadString

                        'CREATE FILE STREAMS FOR IMAGE DUMP
                        If Directory.Exists("tmp") = False Then
                            Directory.CreateDirectory("tmp")
                        End If

                        Dim FS As New FileStream("tmp/" & AssetName & ".png", FileMode.Create)
                        Dim BW As New BinaryWriter(FS)

                        '   RETRIEVE FILE LENGTH
                        Dim FileLength As Integer = reader.ReadInt32

                        '   TO DO: ADD ASSET NAME!!!!

                        '   REMOVE HEADER NULLS - BUG FIX
                        For i = 0 To 3
                            Dim dump As Byte = reader.ReadByte
                        Next

                        '   RETRIEVE TEXTURE
                        For B = 0 To FileLength - 1
                            Dim val As Byte = reader.ReadByte
                            BW.Write(val)
                        Next
                        BW = Nothing
                        FS.Close()

                        '   IMPORT TEXTURE
                        ImportTexture("tmp/" & AssetName & ".png", AssetType)
                    Next

                    reader.Close()
                End Using
                zipStream.Close()
            End Using
            fStream.Close()
        End Using

        'MsgBox("SUCCESSFUL LOAD!!" & LevelScreen.Level.BlockList(22, 22).IsBlocked)

        Return NewLevel
    End Function

    Public Sub SaveLevel(Level As LevelBase, LevelName As String)
        Using fStream As FileStream = New FileStream(LevelName & ".lvl", FileMode.Create)
            Using zipStream As GZipStream = New GZipStream(fStream, CompressionMode.Compress)
                Using writer As New BinaryWriter(zipStream)
                    writer.Write(LevelName)
                    writer.Write(Level.LevelWidth)
                    writer.Write(Level.LevelHeight)
                    writer.Write(Level.StartLocation.X)
                    writer.Write(Level.StartLocation.Y)

                    For X = 0 To Level.LevelWidth
                        For Y = 0 To Level.LevelHeight
                            writer.Write(Level.BlockList(X, Y).SrcRect.X)
                            writer.Write(Level.BlockList(X, Y).SrcRect.Y)
                            writer.Write(Level.BlockList(X, Y).IsBlocked)
                            writer.Write(Level.BlockList(X, Y).IsStepTrigger)
                            writer.Write(Level.BlockList(X, Y).TriggerScript)
                        Next
                    Next
                    writer.Close()
                End Using
                zipStream.Close()
            End Using
            fStream.Close()
        End Using
    End Sub

    Public Sub ImportTexture(Path As String, AssetType As String)
        'Dim t2d As New ImageAsset
        Dim FS As New FileStream(Path, FileMode.Open)
        Dim Name As String = Path
        Name = Replace(Name, "tmp/", "")
        Name = Replace(Name, ".png", "")

        'With t2d
        '.Name = Name
        '.Type = AssetType
        '.Image = Texture2D.FromStream(Globals.Graphics.GraphicsDevice, FS)
        'End With

        'Textures.ImageAssets.Add(t2d)
        FS.Close()

        'Dim FSys As New FileInfo(Path)
        'FSys.Delete()
    End Sub

    Public Sub TextureToPNG() 'Image As ImageAsset)
        ' CHECK FOR TEMP DIRECTORY
        If Directory.Exists("tmp") = False Then
            Directory.CreateDirectory("tmp")
        End If

        'Dim FS As New FileStream("tmp/" & Image.Name & ".png", FileMode.Create)
        'Image.Image.SaveAsPng(FS, Image.Image.Width, Image.Image.Height)
        'FS.Close()
    End Sub

    Public Sub DeleteTempFile(FileName As String)
        'Dim FI As New FileInfo("tmp/" & FileName)
        'FI.Delete()
    End Sub
End Class
