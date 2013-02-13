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
	Public Enum MessageType As Byte
		ChatMessageSystem = &H0
		ChatMessageSay = &H1
		ChatMessageParty = &H2
		ChatMessageRaid = &H3
		ChatMessageGuild = &H4
		ChatMessageOfficer = &H5
		ChatMessageYell = &H6
		ChatMessageWhisper = &H7
		ChatMessageWhisperForeign = &H8
		ChatMessageWhisperInform = &H9
		ChatMessageEmote = &Ha
		ChatMessageTextEmote = &Hb
		ChatMessageMonsterSay = &Hc
		ChatMessageMonsterParty = &Hd
		ChatMessageMonsterYell = &He
		ChatMessageMonsterWhisper = &Hf
		ChatMessageMonsteeEmote = &H10
		ChatMessageChannel = &H11
		ChatMessageChannelJoin = &H12
		ChatMessageChannelLeave = &H13
		ChatMessageChannelList = &H14
		ChatMessageChannelNotice = &H15
		ChatMessageChannelNoticeUser = &H16
		ChatMessageAfk = &H17
		ChatMessageDnd = &H18
		ChatMessageIgnored = &H19
		ChatMessageSkill = &H1a
		ChatMessageLoot = &H1b
		ChatMessageMoney = &H1c
		ChatMessageOpening = &H1d
		ChatMessageTradeskills = &H1e
		ChatMessagePetInfo = &H1f
		ChatMessageCombatMiscInfo = &H20
		ChatMessageCombatXpGain = &H21
		ChatMessageCombatHonorGain = &H22
		ChatMessageCombatFactionChange = &H23
		ChatMessageBgSystemNeutral = &H24
		ChatMessageBgSystemAlliance = &H25
		ChatMessageBgSystemHorde = &H26
		ChatMessageRaidLeader = &H27
		ChatMessageRaidWarning = &H28
		ChatMessageRaidBossEmote = &H29
		ChatMessageRaidBossWhisper = &H2a
		ChatMessageFiltred = &H2b
		ChatMessageBattleground = &H2c
		ChatMessageBattlegroundLeader = &H2d
		ChatMessageRestricted = &H2e
		ChatMessageBattlenet = &H2f
		ChatMessageAchievment = &H30
		ChatMessageGuildAchievment = &H31
		ChatMessageArenaPoints = &H32
		ChatMessagePartyLeader = &H33
	End Enum
End Namespace
