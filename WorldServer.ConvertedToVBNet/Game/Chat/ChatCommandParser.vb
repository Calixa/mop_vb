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


Imports System.Collections.Generic
Imports System.Reflection
Imports Framework.Configuration
Imports WorldServer.Network

Namespace Game.Chat
	Public Class ChatCommandParser
		Public Shared ChatCommands As New Dictionary(Of String, HandleChatCommand)()
		Public Delegate Sub HandleChatCommand(args As String(), ByRef session As WorldClass)

		Public Shared Sub DefineChatCommands()
			Dim currentAsm As Assembly = Assembly.GetExecutingAssembly()
            For Each type As Type In currentAsm.GetTypes()
                For Each methodInfo As MethodInfo In type.GetMethods()
                    Dim chatAttr As ChatCommandAttribute = methodInfo.GetCustomAttribute(Of ChatCommandAttribute)()

                    If chatAttr IsNot Nothing Then
                        ChatCommands(chatAttr.ChatCommand) = DirectCast([Delegate].CreateDelegate(GetType(HandleChatCommand), methodInfo), HandleChatCommand)
                    End If
                Next
            Next
		End Sub

		Public Shared Sub ExecuteChatHandler(chatCommand As String, ByRef session As WorldClass)
            Dim args As String() = chatCommand.Split(New String() {" "}, StringSplitOptions.None)
            Dim command As String = args(0).Remove(0, 1)

			If ChatCommands.ContainsKey(command) Then
				ChatCommands(command).Invoke(args, session)
			End If
		End Sub

		Public Shared Function CheckForCommand(command As String) As Boolean
            Dim commandStarts As String() = WorldConfig.GMCommandStart.Split(New String() {" "}, StringSplitOptions.None)

			For Each s As String In commandStarts
				If command.StartsWith(s) Then
					Return True
				End If
			Next

			Return False
		End Function
	End Class
End Namespace
