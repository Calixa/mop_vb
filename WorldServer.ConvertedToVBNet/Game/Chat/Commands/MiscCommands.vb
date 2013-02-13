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


Imports System.Text
Imports WorldServer.Game.Packets.PacketHandler
Imports WorldServer.Network
Imports System.Collections.Generic

Namespace Game.Chat.Commands
    Public Class MiscCommands
        Inherits Globals
        <ChatCommand("help")> _
        Public Shared Sub Help(args As String(), ByRef session As WorldClass)
            Dim commandList As New StringBuilder()

            For Each command As KeyValuePair(Of String, ChatCommandParser.HandleChatCommand) In ChatCommandParser.ChatCommands
                Dim helpAttribute As ChatCommandAttribute() = DirectCast(command.Value.Method.GetCustomAttributes(GetType(ChatCommandAttribute), False), ChatCommandAttribute())
                For Each desc As ChatCommandAttribute In helpAttribute
                    If desc.Description <> "" Then
                        commandList.AppendLine("!" & command.Key & " [" & desc.Description & "]")
                    Else
                        commandList.AppendLine("!" & command.Key)
                    End If
                Next
            Next

            ChatHandler.SendMessageByType(session, 0, 0, commandList.ToString())
        End Sub

        <ChatCommand("save")> _
        Public Shared Sub Save(args As String(), ByRef session As WorldClass)
            ObjectMgr.SavePositionToDB(session.Character)

            ChatHandler.SendMessageByType(session, 0, 0, "Your character is successfully saved to the database!")
        End Sub
    End Class
End Namespace
