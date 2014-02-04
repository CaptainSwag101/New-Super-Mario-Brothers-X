Public Class Backpack
    Public ItemMax As Integer = 9 ' NUMBER OF INVENTORY SLOTS
    Public Items As New List(Of Item)

    Public Sub Add(NewItem As Item)
        If Items.Count <= ItemMax Then
            For Each i As Item In Items
                If i.Name = NewItem.Name Then
                    If i.IsStackable = True And i.Count < 16 Then
                        i.Count += 1
                        Exit Sub
                    Else
                        Items.Add(NewItem)
                        Exit Sub
                    End If
                End If
            Next

            Items.Add(NewItem)
        End If
    End Sub

    Public Sub Remove(ItemID As Item)
        If Items.Item(Items.IndexOf(ItemID)).Count > 0 Then
            Items.Item(Items.IndexOf(ItemID)).Count -= 1
        Else
            Items.RemoveAt(Items.IndexOf(ItemID))
        End If
    End Sub
End Class
