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


Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports System.Collections
Imports WorldServer.Game.Spawns

Namespace Game.WorldEntities
	Public Class WorldObject
		Inherits Globals
		' General object data
		Public Guid As UInt64
		Public Position As Vector4
		Public Map As UInt32

		' Some data
		Public TargetGuid As UInt64

		Public Property IsInWorld() As Boolean
			Get
				Return m_IsInWorld
			End Get
			Set
				m_IsInWorld = Value
			End Set
		End Property
		Private m_IsInWorld As Boolean
		Public MaskSize As Integer
		Public Mask As BitArray
		Public UpdateData As New Hashtable()

		Public Sub New()
		End Sub

		Public Sub New(dataLength As Integer)
			IsInWorld = False
			MaskSize = (dataLength + 32) \ 32
			Mask = New BitArray(dataLength, False)
		End Sub

		Public Function CheckUpdateDistance(obj As WorldObject) As Boolean
			If Map = obj.Map Then
				Dim disX As Single = CSng(Math.Pow(Position.X - obj.Position.X, 2))
				Dim disY As Single = CSng(Math.Pow(Position.Y - obj.Position.Y, 2))
				Dim disZ As Single = CSng(Math.Pow(Position.Z - obj.Position.Z, 2))

				Dim distance As Single = disX + disY + disZ

				Return If(distance <= 10000, True, False)
			End If

			Return False
		End Function

		Public Overridable Sub SetUpdateFields()
		End Sub

		Public Sub SetUpdateField(Of T)(index As Integer, value As T, Optional offset As Byte = 0)
			Select Case value.[GetType]().Name
				Case "SByte", "Int16"
					If True Then
						Mask.[Set](index, True)

						If UpdateData.ContainsKey(index) Then
							UpdateData(index) = CInt(CInt(UpdateData(index)) Or CInt(CInt(Convert.ChangeType(value, GetType(Integer))) << (offset * (If(value.[GetType]().Name = "Byte", 8, 16)))))
						Else
							UpdateData(index) = CInt(CInt(Convert.ChangeType(value, GetType(Integer))) << (offset * (If(value.[GetType]().Name = "Byte", 8, 16))))
						End If

						Exit Select
					End If
				Case "Byte", "UInt16"
					If True Then
						Mask.[Set](index, True)

						If UpdateData.ContainsKey(index) Then
							UpdateData(index) = CUInt(CUInt(UpdateData(index)) Or CUInt(CUInt(Convert.ChangeType(value, GetType(UInteger))) << (offset * (If(value.[GetType]().Name = "Byte", 8, 16)))))
						Else
							UpdateData(index) = CUInt(CUInt(Convert.ChangeType(value, GetType(UInteger))) << (offset * (If(value.[GetType]().Name = "Byte", 8, 16))))
						End If

						Exit Select
					End If
				Case "Int64"
					If True Then
						Mask.[Set](index, True)
						Mask.[Set](index + 1, True)

						Dim tmpValue As Long = CLng(Convert.ChangeType(value, GetType(Long)))

						UpdateData(index) = CUInt(tmpValue And Int32.MaxValue)
						UpdateData(index + 1) = CUInt((tmpValue >> 32) And Int32.MaxValue)

						Exit Select
					End If
				Case "UInt64"
					If True Then
						Mask.[Set](index, True)
						Mask.[Set](index + 1, True)

						Dim tmpValue As ULong = CULng(Convert.ChangeType(value, GetType(ULong)))

						UpdateData(index) = CUInt(tmpValue And UInt32.MaxValue)
						UpdateData(index + 1) = CUInt((tmpValue >> 32) And UInt32.MaxValue)

						Exit Select
					End If
				Case Else
					If True Then
						Mask.[Set](index, True)
						UpdateData(index) = value

						Exit Select
					End If
			End Select
		End Sub

		Public Sub WriteUpdateFields(ByRef packet As PacketWriter, Optional sendAllFields As Boolean = False)
			packet.WriteUInt8(CByte(MaskSize))
			packet.WriteBitArray(Mask, MaskSize * 4)
			' Int32 = 4 Bytes
			For i As Integer = 0 To Mask.Count - 1
				If Mask.[Get](i) Then
					Try
						Select Case UpdateData(i).[GetType]().Name
							Case "UInt32"
								packet.WriteUInt32(CUInt(UpdateData(i)))
								Exit Select
							Case "Single"
								packet.WriteFloat(CSng(UpdateData(i)))
								Exit Select
							Case Else
								packet.WriteInt32(CInt(UpdateData(i)))
								Exit Select
						End Select
					Catch
						If sendAllFields Then
							packet.WriteInt32(0)
						End If
					End Try
				End If
			Next
		End Sub

		Public Sub WriteDynamicUpdateFields(ByRef packet As PacketWriter)
			packet.WriteUInt8(0)
		End Sub

		Public Sub RemoveFromWorld()

		End Sub

		Public Function ToCharacter() As Character
			Return TryCast(Me, Character)
		End Function

		Public Function ToCreature() As CreatureSpawn
			Return TryCast(Me, CreatureSpawn)
		End Function

		Public Function ToGameObject() As GameObjectSpawn
			Return TryCast(Me, GameObjectSpawn)
		End Function
	End Class
End Namespace
