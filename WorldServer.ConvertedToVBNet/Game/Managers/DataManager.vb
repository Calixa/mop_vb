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
Imports Framework.Singleton
Imports WorldServer.Game.ObjectDefines
Imports WorldServer.Game.WorldEntities
Imports System.Collections.Concurrent

Namespace Game.Managers
	Public Class DataManager
		Inherits SingletonBase(Of DataManager)
		Private Creatures As ConcurrentDictionary(Of Int32, Creature)
		Private GameObjects As ConcurrentDictionary(Of Int32, GameObject)

		Private Sub New()
			Creatures = New ConcurrentDictionary(Of Int32, Creature)()
			GameObjects = New ConcurrentDictionary(Of Int32, GameObject)()

			Initialize()
		End Sub

		Public Function Add(creature As Creature) As Boolean
			Return Creatures.TryAdd(creature.Stats.Id, creature)
		End Function

		Public Function Remove(creature As Creature) As Creature
            Dim removedCreature As New Creature
			Creatures.TryRemove(creature.Stats.Id, removedCreature)

			Return removedCreature
		End Function

		Public Function GetCreatures() As ConcurrentDictionary(Of Int32, Creature)
			Return Creatures
		End Function

		Public Function FindCreature(id As Integer) As Creature
            Dim creature As New Creature
			Creatures.TryGetValue(id, creature)

			Return creature
		End Function

		Public Sub LoadCreatureData()
			Dim result As SQLResult = DB.World.[Select]("SELECT cs.Id FROM creature_stats cs LEFT JOIN creature_data cd ON cs.Id = cd.Id WHERE cd.Id IS NULL")

			If result.Count <> 0 Then
                Dim missingIds As Object() = result.ReadAllValuesFromField("Id")
				DB.World.ExecuteBigQuery("creature_data", "Id", 1, result.Count, missingIds)

				Log.Message(LogType.DB, "Added {0} default data definition for creatures.", missingIds.Length)
			End If

			result = DB.World.[Select]("SELECT * FROM creature_stats cs RIGHT JOIN creature_data cd ON cs.Id = cd.Id WHERE cs.id IS NOT NULL")

			For r As Integer = 0 To result.Count - 1
				Dim Stats As New CreatureStats() With { _
					.Id = result.Read(Of Int32)(r, "Id"), _
					.Name = result.Read(Of [String])(r, "Name"), _
					.SubName = result.Read(Of [String])(r, "SubName"), _
					.IconName = result.Read(Of [String])(r, "IconName"), _
					.Type = result.Read(Of Int32)(r, "Type"), _
					.Family = result.Read(Of Int32)(r, "Family"), _
					.Rank = result.Read(Of Int32)(r, "Rank"), _
					.HealthModifier = result.Read(Of [Single])(r, "HealthModifier"), _
					.PowerModifier = result.Read(Of [Single])(r, "PowerModifier"), _
					.RacialLeader = result.Read(Of [Byte])(r, "RacialLeader"), _
					.MovementInfoId = result.Read(Of Int32)(r, "MovementInfoId"), _
					.ExpansionRequired = result.Read(Of Int32)(r, "ExpansionRequired") _
				}

				For i As Integer = 0 To Stats.Flag.Capacity - 1
					Stats.Flag.Add(result.Read(Of Int32)(r, "Flag", i))
				Next

				For i As Integer = 0 To Stats.QuestKillNpcId.Capacity - 1
					Stats.QuestKillNpcId.Add(result.Read(Of Int32)(r, "QuestKillNpcId", i))
				Next

				For i As Integer = 0 To Stats.DisplayInfoId.Capacity - 1
					Stats.DisplayInfoId.Add(result.Read(Of Int32)(r, "DisplayInfoId", i))
				Next

				For i As Integer = 0 To Stats.QuestItemId.Capacity - 1
					Stats.QuestItemId.Add(result.Read(Of Int32)(r, "QuestItemId", i))
				Next


				Add(New Creature() With { _
					.Data = New CreatureData() With { _
						.Health = result.Read(Of Int32)(r, "Health"), _
						.Level = result.Read(Of [Byte])(r, "Level"), _
						.[Class] = result.Read(Of [Byte])(r, "Class"), _
						.Faction = result.Read(Of Int32)(r, "Faction"), _
						.Scale = result.Read(Of Int32)(r, "Scale"), _
						.UnitFlags = result.Read(Of Int32)(r, "UnitFlags"), _
						.UnitFlags2 = result.Read(Of Int32)(r, "UnitFlags2"), _
						.NpcFlags = result.Read(Of Int32)(r, "NpcFlags") _
					}, _
					.Stats = Stats _
				})
			Next

			Log.Message(LogType.DB, "Loaded {0} creatures.", Creatures.Count)
		End Sub

		Public Function Add(gameobject As GameObject) As Boolean
			Return GameObjects.TryAdd(gameobject.Stats.Id, gameobject)
		End Function

		Public Function Remove(gameobject As GameObject) As GameObject
            Dim removedGameObject As New GameObject
			GameObjects.TryRemove(gameobject.Stats.Id, removedGameObject)

			Return removedGameObject
		End Function

		Public Function GetGameObjects() As ConcurrentDictionary(Of Int32, GameObject)
			Return GameObjects
		End Function

		Public Function FindGameObject(id As Integer) As GameObject
            Dim gameObject As New GameObject
			GameObjects.TryGetValue(id, gameObject)

			Return gameObject
		End Function

		Public Sub LoadGameObject()
			Dim result As SQLResult = DB.World.[Select]("SELECT * FROM gameobject_stats")

			For r As Integer = 0 To result.Count - 1
				Dim Stats As New GameObjectStats() With { _
					.Id = result.Read(Of Int32)(r, "Id"), _
					.Type = result.Read(Of Int32)(r, "Type"), _
					.DisplayInfoId = result.Read(Of Int32)(r, "DisplayInfoId"), _
					.Name = result.Read(Of [String])(r, "Name"), _
					.IconName = result.Read(Of [String])(r, "IconName"), _
					.CastBarCaption = result.Read(Of [String])(r, "CastBarCaption"), _
					.Size = result.Read(Of [Single])(r, "Size"), _
					.ExpansionRequired = result.Read(Of Int32)(r, "ExpansionRequired") _
				}

				For i As Integer = 0 To Stats.Data.Capacity - 1
					Stats.Data.Add(result.Read(Of Int32)(r, "Data", i))
				Next

				For i As Integer = 0 To Stats.QuestItemId.Capacity - 1
					Stats.QuestItemId.Add(result.Read(Of Int32)(r, "QuestItemId", i))
				Next

				Add(New GameObject() With { _
					.Stats = Stats _
				})
			Next

			Log.Message(LogType.DB, "Loaded {0} gameobjects.", GameObjects.Count)
		End Sub

		Public Sub Initialize()
			LoadCreatureData()
			LoadGameObject()
		End Sub
	End Class
End Namespace
