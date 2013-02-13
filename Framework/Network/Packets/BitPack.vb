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



Namespace Network.Packets
	Public Class BitPack
		Private writer As PacketWriter

		Private GuidBytes As Byte()
		Private GuildGuidBytes As Byte()
		Private TransportGuidBytes As Byte()
		Private TargetGuidBytes As Byte()

        Private Property BitPosition() As Integer
            Get
                Return m_BitPosition
            End Get
            Set(value As Integer)
                m_BitPosition = Value
            End Set
        End Property
        Private m_BitPosition As Integer
		Private Property BitValue() As Byte
			Get
				Return m_BitValue
			End Get
			Set
				m_BitValue = Value
			End Set
		End Property
		Private m_BitValue As Byte

		Public WriteOnly Property Guid() As UInt64
			Set
				GuidBytes = BitConverter.GetBytes(value)
			End Set
		End Property
		Public WriteOnly Property GuildGuid() As UInt64
			Set
				GuildGuidBytes = BitConverter.GetBytes(value)
			End Set
		End Property
		Public WriteOnly Property TargetGuid() As UInt64
			Set
				TargetGuidBytes = BitConverter.GetBytes(value)
			End Set
		End Property
		Public WriteOnly Property TransportGuid() As UInt64
			Set
				TransportGuidBytes = BitConverter.GetBytes(value)
			End Set
		End Property

		Public Sub New(writer As PacketWriter, Optional guid__1 As UInt64 = 0, Optional guildGuid__2 As UInt64 = 0, Optional targetGuid__3 As UInt64 = 0, Optional transportGuid__4 As UInt64 = 0)
			Guid = guid__1
			GuildGuid = guildGuid__2
			TargetGuid = targetGuid__3
			TransportGuid = transportGuid__4

			Me.writer = writer
			BitPosition = 8
		End Sub

		Public Sub Write(Of T)(bit As T)
			BitPosition -= 1

			If Convert.ToBoolean(bit) Then
				BitValue = BitValue Or CByte(1 << (BitPosition))
			End If

			If BitPosition = 0 Then
				BitPosition = 8
				writer.WriteUInt8(BitValue)
				BitValue = 0
			End If
		End Sub

		Public Sub Write(Of T)(bit As T, count As Integer)
			For i As Integer = count - 1 To 0 Step -1
				Write(Of T)(DirectCast(Convert.ChangeType(((Convert.ToInt32(bit) >> i) And 1), GetType(T)), T))
			Next
		End Sub

		Public Sub WriteGuidMask(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                Write(GuidBytes(order(i)))
            Next
		End Sub

		Public Sub WriteGuildGuidMask(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                Write(GuildGuidBytes(order(i)))
            Next
		End Sub

		Public Sub WriteTargetGuidMask(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                Write(TargetGuidBytes(order(i)))
            Next
		End Sub

		Public Sub WriteTransportGuidMask(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                Write(TransportGuidBytes(order(i)))
            Next
		End Sub

		Public Sub WriteGuidBytes(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                If GuidBytes(order(i)) <> 0 Then
                    writer.WriteUInt8(CByte(GuidBytes(order(i)) Xor 1))
                End If
            Next
		End Sub

		Public Sub WriteGuildGuidBytes(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                If GuildGuidBytes(order(i)) <> 0 Then
                    writer.WriteUInt8(CByte(GuildGuidBytes(order(i)) Xor 1))
                End If
            Next
		End Sub

		Public Sub WriteTargetGuidBytes(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                If TargetGuidBytes(order(i)) <> 0 Then
                    writer.WriteUInt8(CByte(TargetGuidBytes(order(i)) Xor 1))
                End If
            Next
		End Sub

		Public Sub WriteTransportGuidBytes(ParamArray order As Byte())
            For i As Integer = 0 To order.Length - 1
                If TransportGuidBytes(order(i)) <> 0 Then
                    writer.WriteUInt8(CByte(TransportGuidBytes(order(i)) Xor 1))
                End If
            Next
		End Sub

		Public Sub Flush()
			If BitPosition = 8 Then
				Return
			End If

			writer.WriteUInt8(BitValue)
			BitValue = 0
			BitPosition = 8
		End Sub
	End Class
End Namespace
