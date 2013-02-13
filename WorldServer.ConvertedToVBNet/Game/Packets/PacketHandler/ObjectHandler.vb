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
Imports System.Collections.Generic
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class ObjectHandler
		Inherits Globals
		Public Shared Sub HandleUpdateObjectCreate(ByRef session As WorldClass)
			Dim character As WorldObject = session.Character
			Dim updateObject As New PacketWriter(LegacyMessage.UpdateObject)

			updateObject.WriteUInt16(CUShort(character.Map))
			updateObject.WriteUInt32(1)
			updateObject.WriteUInt8(CByte(UpdateType.CreateObject))
            updateObject.WriteGuid(CLng(character.Guid))
			updateObject.WriteUInt8(CByte(ObjectType.Player))

			Dim updateFlags As UpdateFlag = UpdateFlag.Alive Or UpdateFlag.Rotation Or UpdateFlag.Self
			WorldMgr.WriteUpdateObjectMovement(updateObject, character, updateFlags)

			character.WriteUpdateFields(updateObject)
			character.WriteDynamicUpdateFields(updateObject)

			session.Send(updateObject)

            Dim tempSessions As Dictionary(Of ULong, WorldClass) = New Dictionary(Of ULong, WorldClass)(WorldMgr.Sessions)
			tempSessions.Remove(character.Guid)

			If tempSessions IsNot Nothing Then
                For Each s As KeyValuePair(Of ULong, WorldClass) In tempSessions
                    If s.Value.Character.CheckUpdateDistance(character) Then
                        updateObject = New PacketWriter(LegacyMessage.UpdateObject)

                        updateObject.WriteUInt16(CUShort(character.Map))
                        updateObject.WriteUInt32(1)
                        updateObject.WriteUInt8(CByte(UpdateType.CreateObject))
                        updateObject.WriteGuid(CLng(character.Guid))
                        updateObject.WriteUInt8(CByte(ObjectType.Player))

                        updateFlags = UpdateFlag.Alive Or UpdateFlag.Rotation
                        WorldMgr.WriteUpdateObjectMovement(updateObject, character, updateFlags)

                        character.WriteUpdateFields(updateObject)
                        character.WriteDynamicUpdateFields(updateObject)

                        s.Value.Send(updateObject)
                    End If
                Next

                For Each s As KeyValuePair(Of ULong, WorldClass) In tempSessions
                    Dim pChar As WorldObject = s.Value.Character

                    If character.CheckUpdateDistance(pChar) Then
                        character.ToCharacter().InRangeObjects.Add(pChar.Guid, pChar)

                        updateObject = New PacketWriter(LegacyMessage.UpdateObject)

                        updateObject.WriteUInt16(CUShort(pChar.Map))
                        updateObject.WriteUInt32(1)
                        updateObject.WriteUInt8(CByte(UpdateType.CreateObject))
                        updateObject.WriteGuid(CLng(pChar.Guid))
                        updateObject.WriteUInt8(CByte(ObjectType.Player))

                        updateFlags = UpdateFlag.Alive Or UpdateFlag.Rotation
                        WorldMgr.WriteUpdateObjectMovement(updateObject, pChar, updateFlags)

                        pChar.WriteUpdateFields(updateObject)
                        pChar.WriteDynamicUpdateFields(updateObject)

                        session.Send(updateObject)
                    End If
                Next
			End If
		End Sub

		Public Shared Function HandleObjectDestroy(ByRef session As WorldClass, guid As ULong) As PacketWriter
			Dim objectDestroy As New PacketWriter(LegacyMessage.ObjectDestroy)

			objectDestroy.WriteUInt64(guid)
			objectDestroy.WriteUInt8(0)

			Return objectDestroy
		End Function

		<Opcode(ClientMessage.ObjectUpdateFailed, "16357")> _
		Public Shared Sub HandleObjectUpdateFailed(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim guidMask As Byte() = {6, 1, 7, 5, 0, 4, _
				2, 3}
			Dim guidBytes As Byte() = {2, 3, 7, 4, 5, 1, _
				0, 6}

			Dim GuidUnpacker As New BitUnpack(packet)

			Dim guid As ULong = GuidUnpacker.GetGuid(guidMask, guidBytes)
			Log.Message(LogType.DEBUG, "ObjectUpdate failed for object with Guid {0}", guid)
		End Sub
	End Class
End Namespace
