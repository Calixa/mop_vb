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
Imports Framework.Console
Imports Framework.Console.Commands
Imports Framework.Database
Imports Framework.DBC
Imports Framework.Logging
Imports WorldServer.Game
Imports WorldServer.Game.Chat
Imports WorldServer.Game.Packets
Imports WorldServer.Network

Class WorldServer
	Friend Shared Sub Main(args As String())
		Log.ServerType = "World"

		Log.Message(LogType.INIT, "___________________________________________")
		Log.Message(LogType.INIT, "    __                                     ")
		Log.Message(LogType.INIT, "    / |                     ,              ")
		Log.Message(LogType.INIT, "---/__|---)__----__--_/_--------------_--_-")
		Log.Message(LogType.INIT, "  /   |  /   ) /   ' /    /   /   /  / /  )")
		Log.Message(LogType.INIT, "_/____|_/_____(___ _(_ __/___(___(__/_/__/_")
		Log.Message(LogType.INIT, "___________________________________________")
		Log.Message()

		Log.Message(LogType.NORMAL, "Starting Arctium WorldServer...")

		DB.Characters.Init(WorldConfig.CharDBHost, WorldConfig.CharDBUser, WorldConfig.CharDBPassword, WorldConfig.CharDBDataBase, WorldConfig.CharDBPort)
		DB.Realms.Init(RealmConfig.RealmDBHost, RealmConfig.RealmDBUser, RealmConfig.RealmDBPassword, RealmConfig.RealmDBDataBase, RealmConfig.RealmDBPort)
		DB.World.Init(WorldConfig.WorldDBHost, WorldConfig.WorldDBUser, WorldConfig.WorldDBPassword, WorldConfig.WorldDBDataBase, WorldConfig.WorldDBPort)
		Log.Message()

		DBCLoader.Init()
		DB2Loader.Init()

		Globals.InitializeManager()

		' Set all accounts offline
		DB.Realms.Execute("UPDATE accounts SET online = 0")

		WorldClass.world = New WorldNetwork()

		If WorldClass.world.Start(WorldConfig.BindIP, CInt(WorldConfig.BindPort)) Then
			WorldClass.world.AcceptConnectionThread()
			Log.Message(LogType.NORMAL, "WorldServer listening on {0} port {1}.", WorldConfig.BindIP, WorldConfig.BindPort)
			Log.Message(LogType.NORMAL, "WorldServer successfully started!")

			PacketManager.DefineOpcodeHandler()
			ChatCommandParser.DefineChatCommands()
		Else
			Log.Message(LogType.[ERROR], "Server couldn't be started: ")
		End If

		' Free memory...
		GC.Collect()
		Log.Message(LogType.NORMAL, "Total Memory: {0} Kilobytes", GC.GetTotalMemory(False) \ 1024)

		' Init Command handlers...
		CommandDefinitions.Initialize()
		CommandManager.InitCommands()
	End Sub
End Class
