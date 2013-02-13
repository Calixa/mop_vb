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


Imports Framework.Constants
Imports Framework.Database
Imports Framework.DBC
Imports Framework.Network.Packets
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class CinematicHandler
		Public Shared Sub HandleStartCinematic(ByRef session As WorldClass)
			Dim pChar As Character = session.Character

			Dim startCinematic As New PacketWriter(LegacyMessage.StartCinematic)

			startCinematic.WriteUInt32(DBCStorage.RaceStorage(pChar.Race).CinematicSequence)

			session.Send(startCinematic)

			If pChar.LoginCinematic Then
				DB.Characters.Execute("UPDATE characters SET loginCinematic = 0 WHERE guid = ?", pChar.Guid)
			End If
		End Sub
	End Class
End Namespace
