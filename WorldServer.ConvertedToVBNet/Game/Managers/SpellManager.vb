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
	Public NotInheritable Class SpellManager
		Inherits SingletonBase(Of SpellManager)
		Private Sub New()
		End Sub

		Public Sub LoadSpells(pChar As Character)
			Dim result As SQLResult = DB.Characters.[Select]("SELECT * FROM character_spells WHERE guid = ? ORDER BY spellId ASC", pChar.Guid)

			If result.Count = 0 Then
				result = DB.Characters.[Select]("SELECT spellId FROM character_creation_spells WHERE race = ? AND class = ? ORDER BY spellId ASC", pChar.Race, pChar.[Class])

				For i As Integer = 0 To result.Count - 1
					AddSpell(pChar, result.Read(Of UInteger)(i, "spellId"))
				Next

				SaveSpells(pChar)
			Else
				For i As Integer = 0 To result.Count - 1
					AddSpell(pChar, result.Read(Of UInteger)(i, "spellId"))
				Next
			End If
		End Sub

		Public Sub SaveSpells(pChar As Character)
			pChar.SpellList.ForEach(Function(spell) DB.Characters.Execute("INSERT INTO character_spells (guid, spellId) VALUES (?, ?)", pChar.Guid, spell.SpellId))
		End Sub

		Public Sub AddSpell(pChar As Character, spellId As UInteger)
			Dim newspell As New PlayerSpell() With { _
				.SpellId = spellId, _
				.State = PlayerSpellState.Unchanged, _
				.Dependent = False _
			}

			pChar.SpellList.Add(newspell)
		End Sub
	End Class
End Namespace
