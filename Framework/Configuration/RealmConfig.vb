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
	Public NotInheritable Class RealmConfig
		Private Sub New()
		End Sub
		Shared config As New Config("./Configs/RealmServer.conf")

		Public Shared RealmDBHost As String = config.Read("RealmDB.Host", "")
		Public Shared RealmDBPort As Integer = config.Read("RealmDB.Port", 3306)
		Public Shared RealmDBUser As String = config.Read("RealmDB.User", "")
		Public Shared RealmDBPassword As String = config.Read("RealmDB.Password", "")
		Public Shared RealmDBDataBase As String = config.Read("RealmDB.Database", "")

		Public Shared BindIP As String = config.Read("Bind.IP", "0.0.0.0")
		Public Shared BindPort As UInteger = config.Read(Of UInteger)("Bind.Port", 3724)

        Public Shared LogLevel As LogType = DirectCast((config.Read(Of UInteger)("LogLevel", 0, True)), LogType)
	End Class
End Namespace
