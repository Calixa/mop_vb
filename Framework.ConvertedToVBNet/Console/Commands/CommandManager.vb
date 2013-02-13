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
Imports System.Collections.Generic
Imports System.Threading

Namespace Console
	Public Class CommandManager
		Protected Shared CommandHandlers As New Dictionary(Of String, HandleCommand)()
		Public Delegate Sub HandleCommand(args As String())

		Public Shared Sub InitCommands()
			While True
				Thread.Sleep(1)
				Log.Message(LogType.CMD, "Arctium >> ")

				Dim line As String() = System.Console.ReadLine().Split(New String() {" "}, StringSplitOptions.None)
				Dim args As String() = New String(line.Length - 2) {}
				Array.Copy(line, 1, args, 0, line.Length - 1)

				InvokeHandler(line(0).ToLower(), args)
			End While
		End Sub

		Public Shared Sub DefineCommand(command As String, handler As HandleCommand)
			CommandHandlers(command) = handler
		End Sub

		Protected Shared Function InvokeHandler(command As String, ParamArray args As String()) As Boolean
			If CommandHandlers.ContainsKey(command) Then
				CommandHandlers(command).Invoke(args)
				Return True
			Else
				Return False
			End If
		End Function
	End Class
End Namespace
