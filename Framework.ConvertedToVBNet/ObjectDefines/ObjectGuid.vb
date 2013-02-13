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

Namespace ObjectDefines
	Public Class ObjectGuid
		Public Property Guid() As UInt64
			Get
				Return m_Guid
			End Get
			Set
				m_Guid = Value
			End Set
		End Property
		Private m_Guid As UInt64

		Public Sub New(low As ULong, id As Integer, highType As HighGuidType)
			Guid = CULng(low Or (CULng(id) << 32) Or CULng(highType) << 52)
		End Sub

		Public Shared Function GetGuidType(guid As ULong) As HighGuidType
			Return CType(guid >> 52, HighGuidType)
		End Function

        Public Shared Function GetId(guid As Long) As Integer
            Return CInt((guid >> 32) And &HFFFFF)
        End Function

        Public Shared Function GetGuid(guid As Long) As ULong
            Return CULng(guid And &HFFFFFFF)
        End Function
	End Class
End Namespace
