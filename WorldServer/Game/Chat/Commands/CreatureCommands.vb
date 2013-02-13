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
	Public Class CreatureCommands
		Inherits Globals
		<ChatCommand("addnpc")> _
		Public Shared Sub AddNpc(args As String(), ByRef session As WorldClass)
            Dim pChar As Character = session.Character

			Dim creatureId As Integer = CommandParser.Read(Of Integer)(args, 1)

			Dim creature As Creature = DataMgr.FindCreature(creatureId)
			If creature IsNot Nothing Then
                Dim spawn As New CreatureSpawn() With { _
                    .Guid = CULng(CreatureSpawn.GetLastGuid() + 1), _
                    .Id = creatureId, _
                    .Creature = creature, _
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

		<ChatCommand("delnpc")> _
		Public Shared Sub DeleteNpc(args As String(), ByRef session As WorldClass)
            Dim pChar As Character = session.Character
            Dim spawn As CreatureSpawn = SpawnMgr.FindSpawn(pChar.TargetGuid)

			If spawn IsNot Nothing Then
				SpawnMgr.RemoveSpawn(spawn)

				WorldMgr.SendToInRangeCharacter(pChar, ObjectHandler.HandleObjectDestroy(session, pChar.TargetGuid))
				ChatHandler.SendMessageByType(session, 0, 0, "Selected Spawn successfully removed.")
			Else
				ChatHandler.SendMessageByType(session, 0, 0, "Not a creature.")
			End If
		End Sub
	End Class
End Namespace
