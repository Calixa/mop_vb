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
    Public Class GameObjectSpawn
        Inherits WorldObject
        Public Id As Int32
        Public FactionTemplate As UInt32
        Public AnimProgress As [Byte]
        Public Activated As [Boolean]
        Public GameObject As GameObject

        Public Sub New(Optional updateLength As Integer = CInt(GameObjectFields.[End]))
            MyBase.New(updateLength)
        End Sub

        Public Shared Function GetLastGuid() As ULong
            Dim result As SQLResult = DB.World.[Select]("SELECT * FROM `gameobject_spawns` ORDER BY `guid` DESC LIMIT 1")
            If result.Count <> 0 Then
                Return result.Read(Of ULong)(0, "guid")
            End If

            Return 0
        End Function

        Public Sub CreateFullGuid()
            Guid = New ObjectGuid(Guid, Id, HighGuidType.GameObject).Guid
        End Sub

        Public Sub CreateData(gameobject__1 As GameObject)
            GameObject = gameobject__1
        End Sub

        Public Function AddToDB() As Boolean
            If DB.World.Execute("INSERT INTO gameobject_spawns (Id, Map, X, Y, Z, O) VALUES (?, ?, ?, ?, ?, ?)", Id, Map, Position.X, Position.Y, Position.Z, _
                Position.O) Then
                Log.Message(LogType.DB, "Gameobject (Id: {0}) successfully spawned (Guid: {1})", Id, Guid)
                Return True
            End If

            Return False
        End Function

        Public Sub AddToWorld()
            CreateFullGuid()
            CreateData(GameObject)
            SetUpdateFields()

            Globals.SpawnMgr.AddSpawn(Me, GameObject)

            Dim obj As WorldObject = Me
            Dim updateFlags As UpdateFlag = UpdateFlag.Rotation Or UpdateFlag.StationaryPosition

            For Each v As KeyValuePair(Of ULong, Network.WorldClass) In Globals.WorldMgr.Sessions
                Dim pChar As Character = v.Value.Character

                If pChar.CheckUpdateDistance(Me) Then
                    Dim updateObject As New PacketWriter(LegacyMessage.UpdateObject)

                    updateObject.WriteUInt16(CUShort(Map))
                    updateObject.WriteUInt32(1)

                    WorldMgr.WriteCreateObject(updateObject, obj, updateFlags, ObjectType.GameObject)

                    v.Value.Send(updateObject)
                End If
            Next
        End Sub

        Public Overrides Sub SetUpdateFields()
            ' ObjectFields
            SetUpdateField(Of UInt64)(CInt(ObjectFields.Guid), Guid)
            SetUpdateField(Of UInt64)(CInt(ObjectFields.Data), 0)
            SetUpdateField(Of Int32)(CInt(ObjectFields.Type), &H21)
            SetUpdateField(Of Int32)(CInt(ObjectFields.Entry), Id)
            SetUpdateField(Of [Single])(CInt(ObjectFields.Scale), GameObject.Stats.Size)

            ' GameObjectFields
            SetUpdateField(Of UInt64)(CInt(GameObjectFields.CreatedBy), 0)
            SetUpdateField(Of Int32)(CInt(GameObjectFields.DisplayID), GameObject.Stats.DisplayInfoId)
            SetUpdateField(Of Int32)(CInt(GameObjectFields.Flags), GameObject.Stats.Flags)
            SetUpdateField(Of [Single])(CInt(GameObjectFields.ParentRotation), 0)
            SetUpdateField(Of [Single])(CInt(GameObjectFields.ParentRotation) + 1, 0)
            SetUpdateField(Of [Single])(CInt(GameObjectFields.ParentRotation) + 2, 0)
            SetUpdateField(Of [Single])(CInt(GameObjectFields.ParentRotation) + 3, 1)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.AnimProgress), AnimProgress)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.AnimProgress), 0, 1)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.AnimProgress), 255, 2)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.AnimProgress), 255, 3)
            SetUpdateField(Of UInt32)(CInt(GameObjectFields.FactionTemplate), FactionTemplate)
            SetUpdateField(Of Int32)(CInt(GameObjectFields.Level), 0)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.PercentHealth), Convert.ToByte(Activated))
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.PercentHealth), CByte(GameObject.Stats.Type), 1)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.PercentHealth), 0, 2)
            SetUpdateField(Of [Byte])(CInt(GameObjectFields.PercentHealth), 255, 3)
        End Sub
    End Class
End Namespace
