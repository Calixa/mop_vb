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
Imports Framework.Database
Imports Framework.DBC
Imports System.Collections.Generic

Namespace Game.WorldEntities
	Public Class Character
		Inherits WorldObject
		Public AccountId As UInt32
		Public Name As [String]
		Public Race As [Byte]
		Public [Class] As [Byte]
		Public Gender As [Byte]
		Public Skin As [Byte]
		Public Face As [Byte]
		Public HairStyle As [Byte]
		Public HairColor As [Byte]
		Public FacialHair As [Byte]
		Public Level As [Byte]
		Public Zone As UInt32
		Public GuildGuid As UInt64
		Public PetDisplayInfo As UInt32
		Public PetLevel As UInt32
		Public PetFamily As UInt32
		Public CharacterFlags As UInt32
		Public CustomizeFlags As UInt32
		Public LoginCinematic As [Boolean]

		Public InRangeObjects As New Dictionary(Of ULong, WorldObject)()

		Public Skills As New List(Of Skill)()
		Public SpellList As New List(Of PlayerSpell)()

		Public Sub New(guid__1 As UInt64, Optional updateLength As Integer = CInt(PlayerFields.[End]))
			MyBase.New(updateLength)
			Dim result As SQLResult = DB.Characters.[Select]("SELECT * FROM characters WHERE guid = ?", guid__1)

			Guid = result.Read(Of UInt64)(0, "Guid")
			AccountId = result.Read(Of UInt32)(0, "AccountId")
			Name = result.Read(Of [String])(0, "Name")
			Race = result.Read(Of [Byte])(0, "Race")
			[Class] = result.Read(Of [Byte])(0, "Class")
			Gender = result.Read(Of [Byte])(0, "Gender")
			Skin = result.Read(Of [Byte])(0, "Skin")
			Face = result.Read(Of [Byte])(0, "Face")
			HairStyle = result.Read(Of [Byte])(0, "HairStyle")
			HairColor = result.Read(Of [Byte])(0, "HairColor")
			FacialHair = result.Read(Of [Byte])(0, "FacialHair")
			Level = result.Read(Of [Byte])(0, "Level")
			Zone = result.Read(Of UInt32)(0, "Zone")
			Map = result.Read(Of UInt32)(0, "Map")
			Position.X = result.Read(Of [Single])(0, "X")
			Position.Y = result.Read(Of [Single])(0, "Y")
			Position.Z = result.Read(Of [Single])(0, "Z")
			Position.O = result.Read(Of [Single])(0, "O")
			GuildGuid = result.Read(Of UInt64)(0, "GuildGuid")
			PetDisplayInfo = result.Read(Of UInt32)(0, "PetDisplayId")
			PetLevel = result.Read(Of UInt32)(0, "PetLevel")
			PetFamily = result.Read(Of UInt32)(0, "PetFamily")
			CharacterFlags = result.Read(Of UInt32)(0, "CharacterFlags")
			CustomizeFlags = result.Read(Of UInt32)(0, "CustomizeFlags")
			LoginCinematic = result.Read(Of [Boolean])(0, "LoginCinematic")

			Globals.SpellMgr.LoadSpells(Me)
			Globals.SkillMgr.LoadSkills(Me)

			SetUpdateFields()
		End Sub

		Public Overrides Sub SetUpdateFields()
			' ObjectFields
			SetUpdateField(Of UInt64)(CInt(ObjectFields.Guid), Guid)
			SetUpdateField(Of UInt64)(CInt(ObjectFields.Data), 0)
			SetUpdateField(Of Int32)(CInt(ObjectFields.Type), &H19)
			SetUpdateField(Of [Single])(CInt(ObjectFields.Scale), 1F)

			' UnitFields
			SetUpdateField(Of UInt64)(CInt(UnitFields.Charm), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.Summon), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.Critter), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.CharmedBy), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.SummonedBy), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.CreatedBy), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.Target), 0)
			SetUpdateField(Of UInt64)(CInt(UnitFields.ChannelObject), 0)

			SetUpdateField(Of Int32)(CInt(UnitFields.Health), 123)

			For i As Integer = 0 To 4
				SetUpdateField(Of Int32)(CInt(UnitFields.Power) + i, 0)
			Next

			SetUpdateField(Of Int32)(CInt(UnitFields.MaxHealth), 123)

			For i As Integer = 0 To 4
				SetUpdateField(Of Int32)(CInt(UnitFields.MaxPower) + i, 0)
			Next

			SetUpdateField(Of Int32)(CInt(UnitFields.PowerRegenFlatModifier), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.PowerRegenInterruptedFlatModifier), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.BaseHealth), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.BaseMana), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.Level), Level)
			SetUpdateField(Of Int32)(CInt(UnitFields.FactionTemplate), CInt(DBCStorage.RaceStorage(Race).FactionID))
			SetUpdateField(Of Int32)(CInt(UnitFields.Flags), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.Flags2), 0)

			For i As Integer = 0 To 4
				SetUpdateField(Of Int32)(CInt(UnitFields.Stats) + i, 0)
				SetUpdateField(Of Int32)(CInt(UnitFields.StatPosBuff) + i, 0)
				SetUpdateField(Of Int32)(CInt(UnitFields.StatNegBuff) + i, 0)
			Next

			SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), Race, 0)
			SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), [Class], 1)
			SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), Gender, 2)
			SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), 0, 3)

			Dim displayId As UInteger = If(Gender = 0, DBCStorage.RaceStorage(Race).model_m, DBCStorage.RaceStorage(Race).model_f)
			SetUpdateField(Of Int32)(CInt(UnitFields.DisplayID), CInt(displayId))
			SetUpdateField(Of Int32)(CInt(UnitFields.NativeDisplayID), CInt(displayId))
			SetUpdateField(Of Int32)(CInt(UnitFields.MountDisplayID), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.DynamicFlags), 0)

			SetUpdateField(Of [Single])(CInt(UnitFields.BoundingRadius), 0.389F)
			SetUpdateField(Of [Single])(CInt(UnitFields.CombatReach), 1.5F)
			SetUpdateField(Of [Single])(CInt(UnitFields.MinDamage), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.MaxDamage), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.ModCastingSpeed), 1)
			SetUpdateField(Of Int32)(CInt(UnitFields.AttackPower), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.RangedAttackPower), 0)

			For i As Integer = 0 To 6
				SetUpdateField(Of Int32)(CInt(UnitFields.Resistances) + i, 0)
				SetUpdateField(Of Int32)(CInt(UnitFields.ResistanceBuffModsPositive) + i, 0)
				SetUpdateField(Of Int32)(CInt(UnitFields.ResistanceBuffModsNegative) + i, 0)
			Next

			SetUpdateField(Of [Byte])(CInt(UnitFields.AnimTier), 0, 0)
			SetUpdateField(Of [Byte])(CInt(UnitFields.AnimTier), 0, 1)
			SetUpdateField(Of [Byte])(CInt(UnitFields.AnimTier), 0, 2)
			SetUpdateField(Of [Byte])(CInt(UnitFields.AnimTier), 0, 3)

			SetUpdateField(Of Int16)(CInt(UnitFields.RangedAttackRoundBaseTime), 0)
			SetUpdateField(Of Int16)(CInt(UnitFields.RangedAttackRoundBaseTime), 0, 1)
			SetUpdateField(Of [Single])(CInt(UnitFields.MinOffHandDamage), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.MaxOffHandDamage), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.AttackPowerModPos), 0)
			SetUpdateField(Of Int32)(CInt(UnitFields.RangedAttackPowerModPos), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.MinRangedDamage), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.MaxRangedDamage), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.AttackPowerMultiplier), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.RangedAttackPowerMultiplier), 0)
			SetUpdateField(Of [Single])(CInt(UnitFields.MaxHealthModifier), 1)

			' PlayerFields
			SetUpdateField(Of Int32)(CInt(PlayerFields.MaxLevel), 90)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.HairColorID), Skin, 0)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.HairColorID), Face, 1)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.HairColorID), HairStyle, 2)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.HairColorID), HairColor, 3)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.RestState), FacialHair, 0)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.RestState), 0, 1)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.RestState), 0, 2)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.RestState), 2, 3)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.ArenaFaction), Gender, 0)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.ArenaFaction), 0, 1)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.ArenaFaction), 0, 2)
			SetUpdateField(Of [Byte])(CInt(PlayerFields.ArenaFaction), 0, 3)
			SetUpdateField(Of Int32)(CInt(PlayerFields.WatchedFactionIndex), -1)
			SetUpdateField(Of Int32)(CInt(PlayerFields.XP), 0)
			SetUpdateField(Of Int32)(CInt(PlayerFields.NextLevelXP), 400)
			SetUpdateField(Of Int32)(CInt(PlayerFields.PlayerFlags), 0)
			SetUpdateField(Of Int32)(CInt(PlayerFields.CharacterPoints), 0)
			SetUpdateField(Of Int32)(CInt(PlayerFields.GuildRankID), 0)
			SetUpdateField(Of [Single])(CInt(PlayerFields.CritPercentage), 0)
			SetUpdateField(Of [Single])(CInt(PlayerFields.RangedCritPercentage), 0)
			SetUpdateField(Of Int32)(CInt(PlayerFields.ModHealingDonePos), 0)

			For i As Integer = 0 To 6
				SetUpdateField(Of [Single])(CInt(PlayerFields.SpellCritPercentage) + i, 0)
				SetUpdateField(Of Int32)(CInt(PlayerFields.ModDamageDonePos) + i, 0)
				SetUpdateField(Of Int32)(CInt(PlayerFields.ModDamageDoneNeg) + i, 0)
				SetUpdateField(Of [Single])(CInt(PlayerFields.ModDamageDonePercent) + i, 0)
			Next

			SetUpdateField(Of UInt64)(CInt(PlayerFields.Coinage), 0)

			For i As Integer = 0 To 447
				If i < Skills.Count Then
					SetUpdateField(Of UInt32)(CInt(PlayerFields.Skill) + i, Skills(i).Id)
				Else
					SetUpdateField(Of UInt32)(CInt(PlayerFields.Skill) + i, 0)
				End If
			Next

			For i As Integer = 0 To 749
				SetUpdateField(Of Int32)(CInt(PlayerFields.QuestLog) + i, 0)
			Next

			SetUpdateField(Of UInt32)(CInt(PlayerFields.HomePlayerRealm), 1)
			SetUpdateField(Of [Single])(CInt(PlayerFields.BlockPercentage), 0)

			For i As Integer = 0 To 199
				SetUpdateField(Of UInt32)(CInt(PlayerFields.ExploredZones) + i, 0)
			Next
		End Sub

		Public Shared Function NormalizeName(name As String) As String
			Return name(0).ToString().ToUpper() & name.Remove(0, 1).ToLower()
		End Function
	End Class
End Namespace
