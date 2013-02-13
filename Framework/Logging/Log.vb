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
Imports DefaultConsole = System.Console
Imports Framework.ObjectDefines

Namespace Logging
	Public Class Log
		Public Shared Property ServerType() As String
			Get
				Return m_ServerType
			End Get
			Set
				m_ServerType = Value
			End Set
		End Property
		Private Shared m_ServerType As String

		Public Shared Sub Message()
			SetLogger(LogType.[DEFAULT], "")
		End Sub

		Public Shared Sub Message(type As LogType, text As String, ParamArray args As Object())
			SetLogger(type, text, args)
		End Sub

		Private Shared Sub SetLogger(type As LogType, text As String, ParamArray args As Object())
			Select Case type
				Case LogType.NORMAL
					DefaultConsole.ForegroundColor = ConsoleColor.Green
					text = text.Insert(0, "System: ")
					Exit Select
				Case LogType.[ERROR]
					DefaultConsole.ForegroundColor = ConsoleColor.Red
					text = text.Insert(0, "Error: ")
					Exit Select
				Case LogType.DUMP
					DefaultConsole.ForegroundColor = ConsoleColor.Yellow
					Exit Select
				Case LogType.INIT
					DefaultConsole.ForegroundColor = ConsoleColor.Cyan
					Exit Select
				Case LogType.DB
					DefaultConsole.ForegroundColor = ConsoleColor.DarkBlue
					Exit Select
				Case LogType.CMD
					DefaultConsole.ForegroundColor = ConsoleColor.Green
					Exit Select
				Case LogType.DEBUG
					DefaultConsole.ForegroundColor = ConsoleColor.DarkRed
					Exit Select
				Case Else
					DefaultConsole.ForegroundColor = ConsoleColor.White
					Exit Select
			End Select

			If ((If(Log.ServerType = "World", WorldConfig.LogLevel, RealmConfig.LogLevel)) And type) = type Then
				If type.Equals(LogType.INIT) Or type.Equals(LogType.[DEFAULT]) Then
					DefaultConsole.WriteLine(text, args)
				ElseIf type.Equals(LogType.DUMP) OrElse type.Equals(LogType.CMD) Then
					DefaultConsole.WriteLine(text, args)
				Else
					DefaultConsole.WriteLine("[" & DateTime.Now.ToLongTimeString() & "] " & text, args)
				End If
			End If
		End Sub
	End Class
End Namespace
