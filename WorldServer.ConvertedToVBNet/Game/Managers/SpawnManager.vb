'
' * Copyright (C) 2012-2013 Arctium <http://arctium.org>
' * 
' * This program is free software: you can redistribute it and/or modify
' * it under the terms of the GNU General Public License as published by
' * the Free Software Foundation, either version 3 of the License, or
' * (at your option) any later version.
' *
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' *
' * You should have received a copy of the GNU General Public License
' * along with this program.  If not, see <http://www.gnu.org/licenses/>.
' 


Imports Framework.Database
Imports Framework.Logging
Imports Framework.ObjectDefines
Imports Framework.Singleton
Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports WorldServer.Game.Spawns
Imports WorldServer.Game.WorldEntities

Namespace Game.Managers
	Public NotInheritable Class SpawnManager
		Inherits SingletonBase(Of SpawnManager)
		Public CreatureSpawns As ConcurrentDictionary(Of ULong, CreatureSpawn)
		Public GameObjectSpawns As ConcurrentDictionary(Of ULong, GameObjectSpawn)

		Private Sub New()
			CreatureSpawns = New ConcurrentDictionary(Of ULong, CreatureSpawn)()
			GameObjectSpawns = New ConcurrentDictionary(Of ULong, GameObjectSpawn)()

			Initialize()
		End Sub

		Public Sub Initialize()
			LoadCreatureSpawns()
			LoadGameObjectSpawns()
		End Sub

		Public Function AddSpawn(spawn As CreatureSpawn) As Boolean
			Return CreatureSpawns.TryAdd(spawn.Guid, spawn)
		End Function

		Public Sub RemoveSpawn(spawn As CreatureSpawn)
            Dim removedSpawn As New CreatureSpawn
			CreatureSpawns.TryRemove(spawn.Guid, removedSpawn)

            DB.World.Execute("DELETE FROM creature_spawns WHERE Guid = ?", ObjectGuid.GetGuid(CLng(spawn.Guid)))
		End Sub

		Public Function FindSpawn(guid As ULong) As CreatureSpawn
            Dim spawn As New CreatureSpawn
			CreatureSpawns.TryGetValue(guid, spawn)

			Return spawn
		End Function

		Public Function GetInRangeCreatures(obj As WorldObject) As IEnumerable(Of CreatureSpawn)
            '			Dim list As New List(Of IDictionary)() 
            For Each c As KeyValuePair(Of ULong, CreatureSpawn) In CreatureSpawns
                If Not obj.ToCharacter().InRangeObjects.ContainsKey(c.Key) Then
                    If obj.CheckUpdateDistance(c.Value) Then
						yield Return c.Value
                    End If
                End If
            Next
		End Function
		
		Public Function GetInRangeGameObjects(obj As WorldObject) As IEnumerable(Of GameObjectSpawn)
            For Each g As KeyValuePair(Of ULong, GameObjectSpawn) In GameObjectSpawns
                If Not obj.ToCharacter().InRangeObjects.ContainsKey(g.Key) Then
                    If obj.CheckUpdateDistance(g.Value) Then
						yield Return g.Value
                    End If
                End If
            Next
		End Function

		Public Function GetOutOfRangeCreatures(obj As WorldObject) As IEnumerable(Of CreatureSpawn)
            For Each c As KeyValuePair(Of ULong, CreatureSpawn) In CreatureSpawns
                If obj.ToCharacter().InRangeObjects.ContainsKey(c.Key) Then
                    If Not obj.CheckUpdateDistance(c.Value) Then
						yield Return c.Value
                    End If
                End If
            Next
		End Function

		Public Function GetOutOfRangeGameObjects(obj As WorldObject) As IEnumerable(Of GameObjectSpawn)
            For Each g As KeyValuePair(Of ULong, GameObjectSpawn) In GameObjectSpawns
                If obj.ToCharacter().InRangeObjects.ContainsKey(g.Key) Then
                    If Not obj.CheckUpdateDistance(g.Value) Then
						yield Return g.Value
                    End If
                End If
            Next
		End Function

		Public Sub LoadCreatureSpawns()
			Dim result As SQLResult = DB.World.[Select]("SELECT * FROM creature_spawns")

			For i As Integer = 0 To result.Count - 1
                Dim guid As ULong = result.Read(Of UInt64)(i, "Guid")
                Dim id As Integer = result.Read(Of Int32)(i, "Id")

                Dim data As Creature = Globals.DataMgr.FindCreature(id)
                If data Is Nothing Then
                    Log.Message(LogType.[ERROR], "Loading a creature spawn (Guid: {0}) with non-existing stats (Id: {1}) skipped.", guid, id)
                    Continue For
                End If


                Dim spawn As New CreatureSpawn() With { _
                    .Guid = guid, _
                    .Id = id, _
                    .Map = result.Read(Of UInt32)(i, "Map"), _
                    .Position = New Vector4() With { _
                        .X = result.Read(Of [Single])(i, "X"), _
                        .Y = result.Read(Of [Single])(i, "Y"), _
                        .Z = result.Read(Of [Single])(i, "Z"), _
                        .O = result.Read(Of [Single])(i, "O") _
                    } _
                }

                spawn.CreateFullGuid()
                spawn.CreateData(data)
                spawn.SetUpdateFields()

                AddSpawn(spawn)
            Next

            Log.Message(LogType.DB, "Loaded {0} creature spawns.", CreatureSpawns.Count)
        End Sub

        Public Function AddSpawn(spawn As GameObjectSpawn, ByRef data As GameObject) As Boolean
            Return GameObjectSpawns.TryAdd(spawn.Guid, spawn)
        End Function

        Public Sub RemoveSpawn(spawn As GameObjectSpawn)
            Dim removedGameObject As New GameObjectSpawn
            GameObjectSpawns.TryRemove(spawn.Guid, removedGameObject)

            DB.World.Execute("DELETE FROM creature_spawns WHERE Guid = ?", ObjectGuid.GetGuid(CLng(spawn.Guid)))
        End Sub

        Public Sub LoadGameObjectSpawns()
            Dim result As SQLResult = DB.World.[Select]("SELECT * FROM gameobject_spawns")

            For i As Integer = 0 To result.Count - 1
                Dim guid As ULong = result.Read(Of UInt64)(i, "Guid")
                Dim id As Integer = result.Read(Of Int32)(i, "Id")

                Dim data As GameObject = Globals.DataMgr.FindGameObject(id)
                If data Is Nothing Then
                    Log.Message(LogType.[ERROR], "Loading a gameobject spawn (Guid: {0}) with non-existing stats (Id: {1}) skipped.", guid, id)
                    Continue For
                End If



                Dim spawn As New GameObjectSpawn() With { _
                    .Guid = guid, _
                    .Id = id, _
                    .Map = result.Read(Of UInt32)(i, "Map"), _
                    .Position = New Vector4() With { _
                        .X = result.Read(Of [Single])(i, "X"), _
                        .Y = result.Read(Of [Single])(i, "Y"), _
                        .Z = result.Read(Of [Single])(i, "Z"), _
                        .O = result.Read(Of [Single])(i, "O") _
                    }, _
                    .FactionTemplate = result.Read(Of UInt32)(i, "FactionTemplate"), _
                    .AnimProgress = result.Read(Of [Byte])(i, "AnimProgress"), _
                    .Activated = result.Read(Of Boolean)(i, "Activated") _
                }

                spawn.CreateFullGuid()
                spawn.CreateData(data)
                spawn.SetUpdateFields()

                AddSpawn(spawn, data)
            Next

            Log.Message(LogType.DB, "Loaded {0} gameobject spawns.", GameObjectSpawns.Count)
        End Sub
	End Class
End Namespace
