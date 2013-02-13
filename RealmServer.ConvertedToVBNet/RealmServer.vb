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
Imports Framework.Database
Imports Framework.Logging
Imports RealmServer.Framework.Network.Realm

Class RealmServer
    Friend Shared Sub Main(args As String())
        Log.ServerType = "Realm"

        Log.Message(LogType.INIT, "___________________________________________")
        Log.Message(LogType.INIT, "    __                                     ")
        Log.Message(LogType.INIT, "    / |                     ,              ")
        Log.Message(LogType.INIT, "---/__|---)__----__--_/_--------------_--_-")
        Log.Message(LogType.INIT, "  /   |  /   ) /   ' /    /   /   /  / /  )")
        Log.Message(LogType.INIT, "_/____|_/_____(___ _(_ __/___(___(__/_/__/_")
        Log.Message(LogType.INIT, "___________________________________________")
        Log.Message()

        Log.Message(LogType.NORMAL, "Starting Arctium RealmServer...")

        DB.Realms.Init(RealmConfig.RealmDBHost, RealmConfig.RealmDBUser, RealmConfig.RealmDBPassword, RealmConfig.RealmDBDataBase, RealmConfig.RealmDBPort)

        RealmClass.realm = New RealmNetwork()

        ' Add realms from database.
        Log.Message(LogType.NORMAL, "Updating Realm List...")
        Log.Message()
        Dim result As SQLResult = DB.Realms.[Select]("SELECT * FROM realms")
        For i As Integer = 0 To result.Count - 1
            RealmClass.Realms.Add(New Global.Framework.ObjectDefines.Realm() With { _
                .Id = result.Read(Of UInteger)(i, "id"), _
                .Name = result.Read(Of String)(i, "name"), _
                .IP = result.Read(Of String)(i, "ip"), _
                .Port = result.Read(Of UInteger)(i, "port") _
            })

            Log.Message(LogType.NORMAL, "Added Realm ""{0}""", RealmClass.Realms(i).Name)
        Next
        Log.Message()

        If RealmClass.realm.Start(RealmConfig.BindIP, CInt(RealmConfig.BindPort)) Then
            RealmClass.realm.AcceptConnectionThread()
            Log.Message(LogType.NORMAL, "RealmServer listening on {0} port {1}.", RealmConfig.BindIP, RealmConfig.BindPort)
            Log.Message(LogType.NORMAL, "RealmServer successfully started!")
        Else
            Log.Message(LogType.[ERROR], "RealmServer couldn't be started: ")
        End If

        ' Free memory...
        GC.Collect()
        Log.Message(LogType.NORMAL, "Total Memory: {0} Kilobytes", GC.GetTotalMemory(False) \ 1024)
    End Sub
End Class
