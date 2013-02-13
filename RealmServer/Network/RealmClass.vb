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


Imports Framework.Constants.Authentication
Imports Framework.Cryptography
Imports Framework.Database
Imports Framework.Logging
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Framework.ObjectDefines.Realm

Namespace Framework.Network.Realm
	Public Class RealmClass
		Public Shared Property account() As Account
			Get
				Return m_account
			End Get
			Set
				m_account = Value
			End Set
		End Property
		Private Shared m_account As Account
        Public Shared Realms As New List(Of Global.Framework.ObjectDefines.Realm)()
		Public Shared realm As RealmNetwork
		Public Property SecureRemotePassword() As SRP6
			Get
				Return m_SecureRemotePassword
			End Get
			Set
				m_SecureRemotePassword = Value
			End Set
		End Property
		Private m_SecureRemotePassword As SRP6
		Public clientSocket As Socket
		Private DataBuffer As Byte()

		Public Sub New()
			account = New Account()
			SecureRemotePassword = New SRP6()
		End Sub

		Private Sub HandleRealmData(data As Byte())
			Dim reader As New PacketReader(data, False)
			Dim cmd As ClientLink = CType(reader.ReadUInt8(), ClientLink)

			Select Case cmd
				Case ClientLink.CMD_AUTH_LOGON_CHALLENGE, ClientLink.CMD_AUTH_RECONNECT_CHALLENGE
					HandleAuthLogonChallenge(Me, reader)
					Exit Select
				Case ClientLink.CMD_AUTH_LOGON_PROOF, ClientLink.CMD_AUTH_RECONNECT_PROOF
					HandleAuthLogonProof(Me, reader)
					Exit Select
				Case ClientLink.CMD_REALM_LIST
					HandleRealmList(Me, reader)
					Exit Select
				Case Else
					Log.Message(LogType.NORMAL, "Received unknown ClientLink: {0}", cmd)
					Exit Select
			End Select
		End Sub

		Public Sub HandleAuthLogonChallenge(session As RealmClass, data As PacketReader)
			Log.Message(LogType.NORMAL, "AuthLogonChallenge")

			data.Skip(10)
			Dim ClientBuild As UShort = data.ReadUInt16()
			data.Skip(8)
			account.Language = data.ReadStringFromBytes(4)
			data.Skip(4)

			account.IP = data.ReadIPAddress()
			account.Name = data.ReadAccountName()

			Dim result As SQLResult = DB.Realms.[Select]("SELECT id, name, password, expansion, gmlevel, securityFlags, online FROM accounts WHERE name = ?", account.Name)

			Dim logonChallenge As New PacketWriter()
			logonChallenge.WriteUInt8(CByte(ClientLink.CMD_AUTH_LOGON_CHALLENGE))
			logonChallenge.WriteUInt8(0)

			If result.Count <> 0 Then
				If result.Read(Of Boolean)(0, "online") Then
					logonChallenge.WriteUInt8(CByte(AuthResults.WOW_FAIL_ALREADY_ONLINE))
					session.Send(logonChallenge)
					Return
				End If

				account.Id = result.Read(Of Int32)(0, "id")
				account.Expansion = result.Read(Of [Byte])(0, "expansion")
				account.SecurityFlags = result.Read(Of [Byte])(0, "securityFlags")

				DB.Realms.Execute("UPDATE accounts SET ip = ?, language = ? WHERE id = ?", account.IP, account.Language, account.Id)

				Dim username As Byte() = Encoding.ASCII.GetBytes(result.Read(Of [String])(0, "name").ToUpper())
				Dim password As Byte() = Encoding.ASCII.GetBytes(result.Read(Of [String])(0, "password").ToUpper())

				' WoW 5.1.0.16357 (5.1.0a)
				If ClientBuild = 16357 Then
					session.SecureRemotePassword.CalculateX(username, password)
					Dim buf As Byte() = New Byte(15) {}
					SRP6.RAND_bytes(buf, &H10)

					logonChallenge.WriteUInt8(CByte(AuthResults.WOW_SUCCESS))
                    logonChallenge.WriteBytes(session.SecureRemotePassword.b1)
					logonChallenge.WriteUInt8(1)
					logonChallenge.WriteUInt8(session.SecureRemotePassword.g(0))
					logonChallenge.WriteUInt8(&H20)
					logonChallenge.WriteBytes(session.SecureRemotePassword.N)
					logonChallenge.WriteBytes(session.SecureRemotePassword.salt)
					logonChallenge.WriteBytes(buf)

					' Security flags
					logonChallenge.WriteUInt8(account.SecurityFlags)

					' Enable authenticator
					If (account.SecurityFlags And 4) <> 0 Then
						logonChallenge.WriteUInt8(1)
					End If
				End If
			Else
				logonChallenge.WriteUInt8(CByte(AuthResults.WOW_FAIL_UNKNOWN_ACCOUNT))
			End If

			session.Send(logonChallenge)
		End Sub

		Public Sub HandleAuthAuthenticator(session As RealmClass, data As PacketReader)
			Log.Message(LogType.NORMAL, "AuthAuthenticator")
		End Sub

        Public Sub HandleAuthLogonProof(session As RealmClass, data As PacketReader)
            Log.Message(LogType.NORMAL, "AuthLogonProof")

            Dim logonProof As New PacketWriter()

            Dim a As Byte() = New Byte(31) {}
            Dim m1 As Byte() = New Byte(19) {}

            Array.Copy(DataBuffer, 1, a, 0, 32)
            Array.Copy(DataBuffer, 33, m1, 0, 20)

            session.SecureRemotePassword.CalculateU(a)
            session.SecureRemotePassword.CalculateM2(m1)
            session.SecureRemotePassword.CalculateK()

            For Each b As Byte In session.SecureRemotePassword.K
                If b < &H10 Then
                    account.SessionKey += "0" & [String].Format("{0:X}", b)
                Else
                    account.SessionKey += [String].Format("{0:X}", b)
                End If
            Next

            logonProof.WriteUInt8(CByte(ClientLink.CMD_AUTH_LOGON_PROOF))
            logonProof.WriteUInt8(0)
            logonProof.WriteBytes(session.SecureRemotePassword.M2)
            logonProof.WriteUInt32(&H800000)
            logonProof.WriteUInt32(0)
            logonProof.WriteUInt16(0)

            DB.Realms.Execute("UPDATE accounts SET sessionkey = ? WHERE id = ?", account.SessionKey, account.Id)

            session.Send(logonProof)
        End Sub

		Public Sub HandleRealmList(session As RealmClass, data As PacketReader)
			Log.Message(LogType.NORMAL, "RealmList")

			Dim realmData As New PacketWriter()

            For Each r As Global.Framework.ObjectDefines.Realm In Realms
                '                Realms.ForEach(Function(r)
                realmData.WriteUInt8(1)
                realmData.WriteUInt8(0)
                realmData.WriteUInt8(0)
                realmData.WriteCString(r.Name)
                realmData.WriteCString(r.IP & ":" & r.Port)
                realmData.WriteFloat(0)
                realmData.WriteUInt8(0)
                ' CharCount
                realmData.WriteUInt8(1)
                realmData.WriteUInt8(&H2C)

                '               End Function)
            Next
            Dim realmList As New PacketWriter()
            realmList.WriteUInt8(CByte(ClientLink.CMD_REALM_LIST))
            realmList.WriteUInt16(CUShort(realmData.BaseStream.Length + 8))
            realmList.WriteUInt32(0)
            realmList.WriteUInt16(CUShort(Realms.Count))
            realmList.WriteBytes(realmData.ReadDataToSend())
            realmList.WriteUInt8(0)
            realmList.WriteUInt8(&H10)

            session.Send(realmList)
        End Sub

        Public Sub Recieve()
            While realm.listenSocket
                Thread.Sleep(1)
                If clientSocket.Available > 0 Then
                    DataBuffer = New Byte(clientSocket.Available - 1) {}
                    clientSocket.Receive(DataBuffer, DataBuffer.Length, SocketFlags.None)

                    HandleRealmData(DataBuffer)
                End If
            End While

            clientSocket.Close()
        End Sub

        Public Sub Send(packet As PacketWriter)
            DataBuffer = packet.ReadDataToSend(True)

            Try
                clientSocket.BeginSend(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, New AsyncCallback(AddressOf FinishSend), clientSocket)
                packet.Flush()
            Catch ex As Exception
                Log.Message(LogType.[ERROR], "{0}", ex.Message)
                Log.Message()
                clientSocket.Close()
            End Try
        End Sub

        Public Sub FinishSend(result As IAsyncResult)
            clientSocket.EndSend(result)
        End Sub
    End Class
End Namespace
