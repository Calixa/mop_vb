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
	Public Class TimeHandler
		Inherits Globals
		<Opcode(ClientMessage.ReadyForAccountDataTimes, "16357")> _
		Public Shared Sub HandleAccountDataInitialized(ByRef packet As PacketReader, ByRef session As WorldClass)
			WorldMgr.WriteAccountData(AccountDataMasks.GlobalCacheMask, session)
		End Sub

		<Opcode(ClientMessage.RequestUITime, "")> _
		Public Shared Sub HandleUITime(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim uiTime As New PacketWriter(LegacyMessage.UITime)

			uiTime.WriteUnixTime()

			session.Send(uiTime)
		End Sub

		<Opcode(ClientMessage.SetRealmSplitState, "16357")> _
		Public Shared Sub HandleRealmSplitStateResponse(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim realmSplitState As UInteger = 0

			Dim realmSplitStateResp As New PacketWriter(LegacyMessage.RealmSplitStateResponse)

			realmSplitStateResp.WriteUInt32(packet.ReadUInt32())
			realmSplitStateResp.WriteUInt32(realmSplitState)
			realmSplitStateResp.WriteCString("01/01/01")

			session.Send(realmSplitStateResp)

			' Crash!!!
			' Wrong data sent...
			' AddonMgr.WriteAddonData(ref session);
		End Sub
	End Class
End Namespace
