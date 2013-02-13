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


Imports System.Runtime.InteropServices

Namespace DBC
	Public Structure Db2Header
		Public Signature As Integer
		Public RecordsCount As Integer
		Public FieldsCount As Integer
		Public RecordSize As Integer
		Public StringTableSize As Integer

		Public ReadOnly Property IsDB2() As Boolean
			Get
				Return Signature = &H32424457
			End Get
		End Property

		Public ReadOnly Property DataSize() As Long
			Get
				Return CLng(RecordsCount * RecordSize)
			End Get
		End Property

		Public ReadOnly Property StartStringPosition() As Long
			Get
				Return DataSize + CLng(Marshal.SizeOf(GetType(Db2Header)))
			End Get
		End Property
	End Structure

	Public Structure ItemEntry
		Public Id As UInteger
		' 0
		Public [Class] As UInteger
		' 1
		Public SubClass As UInteger
		' 2
		Public Unk0 As Integer
		' 3
		Public Material As Integer
		' 4
		Public DisplayId As UInteger
		' 5
		Public InventoryType As UInteger
		' 6
		Public Sheath As UInteger
		' 7
	End Structure
End Namespace
