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
Imports Framework.Network.Packets
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class LogoutHandler
		Inherits Globals
		<Opcode(ClientMessage.Logout, "16357")> _
		Public Shared Sub HandleLogoutComplete(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim pChar as Worldentities.Character = session.Character

			ObjectMgr.SavePositionToDB(pChar)

			Dim logoutComplete As New PacketWriter(LegacyMessage.LogoutComplete)
			session.Send(logoutComplete)

			' Destroy object after logout
			Dim objectDestroy As New PacketWriter(LegacyMessage.ObjectDestroy)

			objectDestroy.WriteUInt64(pChar.Guid)
			objectDestroy.WriteUInt8(0)

			WorldMgr.SendToInRangeCharacter(pChar, objectDestroy)
			WorldMgr.DeleteSession(pChar.Guid)
		End Sub
	End Class
End Namespace
