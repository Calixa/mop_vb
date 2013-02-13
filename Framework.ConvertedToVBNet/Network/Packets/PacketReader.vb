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
Imports System.IO
Imports System.Text

Namespace Network.Packets
	Public Class PacketReader
		Inherits BinaryReader
		Public Property Opcode() As ClientMessage
			Get
				Return m_Opcode
			End Get
			Set
				m_Opcode = Value
			End Set
		End Property
		Private m_Opcode As ClientMessage
		Public Property Size() As UShort
			Get
				Return m_Size
			End Get
			Set
				m_Size = Value
			End Set
		End Property
		Private m_Size As UShort
		Public Property Storage() As Byte()
			Get
				Return m_Storage
			End Get
			Set
				m_Storage = Value
			End Set
		End Property
		Private m_Storage As Byte()

		Public Sub New(data As Byte(), Optional worldPacket As Boolean = True)
			MyBase.New(New MemoryStream(data))
			If worldPacket Then
				Dim size__1 As UShort = Me.ReadUInt16()
				Opcode = CType(Me.ReadUInt16(), ClientMessage)

				If Opcode = ClientMessage.TransferInitiate Then
					Size = CUShort((size__1 Mod &H100) + (size__1 \ &H100))
				Else
					Size = size__1
				End If

				Storage = New Byte(Size - 1) {}
				Array.Copy(data, 4, Storage, 0, Size)
			End If
		End Sub

		Public Function ReadInt8() As SByte
			Return MyBase.ReadSByte()
		End Function

		Public Shadows Function ReadInt16() As Short
			Return MyBase.ReadInt16()
		End Function

		Public Shadows Function ReadInt32() As Integer
			Return MyBase.ReadInt32()
		End Function

		Public Shadows Function ReadInt64() As Long
			Return MyBase.ReadInt64()
		End Function

		Public Function ReadUInt8() As Byte
			Return MyBase.ReadByte()
		End Function

		Public Shadows Function ReadUInt16() As UShort
			Return MyBase.ReadUInt16()
		End Function

		Public Shadows Function ReadUInt32() As UInteger
			Return MyBase.ReadUInt32()
		End Function

		Public Shadows Function ReadUInt64() As ULong
			Return MyBase.ReadUInt64()
		End Function

		Public Function ReadFloat() As Single
			Return MyBase.ReadSingle()
		End Function

		Public Shadows Function ReadDouble() As Double
			Return MyBase.ReadDouble()
		End Function

		Public Function ReadCString() As String
			Dim tmpString As New StringBuilder()
			Dim tmpChar As Char = MyBase.ReadChar()
			Dim tmpEndChar As Char = Convert.ToChar(Encoding.UTF8.GetString(New Byte() {0}))

			While tmpChar <> tmpEndChar
				tmpString.Append(tmpChar)
				tmpChar = MyBase.ReadChar()
			End While

			Return tmpString.ToString()
		End Function

		Public Overloads Function ReadString(count As UInteger) As String
			Dim stringArray As Byte() = ReadBytes(count)
			Return Encoding.ASCII.GetString(stringArray)
		End Function

		Public Overloads Function ReadBytes(count As UInteger) As Byte()
			Return MyBase.ReadBytes(CInt(count))
		End Function

		Public Function ReadStringFromBytes(count As UInteger) As String
			Dim stringArray As Byte() = ReadBytes(count)
			Array.Reverse(stringArray)

			Return Encoding.ASCII.GetString(stringArray)
		End Function

		Public Function ReadIPAddress() As String
			Dim ip As Byte() = New Byte(3) {}

			For i As Integer = 0 To 3
				ip(i) = ReadUInt8()
			Next

			Return ip(0) & "." & ip(1) & "." & ip(2) & "." & ip(3)
		End Function

		Public Function ReadAccountName() As String
			Dim nameBuilder As New StringBuilder()

			Dim nameLength As Byte = ReadUInt8()
			Dim name As Char() = New Char(nameLength - 1) {}

			For i As Integer = 0 To nameLength - 1
				name(i) = MyBase.ReadChar()
				nameBuilder.Append(name(i))
			Next

			Return nameBuilder.ToString().ToUpper()
		End Function

		Public Sub Skip(count As Integer)
			MyBase.BaseStream.Position += count
		End Sub
	End Class
End Namespace
