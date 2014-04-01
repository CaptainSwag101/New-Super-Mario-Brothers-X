Public Class LevelScreen
  Inherits BaseScreen
  
  Public Level As LevelBase
  Public NPCList As New List(Of NPC)
  Public Gravity As Integer = 0.05
  Public Friction As Integer = 0.1
  
  Public Sub New(levelPath As String, Me)
	Dim LH As New LevelHandler
	Level = LH.LoadLevel(levelPath)
    
  End Sub
  
  Private Sub NPCChat(Dir As Short)
        Dim V As New Vector2
        V = InvokeVector(Dir)

        If BlockList(V.X, V.Y).Entity IsNot Nothing Then
          ScreenManager.AddScreen(New NPCDialog(BlockList(V.X, V.Y).Entity.Dialog))
        End If
    End Sub

    Public Function InvokeVector(Dir As Short) As Vector2
        Dim V As Vector2

        Select Case Dir
            Case 1 ' DOWN 
                V = New Vector2(PlayerX, PlayerY + 1)
            Case 2 ' LEFT
                V = New Vector2(PlayerX - 1, PlayerY)
            Case 3 ' RIGHT
                V = New Vector2(PlayerX + 1, PlayerY)
            Case 4 ' UP
                V = New Vector2(PlayerX, PlayerY - 1)
        End Select

        Return V
    End Function

    Private Sub ProcessTrigger(TriggerScript As String)
        ' Action|Value|Optional Params
        TriggerActivated = True

        Dim TriggerAction As String = Split(TriggerScript, "|")(0)
        Dim TriggerValue As String = Split(TriggerScript, "|")(1)
        Dim TriggerParams As String = ""

        If Split(TriggerScript, "|").Count > 2 Then
            TriggerParams = Split(TriggerScript, "|")(2)
        End If

        Select Case TriggerAction
            Case "LoadMap"
                Dim MH As New MapHandler
                Map = Nothing
                Map = MH.LoadMap(TriggerValue, Me)
                MapWidth = Map.MapWidth
                MapHeight = Map.MapHeight

                MapX = Map.StartLocation.X
                MapY = Map.StartLocation.Y

                ' OVERRIDE START COORDS
                If TriggerParams <> "" Then
                    MapX = Split(TriggerParams, ":")(0)
                    MapY = Split(TriggerParams, ":")(1)
                End If

                PlayerX = MapX + PlayerScreenX
                PlayerY = MapY + PlayerScreenY
            Case "Teleport"
                'TELEPORT DESTINATION
                If TriggerParams <> "" Then
                    MapX = Split(TriggerParams, ":")(0) - PlayerScreenX
                    MapY = Split(TriggerParams, ":")(1) - PlayerScreenY
                End If

                PlayerX = MapX + PlayerScreenX
                PlayerY = MapY + PlayerScreenY
        End Select
    End Sub
	
	Public Sub Update()
		MyBase.Update()
		
		'If Math.Abs(YSpeed) <> 0 And Level.BlockList(PlayerX, PlayerY - 1).IsBlocked = False Then
			If YSpeed > 0 Then
				YSpeed -= Gravity * (10 ^ FixFloat(YSpeed, Gravity))
			ElseIf YSpeed < -6 Then 
				YSpeed = -6
			End If
		'End If
		
		If Math.Abs(XSpeed) <> 0 Then
			If XSpeed > 0 Then
				XSpeed -= Friction * (10 ^ FixFloat(XSpeed, Friction))
			ElseIf XSpeed < 0 Then
				XSpeed = -(Math.Abs(XSpeed) - Friction * (10 ^ FixFloat(Math.Abs(XSpeed, Friction)))
			End If
		End If
	End Sub
	
	Public Sub Draw()
		
	End Sub
	
	Private Function FixFloat(speed As Float, Mod As Integer) As Integer
		Dim Exponent As Integer = 0
		While (speed - (Mod * (10 ^ Exponent))) = speed
			Exponent += 1
		End While
		Return Exponent
	End Function
End Class
