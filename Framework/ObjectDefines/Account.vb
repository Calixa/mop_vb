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


Namespace ObjectDefines
	Public Class Account
		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set
				m_Id = Value
			End Set
		End Property
		Private m_Id As Integer
		Public Property Name() As String
			Get
				Return m_Name
			End Get
			Set
				m_Name = Value
			End Set
		End Property
		Private m_Name As String
		Public Property Password() As String
			Get
				Return m_Password
			End Get
			Set
				m_Password = Value
			End Set
		End Property
		Private m_Password As String
		Public Property SessionKey() As String
			Get
				Return m_SessionKey
			End Get
			Set
				m_SessionKey = Value
			End Set
		End Property
		Private m_SessionKey As String
		Public Property SecurityFlags() As Byte
			Get
				Return m_SecurityFlags
			End Get
			Set
				m_SecurityFlags = Value
			End Set
		End Property
		Private m_SecurityFlags As Byte
		Public Property Expansion() As Byte
			Get
				Return m_Expansion
			End Get
			Set
				m_Expansion = Value
			End Set
		End Property
		Private m_Expansion As Byte
		Public Property GMLevel() As Byte
			Get
				Return m_GMLevel
			End Get
			Set
				m_GMLevel = Value
			End Set
		End Property
		Private m_GMLevel As Byte
		Public Property IP() As String
			Get
				Return m_IP
			End Get
			Set
				m_IP = Value
			End Set
		End Property
		Private m_IP As String
		Public Property Language() As String
			Get
				Return m_Language
			End Get
			Set
				m_Language = Value
			End Set
		End Property
		Private m_Language As String
		Public Property Online() As Boolean
			Get
				Return m_Online
			End Get
			Set
				m_Online = Value
			End Set
		End Property
		Private m_Online As Boolean
	End Class
End Namespace
