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


Namespace Constants
	Public Enum ObjectFields
		Guid = &H0
		Data = &H2
		Type = &H4
		Entry = &H5
		Scale = &H6
		[End] = &H7
	End Enum

	Public Enum DynamicObjectArrays
		' Empty
		[End] = &H0
	End Enum

	Public Enum ItemFields
		Owner = ObjectFields.[End] + &H0
		ContainedIn = ObjectFields.[End] + &H2
		Creator = ObjectFields.[End] + &H4
		GiftCreator = ObjectFields.[End] + &H6
		StackCount = ObjectFields.[End] + &H8
		Expiration = ObjectFields.[End] + &H9
		SpellCharges = ObjectFields.[End] + &Ha
		DynamicFlags = ObjectFields.[End] + &Hf
		Enchantment = ObjectFields.[End] + &H10
		PropertySeed = ObjectFields.[End] + &H37
		RandomPropertiesID = ObjectFields.[End] + &H38
		Durability = ObjectFields.[End] + &H39
		MaxDurability = ObjectFields.[End] + &H3a
		CreatePlayedTime = ObjectFields.[End] + &H3b
		ModifiersMask = ObjectFields.[End] + &H3c
		[End] = ObjectFields.[End] + &H3d
	End Enum

	Public Enum ItemDynamicArrays
		' Empty
		[End] = DynamicObjectArrays.[End]
	End Enum

	Public Enum ContainerFields
		NumSlots = ItemFields.[End] + &H0
		Slots = ItemFields.[End] + &H1
		[End] = ItemFields.[End] + &H49
	End Enum

	Public Enum ContainerDynamicArrays
		' Empty
		[End] = ItemDynamicArrays.[End]
	End Enum

	Public Enum UnitFields
		Charm = ObjectFields.[End] + &H0
		Summon = ObjectFields.[End] + &H2
		Critter = ObjectFields.[End] + &H4
		CharmedBy = ObjectFields.[End] + &H6
		SummonedBy = ObjectFields.[End] + &H8
		CreatedBy = ObjectFields.[End] + &Ha
		Target = ObjectFields.[End] + &Hc
		ChannelObject = ObjectFields.[End] + &He
		SummonedByHomeRealm = ObjectFields.[End] + &H10
		ChannelSpell = ObjectFields.[End] + &H11
		DisplayPower = ObjectFields.[End] + &H12
		OverrideDisplayPowerID = ObjectFields.[End] + &H13
		Health = ObjectFields.[End] + &H14
		Power = ObjectFields.[End] + &H15
		MaxHealth = ObjectFields.[End] + &H1a
		MaxPower = ObjectFields.[End] + &H1b
		PowerRegenFlatModifier = ObjectFields.[End] + &H20
		PowerRegenInterruptedFlatModifier = ObjectFields.[End] + &H25
		Level = ObjectFields.[End] + &H2a
		FactionTemplate = ObjectFields.[End] + &H2b
		VirtualItemID = ObjectFields.[End] + &H2c
		Flags = ObjectFields.[End] + &H2f
		Flags2 = ObjectFields.[End] + &H30
		AuraState = ObjectFields.[End] + &H31
		AttackRoundBaseTime = ObjectFields.[End] + &H32
		RangedAttackRoundBaseTime = ObjectFields.[End] + &H34
		BoundingRadius = ObjectFields.[End] + &H35
		CombatReach = ObjectFields.[End] + &H36
		DisplayID = ObjectFields.[End] + &H37
		NativeDisplayID = ObjectFields.[End] + &H38
		MountDisplayID = ObjectFields.[End] + &H39
		MinDamage = ObjectFields.[End] + &H3a
		MaxDamage = ObjectFields.[End] + &H3b
		MinOffHandDamage = ObjectFields.[End] + &H3c
		MaxOffHandDamage = ObjectFields.[End] + &H3d
		AnimTier = ObjectFields.[End] + &H3e
		PetNumber = ObjectFields.[End] + &H3f
		PetNameTimestamp = ObjectFields.[End] + &H40
		PetExperience = ObjectFields.[End] + &H41
		PetNextLevelExperience = ObjectFields.[End] + &H42
		DynamicFlags = ObjectFields.[End] + &H43
		ModCastingSpeed = ObjectFields.[End] + &H44
		ModSpellHaste = ObjectFields.[End] + &H45
		ModHaste = ObjectFields.[End] + &H46
		ModHasteRegen = ObjectFields.[End] + &H47
		CreatedBySpell = ObjectFields.[End] + &H48
		NpcFlags = ObjectFields.[End] + &H49
		EmoteState = ObjectFields.[End] + &H4b
		Stats = ObjectFields.[End] + &H4c
		StatPosBuff = ObjectFields.[End] + &H51
		StatNegBuff = ObjectFields.[End] + &H56
		Resistances = ObjectFields.[End] + &H5b
		ResistanceBuffModsPositive = ObjectFields.[End] + &H62
		ResistanceBuffModsNegative = ObjectFields.[End] + &H69
		BaseMana = ObjectFields.[End] + &H70
		BaseHealth = ObjectFields.[End] + &H71
		ShapeshiftForm = ObjectFields.[End] + &H72
		AttackPower = ObjectFields.[End] + &H73
		AttackPowerModPos = ObjectFields.[End] + &H74
		AttackPowerModNeg = ObjectFields.[End] + &H75
		AttackPowerMultiplier = ObjectFields.[End] + &H76
		RangedAttackPower = ObjectFields.[End] + &H77
		RangedAttackPowerModPos = ObjectFields.[End] + &H78
		RangedAttackPowerModNeg = ObjectFields.[End] + &H79
		RangedAttackPowerMultiplier = ObjectFields.[End] + &H7a
		MinRangedDamage = ObjectFields.[End] + &H7b
		MaxRangedDamage = ObjectFields.[End] + &H7c
		PowerCostModifier = ObjectFields.[End] + &H7d
		PowerCostMultiplier = ObjectFields.[End] + &H84
		MaxHealthModifier = ObjectFields.[End] + &H8b
		HoverHeight = ObjectFields.[End] + &H8c
		MinItemLevel = ObjectFields.[End] + &H8d
		MaxItemLevel = ObjectFields.[End] + &H8e
		WildBattlePetLevel = ObjectFields.[End] + &H8f
		BattlePetCompanionGUID = ObjectFields.[End] + &H90
		BattlePetCompanionNameTimestamp = ObjectFields.[End] + &H92
		[End] = ObjectFields.[End] + &H93
	End Enum

	Public Enum UnitDynamicArrays
		PassiveSpells = DynamicObjectArrays.[End] + &H0
		[End] = DynamicObjectArrays.[End] + &H101
	End Enum

	Public Enum PlayerFields
		DuelArbiter = UnitFields.[End] + &H0
		PlayerFlags = UnitFields.[End] + &H2
		GuildRankID = UnitFields.[End] + &H3
		GuildDeleteDate = UnitFields.[End] + &H4
		GuildLevel = UnitFields.[End] + &H5
		HairColorID = UnitFields.[End] + &H6
		RestState = UnitFields.[End] + &H7
		ArenaFaction = UnitFields.[End] + &H8
		DuelTeam = UnitFields.[End] + &H9
		GuildTimeStamp = UnitFields.[End] + &Ha
		QuestLog = UnitFields.[End] + &Hb
		VisibleItems = UnitFields.[End] + &H2f9
		PlayerTitle = UnitFields.[End] + &H31f
		FakeInebriation = UnitFields.[End] + &H320
		HomePlayerRealm = UnitFields.[End] + &H321
		CurrentSpecID = UnitFields.[End] + &H322
		TaxiMountAnimKitID = UnitFields.[End] + &H323
		CurrentBattlePetBreedQuality = UnitFields.[End] + &H324
		InvSlots = UnitFields.[End] + &H325
		FarsightObject = UnitFields.[End] + &H3d1
		KnownTitles = UnitFields.[End] + &H3d3
		Coinage = UnitFields.[End] + &H3db
		XP = UnitFields.[End] + &H3dd
		NextLevelXP = UnitFields.[End] + &H3de
		Skill = UnitFields.[End] + &H3df
		SpellCritPercentage = UnitFields.[End] + &H59f
		CharacterPoints = UnitFields.[End] + &H5a6
		MaxTalentTiers = UnitFields.[End] + &H5a7
		TrackCreatureMask = UnitFields.[End] + &H5a8
		TrackResourceMask = UnitFields.[End] + &H5a9
		Expertise = UnitFields.[End] + &H5aa
		OffhandExpertise = UnitFields.[End] + &H5ab
		RangedExpertise = UnitFields.[End] + &H5ac
		BlockPercentage = UnitFields.[End] + &H5ad
		DodgePercentage = UnitFields.[End] + &H5ae
		ParryPercentage = UnitFields.[End] + &H5af
		CritPercentage = UnitFields.[End] + &H5b0
		RangedCritPercentage = UnitFields.[End] + &H5b1
		OffhandCritPercentage = UnitFields.[End] + &H5b2
		ShieldBlock = UnitFields.[End] + &H5b3
		ShieldBlockCritPercentage = UnitFields.[End] + &H5b4
		Mastery = UnitFields.[End] + &H5b5
		PvpPowerDamage = UnitFields.[End] + &H5b6
		PvpPowerHealing = UnitFields.[End] + &H5b7
		ExploredZones = UnitFields.[End] + &H5b8
		ModDamageDonePos = UnitFields.[End] + &H680
		ModDamageDoneNeg = UnitFields.[End] + &H687
		ModDamageDonePercent = UnitFields.[End] + &H68e
		RestStateBonusPool = UnitFields.[End] + &H695
		ModHealingDonePos = UnitFields.[End] + &H696
		ModHealingPercent = UnitFields.[End] + &H697
		ModHealingDonePercent = UnitFields.[End] + &H698
		WeaponDmgMultipliers = UnitFields.[End] + &H699
		ModPeriodicHealingDonePercent = UnitFields.[End] + &H69c
		ModSpellPowerPercent = UnitFields.[End] + &H69d
		ModResiliencePercent = UnitFields.[End] + &H69e
		OverrideSpellPowerByAPPercent = UnitFields.[End] + &H69f
		OverrideAPBySpellPowerPercent = UnitFields.[End] + &H6a0
		ModTargetResistance = UnitFields.[End] + &H6a1
		ModTargetPhysicalResistance = UnitFields.[End] + &H6a2
		LifetimeMaxRank = UnitFields.[End] + &H6a3
		SelfResSpell = UnitFields.[End] + &H6a4
		PvpMedals = UnitFields.[End] + &H6a5
		BuybackPrice = UnitFields.[End] + &H6a6
		BuybackTimestamp = UnitFields.[End] + &H6b2
		YesterdayHonorableKills = UnitFields.[End] + &H6be
		LifetimeHonorableKills = UnitFields.[End] + &H6bf
		WatchedFactionIndex = UnitFields.[End] + &H6c0
		CombatRatings = UnitFields.[End] + &H6c1
		ArenaTeams = UnitFields.[End] + &H6dc
		RuneRegen = UnitFields.[End] + &H6f1
		NoReagentCostMask = UnitFields.[End] + &H6f5
		GlyphSlots = UnitFields.[End] + &H6f9
		Glyphs = UnitFields.[End] + &H6ff
		BattlegroundRating = UnitFields.[End] + &H705
		MaxLevel = UnitFields.[End] + &H706
		GlyphSlotsEnabled = UnitFields.[End] + &H707
		Researching = UnitFields.[End] + &H708
		ProfessionSkillLine = UnitFields.[End] + &H710
		PetSpellPower = UnitFields.[End] + &H712
		UiHitModifier = UnitFields.[End] + &H713
		UiSpellHitModifier = UnitFields.[End] + &H714
		HomeRealmTimeOffset = UnitFields.[End] + &H715
		ModRangedHaste = UnitFields.[End] + &H716
		ModPetHaste = UnitFields.[End] + &H717
		SummonedBattlePetGUID = UnitFields.[End] + &H718
		OverrideSpellsID = UnitFields.[End] + &H71a
		[End] = UnitFields.[End] + &H71b
	End Enum

	Public Enum PlayerDynamicArrays
		ResearchSites = UnitDynamicArrays.[End] + &H0
		DailyQuestsCompleted = UnitDynamicArrays.[End] + &H2
		[End] = UnitDynamicArrays.[End] + &H4
	End Enum

	Public Enum GameObjectFields
		CreatedBy = ObjectFields.[End] + &H0
		DisplayID = ObjectFields.[End] + &H2
		Flags = ObjectFields.[End] + &H3
		ParentRotation = ObjectFields.[End] + &H4
		AnimProgress = ObjectFields.[End] + &H8
		FactionTemplate = ObjectFields.[End] + &H9
		Level = ObjectFields.[End] + &Ha
		PercentHealth = ObjectFields.[End] + &Hb
		[End] = ObjectFields.[End] + &Hc
	End Enum

	Public Enum GameObjectDynamicArrays
		' One field, unknown
		UnknownField = DynamicObjectArrays.[End] + &H0
		[End] = DynamicObjectArrays.[End] + &H1
	End Enum

	Public Enum DynamicObjectFields
		Caster = ObjectFields.[End] + &H0
		TypeAndVisualID = ObjectFields.[End] + &H2
		SpellId = ObjectFields.[End] + &H3
		Radius = ObjectFields.[End] + &H4
		CastTime = ObjectFields.[End] + &H5
		[End] = ObjectFields.[End] + &H6
	End Enum

	Public Enum DynamicObjectDynamicArrays
		' Empty
		[End] = DynamicObjectArrays.[End]
	End Enum

	Public Enum CorpseFields
		Owner = ObjectFields.[End] + &H0
		PartyGuid = ObjectFields.[End] + &H2
		DisplayId = ObjectFields.[End] + &H4
		Items = ObjectFields.[End] + &H5
		SkinId = ObjectFields.[End] + &H18
		FacialHairStyleId = ObjectFields.[End] + &H19
		Flags = ObjectFields.[End] + &H1a
		DynamicFlags = ObjectFields.[End] + &H1b
		[End] = ObjectFields.[End] + &H1c
	End Enum

	Public Enum CorpseDynamicArrays
		' Empty
		[End] = DynamicObjectArrays.[End]
	End Enum

	Public Enum AreaTriggerFields
		Caster = ObjectFields.[End] + &H0
		SpellId = ObjectFields.[End] + &H2
		SpellVisualId = ObjectFields.[End] + &H3
		Duration = ObjectFields.[End] + &H4
		[End] = ObjectFields.[End] + &H5
	End Enum

	Public Enum AreaTriggerDynamicArrays
		' Empty
		[End] = DynamicObjectArrays.[End]
	End Enum

	Public Enum SceneObjectFields
		ScriptPackageId = ObjectFields.[End] + &H0
		RndSeedVal = ObjectFields.[End] + &H1
		CreatedBy = ObjectFields.[End] + &H2
		[End] = ObjectFields.[End] + &H4
	End Enum

	Public Enum SceneObjectDynamicArrays
		' Empty
		[End] = DynamicObjectArrays.[End]
	End Enum
End Namespace
