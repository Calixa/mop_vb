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


Imports Framework.Console
Imports WorldServer.Game.Packets.PacketHandler
Imports WorldServer.Game.Spawns
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Chat.Commands
	Public Class GameObjectCommands
		Inherits Globals
		<ChatCommand("addobject")> _
		Public Shared Sub AddObject(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

			Dim objectId As Integer = CommandParser.Read(Of Integer)(args, 1)

			Dim gObject As GameObject = DataMgr.FindGameObject(objectId)
			If gObject IsNot Nothing Then
                Dim spawn As New GameObjectSpawn() With { _
                     .Guid = CULng(GameObjectSpawn.GetLastGuid() + 1), _
                     .Id = objectId, _
                     .GameObject = gObject, _
                     .Position = pChar.Position, _
                     .Map = pChar.Map _
                }

				If spawn.AddToDB() Then
					spawn.AddToWorld()
					ChatHandler.SendMessageByType(session, 0, 0, "Spawn successfully added.")
				Else
					ChatHandler.SendMessageByType(session, 0, 0, "Spawn can't be added.")
				End If
			End If
		End Sub
	End Class
End Namespace
