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



Namespace Game.Chat
	Public Class ChatCommandAttribute
		Inherits Attribute
		Public Property ChatCommand() As String
			Get
				Return m_ChatCommand
			End Get
			Set
				m_ChatCommand = Value
			End Set
		End Property
		Private m_ChatCommand As String
		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set
				m_Description = Value
			End Set
		End Property
		Private m_Description As String

		Public Sub New(chatCommand__1 As String, Optional description__2 As String = "")
			ChatCommand = chatCommand__1
			Description = description__2
		End Sub
	End Class
End Namespace
