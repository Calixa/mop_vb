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


Imports Framework.Database
Imports Framework.Singleton
Imports WorldServer.Game.WorldEntities

Namespace Game.Managers
	Public Class SkillManager
		Inherits SingletonBase(Of SkillManager)
		Private Sub New()
		End Sub

		Public Sub LoadSkills(pChar As Character)
			Dim result As SQLResult = DB.Characters.[Select]("SELECT * FROM character_skills WHERE guid = ? ORDER BY skill ASC", pChar.Guid)

			If result.Count = 0 Then
				result = DB.Characters.[Select]("SELECT skill FROM character_creation_skills WHERE race = ? ORDER BY skill ASC", pChar.Race, pChar.[Class])

				For i As Integer = 0 To result.Count - 1
					AddSkill(pChar, result.Read(Of UInteger)(i, "skill"))
				Next

				SaveSkills(pChar)
			Else
				For i As Integer = 0 To result.Count - 1
					AddSkill(pChar, result.Read(Of UInteger)(i, "skill"))
				Next
			End If
		End Sub

		Public Sub SaveSkills(pChar As Character)
			pChar.Skills.ForEach(Function(skill) DB.Characters.Execute("INSERT INTO character_skills (guid, skill) VALUES (?, ?)", pChar.Guid, skill.Id))
		End Sub

		Public Sub AddSkill(pChar As Character, skillId As UInteger, Optional skillLevel As UInteger = 0)
			Dim skill As New Skill() With { _
				.Id = skillId, _
				.SkillLevel = skillLevel _
			}

			pChar.Skills.Add(skill)
		End Sub
	End Class
End Namespace
