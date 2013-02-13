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
Imports Framework.Constants.ObjectSettings
Imports Framework.Network.Packets
Imports Framework.ObjectDefines
Imports Framework.Singleton
Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Managers
	Public NotInheritable Class WorldManager
		Inherits SingletonBase(Of WorldManager)
		Public Sessions As ConcurrentDictionary(Of ULong, WorldClass)
		Public Property Session() As WorldClass
			Get
				Return m_Session
			End Get
			Set
				m_Session = Value
			End Set
		End Property
		Private m_Session As WorldClass

		Shared ReadOnly taskObject As New Object()

		Private Sub New()
			Sessions = New ConcurrentDictionary(Of ULong, WorldClass)()

			StartRangeUpdateTimers()
		End Sub

		Public Sub StartRangeUpdateTimers()
            Dim updateTask__1 As Thread = New Thread(AddressOf UpdateTask)
			updateTask__1.IsBackground = True
			updateTask__1.Start()
		End Sub

		Private Sub UpdateTask()
			While True
				SyncLock taskObject
					Thread.Sleep(50)

                    For Each s As KeyValuePair(Of ULong, WorldClass) In Sessions.ToList()
                        Dim session As WorldClass = s.Value
                        Dim pChar As WorldEntities.Character = session.Character

                        WriteInRangeObjects(Globals.SpawnMgr.GetInRangeCreatures(pChar), session, ObjectType.Unit)
                        WriteInRangeObjects(Globals.SpawnMgr.GetInRangeGameObjects(pChar), session, ObjectType.GameObject)
                        WriteInRangeObjects(GetInRangeCharacter(pChar), session, ObjectType.Player)

                        WriteOutOfRangeObjects(Globals.SpawnMgr.GetOutOfRangeCreatures(pChar), session)
                        WriteOutOfRangeObjects(Globals.SpawnMgr.GetOutOfRangeGameObjects(pChar), session)
                        WriteOutOfRangeObjects(GetOutOfRangeCharacter(pChar), session)
                    Next
				End SyncLock
			End While
		End Sub

		Public Function AddSession(guid As ULong, ByRef session As WorldClass) As Boolean
			Return Sessions.TryAdd(guid, session)
		End Function

		Public Function DeleteSession(guid As ULong) As WorldClass
            Dim removedSession As New WorldClass
			Sessions.TryRemove(guid, removedSession)

			Return removedSession
		End Function

		Public Function GetSession(name As String) As WorldClass
            For Each s As KeyValuePair(Of ULong, WorldClass) In Sessions
                If s.Value.Character.Name = name Then
                    Return s.Value
                End If
            Next

			Return Nothing
		End Function

		Public Function GetSession(guid As ULong) As WorldClass
            Dim session As New WorldClass
			Sessions.TryGetValue(guid, session)

			Return session
		End Function

		Public Sub WriteCreateObject(ByRef updateObject As PacketWriter, obj As WorldObject, updateFlags As UpdateFlag, type As ObjectType)
			updateObject.WriteUInt8(CByte(UpdateType.CreateObject))
            updateObject.WriteGuid(CLng(obj.Guid))
			updateObject.WriteUInt8(CByte(type))

			Globals.WorldMgr.WriteUpdateObjectMovement(updateObject, obj, updateFlags)

			obj.WriteUpdateFields(updateObject)
			obj.WriteDynamicUpdateFields(updateObject)
		End Sub

		Private Sub WriteInRangeObjects(objects As IEnumerable(Of WorldObject), session As WorldClass, type As ObjectType)
            Dim pChar As WorldEntities.Character = session.Character
            Dim count As Integer = objects.Count()
            Dim updateFlags As UpdateFlag = UpdateFlag.Rotation

			If count > 0 Then
				updateFlags = updateFlags Or If(type = ObjectType.GameObject, UpdateFlag.StationaryPosition, UpdateFlag.Alive)

				Dim updateObject As New PacketWriter(LegacyMessage.UpdateObject)
				updateObject.WriteUInt16(CUShort(pChar.Map))
				updateObject.WriteUInt32(CUInt(count))

                For Each o As WorldObject In objects
                    Dim obj As WorldObject = o

                    If Not pChar.InRangeObjects.ContainsKey(o.Guid) Then
                        WriteCreateObject(updateObject, obj, updateFlags, type)

                        If pChar.Guid <> o.Guid Then
                            pChar.InRangeObjects.Add(obj.Guid, obj)
                        End If
                    End If
                Next

				session.Send(updateObject)
			End If
		End Sub

		Private Sub WriteOutOfRangeObjects(objects As IEnumerable(Of WorldObject), session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character
            Dim count As Integer = objects.Count()

			If count > 0 Then
				Dim updateObject As New PacketWriter(LegacyMessage.UpdateObject)

				updateObject.WriteUInt16(CUShort(pChar.Map))
				updateObject.WriteUInt32(1)
				updateObject.WriteUInt8(CByte(UpdateType.OutOfRange))
				updateObject.WriteUInt32(CUInt(count))

                For Each o As WorldObject In objects
                    updateObject.WriteGuid(CLng(o.Guid))

                    pChar.InRangeObjects.Remove(o.Guid)
                Next

				session.Send(updateObject)
			End If
		End Sub

		Public Function GetInRangeCharacter(obj As WorldObject) As IEnumerable(Of Character)
            For Each c As KeyValuePair(Of ULong, WorldClass) In Sessions.ToList()
                If Not obj.ToCharacter().InRangeObjects.ContainsKey(c.Key) Then
                    If obj.CheckUpdateDistance(c.Value.Character) Then
						yield Return c.Value.Character
                    End If
                End If
            Next
		End Function

		Public Function GetOutOfRangeCharacter(obj As WorldObject) As IEnumerable(Of Character)
            For Each c As KeyValuePair(Of ULong, WorldClass) In Sessions.ToList()
                If obj.ToCharacter().InRangeObjects.ContainsKey(c.Key) Then
                    If Not obj.CheckUpdateDistance(c.Value.Character) Then
						yield Return c.Value.Character
                    End If
                End If
            Next
		End Function

		Public Sub WriteAccountData(mask As AccountDataMasks, ByRef session As WorldClass)
			Dim accountInitialized As New PacketWriter(LegacyMessage.AccountDataInitialized)
			accountInitialized.WriteUnixTime()
			accountInitialized.WriteUInt8(0)
			accountInitialized.WriteUInt32(CUInt(mask))

			For i As Integer = 0 To 8
				If (CInt(mask) And (1 << i)) <> 0 Then
					If i = 1 AndAlso mask = AccountDataMasks.GlobalCacheMask Then
						accountInitialized.WriteUnixTime()
					Else
						accountInitialized.WriteUInt32(0)
					End If
				End If
			Next

			session.Send(accountInitialized)
		End Sub

		Public Sub SendToInRangeCharacter(pChar As Character, packet As PacketWriter)
            For Each c As KeyValuePair(Of ULong, WorldClass) In Sessions.ToList()
                Dim iChar As New WorldObject
                If pChar.InRangeObjects.TryGetValue(c.Value.Character.Guid, iChar) Then
                    c.Value.Send(packet)
                End If
            Next
		End Sub

		Public Sub WriteUpdateObjectMovement(ByRef packet As PacketWriter, ByRef wObject As WorldObject, updateFlags As UpdateFlag)
			Dim values As New ObjectMovementValues(updateFlags)
			Dim BitPack As New BitPack(packet, wObject.Guid)

			BitPack.Write(0)
			' New in 5.1.0, 654, Unknown
			BitPack.Write(values.Bit0)
			BitPack.Write(values.HasRotation)
			BitPack.Write(values.HasTarget)
			BitPack.Write(values.Bit2)
			BitPack.Write(values.HasUnknown3)
			BitPack.Write(values.BitCounter, 24)
			BitPack.Write(values.HasUnknown)
			BitPack.Write(values.HasGoTransportPosition)
			BitPack.Write(values.HasUnknown2)
			BitPack.Write(0)
			' New in 5.1.0, 784, Unknown
			BitPack.Write(values.IsSelf)
			BitPack.Write(values.Bit1)
			BitPack.Write(values.IsAlive)
			BitPack.Write(values.Bit3)
			BitPack.Write(values.HasUnknown4)
			BitPack.Write(values.HasStationaryPosition)
			BitPack.Write(values.IsVehicle)
			BitPack.Write(values.BitCounter2, 21)
			BitPack.Write(values.HasAnimKits)

			If values.IsAlive Then
				BitPack.WriteGuidMask(3)
				BitPack.Write(0)
				' IsInterpolated, not implanted
				BitPack.Write(1)
				' Unknown_Alive_2, Reversed
				BitPack.Write(0)
				' Unknown_Alive_4
				BitPack.WriteGuidMask(2)
				BitPack.Write(0)
				' Unknown_Alive_1
				BitPack.Write(1)
				' Pitch or splineElevation, not implanted
				BitPack.Write(True)
				' MovementFlags2 are not implanted
				BitPack.WriteGuidMask(4, 5)
				BitPack.Write(0, 24)
				' BitCounter_Alive_1
				BitPack.Write(1)
				' Pitch or splineElevation, not implanted
				BitPack.Write(Not values.IsAlive)
				BitPack.Write(0)
				' Unknown_Alive_3
				BitPack.WriteGuidMask(0, 6, 7)
				BitPack.Write(values.IsTransport)
				BitPack.Write(Not values.HasRotation)

						' Transports not implanted.
				If values.IsTransport Then
				End If

				' MovementFlags2 are not implanted
'                 * if (movementFlag2 != 0)
'                 *     BitPack.Write(0, 12);


				BitPack.Write(True)
				' Movementflags are not implanted
				BitPack.WriteGuidMask(1)

				' IsInterpolated, not implanted
'                 * if (IsInterpolated)
'                 * {
'                 *     BitPack.Write(0);            // IsFalling
'                 * }


					' HasSplineData, don't write simple basic splineData
					' Movementflags are not implanted
'                if (movementFlags != 0)
'                    BitPack.Write((uint)movementFlags, 30);


					' Don't send basic spline data and disable advanced data
					' if (HasSplineData)
					'BitPack.Write(0);             // Disable advance splineData
				BitPack.Write(0)
			End If

			BitPack.Flush()

			If values.IsAlive Then
				packet.WriteFloat(CSng(MovementSpeed.FlyBackSpeed))

				' Don't send basic spline data
				'if (HasSplineBasicData)
'                {
'                    // Advanced spline data not implanted
'                    if (HasAdvancedSplineData)
'                    {
'
'                    }
'
'                    packet.WriteFloat(character.X);
'                    packet.WriteFloat(character.Y);
'                    packet.WriteUInt32(0);
'                    packet.WriteFloat(character.Z);
'                }


				packet.WriteFloat(CSng(MovementSpeed.SwimSpeed))

						' Not implanted
				If values.IsTransport Then
				End If

				BitPack.WriteGuidBytes(1)
				packet.WriteFloat(CSng(MovementSpeed.TurnSpeed))
				packet.WriteFloat(wObject.Position.Y)
				BitPack.WriteGuidBytes(3)
				packet.WriteFloat(wObject.Position.Z)
				packet.WriteFloat(wObject.Position.O)
				packet.WriteFloat(CSng(MovementSpeed.RunBackSpeed))
				BitPack.WriteGuidBytes(0, 6)
				packet.WriteFloat(wObject.Position.X)
				packet.WriteFloat(CSng(MovementSpeed.WalkSpeed))
				BitPack.WriteGuidBytes(5)
				packet.WriteUInt32(0)
				packet.WriteFloat(CSng(MovementSpeed.PitchSpeed))
				BitPack.WriteGuidBytes(2)
				packet.WriteFloat(CSng(MovementSpeed.RunSpeed))
				BitPack.WriteGuidBytes(7)
				packet.WriteFloat(CSng(MovementSpeed.SwimBackSpeed))
				BitPack.WriteGuidBytes(4)
				packet.WriteFloat(CSng(MovementSpeed.FlySpeed))
			End If

			If values.HasStationaryPosition Then
				packet.WriteFloat(wObject.Position.X)
				packet.WriteFloat(wObject.Position.O)
				packet.WriteFloat(wObject.Position.Y)
				packet.WriteFloat(wObject.Position.Z)
			End If

			If values.HasRotation Then
				packet.WriteInt64(Quaternion.GetCompressed(wObject.Position.O))
			End If
		End Sub
	End Class
End Namespace
