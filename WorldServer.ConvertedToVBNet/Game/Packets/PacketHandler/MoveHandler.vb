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
Imports Framework.Constants.Movement
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class MoveHandler
		Inherits Globals
		<Opcode(ClientMessage.MoveStartForward, "16357")> _
		Public Shared Sub HandleMoveStartForward(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim movementValues As New ObjectMovementValues()
			Dim BitUnpack As New BitUnpack(packet)

			Dim guidMask As Boolean() = New Boolean(7) {}
			Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                 .X = packet.ReadFloat(), _
                 .Y = packet.ReadFloat(), _
                 .Z = packet.ReadFloat() _
            }

			guidMask(5) = BitUnpack.GetBit()
			guidMask(4) = BitUnpack.GetBit()

			Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

			guidMask(3) = BitUnpack.GetBit()

			movementValues.IsTransport = BitUnpack.GetBit()
			movementValues.HasRotation = Not BitUnpack.GetBit()

			Dim Unknown As Boolean = BitUnpack.GetBit()

			guidMask(6) = BitUnpack.GetBit()

			movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

			guidMask(0) = BitUnpack.GetBit()
			guidMask(2) = BitUnpack.GetBit()

			Dim Unknown2 As Boolean = BitUnpack.GetBit()
			Dim Unknown3 As Boolean = BitUnpack.GetBit()

			guidMask(7) = BitUnpack.GetBit()

			movementValues.IsAlive = Not BitUnpack.GetBit()

			Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

			guidMask(1) = BitUnpack.GetBit()

			Dim HasTime As Boolean = Not BitUnpack.GetBit()
			Dim HasPitch As Boolean = Not BitUnpack.GetBit()

			movementValues.HasMovementFlags = Not BitUnpack.GetBit()
			Dim Unknown4 As Boolean = BitUnpack.GetBit()


			If movementValues.HasMovementFlags2 Then
				movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
			End If

			'if (movementValues.IsTransport)
'            {
'
'            }
'            
'            if (IsInterpolated)
'            {
'
'            }


			If movementValues.HasMovementFlags Then
				movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
			End If

			If guidMask(1) Then
				guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
			End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

			If guidMask(0) Then
				guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(4) Then
				guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(2) Then
				guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(5) Then
				guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(3) Then
				guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(7) Then
				guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(6) Then
				guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
			End If

			'if (movementValues.IsTransport)
'            {
'
'            }
'            
'            if (IsInterpolated)
'            {
'
'            }


			If movementValues.IsAlive Then
				movementValues.Time = packet.ReadUInt32()
			End If

			If movementValues.HasRotation Then
				vector.O = packet.ReadFloat()
			End If

			If HasSplineElevation Then
				packet.ReadFloat()
			End If

			If HasTime Then
				movementValues.Time = packet.ReadUInt32()
			End If

			If HasPitch Then
				packet.ReadFloat()
			End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
			HandleMoveUpdate(guid, movementValues, vector)
		End Sub

		<Opcode(ClientMessage.MoveStartBackward, "16357")> _
		Public Shared Sub HandleMoveStartBackward(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim movementValues As New ObjectMovementValues()
			Dim BitUnpack As New BitUnpack(packet)

			Dim guidMask As Boolean() = New Boolean(7) {}
			Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .X = packet.ReadFloat(), _
                .Z = packet.ReadFloat(), _
                .Y = packet.ReadFloat() _
            }

			guidMask(3) = BitUnpack.GetBit()
			guidMask(6) = BitUnpack.GetBit()

			movementValues.HasMovementFlags = Not BitUnpack.GetBit()

			movementValues.IsInterpolated = BitUnpack.GetBit()
			Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

			movementValues.HasRotation = Not BitUnpack.GetBit()

			guidMask(4) = BitUnpack.GetBit()

			movementValues.IsAlive = Not BitUnpack.GetBit()

			guidMask(1) = BitUnpack.GetBit()

			movementValues.IsTransport = BitUnpack.GetBit()

			Dim Unknown2 As Boolean = BitUnpack.GetBit()

			guidMask(0) = BitUnpack.GetBit()

			Dim Unknown As Boolean = BitUnpack.GetBit()

			movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

			guidMask(2) = BitUnpack.GetBit()

			Dim HasPitch As Boolean = Not BitUnpack.GetBit()
			Dim Unknown3 As Boolean = BitUnpack.GetBit()

			guidMask(5) = BitUnpack.GetBit()
			guidMask(7) = BitUnpack.GetBit()

			Dim HasTime As Boolean = Not BitUnpack.GetBit()

			Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

			'if (movementValues.IsTransport)
'            {
'
'            }


			If movementValues.IsInterpolated Then
				movementValues.IsInterpolated2 = BitUnpack.GetBit()
			End If

			If movementValues.HasMovementFlags2 Then
				movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
			End If

			If movementValues.HasMovementFlags Then
				movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
			End If

			If guidMask(6) Then
				guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(4) Then
				guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(0) Then
				guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(1) Then
				guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(5) Then
				guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(2) Then
				guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
			End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

			If guidMask(7) Then
				guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
			End If
			If guidMask(3) Then
				guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
			End If

			'if (movementValues.IsTransport)
'            {
'
'            }


			If HasSplineElevation Then
				packet.ReadFloat()
			End If

			If movementValues.IsInterpolated Then
				If movementValues.IsInterpolated2 Then
					packet.ReadFloat()
					packet.ReadFloat()
					packet.ReadFloat()
				End If

				packet.ReadUInt32()
				packet.ReadFloat()
			End If

			If HasTime Then
				movementValues.Time = packet.ReadUInt32()
			End If

			If HasPitch Then
				packet.ReadFloat()
			End If

			If movementValues.HasRotation Then
				vector.O = packet.ReadFloat()
			End If

			If movementValues.IsAlive Then
				movementValues.Time = packet.ReadUInt32()
			End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
			HandleMoveUpdate(guid, movementValues, vector)
		End Sub

		<Opcode(ClientMessage.MoveHeartBeat, "16357")> _
		Public Shared Sub HandleMoveHeartBeat(ByRef packet As PacketReader, ByRef session As WorldClass)
			Dim movementValues As New ObjectMovementValues()
			Dim BitUnpack As New BitUnpack(packet)

			Dim guidMask As Boolean() = New Boolean(7) {}
			Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .X = packet.ReadFloat(), _
                .Y = packet.ReadFloat(), _
                .Z = packet.ReadFloat() _
            }

            movementValues.HasMovementFlags = Not BitUnpack.GetBit()
            movementValues.IsInterpolated = BitUnpack.GetBit()

            Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

            movementValues.IsAlive = Not BitUnpack.GetBit()
            movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

            Dim HasPitch As Boolean = Not BitUnpack.GetBit()

            guidMask(4) = BitUnpack.GetBit()

            movementValues.IsTransport = BitUnpack.GetBit()

            guidMask(7) = BitUnpack.GetBit()
            guidMask(0) = BitUnpack.GetBit()

            Dim Unknown2 As Boolean = BitUnpack.GetBit()

            guidMask(3) = BitUnpack.GetBit()

            Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

            guidMask(1) = BitUnpack.GetBit()

            Dim Unknown3 As Boolean = BitUnpack.GetBit()

            guidMask(5) = BitUnpack.GetBit()
            guidMask(2) = BitUnpack.GetBit()

            movementValues.HasRotation = Not BitUnpack.GetBit()
            Dim Unknown4 As Boolean = BitUnpack.GetBit()

            guidMask(6) = BitUnpack.GetBit()

            Dim HasTime As Boolean = Not BitUnpack.GetBit()

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.IsInterpolated Then
                movementValues.IsInterpolated2 = BitUnpack.GetBit()
            End If

            If movementValues.HasMovementFlags Then
                movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
            End If

            If movementValues.HasMovementFlags2 Then
                movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
            End If

            If guidMask(7) Then
                guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
            End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

            If guidMask(1) Then
                guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(3) Then
                guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(0) Then
                guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(5) Then
                guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(4) Then
                guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(6) Then
                guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(2) Then
                guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
            End If

            If movementValues.IsInterpolated Then
                If movementValues.IsInterpolated2 Then
                    packet.ReadFloat()
                    packet.ReadFloat()
                    packet.ReadFloat()
                End If

                packet.ReadFloat()
                packet.ReadUInt32()
            End If


            If HasTime Then
                movementValues.Time = packet.ReadUInt32()
            End If

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.HasRotation Then
                vector.O = packet.ReadFloat()
            End If

            If HasPitch Then
                packet.ReadFloat()
            End If

            If movementValues.IsAlive Then
                movementValues.Time = packet.ReadUInt32()
            End If

            If HasSplineElevation Then
                packet.ReadFloat()
            End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
            HandleMoveUpdate(guid, movementValues, vector)
        End Sub

        <Opcode(ClientMessage.MoveStop, "16357")> _
        Public Shared Sub HandleMoveStop(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim movementValues As New ObjectMovementValues()
            Dim BitUnpack As New BitUnpack(packet)

            Dim guidMask As Boolean() = New Boolean(7) {}
            Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .X = packet.ReadFloat(), _
                .Z = packet.ReadFloat(), _
                .Y = packet.ReadFloat() _
            }

            movementValues.HasMovementFlags = Not BitUnpack.GetBit()
            movementValues.IsTransport = BitUnpack.GetBit()

            Dim HasPitch As Boolean = Not BitUnpack.GetBit()

            movementValues.HasRotation = Not BitUnpack.GetBit()

            Dim Unknown As Boolean = BitUnpack.GetBit()
            Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

            Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

            Dim HasTime As Boolean = Not BitUnpack.GetBit()

            guidMask(4) = BitUnpack.GetBit()

            Dim Unknown2 As Boolean = BitUnpack.GetBit()

            guidMask(6) = BitUnpack.GetBit()
            guidMask(0) = BitUnpack.GetBit()
            guidMask(5) = BitUnpack.GetBit()
            guidMask(1) = BitUnpack.GetBit()

            movementValues.IsAlive = Not BitUnpack.GetBit()

            guidMask(7) = BitUnpack.GetBit()
            guidMask(2) = BitUnpack.GetBit()

            Dim Unknown3 As Boolean = BitUnpack.GetBit()

            guidMask(3) = BitUnpack.GetBit()

            movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()
            Dim Unknown4 As Boolean = BitUnpack.GetBit()

            'if (IsInterpolated)
            '            {
            '
            '            }
            '            
            '            if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.HasMovementFlags2 Then
                movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
            End If

            If movementValues.HasMovementFlags Then
                movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
            End If

            If guidMask(1) Then
                guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(2) Then
                guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(4) Then
                guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(3) Then
                guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
            End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

            If guidMask(5) Then
                guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(0) Then
                guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(6) Then
                guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(7) Then
                guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
            End If

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If HasTime Then
                movementValues.Time = packet.ReadUInt32()
            End If

            If HasPitch Then
                packet.ReadFloat()
            End If

            'if (IsInterpolated)
            '            {
            '
            '            }


            If movementValues.IsAlive Then
                movementValues.Time = packet.ReadUInt32()
            End If

            If movementValues.HasRotation Then
                vector.O = packet.ReadFloat()
            End If

            If HasSplineElevation Then
                packet.ReadFloat()
            End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
            HandleMoveUpdate(guid, movementValues, vector)
        End Sub

        <Opcode(ClientMessage.MoveStartTurnLeft, "16357")> _
        Public Shared Sub HandleMoveStartTurnLeft(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim movementValues As New ObjectMovementValues()
            Dim BitUnpack As New BitUnpack(packet)

            Dim guidMask As Boolean() = New Boolean(7) {}
            Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .Z = packet.ReadFloat(), _
                .Y = packet.ReadFloat(), _
                .X = packet.ReadFloat() _
            }

            Dim Unknown As Boolean = BitUnpack.GetBit()
            Dim Unknown2 As Boolean = BitUnpack.GetBit()

            Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

            guidMask(2) = BitUnpack.GetBit()
            guidMask(4) = BitUnpack.GetBit()
            guidMask(7) = BitUnpack.GetBit()
            guidMask(1) = BitUnpack.GetBit()

            Dim HasPitch As Boolean = Not BitUnpack.GetBit()

            guidMask(0) = BitUnpack.GetBit()

            movementValues.IsAlive = Not BitUnpack.GetBit()
            movementValues.IsTransport = BitUnpack.GetBit()

            Dim Unknown3 As Boolean = BitUnpack.GetBit()

            guidMask(6) = BitUnpack.GetBit()

            movementValues.HasMovementFlags = Not BitUnpack.GetBit()

            Dim Unknown4 As Boolean = BitUnpack.GetBit()
            movementValues.HasRotation = Not BitUnpack.GetBit()

            movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

            guidMask(3) = BitUnpack.GetBit()
            guidMask(5) = BitUnpack.GetBit()

            Dim HasTime As Boolean = Not BitUnpack.GetBit()
            Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

            'if (movementValues.IsTransport)
            '            {
            '
            '            }
            '            
            '            if (IsInterpolated)
            '            {
            '
            '            }


            If movementValues.HasMovementFlags2 Then
                movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
            End If

            If movementValues.HasMovementFlags Then
                movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
            End If

            If guidMask(4) Then
                guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(6) Then
                guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
            End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

            If guidMask(1) Then
                guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(2) Then
                guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(0) Then
                guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(7) Then
                guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(5) Then
                guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(3) Then
                guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
            End If

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.IsAlive Then
                movementValues.Time = packet.ReadUInt32()
            End If

            If HasPitch Then
                packet.ReadFloat()
            End If

            'if (IsInterpolated)
            '            {
            '            
            '            }



            If movementValues.HasRotation Then
                vector.O = packet.ReadFloat()
            End If

            If HasSplineElevation Then
                packet.ReadFloat()
            End If

            If HasTime Then
                movementValues.Time = packet.ReadUInt32()
            End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
            HandleMoveUpdate(guid, movementValues, vector)
        End Sub

        <Opcode(ClientMessage.MoveStartTurnRight, "16357")> _
        Public Shared Sub HandleMoveStartTurnRight(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim movementValues As New ObjectMovementValues()
            Dim BitUnpack As New BitUnpack(packet)

            Dim guidMask As Boolean() = New Boolean(7) {}
            Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .Y = packet.ReadFloat(), _
                .Z = packet.ReadFloat(), _
                .X = packet.ReadFloat() _
            }

            guidMask(5) = BitUnpack.GetBit()
            guidMask(3) = BitUnpack.GetBit()

            Dim HasTime As Boolean = Not BitUnpack.GetBit()

            guidMask(1) = BitUnpack.GetBit()

            movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

            guidMask(0) = BitUnpack.GetBit()

            Dim Unknown4 As Boolean = BitUnpack.GetBit()

            Dim Unknown As Boolean = BitUnpack.GetBit()

            movementValues.HasRotation = Not BitUnpack.GetBit()

            Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

            Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

            guidMask(4) = BitUnpack.GetBit()

            movementValues.IsTransport = BitUnpack.GetBit()

            Dim Unknown2 As Boolean = BitUnpack.GetBit()

            guidMask(2) = BitUnpack.GetBit()

            movementValues.IsAlive = Not BitUnpack.GetBit()

            Dim HasPitch As Boolean = Not BitUnpack.GetBit()

            movementValues.HasMovementFlags = Not BitUnpack.GetBit()

            guidMask(7) = BitUnpack.GetBit()
            guidMask(6) = BitUnpack.GetBit()

            Dim Unknown3 As Boolean = BitUnpack.GetBit()

            If movementValues.HasMovementFlags Then
                movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
            End If

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.HasMovementFlags2 Then
                movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
            End If

            'if (IsInterpolated)
            '            {
            '
            '            }


            If guidMask(3) Then
                guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(5) Then
                guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(4) Then
                guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(6) Then
                guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
            End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

            If guidMask(2) Then
                guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(7) Then
                guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(1) Then
                guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(0) Then
                guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
            End If


            If movementValues.HasRotation Then
                vector.O = packet.ReadFloat()
            End If

            'if (IsInterpolated)
            '            {
            '
            '            }


            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If HasTime Then
                movementValues.Time = packet.ReadUInt32()
            End If

            If HasPitch Then
                packet.ReadFloat()
            End If

            If HasSplineElevation Then
                packet.ReadFloat()
            End If

            If movementValues.IsAlive Then
                movementValues.Time = packet.ReadUInt32()
            End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
            HandleMoveUpdate(guid, movementValues, vector)
        End Sub

        <Opcode(ClientMessage.MoveStopTurn, "16357")> _
        Public Shared Sub HandleMoveStopTurn(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim movementValues As New ObjectMovementValues()
            Dim BitUnpack As New BitUnpack(packet)

            Dim guidMask As Boolean() = New Boolean(7) {}
            Dim guidBytes As Byte() = New Byte(7) {}

            Dim vector As New Vector4() With { _
                .X = packet.ReadFloat(), _
                .Z = packet.ReadFloat(), _
                .Y = packet.ReadFloat() _
            }

            Dim HasTime As Boolean = Not BitUnpack.GetBit()

            guidMask(5) = BitUnpack.GetBit()

            Dim Unknown As Boolean = BitUnpack.GetBit()

            movementValues.IsTransport = BitUnpack.GetBit()

            Dim Unknown2 As Boolean = BitUnpack.GetBit()

            guidMask(3) = BitUnpack.GetBit()

            Dim HasSplineElevation As Boolean = Not BitUnpack.GetBit()

            guidMask(0) = BitUnpack.GetBit()

            Dim HasPitch As Boolean = Not BitUnpack.GetBit()

            Dim counter As UInteger = BitUnpack.GetBits(Of UInteger)(24)

            guidMask(1) = BitUnpack.GetBit()
            guidMask(7) = BitUnpack.GetBit()

            movementValues.HasMovementFlags = Not BitUnpack.GetBit()
            movementValues.IsAlive = Not BitUnpack.GetBit()

            guidMask(2) = BitUnpack.GetBit()
            guidMask(6) = BitUnpack.GetBit()

            movementValues.HasRotation = Not BitUnpack.GetBit()

            Dim Unknown3 As Boolean = BitUnpack.GetBit()

            movementValues.HasMovementFlags2 = Not BitUnpack.GetBit()

            Dim Unknown4 As Boolean = BitUnpack.GetBit()

            guidMask(4) = BitUnpack.GetBit()

            'if (movementValues.IsTransport)
            '            {
            '
            '            }
            '            
            '            if (IsInterpolated)
            '            {
            '
            '            }


            If movementValues.HasMovementFlags Then
                movementValues.MovementFlags = CType(BitUnpack.GetBits(Of UInteger)(30), MovementFlag)
            End If

            If movementValues.HasMovementFlags2 Then
                movementValues.MovementFlags2 = CType(BitUnpack.GetBits(Of UInteger)(13), MovementFlag2)
            End If

            If guidMask(6) Then
                guidBytes(6) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(0) Then
                guidBytes(0) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(5) Then
                guidBytes(5) = CByte(packet.ReadUInt8() Xor 1)
            End If

            For i As Long = 0 To counter - 1
                packet.ReadUInt32()
            Next

            If guidMask(1) Then
                guidBytes(1) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(7) Then
                guidBytes(7) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(3) Then
                guidBytes(3) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(4) Then
                guidBytes(4) = CByte(packet.ReadUInt8() Xor 1)
            End If
            If guidMask(2) Then
                guidBytes(2) = CByte(packet.ReadUInt8() Xor 1)
            End If

            'if (movementValues.IsTransport)
            '            {
            '
            '            }


            If movementValues.HasRotation Then
                vector.O = packet.ReadFloat()
            End If

            If movementValues.IsAlive Then
                movementValues.Time = packet.ReadUInt32()
            End If

            'if (IsInterpolated)
            '            {
            '
            '            }


            If HasPitch Then
                packet.ReadFloat()
            End If

            If HasSplineElevation Then
                packet.ReadFloat()
            End If

            If HasTime Then
                movementValues.Time = packet.ReadUInt32()
            End If

            Dim guid As ULong = BitConverter.ToUInt64(guidBytes, 0)
            HandleMoveUpdate(guid, movementValues, vector)
        End Sub

		Public Shared Sub HandleMoveUpdate(guid As ULong, movementValues As ObjectMovementValues, vector As Vector4)
			Dim moveUpdate As New PacketWriter(JAMCMessage.MoveUpdate)
			Dim BitPack As New BitPack(moveUpdate, guid)

			BitPack.WriteGuidMask(0)
			BitPack.Write(Not movementValues.HasMovementFlags)
			BitPack.Write(Not movementValues.HasRotation)
			BitPack.WriteGuidMask(2, 6)
			BitPack.Write(Not movementValues.HasMovementFlags2)
			BitPack.WriteGuidMask(7)
			BitPack.Write(Of UInteger)(0, 24)
			BitPack.WriteGuidMask(1)

			If movementValues.HasMovementFlags Then
				BitPack.Write(CUInt(movementValues.MovementFlags), 30)
			End If

			BitPack.WriteGuidMask(4)
			BitPack.Write(Not movementValues.IsAlive)
			BitPack.Write(0)

			If movementValues.HasMovementFlags2 Then
				BitPack.Write(CUInt(movementValues.MovementFlags2), 13)
			End If

			BitPack.Write(0)
			BitPack.WriteGuidMask(5)
			BitPack.Write(True)
			BitPack.Write(0)
			BitPack.Write(movementValues.IsInterpolated)
			BitPack.Write(0)
			BitPack.Write(True)
			BitPack.WriteGuidMask(3)
			BitPack.Write(True)

			If movementValues.IsInterpolated Then
				BitPack.Write(movementValues.IsInterpolated2)
			End If

			BitPack.Flush()

			If movementValues.IsInterpolated Then
				moveUpdate.WriteUInt32(0)

				If movementValues.IsInterpolated2 Then
					moveUpdate.WriteFloat(0)
					moveUpdate.WriteFloat(0)
					moveUpdate.WriteFloat(0)
				End If

				moveUpdate.WriteFloat(0)
			End If

			BitPack.WriteGuidBytes(2)

			If movementValues.IsAlive Then
				moveUpdate.WriteUInt32(movementValues.Time)
			End If

			BitPack.WriteGuidBytes(5, 7)

			moveUpdate.WriteFloat(vector.Z)

			BitPack.WriteGuidBytes(4, 3, 1, 6, 0)

			moveUpdate.WriteFloat(vector.X)

			If movementValues.HasRotation Then
				moveUpdate.WriteFloat(vector.O)
			End If

			moveUpdate.WriteFloat(vector.Y)

            Dim session As WorldClass = WorldMgr.GetSession(guid)
			If session IsNot Nothing Then
				Dim pChar As Character = session.Character

				ObjectMgr.SetPosition(pChar, vector, False)
				WorldMgr.SendToInRangeCharacter(pChar, moveUpdate)
			End If
		End Sub

		Public Shared Sub HandleMoveSetWalkSpeed(ByRef session As WorldClass, Optional speed As Single = 2.5F)
			Dim setWalkSpeed As New PacketWriter(JAMCMessage.MoveSetWalkSpeed)
			Dim BitPack As New BitPack(setWalkSpeed, session.Character.Guid)

			setWalkSpeed.WriteUInt32(0)
			setWalkSpeed.WriteFloat(speed)

			BitPack.WriteGuidMask(6, 2, 1, 4, 5, 3, _
				7, 0)
			BitPack.Flush()

			BitPack.WriteGuidBytes(1, 6, 3, 0, 7, 4, _
				2, 5)

			session.Send(setWalkSpeed)
		End Sub

		Public Shared Sub HandleMoveSetRunSpeed(ByRef session As WorldClass, Optional speed As Single = 7F)
			Dim setRunSpeed As New PacketWriter(JAMCMessage.MoveSetRunSpeed)
			Dim BitPack As New BitPack(setRunSpeed, session.Character.Guid)

			BitPack.WriteGuidMask(0, 4, 1, 6, 3, 5, _
				7, 2)
			BitPack.Flush()

			setRunSpeed.WriteFloat(speed)
			BitPack.WriteGuidBytes(7)
			setRunSpeed.WriteUInt32(0)
			BitPack.WriteGuidBytes(3, 6, 0, 4, 1, 5, _
				2)

			session.Send(setRunSpeed)
		End Sub

		Public Shared Sub HandleMoveSetSwimSpeed(ByRef session As WorldClass, Optional speed As Single = 4.72222F)
			Dim setSwimSpeed As New PacketWriter(JAMCMessage.MoveSetSwimSpeed)
			Dim BitPack As New BitPack(setSwimSpeed, session.Character.Guid)

			BitPack.WriteGuidMask(4, 0, 7, 5, 6, 1, _
				2, 3)
			BitPack.Flush()

			setSwimSpeed.WriteUInt32(0)

			BitPack.WriteGuidBytes(3, 7, 0, 1, 4, 5, _
				2)
			setSwimSpeed.WriteFloat(speed)
			BitPack.WriteGuidBytes(6)

			session.Send(setSwimSpeed)
		End Sub

		Public Shared Sub HandleMoveSetFlightSpeed(ByRef session As WorldClass, Optional speed As Single = 7F)
			Dim setFlightSpeed As New PacketWriter(JAMCMessage.MoveSetFlightSpeed)
			Dim BitPack As New BitPack(setFlightSpeed, session.Character.Guid)

			BitPack.WriteGuidMask(6, 1, 7, 4, 5, 3, _
				0, 2)
			BitPack.Flush()

			BitPack.WriteGuidBytes(0, 4, 6)
			setFlightSpeed.WriteFloat(speed)
			BitPack.WriteGuidBytes(7, 2)
			setFlightSpeed.WriteUInt32(0)
			BitPack.WriteGuidBytes(5, 1, 3)

			session.Send(setFlightSpeed)
		End Sub

		Public Shared Sub HandleMoveSetCanFly(ByRef session As WorldClass)
			Dim setCanFly As New PacketWriter(JAMCMessage.MoveSetCanFly)
			Dim BitPack As New BitPack(setCanFly, session.Character.Guid)

			setCanFly.WriteUInt32(0)

			BitPack.WriteGuidMask(4, 5, 0, 6, 2, 1, _
				3, 7)
			BitPack.Flush()

			BitPack.WriteGuidBytes(5, 3, 1, 6, 7, 2, _
				4, 0)

			session.Send(setCanFly)
		End Sub

		Public Shared Sub HandleMoveUnsetCanFly(ByRef session As WorldClass)
			Dim unsetCanFly As New PacketWriter(JAMCMessage.MoveUnsetCanFly)
			Dim BitPack As New BitPack(unsetCanFly, session.Character.Guid)

			unsetCanFly.WriteUInt32(0)

			BitPack.WriteGuidMask(1, 4, 6, 2, 5, 7, _
				0, 3)
			BitPack.Flush()

			BitPack.WriteGuidBytes(3, 1, 5, 7, 0, 6, _
				2, 4)

			session.Send(unsetCanFly)
		End Sub

		Public Shared Sub HandleMoveTeleport(ByRef session As WorldClass, vector As Vector4)
			Dim IsTransport As Boolean = False
			Dim Unknown As Boolean = False

			Dim moveTeleport As New PacketWriter(JAMCMessage.MoveTeleport)
			Dim BitPack As New BitPack(moveTeleport, session.Character.Guid)

			moveTeleport.WriteUInt32(0)
			moveTeleport.WriteFloat(vector.X)
			moveTeleport.WriteFloat(vector.Y)
			moveTeleport.WriteFloat(vector.Z)
			moveTeleport.WriteFloat(vector.O)

			BitPack.WriteGuidMask(3, 1, 7)
			BitPack.Write(Unknown)
			BitPack.WriteGuidMask(6)

			' Unknown bits
			If Unknown Then
				BitPack.Write(False)
				BitPack.Write(False)
			End If

			BitPack.WriteGuidMask(0, 4)

			BitPack.Write(IsTransport)
			BitPack.WriteGuidMask(2)

			' Transport guid
			If IsTransport Then
				BitPack.WriteTransportGuidMask(7, 5, 2, 1, 0, 4, _
					3, 6)
			End If

			BitPack.WriteGuidMask(5)

			BitPack.Flush()

			If IsTransport Then
				BitPack.WriteTransportGuidBytes(1, 5, 7, 0, 3, 4, _
					6, 2)
			End If

			BitPack.WriteGuidBytes(3)

			If Unknown Then
				moveTeleport.WriteUInt8(0)
			End If

			BitPack.WriteGuidBytes(2, 1, 7, 5, 6, 4, _
				0)

			session.Send(moveTeleport)
		End Sub

		Public Shared Sub HandleTransferPending(ByRef session As WorldClass, mapId As UInteger)
			Dim Unknown As Boolean = False
			Dim IsTransport As Boolean = False

			Dim transferPending As New PacketWriter(JAMCMessage.TransferPending)
			Dim BitPack As New BitPack(transferPending)

			BitPack.Write(IsTransport)
			BitPack.Write(Unknown)

			If Unknown Then
				transferPending.WriteUInt32(0)
			End If

			transferPending.WriteUInt32(mapId)

			If IsTransport Then
				transferPending.WriteUInt32(0)
				transferPending.WriteUInt32(0)
			End If

			session.Send(transferPending)
		End Sub

		Public Shared Sub HandleNewWorld(ByRef session As WorldClass, vector As Vector4, mapId As UInteger)
			Dim newWorld As New PacketWriter(JAMCMessage.NewWorld)

			newWorld.WriteUInt32(mapId)
			newWorld.WriteFloat(vector.Y)
			newWorld.WriteFloat(vector.O)
			newWorld.WriteFloat(vector.X)
			newWorld.WriteFloat(vector.Z)

			session.Send(newWorld)
		End Sub
	End Class
End Namespace
