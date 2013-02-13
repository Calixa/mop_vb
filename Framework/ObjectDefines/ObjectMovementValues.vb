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

Namespace ObjectDefines
	' Initialize default values for ObjectUpdate movement
	Public Class ObjectMovementValues
		' Bits
		Public HasAnimKits As Boolean = False
		Public HasUnknown As Boolean = False
		Public BitCounter As UInteger = 0
		Public Bit0 As Boolean = False
		Public HasUnknown2 As Boolean = False
		Public IsVehicle As Boolean = False
		Public Bit2 As Boolean = False
		Public HasUnknown3 As Boolean = False
		Public HasStationaryPosition As Boolean = False
		Public HasGoTransportPosition As Boolean = False
		Public IsSelf As Boolean = False
		Public IsAlive As Boolean = False
		Public BitCounter2 As UInteger = 0
		Public Bit3 As Boolean = False
		Public HasUnknown4 As Boolean = False
		Public HasTarget As Boolean = False
		Public Bit1 As Boolean = False
		Public HasRotation As Boolean = False
		Public IsTransport As Boolean = False
		Public HasMovementFlags As Boolean = False
		Public HasMovementFlags2 As Boolean = False
		Public IsInterpolated As Boolean = False
		Public IsInterpolated2 As Boolean = False

		' Data
		Public MovementFlags As MovementFlag = 0
		Public MovementFlags2 As MovementFlag2 = 0
		Public Time As UInteger = 0

		Public Sub New()
		End Sub
		Public Sub New(updateflags As UpdateFlag)
			IsSelf = (updateflags And UpdateFlag.Self) <> 0
			IsAlive = (updateflags And UpdateFlag.Alive) <> 0
			HasRotation = (updateflags And UpdateFlag.Rotation) <> 0
			HasStationaryPosition = (updateflags And UpdateFlag.StationaryPosition) <> 0
			HasTarget = (updateflags And UpdateFlag.Target) <> 0
			IsTransport = (updateflags And UpdateFlag.Transport) <> 0
			HasGoTransportPosition = (updateflags And UpdateFlag.GoTransportPosition) <> 0
			HasAnimKits = (updateflags And UpdateFlag.AnimKits) <> 0
			IsVehicle = (updateflags And UpdateFlag.Vehicle) <> 0
			HasUnknown = (updateflags And UpdateFlag.Unknown) <> 0
			HasUnknown2 = (updateflags And UpdateFlag.Unknown2) <> 0
			HasUnknown3 = (updateflags And UpdateFlag.Unknown3) <> 0
			HasUnknown4 = (updateflags And UpdateFlag.Unknown4) <> 0
		End Sub
	End Class
End Namespace
