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


Imports Framework.Console
Imports WorldServer.Game.Packets.PacketHandler
Imports Framework.ObjectDefines
Imports Framework.Database
Imports WorldServer.Network

Namespace Game.Chat.Commands
	Public Class MovementCommands
		Inherits Globals
		<ChatCommand("fly", "Usage: !fly #state (Turns the fly mode 'on' or 'off')")> _
		Public Shared Sub Fly(args As String(), ByRef session As WorldClass)
			Dim state As String = CommandParser.Read(Of String)(args, 1)

			If state = "on" Then
				MoveHandler.HandleMoveSetCanFly(session)
				ChatHandler.SendMessageByType(session, 0, 0, "Fly mode enabled.")
			ElseIf state = "off" Then
				MoveHandler.HandleMoveUnsetCanFly(session)
				ChatHandler.SendMessageByType(session, 0, 0, "Fly mode disabled.")
			End If
		End Sub

		<ChatCommand("walkspeed", "Usage: !walkspeed #speed (Set the current walk speed)")> _
		Public Shared Sub WalkSpeed(args As String(), ByRef session As WorldClass)
			If args.Length = 1 Then
				MoveHandler.HandleMoveSetWalkSpeed(session)
			Else
                Dim speed As Single = CommandParser.Read(Of Single)(args, 1)

				If speed <= 50 AndAlso speed > 0 Then
					MoveHandler.HandleMoveSetWalkSpeed(session, speed)
					ChatHandler.SendMessageByType(session, 0, 0, "Walk speed set to " & speed & "!")
				Else
					ChatHandler.SendMessageByType(session, 0, 0, "Please enter a value between 0.0 and 50.0!")
				End If

				Return
			End If

			ChatHandler.SendMessageByType(session, 0, 0, "Walk speed set to default.")
		End Sub

		<ChatCommand("runspeed", "Usage: !runspeed #speed (Set the current run speed)")> _
		Public Shared Sub RunSpeed(args As String(), ByRef session As WorldClass)
			If args.Length = 1 Then
				MoveHandler.HandleMoveSetRunSpeed(session)
			Else
                Dim speed As Single = CommandParser.Read(Of Single)(args, 1)
				If speed <= 50 AndAlso speed > 0 Then
					MoveHandler.HandleMoveSetRunSpeed(session, speed)
					ChatHandler.SendMessageByType(session, 0, 0, "Run speed set to " & speed & "!")
				Else
					ChatHandler.SendMessageByType(session, 0, 0, "Please enter a value between 0.0 and 50.0!")
				End If

				Return
			End If

			ChatHandler.SendMessageByType(session, 0, 0, "Run speed set to default.")
		End Sub

		<ChatCommand("swimspeed", "Usage: !swimspeed #speed (Set the current swim speed)")> _
		Public Shared Sub SwimSpeed(args As String(), ByRef session As WorldClass)
			If args.Length = 1 Then
				MoveHandler.HandleMoveSetSwimSpeed(session)
			Else
                Dim speed As Single = CommandParser.Read(Of Single)(args, 1)
				If speed <= 50 AndAlso speed > 0 Then
					MoveHandler.HandleMoveSetSwimSpeed(session, speed)
					ChatHandler.SendMessageByType(session, 0, 0, "Swim speed set to " & speed & "!")
				Else
					ChatHandler.SendMessageByType(session, 0, 0, "Please enter a value between 0.0 and 50.0!")
				End If

				Return
			End If

			ChatHandler.SendMessageByType(session, 0, 0, "Swim speed set to default.")
		End Sub

		<ChatCommand("flightspeed", "Usage: !flightspeed #speed (Set the current flight speed)")> _
		Public Shared Sub FlightSpeed(args As String(), ByRef session As WorldClass)
			If args.Length = 1 Then
				MoveHandler.HandleMoveSetFlightSpeed(session)
			Else
                Dim speed As Single = CommandParser.Read(Of Single)(args, 1)

				If speed <= 50 AndAlso speed > 0 Then
					MoveHandler.HandleMoveSetFlightSpeed(session, speed)
					ChatHandler.SendMessageByType(session, 0, 0, "Flight speed set to " & speed & "!")
				Else
					ChatHandler.SendMessageByType(session, 0, 0, "Please enter a value between 0.0 and 50.0!")
				End If

				Return
			End If

			ChatHandler.SendMessageByType(session, 0, 0, "Flight speed set to default.")
		End Sub

		<ChatCommand("tele", "Usage: !tele [#x #y #z #o #map] or [#location] (Force teleport to a new location by coordinates or location)")> _
		Public Shared Sub Teleport(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character
			Dim vector As Vector4
			Dim mapId As UInteger

			If args.Length > 2 Then
                vector = New Vector4() With { _
                    .X = CommandParser.Read(Of Single)(args, 1), _
                    .Y = CommandParser.Read(Of Single)(args, 2), _
                    .Z = CommandParser.Read(Of Single)(args, 3), _
                    .O = CommandParser.Read(Of Single)(args, 4) _
                }

                mapId = CommandParser.Read(Of UInteger)(args, 5)
            Else
                Dim location As String = CommandParser.Read(Of String)(args, 1)
                Dim result As SQLResult = DB.World.[Select]("SELECT * FROM teleport_locations WHERE location = ?", location)

                If result.Count = 0 Then
                    ChatHandler.SendMessageByType(session, 0, 0, "Teleport location '" & location & "' does not exist.")
                    Return
                End If

                vector = New Vector4() With { _
                    .X = result.Read(Of Single)(0, "X"), _
                    .Y = result.Read(Of Single)(0, "Y"), _
                    .Z = result.Read(Of Single)(0, "Z"), _
                    .O = result.Read(Of Single)(0, "O") _
                }

                mapId = result.Read(Of UInteger)(0, "Map")
			End If

			If pChar.Map = mapId Then
				MoveHandler.HandleMoveTeleport(session, vector)
				ObjectMgr.SetPosition(pChar, vector)
			Else
				MoveHandler.HandleTransferPending(session, mapId)
				MoveHandler.HandleNewWorld(session, vector, mapId)

				ObjectMgr.SetPosition(pChar, vector)
				ObjectMgr.SetMap(pChar, mapId)

				ObjectHandler.HandleUpdateObjectCreate(session)
			End If
		End Sub

		<ChatCommand("start", "Usage: !start (Teleports yourself to your start position)")> _
		Public Shared Sub Start(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

			Dim result As SQLResult = DB.Characters.[Select]("SELECT map, posX, posY, posZ, posO FROM character_creation_data WHERE race = ? AND class = ?", pChar.Race, pChar.[Class])

            Dim vector As New Vector4() With { _
                 .X = result.Read(Of Single)(0, "PosX"), _
                 .Y = result.Read(Of Single)(0, "PosY"), _
                 .Z = result.Read(Of Single)(0, "PosZ"), _
                 .O = result.Read(Of Single)(0, "PosO") _
            }

			Dim mapId As UInteger = result.Read(Of UInteger)(0, "Map")

			If pChar.Map = mapId Then
				MoveHandler.HandleMoveTeleport(session, vector)
				ObjectMgr.SetPosition(pChar, vector)
			Else
				MoveHandler.HandleTransferPending(session, mapId)
				MoveHandler.HandleNewWorld(session, vector, mapId)

				ObjectMgr.SetPosition(pChar, vector)
				ObjectMgr.SetMap(pChar, mapId)

				ObjectHandler.HandleUpdateObjectCreate(session)
			End If
		End Sub

		<ChatCommand("gps", "Usage: !gps (Show your current location)")> _
		Public Shared Sub GPS(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

            Dim message As String = [String].Format("Your position is X: {0}, Y: {1}, Z: {2}, W(O): {3}, Map: {4}, Zone: {5}", pChar.Position.X, pChar.Position.Y, pChar.Position.Z, pChar.Position.O, pChar.Map, _
                pChar.Zone)
			ChatHandler.SendMessageByType(session, 0, 0, message)
		End Sub

		<ChatCommand("addtele", "Usage: !addtele #name (Adds a new teleport location to the world database with the given name)")> _
		Public Shared Sub AddTele(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

			Dim location As String = CommandParser.Read(Of String)(args, 1)
			Dim result As SQLResult = DB.World.[Select]("SELECT * FROM teleport_locations WHERE location = ?", location)

			If result.Count = 0 Then
				If DB.World.Execute("INSERT INTO teleport_locations (location, x, y, z, o, map) " & "VALUES (?, ?, ?, ?, ?, ?)", location, pChar.Position.X, pChar.Position.Y, pChar.Position.Z, pChar.Position.O, _
					pChar.Map) Then
					ChatHandler.SendMessageByType(session, 0, 0, [String].Format("Teleport location '{0}' successfully added.", location))
				End If
			Else
				ChatHandler.SendMessageByType(session, 0, 0, [String].Format("Teleport location '{0}' already exist.", location))
			End If
		End Sub

		<ChatCommand("deltele", "Usage: !deltele #name (Delete the given teleport location from the world database)")> _
		Public Shared Sub DelTele(args As String(), ByRef session As WorldClass)
            Dim pChar As WorldEntities.Character = session.Character

			Dim location As String = CommandParser.Read(Of String)(args, 1)
			If DB.World.Execute("DELETE FROM teleport_locations WHERE location = ?", location) Then
				ChatHandler.SendMessageByType(session, 0, 0, [String].Format("Teleport location '{0}' successfully deleted.", location))
			End If
		End Sub
	End Class
End Namespace
