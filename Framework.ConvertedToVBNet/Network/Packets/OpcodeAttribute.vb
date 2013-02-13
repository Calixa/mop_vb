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

Namespace Network.Packets
	Public Class OpcodeAttribute
		Inherits Attribute
		Public Property Opcode() As ClientMessage
			Get
				Return m_Opcode
			End Get
			Set
				m_Opcode = Value
			End Set
		End Property
		Private m_Opcode As ClientMessage
		Public Property WoWBuild() As String
			Get
				Return m_WoWBuild
			End Get
			Set
				m_WoWBuild = Value
			End Set
		End Property
		Private m_WoWBuild As String

		Public Sub New(opcode__1 As ClientMessage, wowBuild__2 As String)
			Opcode = opcode__1
			WoWBuild = wowBuild__2
		End Sub
	End Class
End Namespace
