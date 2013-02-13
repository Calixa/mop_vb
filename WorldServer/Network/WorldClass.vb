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
Imports Framework.Cryptography
Imports Framework.Logging
Imports Framework.Logging.PacketLogging
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports System.Collections
Imports System.Net
Imports System.Net.Sockets
Imports WorldServer.Game.Packets
Imports WorldServer.Game.WorldEntities

Namespace Network
	Public Class WorldClass
		Public Property Account() As Account
			Get
				Return m_Account
			End Get
			Set
				m_Account = Value
			End Set
		End Property
		Private m_Account As Account
		Public Property Character() As Character
			Get
				Return m_Character
			End Get
			Set
				m_Character = Value
			End Set
		End Property
		Private m_Character As Character
		Public Shared world As WorldNetwork
		Public clientSocket As Socket
		Public PacketQueue As Queue
		Public Crypt As PacketCrypt
		Private DataBuffer As Byte()

		Public Sub New()
			DataBuffer = New Byte(8191) {}
			PacketQueue = New Queue()
			Crypt = New PacketCrypt()
		End Sub

		Public Sub OnData()
			Dim packet As PacketReader = Nothing
			If PacketQueue.Count > 0 Then
				packet = DirectCast(PacketQueue.Dequeue(), PacketReader)
			Else
				packet = New PacketReader(DataBuffer)
			End If

			Dim clientInfo As String = Convert.ToString(DirectCast(clientSocket.RemoteEndPoint, IPEndPoint).Address) & ":" & DirectCast(clientSocket.RemoteEndPoint, IPEndPoint).Port
			PacketLog.WritePacket(clientInfo, Nothing, packet)

			If [Enum].IsDefined(GetType(ClientMessage), packet.Opcode) Then
				PacketManager.InvokeHandler(packet, Me, CType(packet.Opcode, ClientMessage))
			Else
				Log.Message(LogType.DUMP, "UNKNOWN OPCODE: {0} (0x{1:X}), LENGTH: {2}", packet.Opcode, CUShort(packet.Opcode), packet.Size)
			End If
		End Sub

		Public Sub OnConnect()
			Dim TransferInitiate As New PacketWriter(Message.TransferInitiate)
			TransferInitiate.WriteCString("RLD OF WARCRAFT CONNECTION - SERVER TO CLIENT")

			Send(TransferInitiate)

			clientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, AddressOf Receive, Nothing)
		End Sub

		Public Sub Receive(result As IAsyncResult)
			Try
                Dim recievedBytes As Integer = clientSocket.EndReceive(result)
				If recievedBytes <> 0 Then
					If Crypt.IsInitialized Then
						While recievedBytes > 0
							Decode(DataBuffer)

                            Dim length As Integer = BitConverter.ToUInt16(DataBuffer, 0) + 4

                            Dim packetData As Byte() = New Byte(length - 1) {}
                            Array.Copy(DataBuffer, packetData, length)

							Dim packet As New PacketReader(packetData)
							PacketQueue.Enqueue(packet)

							recievedBytes -= length
							Array.Copy(DataBuffer, length, DataBuffer, 0, recievedBytes)
							OnData()
						End While
					Else
						OnData()
					End If

					clientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, AddressOf Receive, Nothing)
				End If
			Catch ex As Exception
				Log.Message(LogType.[ERROR], "{0}", ex.Message)
				Log.Message()
			End Try
		End Sub

		Protected Sub Decode(ByRef data As Byte())
			Crypt.Decrypt(data)

            Dim header As UInteger = BitConverter.ToUInt32(data, 0)
			Dim size As UShort = CUShort(header >> 12)
			Dim opcode As UShort = CUShort(header And &Hfff)

			data(0) = CByte(&Hff And size)
			data(1) = CByte(&Hff And (size >> 8))
			data(2) = CByte(&Hff And opcode)
			data(3) = CByte(&Hff And (opcode >> 8))
		End Sub

		Public Sub Send(ByRef packet As PacketWriter)
			If packet.Opcode = 0 Then
				Return
			End If

            Dim buffer As Byte() = packet.ReadDataToSend()

			Try
				If Crypt.IsInitialized Then
                    Dim totalLength As Long = CUInt(packet.Size) - 2
					totalLength <<= 12
					totalLength = totalLength Or (CUInt(packet.Opcode) And &Hfff)

                    Dim header As Byte() = BitConverter.GetBytes(totalLength)

					Crypt.Encrypt(header)

					buffer(0) = header(0)
					buffer(1) = header(1)
					buffer(2) = header(2)
					buffer(3) = header(3)
				End If

				clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None)

				Dim clientInfo As String = Convert.ToString(DirectCast(clientSocket.RemoteEndPoint, IPEndPoint).Address) & ":" & DirectCast(clientSocket.RemoteEndPoint, IPEndPoint).Port
				PacketLog.WritePacket(clientInfo, packet)

				packet.Flush()
			Catch ex As Exception
				Log.Message(LogType.[ERROR], "{0}", ex.Message)
				Log.Message()

				clientSocket.Close()
			End Try
		End Sub
	End Class
End Namespace
