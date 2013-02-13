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
	Public Class GameObject
		Public Stats As GameObjectStats

		Public Sub New()
		End Sub
		Public Sub New(id As Integer)
			Dim result As SQLResult = DB.World.[Select]("SELECT * FROM gameobject_stats WHERE id = ?", id)

			If result.Count <> 0 Then
				Stats = New GameObjectStats()

				Stats.Id = result.Read(Of Int32)(0, "Id")
				Stats.Type = result.Read(Of Int32)(0, "Type")
				Stats.Flags = result.Read(Of Int32)(0, "Flags")

				Stats.DisplayInfoId = result.Read(Of Int32)(0, "DisplayInfoId")
				Stats.Name = result.Read(Of [String])(0, "Name")
				Stats.IconName = result.Read(Of [String])(0, "IconName")
				Stats.CastBarCaption = result.Read(Of [String])(0, "CastBarCaption")

				For i As Integer = 0 To Stats.Data.Capacity - 1
					Stats.Data.Add(result.Read(Of Int32)(0, "Data", i))
				Next

				Stats.Size = result.Read(Of [Single])(0, "Size")

				For i As Integer = 0 To Stats.QuestItemId.Capacity - 1
					Stats.QuestItemId.Add(result.Read(Of Int32)(0, "QuestItemId", i))
				Next

				Stats.ExpansionRequired = result.Read(Of Int32)(0, "ExpansionRequired")
			End If
		End Sub
	End Class
End Namespace
