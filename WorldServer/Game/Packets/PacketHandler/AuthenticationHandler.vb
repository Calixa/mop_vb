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


Imports Framework.Configuration
Imports Framework.Constants
Imports Framework.Constants.Authentication
Imports Framework.Database
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class AuthenticationHandler
		Inherits Globals
		<Opcode(ClientMessage.TransferInitiate, "16357")> _
		Public Shared Sub HandleAuthChallenge(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim authChallenge As New PacketWriter(JAMCCMessage.AuthChallenge, True)

			authChallenge.WriteUInt32(CUInt(New Random(DateTime.Now.Second).[Next](1, &Hfffffff)))

			For i As Integer = 0 To 7
				authChallenge.WriteUInt32(0)
			Next

			authChallenge.WriteUInt8(1)

			session.Send(authChallenge)
		End Sub

		<Opcode(ClientMessage.AuthSession, "16357")> _
		Public Shared Sub HandleAuthResponse(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim BitUnpack As New BitUnpack(packet)

			packet.Skip(54)

			Dim addonSize As Integer = packet.ReadInt32()
			packet.Skip(addonSize)

			Dim nameLength As UInteger = BitUnpack.GetNameLength(Of UInteger)(12)
			Dim accountName As String = packet.ReadString(nameLength)

			Dim result As SQLResult = DB.Realms.[Select]("SELECT * FROM accounts WHERE name = ?", accountName)
			If result.Count = 0 Then
				session.clientSocket.Close()
			Else
				session.Account = New Account() With { _
					.Id = result.Read(Of Integer)(0, "id"), _
					.Name = result.Read(Of [String])(0, "name"), _
					.Password = result.Read(Of [String])(0, "password"), _
					.SessionKey = result.Read(Of [String])(0, "sessionkey"), _
					.Expansion = result.Read(Of Byte)(0, "expansion"), _
					.GMLevel = result.Read(Of Byte)(0, "gmlevel"), _
					.IP = result.Read(Of [String])(0, "ip"), _
					.Language = result.Read(Of [String])(0, "language") _
				}
			End If

			Dim K As String = session.Account.SessionKey
			Dim kBytes As Byte() = New Byte(K.Length \ 2 - 1) {}

			For i As Integer = 0 To K.Length - 1 Step 2
				kBytes(i \ 2) = Convert.ToByte(K.Substring(i, 2), 16)
			Next

			session.Crypt.Initialize(kBytes)

			Dim realmId As UInteger = WorldConfig.RealmId
			Dim realmClassResult As SQLResult = DB.Realms.[Select]("SELECT class, expansion FROM realm_classes WHERE realmId = ?", realmId)
			Dim realmRaceResult As SQLResult = DB.Realms.[Select]("SELECT race, expansion FROM realm_races WHERE realmId = ?", realmId)

			Dim HasAccountData As Boolean = True
			Dim IsInQueue As Boolean = False

			Dim authResponse As New PacketWriter(JAMCMessage.AuthResponse)
			Dim BitPack As New BitPack(authResponse)

			BitPack.Write(1)
			' HasAccountData
			If HasAccountData Then
				BitPack.Write(realmClassResult.Count, 25)
				' Activation count for classes
				BitPack.Write(0)
				' Unknown, 5.0.4
				BitPack.Write(0)
				' Unknown, 5.1.0
				BitPack.Write(0, 22)
				' Activate character template windows/button
				'if (HasCharacterTemplate)
				'Write bits for char templates...

				BitPack.Write(realmRaceResult.Count, 25)
				' Activation count for races
					' IsInQueue
				BitPack.Write(IsInQueue)
			End If

			If IsInQueue Then
				BitPack.Write(0)
				' Unknown
				BitPack.Flush()

					' QueuePosition
				authResponse.WriteUInt32(0)
			Else
				BitPack.Flush()
			End If

			If HasAccountData Then
				'if (HasCharacterTemplate)
				'Write data for char templates...

				For r As Integer = 0 To realmRaceResult.Count - 1
					authResponse.WriteUInt8(realmRaceResult.Read(Of Byte)(r, "expansion"))
					authResponse.WriteUInt8(realmRaceResult.Read(Of Byte)(r, "race"))
				Next

				authResponse.WriteUInt32(0)
				authResponse.WriteUInt32(0)
				authResponse.WriteUInt8(0)
				authResponse.WriteUInt8(session.Account.Expansion)
				authResponse.WriteUInt8(session.Account.Expansion)

				For c As Integer = 0 To realmClassResult.Count - 1
					authResponse.WriteUInt8(realmClassResult.Read(Of Byte)(c, "class"))
					authResponse.WriteUInt8(realmClassResult.Read(Of Byte)(c, "expansion"))
				Next

				authResponse.WriteUInt32(0)
			End If

			authResponse.WriteUInt8(CByte(AuthCodes.AUTH_OK))

			session.Send(authResponse)

			MiscHandler.HandleUpdateClientCacheVersion(session)
			TutorialHandler.HandleTutorialFlags(session)
		End Sub
	End Class
End Namespace
