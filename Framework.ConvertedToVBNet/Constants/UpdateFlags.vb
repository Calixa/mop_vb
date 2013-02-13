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



Namespace Constants
	<Flags> _
	Public Enum UpdateFlag
		Self = &H1
		Alive = &H2
		Rotation = &H4
		StationaryPosition = &H8
		Target = &H10
		Transport = &H20
		GoTransportPosition = &H40
		AnimKits = &H80
		Vehicle = &H100
		Unknown = &H200
		Unknown2 = &H400
		Unknown3 = &H800
		Unknown4 = &H1000
	End Enum
End Namespace
