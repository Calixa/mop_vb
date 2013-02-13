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
	Public Enum HighGuidMask
		None = &H0
		[Object] = &H1
		Item = &H2
		Container = &H4
		Unit = &H8
		Player = &H10
		GameObject = &H20
		DynamicObject = &H40
		Corpse = &H80
		Guild = &H100
	End Enum
End Namespace
