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


Imports Framework.Logging

Namespace Configuration
	Public NotInheritable Class WorldConfig
		Private Sub New()
		End Sub
		Shared config As New Config("./Configs/WorldServer.conf")

		Public Shared CharDBHost As String = config.Read("CharDB.Host", "")
		Public Shared CharDBPort As Integer = config.Read("CharDB.Port", 3306)
		Public Shared CharDBUser As String = config.Read("CharDB.User", "")
		Public Shared CharDBPassword As String = config.Read("CharDB.Password", "")
		Public Shared CharDBDataBase As String = config.Read("CharDB.Database", "")

		Public Shared WorldDBHost As String = config.Read("WorldDB.Host", "")
		Public Shared WorldDBPort As Integer = config.Read("WorldDB.Port", 3306)
		Public Shared WorldDBUser As String = config.Read("WorldDB.User", "")
		Public Shared WorldDBPassword As String = config.Read("WorldDB.Password", "")
		Public Shared WorldDBDataBase As String = config.Read("WorldDB.Database", "")

		Public Shared RealmId As UInteger = config.Read(Of UInteger)("RealmId", 1)

		Public Shared BindIP As String = config.Read("Bind.IP", "0.0.0.0")
		Public Shared BindPort As UInteger = config.Read(Of UInteger)("Bind.Port", 8100)

		Public Shared DataPath As String = config.Read("DataPath", "./Data/")

		Public Shared GMCommandStart As String = config.Read("GM.Command.Start", "!")

		Public Shared LogLevel As LogType = DirectCast(config.Read(Of UInteger)("LogLevel", 0, True), LogType)
	End Class
End Namespace
