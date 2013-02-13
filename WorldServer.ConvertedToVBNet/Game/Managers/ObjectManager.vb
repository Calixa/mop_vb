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
Imports Framework.ObjectDefines
Imports Framework.Singleton
Imports System.Collections.Generic
Imports WorldServer.Game.WorldEntities

Namespace Game.Managers
	Public NotInheritable Class ObjectManager
		Inherits SingletonBase(Of ObjectManager)
		Private objectList As Dictionary(Of UInt64, WorldObject)

		Private Sub New()
			objectList = New Dictionary(Of UInt64, WorldObject)()
		End Sub

		Public Function FindObject(guid As UInt64) As WorldObject
			For Each kvp As KeyValuePair(Of UInt64, WorldObject) In objectList
				If kvp.Key = guid Then
					Return kvp.Value
				End If
			Next

			Return Nothing
		End Function

		Public Sub SetPosition(ByRef pChar As Character, vector As Vector4, Optional dbUpdate As Boolean = True)
			pChar.Position = vector

			Globals.WorldMgr.Sessions(pChar.Guid).Character = pChar

			If dbUpdate Then
				SavePositionToDB(pChar)
			End If
		End Sub

		Public Sub SetMap(ByRef pChar As Character, mapId As UInteger, Optional dbUpdate As Boolean = True)

			pChar.Map = mapId

			Globals.WorldMgr.Sessions(pChar.Guid).Character = pChar

			If dbUpdate Then
				SavePositionToDB(pChar)
			End If
		End Sub

		Public Sub SetZone(ByRef pChar As Character, zoneId As UInteger, Optional dbUpdate As Boolean = True)
			pChar.Zone = zoneId

			Globals.WorldMgr.Sessions(pChar.Guid).Character = pChar

			If dbUpdate Then
				SaveZoneToDB(pChar)
			End If
		End Sub

		Public Sub SavePositionToDB(pChar As Character)
			DB.Characters.Execute("UPDATE characters SET x = ?, y = ?, z = ?, o = ?, map = ? WHERE guid = ?", pChar.Position.X, pChar.Position.Y, pChar.Position.Z, pChar.Position.O, pChar.Map, _
				pChar.Guid)
		End Sub

		Public Sub SaveZoneToDB(pChar As Character)
			DB.Characters.Execute("UPDATE characters SET zone = ? WHERE guid = ?", pChar.Zone, pChar.Guid)
		End Sub
	End Class
End Namespace
