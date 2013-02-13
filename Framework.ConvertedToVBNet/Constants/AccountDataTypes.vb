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
	Public Enum AccountDataTypes
		GlobalConfigCache = &H0
		CharacterConfigCache = &H1
		GlobalBindingsCache = &H2
		CharacterBindingsCache = &H3
		GlobalMacrosCache = &H4
		CharacterMacrosCache = &H5
		CharacterLayoutCache = &H6
		CharacterChatCache = &H7
	End Enum

	Public Enum AccountDataMasks
		GlobalCacheMask = &H15
		CharacterCacheMask = &Haa
	End Enum
End Namespace
