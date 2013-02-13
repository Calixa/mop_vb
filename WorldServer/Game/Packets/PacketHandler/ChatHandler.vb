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
Imports WorldServer.Game.Chat
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class ChatHandler
		Inherits Globals
		<Opcode(ClientMessage.ChatMessageSay, "16357")> _
		Public Shared Sub HandleChatMessageSay(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)
			Dim language As Integer = packet.ReadInt32()

			Dim messageLength As UInteger = BitUnpack.GetBits(Of UInteger)(9)
			Dim chatMessage As String = packet.ReadString(messageLength)

			If ChatCommandParser.CheckForCommand(chatMessage) Then
				ChatCommandParser.ExecuteChatHandler(chatMessage, session)
			Else
				SendMessageByType(session, MessageType.ChatMessageSay, language, chatMessage)
			End If
		End Sub

		<Opcode(ClientMessage.ChatMessageYell, "16357")> _
		Public Shared Sub HandleChatMessageYell(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)
			Dim language As Integer = packet.ReadInt32()

			Dim messageLength As UInteger = BitUnpack.GetBits(Of UInteger)(9)
			Dim chatMessage As String = packet.ReadString(messageLength)
			SendMessageByType(session, MessageType.ChatMessageYell, language, chatMessage)
		End Sub

		<Opcode(ClientMessage.ChatMessageWhisper, "16357")> _
		Public Shared Sub HandleChatMessageWhisper(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)
			Dim language As Integer = packet.ReadInt32()

			Dim messageLength As UInteger = BitUnpack.GetBits(Of UInteger)(9)
			Dim nameLength As UInteger = BitUnpack.GetNameLength(Of UInteger)(9)

			Dim chatMessage As String = packet.ReadString(messageLength)
			Dim receiverName As String = packet.ReadString(nameLength)

			Dim rSession As WorldClass = WorldMgr.GetSession(receiverName)

			SendMessageByType(rSession, MessageType.ChatMessageWhisper, language, chatMessage)
			SendMessageByType(session, MessageType.ChatMessageWhisperInform, language, chatMessage)
		End Sub

		Public Shared Sub SendMessageByType(ByRef session As WorldClass, type As MessageType, language As Integer, chatMessage As String)
			Dim messageChat As New PacketWriter(LegacyMessage.MessageChat)
			Dim guid As ULong = session.Character.Guid

			messageChat.WriteUInt8(CByte(type))
			messageChat.WriteInt32(language)
			messageChat.WriteUInt64(guid)
			messageChat.WriteUInt32(0)
			messageChat.WriteUInt64(guid)
            messageChat.WriteUInt32(CUInt(chatMessage.Length + 1))
			messageChat.WriteCString(chatMessage)
			messageChat.WriteUInt16(0)

			session.Send(messageChat)
		End Sub
	End Class
End Namespace

