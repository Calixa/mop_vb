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

Namespace DBC
	Public NotInheritable Class DBCStorage
		Private Sub New()
		End Sub
		Friend Shared DBCFileCount As Integer = 0

		Public Shared ClassStorage As Dictionary(Of UInteger, ChrClasses)
		Public Shared RaceStorage As Dictionary(Of UInteger, ChrRaces)
		Public Shared CharStartOutfitStorage As Dictionary(Of UInteger, CharStartOutfit)
		Public Shared NameGenStorage As Dictionary(Of UInteger, NameGen)

		'Strings
		Friend Shared ClassStrings As New Dictionary(Of UInteger, String)()
		Friend Shared RaceStrings As New Dictionary(Of UInteger, String)()
		Friend Shared NameGenStrings As New Dictionary(Of UInteger, String)()
	End Class
End Namespace
