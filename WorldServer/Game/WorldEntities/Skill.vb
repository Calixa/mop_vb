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


Namespace Game.WorldEntities
	Public Class Skill
		Public Property Id() As UInteger
			Get
				Return m_Id
			End Get
			Set
				m_Id = Value
			End Set
		End Property
		Private m_Id As UInteger
		Public Property SkillLevel() As UInteger
			Get
				Return m_SkillLevel
			End Get
			Set
				m_SkillLevel = Value
			End Set
		End Property
		Private m_SkillLevel As UInteger
	End Class
End Namespace