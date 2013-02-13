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
Imports WorldServer.Game.ObjectDefines

Namespace Game.WorldEntities
	Public Class Creature
		Public Stats As CreatureStats
		Public Data As CreatureData

		Public Sub New()
		End Sub
		Public Sub New(id As Integer)
			Dim result As SQLResult = DB.World.[Select]("SELECT * FROM creature_stats WHERE id = ?", id)

			If result.Count <> 0 Then
				Stats = New CreatureStats()

				Stats.Id = result.Read(Of Int32)(0, "Id")
				Stats.Name = result.Read(Of [String])(0, "Name")
				Stats.SubName = result.Read(Of [String])(0, "SubName")
				Stats.IconName = result.Read(Of [String])(0, "IconName")

				For i As Integer = 0 To Stats.Flag.Capacity - 1
					Stats.Flag.Add(result.Read(Of Int32)(0, "Flag", i))
				Next

				Stats.Type = result.Read(Of Int32)(0, "Type")
				Stats.Family = result.Read(Of Int32)(0, "Family")
				Stats.Rank = result.Read(Of Int32)(0, "Rank")

				For i As Integer = 0 To Stats.QuestKillNpcId.Capacity - 1
					Stats.QuestKillNpcId.Add(result.Read(Of Int32)(0, "QuestKillNpcId", i))
				Next

				For i As Integer = 0 To Stats.DisplayInfoId.Capacity - 1
					Stats.DisplayInfoId.Add(result.Read(Of Int32)(0, "DisplayInfoId", i))
				Next

				Stats.HealthModifier = result.Read(Of [Single])(0, "HealthModifier")
				Stats.PowerModifier = result.Read(Of [Single])(0, "PowerModifier")
				Stats.RacialLeader = result.Read(Of [Byte])(0, "RacialLeader")

				For i As Integer = 0 To Stats.QuestItemId.Capacity - 1
					Stats.QuestItemId.Add(result.Read(Of Int32)(0, "QuestItemId", i))
				Next

				Stats.MovementInfoId = result.Read(Of Int32)(0, "MovementInfoId")
				Stats.ExpansionRequired = result.Read(Of Int32)(0, "ExpansionRequired")
			End If

			result = DB.World.[Select]("SELECT * FROM creature_data WHERE id = ?", id)

			If result.Count <> 0 Then
				Data = New CreatureData()

				Data.Health = result.Read(Of Int32)(0, "Health")
				Data.Level = result.Read(Of [Byte])(0, "Level")
				Data.[Class] = result.Read(Of [Byte])(0, "Class")
				Data.Faction = result.Read(Of Int32)(0, "Faction")
				Data.Scale = result.Read(Of Int32)(0, "Scale")
				Data.UnitFlags = result.Read(Of Int32)(0, "UnitFlags")
				Data.UnitFlags2 = result.Read(Of Int32)(0, "UnitFlags2")
				Data.NpcFlags = result.Read(Of Int32)(0, "NpcFlags")
			End If
		End Sub
	End Class
End Namespace
