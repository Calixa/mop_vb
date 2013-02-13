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
	<Flags> _
	Public Enum JAMCCMessage As UInteger
		AuthChallenge = &Hcaf
		SuspendComms = &H82a
		ResumeComms = &He2b
		DropNewConnection = &Hc2f
		ConnectTo = &H82b
		Pong = &H8ae
		ResetCompressionContext = &He2f
		FloodDetected = &Ha2a
	End Enum

	<Flags> _
	Public Enum JAMCMessage As UInteger
		AuthResponse = &Ha15
		WaitQueueUpdate = &He7c
		WaitQueueFinish = &H1f8
		AllAchievementData = &H618
		AllAccountCriteria = &H5d4
		RespondInspectAchievements = &H7b8
		AllGuildAchievements = &H9f5
		SetupCurrency = &H4d0
		SetCurrency = &H918
		ResetWeeklyCurrency = &H911
		GuildSendGuildXP = &Hc10
		GuildSendMaxDailyXP = &H510
		MessageBox = &H8d1
		WardenData = &H6bd
		PhaseShiftChange = &H5f4
		InitialSetup = &H675
		DailyQuestsReset = &H331
		SetQuestCompletedBit = &He7d
		ClearQuestCompletedBit = &H430
		ClearQuestCompletedBits = &Hb95
		AuraPointsDepleted = &H95
		GuildSendRankChange = &H3b0
		ReforgeResult = &H95d
		TradeUpdated = &H2bd
		TradeStatus = &H5b1
		EnumCharactersResult = &H33d
		GenerateRandomCharacterNameResult = &Hb74
		GuildCommandResult = &H9fc
		GuildRoster = &Ha75
		GuildRosterUpdate = &H270
		CurrencyLootRemoved = &Hf1d
		GuildMemberRecipes = &H1b4
		GuildKnownRecipes = &H4fc
		GuildMembersWithRecipe = &H411
		SetupResearchHistory = &Hd7c
		ResearchComplete = &Hf99
		PetSlotUpdated = &Ha5d
		DifferentInstanceFromParty = &Hd91
		UpdateServerPlayerPosition = &H71c
		CategoryCooldown = &Hdd9
		RoleChangedInform = &He70
		GuildRewardList = &H55d
		RolePollInform = &Hf15
		SummonRaidMemberValidateFailed = &Hc31
		BattlefieldStatus_NeedConfirmation = &Hf51
		BattlefieldStatus_Active = &H23c
		BattlefieldStatus_Queued = &H8f9
		BattlefieldStatus_None = &H49c
		BattlefieldStatus_Failed = &H6b4
		BattlefieldList = &H530
		BattlegroundPlayerPositions = &H734
		BattlegroundPlayerJoined = &Hdc
		BattlegroundPlayerLeft = &H1fd
		BattlefieldPortDenied = &H1d
		BFMgrEntryInvite = &Hc3d
		BFMgrEntering = &Heb5
		BFMgrQueueRequestResponse = &Hf35
		BFMgrEjectPending = &He54
		BFMgrEjected = &H7f1
		BFMgrQueueInvite = &Hc1d
		BFMgrExitRequest = &He78
		BFMgrStateChanged = &Hc75
		BattlegroundInfoThrottled = &H315
		QuestCompletionNPCResponse = &Hc9c
		RequestCemeteryListResponse = &H75
		SetForgeMaster = &Hb39
		CheckWargameEntry = &Ha58
		ShowRatings = &Had0
		DBReply = &H59c
		HotfixNotify = &H5b0
		HotfixNotifyBlob = &H9b5
		BattlefieldStatus_WaitForGroups = &H2b0
		GuildNews = &H7d0
		GuildNewsDeleted = &Hf7c
		RatedBattlefieldInfo = &H1fc
		AverageItemLevelInform = &H31d
		GuildCriteriaUpdate = &H799
		GuildAchievementEarned = &H474
		GuildAchievementDeleted = &H694
		GuildCriteriaDeleted = &H8d9
		GuildAchievementMembers = &Hb71
		ArenaTeamCommandResult = &H5b4
		PetAdded = &H13c
		GuildRanks = &Hd50
		GuildXPEarned = &Hbf9
		NewWorld = &H81d
		AbortNewWorld = &H295
		GuildMemberUpdateNote = &Hab4
		GuildInvite = &Hd30
		NotifyMoney = &H99c
		QuestGiverQuestComplete = &He35
		ItemPurchaseRefundResult = &H7b5
		SetItemPurchaseData = &H4d1
		ItemExpirePurchaseRefund = &H795
		InspectHonorStats = &H5d0
		GuildPartyState = &H8bd
		PVPLogData = &Hc5d
		RatedBGStats = &H418
		WargameRequestSuccessfullySentToOpponent = &H759
		DisplayGameError = &Ha11
		PVPOptionsEnabled = &Ha78
		RatedBattlegroundRating = &H935
		SetMaxWeeklyQuantity = &H9d1
		GuildReputationWeeklyCap = &H61c
		GuildReputationReactionChanged = &Hf59
		PetitionAlreadySigned = &Hc39
		RaidMarkersChanged = &H930
		CommentatorPartyInfo = &H871
		StreamingMovies = &H9fd
		TimeSyncRequest = &H410
		TimeAdjustment = &H4f1
		StartTimer = &H9b0
		DisenchantCredit = &H990
		SuspendToken = &Hb5c
		ResumeToken = &H998
		CancelSpellVisual = &H8d8
		PlaySpellVisual = &Hf55
		CancelOrphanSpellVisual = &H590
		PlayOrphanSpellVisual = &Hf9
		LFGuildPost = &He91
		LFGuildBrowse = &H19
		LFGuildRecruits = &H45d
		LFGuildCommandResult = &H570
		LFGuildApplications = &H130
		CancelSpellVisualKit = &H35c
		PlaySpellVisualKit = &H230
		AddItemPassive = &H7f0
		RemoveItemPassive = &H439
		SendItemPassives = &H8dd
		WorldServerInfo = &Hd9
		WeeklySpellUsage = &H3b5
		UpdateWeeklySpellUsage = &H658
		LastWeeklyReset = &Hfd
		GuildChallengeUpdate = &Hf3d
		GuildChallengeCompleted = &H4f9
		LFGuildApplicantListChanged = &H355
		LFGuildApplicationsListChanged = &H2d5
		InspectRatedBGStats = &H2b5
		MoveSetActiveMover = &H7dc
		RuneRegenDebug = &Hcd0
		MoveUpdateRunSpeed = &H49d
		MoveUpdateRunBackSpeed = &H8b5
		MoveUpdateWalkSpeed = &H3b8
		MoveUpdateSwimSpeed = &H6b8
		MoveUpdateSwimBackSpeed = &H7bc
		MoveUpdateFlightSpeed = &H895
		MoveUpdateFlightBackSpeed = &Hb15
		MoveUpdateTurnRate = &Hbb5
		MoveUpdatePitchRate = &Hb70
		MoveUpdateCollisionHeight = &Hdd8
		MoveUpdate = &H294
		MoveUpdateTeleport = &He1d
		MoveUpdateKnockBack = &H175
		MoveUpdateApplyMovementForce = &Hd38
		MoveUpdateRemoveMovementForce = &H579
		MoveSplineSetRunSpeed = &H4b1
		MoveSplineSetRunBackSpeed = &H438
		MoveSplineSetSwimSpeed = &H970
		MoveSplineSetSwimBackSpeed = &Hd70
		MoveSplineSetFlightSpeed = &Hbb0
		MoveSplineSetFlightBackSpeed = &Hdd4
		MoveSplineSetWalkBackSpeed = &H539
		MoveSplineSetTurnRate = &Ha34
		MoveSplineSetPitchRate = &H594
		MoveSetRunSpeed = &H231
		MoveSetRunBackSpeed = &H394
		MoveSetSwimSpeed = &H9d
		MoveSetSwimBackSpeed = &H475
		MoveSetFlightSpeed = &H5fd
		MoveSetFlightBackSpeed = &H159
		MoveSetWalkSpeed = &Hdd1
		MoveSetTurnRate = &H234
		MoveSetPitchRate = &H351
		MoveRoot = &H955
		MoveUnroot = &H15c
		MoveSetWaterWalk = &H21c
		MoveSetLandWalk = &Hd94
		MoveSetFeatherFall = &H9f4
		MoveSetNormalFall = &Hc9d
		MoveSetHovering = &H251
		MoveUnsetHovering = &H3b1
		MoveKnockBack = &H194
		MoveTeleport = &H371
		MoveSetCanFly = &H419
		MoveUnsetCanFly = &H63d
		MoveSetCanTurnWhileFalling = &H451
		MoveUnsetCanTurnWhileFalling = &H2dd
		MoveEnableTransitionBetweenSwimAndFly = &H459
		MoveDisableTransitionBetweenSwimAndFly = &Hd4
		MoveDisableGravity = &Hdd0
		MoveEnableGravity = &Hc79
		MoveDisableCollision = &Hd54
		MoveEnableCollision = &H7d4
		MoveSetCollisionHeight = &H99d
		MoveSetVehicleRecID = &H631
		MoveApplyMovementForce = &Heb8
		MoveRemoveMovementForce = &Hdf4
		MoveSetCompoundState = &Hc34
		MoveSkipTime = &H338
		MoveSplineRoot = &H75d
		MoveSplineUnroot = &H39d
		MoveSplineDisableGravity = &Hb55
		MoveSplineEnableGravity = &H4f8
		MoveSplineDisableCollision = &H34
		MoveSplineEnableCollision = &Ha70
		MoveSplineSetFeatherFall = &H41d
		MoveSplineSetNormalFall = &H854
		MoveSplineSetHover = &H4f4
		MoveSplineUnsetHover = &H698
		MoveSplineSetWaterWalk = &H211
		MoveSplineSetLandWalk = &Hb31
		MoveSplineStartSwim = &H7d5
		MoveSplineStopSwim = &H71d
		MoveSplineSetRunMode = &H319
		MoveSplineSetWalkMode = &H531
		MoveSplineSetFlying = &Hd5d
		MoveSplineUnsetFlying = &H255
		VendorInventory = &Hd3d
		RestrictedAccountWarning = &Hcd8
		GuildReset = &H1f9
		SetPlayHoverAnim = &Hf58
		GuildMoveStarting = &H730
		GuildMoved = &H8fc
		ClearBossEmotes = &H259
		LoadCUFProfiles = &H179
		SuppressNPCGreetings = &H2fc
		PartyInvite = &H574
		DumpRideTicketsResponse = &Hab5
		FeatureSystemStatus = &H7f9
		GuildNameChanged = &H851
		RequestPVPRewardsResponse = &H23d
		SpellInterruptLog = &H635
		GameObjectActivateAnimKit = &Hb19
		MapObjEvents = &Ha7c
		MissileCancel = &Hcf9
		VoidStorageFailed = &H83d
		VoidStorageContents = &Hab1
		VoidStorageTransferChanges = &H8f0
		VoidTransferResult = &H6d8
		VoidItemSwapResponse = &H995
		XPGainAborted = &H398
		GuildFlaggedForRename = &H50
		GuildChangeNameResult = &H4d9
		PrintNotification = &H218
		ClearCooldowns = &H890
		FailedPlayerCondition = &H77d
		CustomLoadScreen = &H6f9
		TransferPending = &Hebc
		GuildBankQueryResults = &Hed1
		GuildBankLogQueryResults = &H774
		GuildBankRemainingWithdrawMoney = &H8b8
		GuildPermissionsQueryResults = &H89c
		GuildEventLogQueryResults = &H8b4
		GuildBankTextQueryResult = &H2f5
		GuildMemberDailyReset = &H554
		GameEventDebugLog = &H291
		ServerPerf = &Hf79
		AreaTriggerMovementUpdate = &H898
		AdjustSplineDuration = &H97d
		LearnTalentFailed = &H58
		LFGJoinResult = &Hbf8
		LFGQueueStatus = &H950
		LFGRoleCheckUpdate = &H8b0
		LFGUpdateStatusNone = &Habd
		LFGUpdateStatus = &H154
		LFGProposalUpdate = &Hdd5
		LFGSearchResults = &H5c
		ServerInfoResponse = &Hf4
		LootContents = &Hdb1
		ShowNeutralPlayerFactionSelectUI = &Hcf1
		NeutralPlayerFactionSelectResult = &H8f5
		ChatIgnoredAccountMuted = &H115
		SORStartExperienceIncomplete = &Ha99
		AccountInfoResponse = &H659
		SetDFFastLaunchResult = &H9d0
		SupercededSpells = &H5d8
		LearnedSpells = &H6b5
		UnlearnedSpells = &Ha35
		PetLearnedSpells = &He71
		PetUnlearnedSpells = &Hb30
		UpdateActionButtons = &H951
		DontAutoPushSpellsToActionBar = &H318
		LFGSlotInvalid = &H6d4
		UpdateDungeonEncounterForLoot = &Hf9d
		SceneObjectEvent = &H2dc
		SendAllItemDurability = &H3d8
		BattlePetUpdates = &Hcfc
		BattlePetTrapLevel = &H339
		PetBattleSlotUpdates = &H535
		BattlePetJournalLockAcquired = &H9dd
		BattlePetJournalLockDenied = &Hc58
		BattlePetJournal = &H98
		BattlePetDeleted = &H434
		BattlePetsHealed = &Hb10
		BattlePetLicenseChanged = &H3f9
		PartyUpdate = &H87c
		ReadyCheckStarted = &H4b0
		ReadyCheckResponse = &He98
		ReadyCheckCompleted = &H17c
		PetBattleRequestFailed = &Hcb0
		PetBattlePVPChallenge = &Hddd
		PetBattleFinalizeLocation = &H3fc
		PetBattleFullUpdate = &Hd35
		PetBattleFirstRound = &Hbd8
		PetBattleRoundResult = &H31c
		PetBattleReplacementsMade = &Hed4
		PetBattleFinalRound = &H199
		PetBattleFinished = &Hb75
		PetBattleChatRestricted = &H874
		PetBattleMaxGameLengthWarning = &He50
		StartElapsedTimer = &He90
		StopElapsedTimer = &Hdbd
		StartElapsedTimers = &H31
		ChallengeModeComplete = &H10
		ChallengeModeRewards = &Had5
		IncreaseCastTimeForSpell = &H278
		ClearAllSpellCharges = &H139
		ChallengeModeAllMapStats = &H415
		ChallengeModeMapStatsUpdate = &Haf1
		ChallengeModeRequestLeadersResult = &Hd31
		ChallengeModeNewPlayerRecord = &He38
		RespecWipeConfirm = &Hbf4
		IsQuestCompleteResponse = &H7f4
		GMCharacterRestoreResponse = &Hdd
		LootResponse = &Hd98
		LootRemoved = &H1f0
		LootUpdated = &H715
		CoinRemoved = &Hf94
		AELootTargets = &H954
		AELootTargetAck = &H59d
		LootReleaseAll = &H1d5
		LootRelease = &H670
		LootMoneyNotify = &Hb38
		StartLootRoll = &H334
		LootRoll = &H77c
		MasterLootCandidateList = &H674
		LootItemList = &H3b9
		LootRollsComplete = &Ha50
		LootAllPassed = &H1bd
		LootRollWon = &H53c
		SetItemChallengeModeData = &Ha30
		ClearItemChallengeModeData = &Hbbd
		ItemPushResult = &He51
		DisplayToast = &Hd59
		AreaTriggerDebugSweep = &Hb98
		AreaTriggerDebugPlayerInside = &H131
		ResetAreaTrigger = &H7f8
		SetPetSpecialization = &H5bc
		BlackMarketOpenResult = &Hed0
		BlackMarketRequestItemsResult = &H751
		BlackMarketBidOnItemResult = &Hb18
		BlackMarketOutbid = &H1dc
		BlackMarketWon = &H975
		ScenarioState = &Hd18
		ScenarioProgressUpdate = &H70
		GroupNewLeader = &Hd71
		SendRaidTargetUpdateAll = &H498
		SendRaidTargetUpdateSingle = &H57c
		RandomRoll = &Ha5c
		InspectResult = &H4dd
		ScenarioPOIs = &H739
		InstanceInfo = &H89d
		ConsoleWrite = &H47c
		AccountCriteriaUpdate = &Hcf5
		PlayScene = &Hbc
		CancelScene = &Hc15
		BattlePetError = &H819
		PetBattleQueueProposeMatch = &Hd11
		PetBattleQueueStatus = &H610
		MailCommandResult = &H39
		AddBattlenetFriendResponse = &Hf14
		ItemUpgradeResult = &Hadd
		MoveCharacterCheatSuccess = &H891
		MoveCharacterCheatFailure = &H875
		AchievementEarned = &H719
		AreaShareInfoResponse = &Hd5c
		LFGTeleportDenied = &H9bd
		AreaShareMappingsResponse = &H450
		BonusRollEmpty = &H511
		UpdateExpansionLevel = &H79
		ControlUpdate = &H578
		ArenaPrepOpponentSpecializations = &He58
		GMTicketGetTicketResponse = &Ha18
		NukeAllObjectsDueToRealmBundlePort = &Hd51
		GMNotifyAreaChange = &Hef8
		ForceObjectRelink = &H391
		DisplayPromotion = &Ha7d
		ClearedPromotion = &H595
		ServerFirstAchievements = &Hd78
		CorpseLocation = &Hf78
		CanDuelResult = &H754
		ImmigrantHostSearchLog = &H778
		SendKnownSpells = &H155
		SendSpellHistory = &H815
		SendSpellCharges = &He30
		SendUnlearnSpells = &Hdf5
		RefreshComponent = &Hdb9
	End Enum

	<Flags> _
	Public Enum LegacyMessage As UInteger
		#Region "Cinematic"
		StartCinematic = &H48d
		#End Region

		AddonInfo = &H760
		UpdateClientCacheVersion = &H72d
		RealmSplitStateResponse = &H5cc
		ResponseCharacterCreate = &Hf25
		ResponseCharacterDelete = &He44
		MessageOfTheDay = &H849
		AccountDataInitialized = &He48
		UpdateObject = &H120
		ObjectDestroy = &H34c
		TutorialFlags = &H6a8
		UITime = 0
		LogoutComplete = &H2a0
		MessageChat = &H9
		CreatureStats = &Haa4
		GameObjectStats = &H80d
		NameCache = &H30d
		RealmCache = &Hd81
	End Enum

	<Flags> _
	Public Enum ClientMessage As Integer
		#Region "Authentication"
		TransferInitiate = &H4f57
		AuthSession = &Hc07
		#End Region

		#Region "CharacterList"
		ReadyForAccountDataTimes = &H5a5
		EnumCharacters = &H146
		RequestCharCreate = &Heb3
		RequestCharDelete = &Hc2d
		RequestRandomCharacterName = &H4e
		SetRealmSplitState = &H261
		#End Region

		#Region "WorldEnter"
		LoadingScreenNotify = &H6
		ViolenceLevel = &H56
		PlayerLogin = &Heba
		#End Region

		#Region "Logout"
		Logout = &H5a0
		#End Region

		#Region "Disconnect"
		LogDisconnect = &Hd47
		#End Region

		#Region "Permanent"
		Ping = &Hca7
		#End Region

		#Region "Uncategorized"
		SuspendCommsAck = -1
		AuthContinuedSession = -1
		EnableNagle = -1
		SuspendTokenResponse = -1
		RequestUITime = -1
		ActivePlayer = &Hf84
		CreatureStats = &H285
		GameObjectStats = &Hbe9
		NameCache = &H1ec
		RealmCache = &Ha4d
		ZoneUpdate = &H88d
		SetSelection = &H17e
		ObjectUpdateFailed = &H2fb
		#End Region

		#Region "ChatMessages"
		ChatMessageSay = &H67a
		ChatMessageYell = &Hf7f
		ChatMessageWhisper = &H306
		#End Region
		#Region "PlayerMovement"
		MoveStartForward = &Hfe
		MoveStartBackward = &H37a
		MoveHeartBeat = &Hbd2
		MoveStop = &H9df
		MoveStartTurnLeft = &H46e
		MoveStartTurnRight = &H9f7
		MoveStopTurn = &H4cb
		#End Region
	End Enum

	<Flags> _
	Public Enum Message As UInteger
		TransferInitiate = &H4f57
	End Enum
End Namespace
