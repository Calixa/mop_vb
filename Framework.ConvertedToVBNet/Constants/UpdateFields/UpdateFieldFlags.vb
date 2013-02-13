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
	Public Enum UpdateFieldFlags
		All = &H1
		Self = &H2
		Owner = &H4
		Empath = &H10
		Party = &H20
		UnitAll = &H40
		ViewerDependet = &H80
		Urgent = &H100
		UrgentSelfOnly = &H200
	End Enum
End Namespace
