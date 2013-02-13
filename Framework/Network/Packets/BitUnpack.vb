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
	Public Class BitUnpack
		Public reader As PacketReader
		Private Position As Integer
		Private Value As Byte

		Public Sub New(reader As PacketReader)
			Me.reader = reader
			Position = 8
			Value = 0
		End Sub

		Public Function GetBit() As Boolean
			If Position = 8 Then
				Value = reader.ReadUInt8()
				Position = 0
			End If

			Dim returnValue As Integer = Value
			Value = CByte(2 * returnValue)
			Position += 1

			Return Convert.ToBoolean(returnValue >> 7)
		End Function

		Public Function GetBits(Of T)(bitCount As Byte) As T
			Dim returnValue As Integer = 0

            For i As Integer = bitCount - 1 To 0 Step -1
                returnValue = If(GetBit(), (1 << i) Or returnValue, returnValue)
            Next

			Return DirectCast(Convert.ChangeType(returnValue, GetType(T)), T)
		End Function

		Public Function GetNameLength(Of T)(bitCount As Byte) As T
			Dim returnValue As Integer = 0

			' Unknown, always before namelength bits...
			GetBit()

            For i As Integer = bitCount - 1 To 0 Step -1
                returnValue = If(GetBit(), (1 << i) Or returnValue, returnValue)
            Next

			Return DirectCast(Convert.ChangeType(returnValue, GetType(T)), T)
		End Function

		Public Function GetGuid(mask As Byte(), bytes As Byte()) As UInt64
			Dim guidMask As Boolean() = New Boolean(mask.Length - 1) {}
			Dim guidBytes As Byte() = New Byte(bytes.Length - 1) {}

			For i As Integer = 0 To guidMask.Length - 1
				guidMask(i) = GetBit()
			Next

            For i As Integer = 0 To bytes.Length - 1
                If guidMask(mask(i)) Then
                    guidBytes(bytes(i)) = CByte(reader.ReadUInt8() Xor 1)
                End If
            Next

			Return BitConverter.ToUInt64(guidBytes, 0)
		End Function
	End Class
End Namespace
