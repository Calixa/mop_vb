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


Imports System.Collections.Generic

Namespace Game.ObjectDefines
	Public Class GameObjectStats
		Public Id As Int32
		Public Type As Int32
		Public Flags As Int32
		Public DisplayInfoId As Int32
		Public Name As [String]
		Public IconName As [String]
		Public CastBarCaption As [String]
		Public Data As New List(Of Int32)(32)
		Public Size As [Single]
		Public QuestItemId As New List(Of Int32)(6)
		Public ExpansionRequired As Int32
	End Class
End Namespace
