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
Imports System.Collections
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace Network.Packets
	Public Class PacketWriter
		Inherits BinaryWriter
		Public Property Opcode() As UInteger
			Get
				Return m_Opcode
			End Get
			Set
				m_Opcode = Value
			End Set
		End Property
		Private m_Opcode As UInteger
		Public Property Size() As UInteger
			Get
				Return m_Size
			End Get
			Set
				m_Size = Value
			End Set
		End Property
		Private m_Size As UInteger
		Public ReadOnly Property Storage() As Byte()
			Get
                Dim data As Byte() = New Byte(CInt(BaseStream.Length - 5)) {}
				Seek(4, SeekOrigin.Begin)

                For i As Integer = 0 To CInt(BaseStream.Length - 5)
                    data(i) = CByte(BaseStream.ReadByte())
                Next
				Return data
			End Get
		End Property

		Public Sub New()
			MyBase.New(New MemoryStream())
		End Sub
		Public Sub New(message As JAMCCMessage, Optional isWorldPacket As Boolean = False)
			MyBase.New(New MemoryStream())
			SetMessageAndHeader(CUInt(message), isWorldPacket)
		End Sub

		Public Sub New(message As JAMCMessage, Optional isWorldPacket As Boolean = False)
			MyBase.New(New MemoryStream())
			SetMessageAndHeader(CUInt(message), isWorldPacket)
		End Sub

		Public Sub New(message As LegacyMessage, Optional isWorldPacket As Boolean = False)
			MyBase.New(New MemoryStream())
			SetMessageAndHeader(CUInt(message), isWorldPacket)
		End Sub

		Public Sub New(message As Message, Optional isWorldPacket As Boolean = False)
			MyBase.New(New MemoryStream())
			SetMessageAndHeader(CUInt(message), isWorldPacket)
		End Sub

		Private Sub SetMessageAndHeader(mess As UInteger, isWorldPacket As Boolean)
			Opcode = mess
			WritePacketHeader(mess, isWorldPacket)
		End Sub

		Protected Sub WritePacketHeader(opcode__1 As UInteger, Optional isWorldPacket As Boolean = False)
			Opcode = opcode__1

			WriteUInt8(0)
			WriteUInt8(0)
			WriteUInt8(CByte(&Hff And opcode__1))
			WriteUInt8(CByte(&Hff And (opcode__1 >> 8)))

			If isWorldPacket Then
				WriteUInt8(CByte(&Hff And (opcode__1 >> 16)))
				WriteUInt8(CByte(&Hff And (opcode__1 >> 24)))
			End If
		End Sub

		Public Function ReadDataToSend(Optional isAuthPacket As Boolean = False) As Byte()
            Dim data As Byte() = New Byte(CInt(BaseStream.Length - 1)) {}
			Seek(0, SeekOrigin.Begin)

            For i As Integer = 0 To CInt(BaseStream.Length - 1)
                data(i) = CByte(BaseStream.ReadByte())
            Next

            Size = CUInt(data.Length - 2)
			If Not isAuthPacket Then
				data(0) = CByte(&Hff And Size)
				data(1) = CByte(&Hff And (Size >> 8))

				If Size > &H7fff Then
					Seek(0, SeekOrigin.Begin)

					Dim bigSize As Byte = CByte(&H80 Or (&Hff And (Size >> 16)))
					WriteUInt8(bigSize)
				End If
			End If

			Return data
		End Function

		Public Sub WriteInt8(data As SByte)
			MyBase.Write(data)
		End Sub

		Public Sub WriteInt16(data As Short)
			MyBase.Write(data)
		End Sub

		Public Sub WriteInt32(data As Integer)
			MyBase.Write(data)
		End Sub

		Public Sub WriteInt64(data As Long)
			MyBase.Write(data)
		End Sub

		Public Sub WriteUInt8(data As Byte)
			MyBase.Write(data)
		End Sub

		Public Sub WriteUInt16(data As UShort)
			MyBase.Write(data)
		End Sub

		Public Sub WriteUInt32(data As UInteger)
			MyBase.Write(data)
		End Sub

		Public Sub WriteUInt64(data As ULong)
			MyBase.Write(data)
		End Sub

		Public Sub WriteFloat(data As Single)
			MyBase.Write(data)
		End Sub

		Public Sub WriteDouble(data As Double)
			MyBase.Write(data)
		End Sub

		Public Sub WriteCString(data As String)
			Dim sBytes As Byte() = Encoding.ASCII.GetBytes(data)
			Me.WriteBytes(sBytes)
			MyBase.Write(CByte(0))
			' String null terminated
		End Sub

		Public Sub WriteString(data As String)
			Dim sBytes As Byte() = Encoding.ASCII.GetBytes(data)
			Me.WriteBytes(sBytes)
		End Sub

		Public Sub WriteUnixTime()
			Dim baseDate As New DateTime(1970, 1, 1)
			Dim currentDate As DateTime = DateTime.Now
			Dim ts As TimeSpan = currentDate - baseDate

			WriteUInt32(Convert.ToUInt32(ts.TotalSeconds))
		End Sub

        Public Sub WriteGuid(guid As Long)
            Dim packedGuid As Byte() = New Byte(8) {}
            Dim length As Integer = 1

            Dim i As Integer = 0
            While guid <> 0
                If (guid And &HFF) <> 0 Then
                    packedGuid(0) = packedGuid(0) Or CByte(1 << i)
                    packedGuid(length) = CByte(guid And &HFF)
                    length += 1
                End If

                guid >>= 8
                i += 1
            End While

            WriteBytes(packedGuid, length)
        End Sub

		Public Sub WriteBytes(data As Byte(), Optional count As Integer = 0)
			If count = 0 Then
				MyBase.Write(data)
			Else
				MyBase.Write(data, 0, count)
			End If
		End Sub

		Public Sub WriteBitArray(buffer As BitArray, Len As Integer)
			Dim bufferarray As Byte() = New Byte(Convert.ToByte((buffer.Length + 8) \ 8)) {}
			buffer.CopyTo(bufferarray, 0)

			WriteBytes(bufferarray.ToArray(), Len)
		End Sub
	End Class
End Namespace
