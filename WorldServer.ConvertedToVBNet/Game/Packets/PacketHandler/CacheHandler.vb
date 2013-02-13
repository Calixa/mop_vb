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


Imports Framework.Configuration
Imports Framework.Constants
Imports Framework.Database
Imports Framework.Network.Packets
Imports WorldServer.Game.ObjectDefines
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network
Imports Framework.Logging
Imports System.Collections.Generic

Namespace Game.Packets.PacketHandler
    Public Class CacheHandler
        Inherits Globals
        <Opcode(ClientMessage.CreatureStats, "16357")> _
        Public Shared Sub HandleCreatureStats(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim id As Integer = packet.ReadInt32()
            Dim guid As ULong = packet.ReadUInt64()

            Dim creature As Creature = DataMgr.FindCreature(id)
            If creature IsNot Nothing Then
                Dim stats As CreatureStats = creature.Stats

                Dim creatureStats As New PacketWriter(LegacyMessage.CreatureStats)

                creatureStats.WriteInt32(stats.Id)
                creatureStats.WriteCString(stats.Name)

                For i As Integer = 0 To 6
                    creatureStats.WriteCString("")
                Next

                creatureStats.WriteCString(stats.SubName)
                creatureStats.WriteCString("")
                creatureStats.WriteCString(stats.IconName)

                For Each v As Integer In stats.Flag
                    creatureStats.WriteInt32(v)
                Next

                creatureStats.WriteInt32(stats.Type)
                creatureStats.WriteInt32(stats.Family)
                creatureStats.WriteInt32(stats.Rank)

                For Each v As Integer In stats.QuestKillNpcId
                    creatureStats.WriteInt32(v)
                Next

                For Each v As Integer In stats.DisplayInfoId
                    creatureStats.WriteInt32(v)
                Next

                creatureStats.WriteFloat(stats.HealthModifier)
                creatureStats.WriteFloat(stats.PowerModifier)

                creatureStats.WriteUInt8(stats.RacialLeader)

                For Each v As Integer In stats.QuestItemId
                    creatureStats.WriteInt32(v)
                Next

                creatureStats.WriteInt32(stats.MovementInfoId)
                creatureStats.WriteInt32(stats.ExpansionRequired)

                session.Send(creatureStats)
            Else
                Log.Message(LogType.DEBUG, "Creature (Id: {0}) not found.", id)
            End If
        End Sub

        <Opcode(ClientMessage.GameObjectStats, "16357")> _
        Public Shared Sub HandleGameObjectStats(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim id As Integer = packet.ReadInt32()
            Dim guid As ULong = packet.ReadUInt64()

            Dim gObject As GameObject = DataMgr.FindGameObject(id)
            If gObject IsNot Nothing Then
                Dim stats As GameObjectStats = gObject.Stats

                Dim gameObjectStats As New PacketWriter(LegacyMessage.GameObjectStats)

                gameObjectStats.WriteInt32(stats.Id)
                gameObjectStats.WriteInt32(stats.Type)
                gameObjectStats.WriteInt32(stats.DisplayInfoId)
                gameObjectStats.WriteCString(stats.Name)

                For i As Integer = 0 To 2
                    gameObjectStats.WriteCString("")
                Next

                gameObjectStats.WriteCString(stats.IconName)
                gameObjectStats.WriteCString(stats.CastBarCaption)
                gameObjectStats.WriteCString("")

                For Each v As Integer In stats.Data
                    gameObjectStats.WriteInt32(v)
                Next

                gameObjectStats.WriteFloat(stats.Size)

                For Each v As Integer In stats.QuestItemId
                    gameObjectStats.WriteInt32(v)
                Next

                gameObjectStats.WriteInt32(stats.ExpansionRequired)

                session.Send(gameObjectStats)
            Else
                Log.Message(LogType.DEBUG, "Gameobject (Id: {0}) not found.", id)
            End If
        End Sub

        <Opcode(ClientMessage.NameCache, "16357")> _
        Public Shared Sub HandleNameCache(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim guid As ULong = packet.ReadUInt64()
            Dim realmId As UInteger = packet.ReadUInt32()

            Dim pSession As WorldClass = WorldMgr.GetSession(guid)
            If pSession IsNot Nothing Then
                Dim pChar As WorldEntities.Character = pSession.Character

                If pChar IsNot Nothing Then
                    Dim nameCache As New PacketWriter(LegacyMessage.NameCache)

                    nameCache.WriteGuid(CLng(guid))
                    nameCache.WriteUInt8(0)
                    nameCache.WriteCString(pChar.Name)
                    nameCache.WriteUInt32(realmId)
                    nameCache.WriteUInt8(pChar.Race)
                    nameCache.WriteUInt8(pChar.Gender)
                    nameCache.WriteUInt8(pChar.[Class])
                    nameCache.WriteUInt8(0)

                    session.Send(nameCache)
                End If
            End If
        End Sub

        <Opcode(ClientMessage.RealmCache, "16357")> _
        Public Shared Sub HandleRealmCache(ByRef packet As PacketReader, ByRef session As WorldClass)
            Dim pChar As Character = session.Character

            Dim realmId As UInteger = packet.ReadUInt32()

            Dim result As SQLResult = DB.Realms.[Select]("SELECT name FROM realms WHERE id = ?", WorldConfig.RealmId)
            Dim realmName As String = result.Read(Of String)(0, "Name")

            Dim nameCache As New PacketWriter(LegacyMessage.RealmCache)

            nameCache.WriteUInt32(realmId)
            nameCache.WriteUInt8(0)
            ' < 0 => End of packet
            nameCache.WriteUInt8(1)
            ' Unknown
            nameCache.WriteCString(realmName)
            nameCache.WriteCString(realmName)

            session.Send(nameCache)
        End Sub
    End Class
End Namespace
