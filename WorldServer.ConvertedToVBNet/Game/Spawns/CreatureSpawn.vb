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
Imports Framework.Logging
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports WorldServer.Game.WorldEntities
Imports System.Collections.Generic

Namespace Game.Spawns
    Public Class CreatureSpawn
        Inherits WorldObject
        Public Id As Int32
        Public Creature As Creature

        Public Sub New(Optional updateLength As Integer = CInt(UnitFields.[End]))
            MyBase.New(updateLength)
        End Sub

        Public Shared Function GetLastGuid() As ULong
            Dim result As SQLResult = DB.World.[Select]("SELECT * FROM `creature_spawns` ORDER BY `guid` DESC LIMIT 1")
            If result.Count <> 0 Then
                Return result.Read(Of ULong)(0, "guid")
            End If

            Return 0
        End Function

        Public Sub CreateFullGuid()
            Guid = New ObjectGuid(Guid, Id, HighGuidType.Unit).Guid
        End Sub

        Public Sub CreateData(creature__1 As Creature)
            Creature = creature__1
        End Sub

        Public Function AddToDB() As Boolean
            If DB.World.Execute("INSERT INTO creature_spawns (Id, Map, X, Y, Z, O) VALUES (?, ?, ?, ?, ?, ?)", Id, Map, Position.X, Position.Y, Position.Z, _
                Position.O) Then
                Log.Message(LogType.DB, "Creature (Id: {0}) successfully spawned (Guid: {1})", Id, Guid)
                Return True
            End If

            Return False
        End Function

        Public Sub AddToWorld()
            CreateFullGuid()
            CreateData(Creature)
            SetUpdateFields()

            Globals.SpawnMgr.AddSpawn(Me)

            Dim obj As WorldObject = Me
            Dim updateFlags As UpdateFlag = UpdateFlag.Alive Or UpdateFlag.Rotation

            For Each v As KeyValuePair(Of ULong, Network.WorldClass) In Globals.WorldMgr.Sessions
                Dim pChar As Character = v.Value.Character

                If pChar.CheckUpdateDistance(Me) Then
                    Dim updateObject As New PacketWriter(LegacyMessage.UpdateObject)

                    updateObject.WriteUInt16(CUShort(Map))
                    updateObject.WriteUInt32(1)

                    WorldMgr.WriteCreateObject(updateObject, obj, updateFlags, ObjectType.Unit)

                    v.Value.Send(updateObject)
                End If
            Next
        End Sub

        Public Overrides Sub SetUpdateFields()
            ' ObjectFields
            SetUpdateField(Of UInt64)(CInt(ObjectFields.Guid), Guid)
            SetUpdateField(Of UInt64)(CInt(ObjectFields.Data), 0)
            SetUpdateField(Of Int32)(CInt(ObjectFields.Entry), Id)
            SetUpdateField(Of Int32)(CInt(ObjectFields.Type), &H9)
            SetUpdateField(Of [Single])(CInt(ObjectFields.Scale), Creature.Data.Scale)

            ' UnitFields
            SetUpdateField(Of UInt64)(CInt(UnitFields.Charm), 1)
            SetUpdateField(Of UInt64)(CInt(UnitFields.Summon), 1)
            SetUpdateField(Of UInt64)(CInt(UnitFields.Critter), 0)
            SetUpdateField(Of UInt64)(CInt(UnitFields.CharmedBy), 0)
            SetUpdateField(Of UInt64)(CInt(UnitFields.SummonedBy), 0)
            SetUpdateField(Of UInt64)(CInt(UnitFields.CreatedBy), 0)
            SetUpdateField(Of UInt64)(CInt(UnitFields.Target), 0)
            SetUpdateField(Of UInt64)(CInt(UnitFields.ChannelObject), 0)

            SetUpdateField(Of Int32)(CInt(UnitFields.Health), Creature.Data.Health)

            For i As Integer = 0 To 4
                SetUpdateField(Of Int32)(CInt(UnitFields.Power) + i, 0)
            Next

            SetUpdateField(Of Int32)(CInt(UnitFields.MaxHealth), Creature.Data.Health)

            For i As Integer = 0 To 4
                SetUpdateField(Of Int32)(CInt(UnitFields.MaxPower) + i, 0)
            Next

            SetUpdateField(Of Int32)(CInt(UnitFields.PowerRegenFlatModifier), 0)
            SetUpdateField(Of Int32)(CInt(UnitFields.PowerRegenInterruptedFlatModifier), 0)
            SetUpdateField(Of Int32)(CInt(UnitFields.BaseHealth), 1)
            SetUpdateField(Of Int32)(CInt(UnitFields.BaseMana), 0)
            SetUpdateField(Of Int32)(CInt(UnitFields.Level), Creature.Data.Level)
            SetUpdateField(Of Int32)(CInt(UnitFields.FactionTemplate), Creature.Data.Faction)
            SetUpdateField(Of Int32)(CInt(UnitFields.Flags), Creature.Data.UnitFlags)
            SetUpdateField(Of Int32)(CInt(UnitFields.Flags2), Creature.Data.UnitFlags2)
            SetUpdateField(Of Int32)(CInt(UnitFields.NpcFlags), Creature.Data.NpcFlags)

            For i As Integer = 0 To 4
                SetUpdateField(Of Int32)(CInt(UnitFields.Stats) + i, 0)
                SetUpdateField(Of Int32)(CInt(UnitFields.StatPosBuff) + i, 0)
                SetUpdateField(Of Int32)(CInt(UnitFields.StatNegBuff) + i, 0)
            Next

            SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), 0, 0)
            SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), 0, 1)
            SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), 0, 2)
            SetUpdateField(Of [Byte])(CInt(UnitFields.DisplayPower), 0, 3)

            SetUpdateField(Of Int32)(CInt(UnitFields.DisplayID), Creature.Stats.DisplayInfoId(0))
            SetUpdateField(Of Int32)(CInt(UnitFields.NativeDisplayID), Creature.Stats.DisplayInfoId(2))
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
        End Sub
    End Class
End Namespace
