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
Imports Framework.Logging
Imports Framework.Network.Packets
Imports WorldServer.Network
Imports Framework.Database
Imports Framework.ObjectDefines

Namespace Game.Packets.PacketHandler
	Public Class MiscHandler
		Inherits Globals
		Public Shared Sub HandleMessageOfTheDay(ByRef session As WorldClass)
			Dim motd As New PacketWriter(LegacyMessage.MessageOfTheDay)
			motd.WriteUInt32(3)

			motd.WriteCString("Arctium MoP test")
			motd.WriteCString("Welcome to our MoP server test.")
			motd.WriteCString("Your development team =)")
			session.Send(motd)
		End Sub

		<Opcode(ClientMessage.Ping, "16357")> _
		Public Shared Sub HandlePong(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim sequence As UInteger = packet.ReadUInt32()
			Dim latency As UInteger = packet.ReadUInt32()

			Dim pong As New PacketWriter(JAMCCMessage.Pong)
			pong.WriteUInt32(sequence)

			session.Send(pong)
		End Sub

		<Opcode(ClientMessage.LogDisconnect, "16357")> _
		Public Shared Sub HandleDisconnectReason(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character
			Dim disconnectReason As UInteger = packet.ReadUInt32()

			If pChar IsNot Nothing Then
				WorldMgr.DeleteSession(pChar.Guid)
			End If

			DB.Realms.Execute("UPDATE accounts SET online = 0 WHERE id = ?", session.Account.Id)

			Log.Message(LogType.DEBUG, "Account with Id {0} disconnected. Reason: {1}", session.Account.Id, disconnectReason)
		End Sub

		Public Shared Sub HandleUpdateClientCacheVersion(ByRef session As WorldClass)
			Dim cacheVersion As New PacketWriter(LegacyMessage.UpdateClientCacheVersion)

			cacheVersion.WriteUInt32(0)

			session.Send(cacheVersion)
		End Sub

		<Opcode(ClientMessage.LoadingScreenNotify, "16357")> _
		Public Shared Sub HandleLoadingScreenNotify(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)

			Dim mapId As UInteger = packet.ReadUInt32()
			Dim loadingScreenState As Boolean = BitUnpack.GetBit()

			Log.Message(LogType.DEBUG, "Loading screen for map '{0}' is {1}.", mapId, If(loadingScreenState, "enabled", "disabled"))
		End Sub

		<Opcode(ClientMessage.ViolenceLevel, "16357")> _
		Public Shared Sub HandleViolenceLevel(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim violenceLevel As Byte = packet.ReadUInt8()

			Log.Message(LogType.DEBUG, "Violence level from account '{0} (Id: {1})' is {2}.", session.Account.Name, session.Account.Id, CType(violenceLevel, ViolenceLevel))
		End Sub

		<Opcode(ClientMessage.ActivePlayer, "16357")> _
		Public Shared Sub HandleActivePlayer(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim active As Byte = packet.ReadUInt8()
			' Always 0
			Log.Message(LogType.DEBUG, "Player {0} (Guid: {1}) is active.", session.Character.Name, session.Character.Guid)
		End Sub

		<Opcode(ClientMessage.ZoneUpdate, "16357")> _
		Public Shared Sub HandleZoneUpdate(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

			Dim zone As UInteger = packet.ReadUInt32()

			ObjectMgr.SetZone(pChar, zone)
		End Sub

		<Opcode(ClientMessage.SetSelection, "16357")> _
		Public Shared Sub HandleSetSelection(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim guidMask As Byte() = {3, 1, 7, 2, 6, 4, _
				0, 5}
			Dim guidBytes As Byte() = {4, 1, 5, 2, 6, 7, _
				0, 3}

			Dim GuidUnpacker As New BitUnpack(packet)

			Dim fullGuid As ULong = GuidUnpacker.GetGuid(guidMask, guidBytes)
            Dim guid As ULong = ObjectGuid.GetGuid(CLng(fullGuid))

            Dim sess As WorldClass = WorldMgr.GetSession(session.Character.Guid)
			sess.Character.TargetGuid = fullGuid

			If guid = 0 Then
				Log.Message(LogType.DEBUG, "Character (Guid: {0}) removed current selection.", session.Character.Guid)
			Else
                Log.Message(LogType.DEBUG, "Character (Guid: {0}) selected a {1} (Guid: {2}, Id: {3}).", session.Character.Guid, ObjectGuid.GetGuidType(fullGuid), guid, ObjectGuid.GetId(CLng(fullGuid)))
			End If
		End Sub
	End Class
End Namespace
