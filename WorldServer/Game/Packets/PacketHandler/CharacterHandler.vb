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
Imports Framework.Logging
Imports Framework.Network.Packets
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports WorldServer.Game.Packets.PacketHandler
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.PacketHandler
	Public Class CharacterHandler
		Inherits Globals
		<Opcode(ClientMessage.EnumCharacters, "16357")> _
		Public Shared Sub HandleEnumCharactersResult(ByRef packet As PacketReader, ByRef session As WorldClass)
			DB.Realms.Execute("UPDATE accounts SET online = 1 WHERE id = ?", session.Account.Id)

			Dim result As SQLResult = DB.Characters.[Select]("SELECT * FROM characters WHERE accountid = ?", session.Account.Id)

			Dim enumCharacters As New PacketWriter(JAMCMessage.EnumCharactersResult)
			Dim BitPack As New BitPack(enumCharacters)

			BitPack.Write(0, 23)
			BitPack.Write(result.Count, 17)

			If result.Count <> 0 Then
				For c As Integer = 0 To result.Count - 1
					Dim name As String = result.Read(Of [String])(c, "Name")
					Dim loginCinematic As Boolean = result.Read(Of [Boolean])(c, "LoginCinematic")

					BitPack.Guid = result.Read(Of UInt64)(c, "Guid")
					BitPack.GuildGuid = result.Read(Of UInt64)(c, "GuildGuid")

					BitPack.WriteGuidMask(7, 0, 4)
					BitPack.WriteGuildGuidMask(2)
					BitPack.WriteGuidMask(5, 3)
					BitPack.Write(CUInt(Encoding.ASCII.GetBytes(name).Length), 7)
					BitPack.WriteGuildGuidMask(0, 5, 3)
					BitPack.Write(loginCinematic)
					BitPack.WriteGuildGuidMask(6, 7)
					BitPack.WriteGuidMask(1)
					BitPack.WriteGuildGuidMask(4, 1)
					BitPack.WriteGuidMask(2, 6)
				Next

				BitPack.Write(1)
				BitPack.Flush()

				For c As Integer = 0 To result.Count - 1
					Dim name As String = result.Read(Of [String])(c, "Name")
					BitPack.Guid = result.Read(Of UInt64)(c, "Guid")
					BitPack.GuildGuid = result.Read(Of UInt64)(c, "GuildGuid")

					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "CharacterFlags"))
					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "PetFamily"))
					enumCharacters.WriteFloat(result.Read(Of [Single])(c, "Z"))

					BitPack.WriteGuidBytes(7)
					BitPack.WriteGuildGuidBytes(6)

					' Not implanted
					For j As Integer = 0 To 22
						enumCharacters.WriteUInt32(0)
						enumCharacters.WriteUInt8(0)
						enumCharacters.WriteUInt32(0)
					Next

					enumCharacters.WriteFloat(result.Read(Of [Single])(c, "X"))
					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Class"))

					BitPack.WriteGuidBytes(5)

					enumCharacters.WriteFloat(result.Read(Of [Single])(c, "Y"))

					BitPack.WriteGuildGuidBytes(3)
					BitPack.WriteGuidBytes(6)

					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "PetLevel"))
					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "PetDisplayId"))

					BitPack.WriteGuidBytes(2)
					BitPack.WriteGuidBytes(1)

					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "HairColor"))
					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "FacialHair"))

					BitPack.WriteGuildGuidBytes(2)

					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "Zone"))
					enumCharacters.WriteUInt8(0)

					BitPack.WriteGuidBytes(0)
					BitPack.WriteGuildGuidBytes(1)

					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Skin"))

					BitPack.WriteGuidBytes(4)
					BitPack.WriteGuildGuidBytes(5)

					enumCharacters.WriteString(name)

					BitPack.WriteGuildGuidBytes(0)

					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Level"))

					BitPack.WriteGuidBytes(3)
					BitPack.WriteGuildGuidBytes(7)

					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "HairStyle"))

					BitPack.WriteGuildGuidBytes(4)

					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Gender"))
					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "Map"))
					enumCharacters.WriteUInt32(result.Read(Of UInt32)(c, "CustomizeFlags"))
					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Race"))
					enumCharacters.WriteUInt8(result.Read(Of [Byte])(c, "Face"))
				Next
			Else
				BitPack.Write(1)
				BitPack.Flush()
			End If
			


			session.Send(enumCharacters)
		End Sub

		<Opcode(ClientMessage.RequestCharCreate, "16357")> _
		Public Shared Sub HandleResponseCharacterCreate(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)

			Dim pClass As Byte = packet.ReadByte()
			Dim hairStyle As Byte = packet.ReadByte()
			Dim facialHair As Byte = packet.ReadByte()
			Dim race As Byte = packet.ReadByte()
			Dim face As Byte = packet.ReadByte()
			Dim skin As Byte = packet.ReadByte()
			Dim gender As Byte = packet.ReadByte()
			Dim hairColor As Byte = packet.ReadByte()
			packet.ReadByte()
			' Always 0
			Dim nameLength As UInteger = BitUnpack.GetNameLength(Of UInteger)(7)
			Dim name As String = Character.NormalizeName(packet.ReadString(nameLength))

			Dim result As SQLResult = DB.Characters.[Select]("SELECT * from characters WHERE name = ?", name)
			Dim writer As New PacketWriter(LegacyMessage.ResponseCharacterCreate)

			If result.Count <> 0 Then
				' Name already in use
				writer.WriteUInt8(&H32)
				session.Send(writer)
				Return
			End If

			result = DB.Characters.[Select]("SELECT map, zone, posX, posY, posZ, posO FROM character_creation_data WHERE race = ? AND class = ?", race, pClass)
			If result.Count = 0 Then
				writer.WriteUInt8(&H31)
				session.Send(writer)
				Return
			End If

			Dim map As UInteger = result.Read(Of UInteger)(0, "map")
			Dim zone As UInteger = result.Read(Of UInteger)(0, "zone")
			Dim posX As Single = result.Read(Of Single)(0, "posX")
			Dim posY As Single = result.Read(Of Single)(0, "posY")
			Dim posZ As Single = result.Read(Of Single)(0, "posZ")
			Dim posO As Single = result.Read(Of Single)(0, "posO")

			DB.Characters.Execute("INSERT INTO characters (name, accountid, race, class, gender, skin, zone, map, x, y, z, o, face, hairstyle, haircolor, facialhair) VALUES (" & "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", name, session.Account.Id, race, pClass, gender, _
				skin, zone, map, posX, posY, posZ, _
				posO, face, hairStyle, hairColor, facialHair)

			' Success
			writer.WriteUInt8(&H2f)
			session.Send(writer)
		End Sub

		<Opcode(ClientMessage.RequestCharDelete, "16357")> _
		Public Shared Sub HandleResponseCharacterDelete(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim guid As UInt64 = packet.ReadUInt64()

			Dim writer As New PacketWriter(LegacyMessage.ResponseCharacterDelete)
			writer.WriteUInt8(&H47)
			session.Send(writer)

			DB.Characters.Execute("DELETE FROM characters WHERE guid = ?", guid)
			DB.Characters.Execute("DELETE FROM character_spells WHERE guid = ?", guid)
			DB.Characters.Execute("DELETE FROM character_skills WHERE guid = ?", guid)
		End Sub

		<Opcode(ClientMessage.RequestRandomCharacterName, "16357")> _
		Public Shared Sub HandleGenerateRandomCharacterNameResult(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim gender As Byte = packet.ReadByte()
			Dim race As Byte = packet.ReadByte()

			Dim names As List(Of String) = DBCStorage.NameGenStorage.Where(Function(n) n.Value.Race = race AndAlso n.Value.Gender = gender).[Select](Function(n) n.Value.Name).ToList()
			Dim rand As New Random(Environment.TickCount)

			Dim NewName As String
			Dim result As SQLResult
			Do
				NewName = names(rand.[Next](names.Count))
				result = DB.Characters.[Select]("SELECT * FROM characters WHERE name = ?", NewName)
			Loop While result.Count <> 0

			Dim writer As New PacketWriter(JAMCMessage.GenerateRandomCharacterNameResult)
			Dim BitPack As New BitPack(writer)

			BitPack.Write(Of Integer)(NewName.Length, 15)
			BitPack.Write(True)
			BitPack.Flush()

			writer.WriteString(NewName)
			session.Send(writer)
		End Sub

		<Opcode(ClientMessage.PlayerLogin, "16357")> _
		Public Shared Sub HandlePlayerLogin(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim guidMask As Byte() = {5, 7, 6, 1, 2, 3, _
				4, 0}
			Dim guidBytes As Byte() = {6, 4, 3, 5, 0, 2, _
				7, 1}

			Dim GuidUnpacker As New BitUnpack(packet)

			Dim guid As ULong = GuidUnpacker.GetGuid(guidMask, guidBytes)
			Log.Message(LogType.DEBUG, "Character with Guid: {0}, AccountId: {1} tried to enter the world.", guid, session.Account.Id)

			session.Character = New Character(guid)

			If Not WorldMgr.AddSession(guid, session) Then
				Log.Message(LogType.[ERROR], "A Character with Guid: {0} is already logged in", guid)
				Return
			End If

			WorldMgr.WriteAccountData(AccountDataMasks.CharacterCacheMask, session)

			MiscHandler.HandleMessageOfTheDay(session)
			SpellHandler.HandleSendKnownSpells(session)

			If session.Character.LoginCinematic Then
				CinematicHandler.HandleStartCinematic(session)
			End If

			ObjectHandler.HandleUpdateObjectCreate(session)
		End Sub
	End Class
End Namespace
