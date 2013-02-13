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



Namespace Constants.Movement
	<Flags> _
	Public Enum MovementFlag
		Forward = &H1
		Backward = &H2
		StrafeLeft = &H4
		StrafeRight = &H8
		TurnLeft = &H10
		TurnRight = &H20
		PitchUp = &H40
		PitchDown = &H80
		RunMode = &H100
		Gravity = &H200
		Root = &H400
		Falling = &H800
		FallReset = &H1000
		PendingStop = &H2000
		PendingStrafeStop = &H4000
		PendingForward = &H8000
		PendingBackward = &H10000
		PendingStrafeLeft = &H20000
		PendingStrafeRight = &H40000
		PendingRoot = &H80000
		Swim = &H100000
		Ascension = &H200000
		Descension = &H400000
		CanFly = &H800000
		Flight = &H1000000
		Jump = &H2000000
		WalkOnWater = &H4000000
		FeatherFall = &H8000000
		HoverMove = &H10000000
		Collision = &H20000000
	End Enum
End Namespace
