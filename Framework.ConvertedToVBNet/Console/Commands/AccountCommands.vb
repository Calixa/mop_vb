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


Imports Framework.Database
Imports Framework.Logging

Namespace Console.Commands
	Public Class AccountCommands
		Inherits CommandParser
		Public Shared Sub CreateAccount(args As String())
			Dim name As String = Read(Of String)(args, 0)
			Dim password As String = Read(Of String)(args, 1)

			If name Is Nothing OrElse password Is Nothing Then
				Return
			End If

			name = name.ToUpper()

			'byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(password));
			'string hashString = BitConverter.ToString(hash).Replace("-", "");

			If Not name.Contains("@") Then
				Log.Message(LogType.[ERROR], "Account name requires an email address")
			Else
				Dim result As SQLResult = DB.Realms.[Select]("SELECT * FROM accounts WHERE name = ?", name)
				If result.Count = 0 Then
					If DB.Realms.Execute("INSERT INTO accounts (name, password, language) VALUES (?, ?, '')", name, password.ToUpper()) Then
						Log.Message(LogType.NORMAL, "Account {0} successfully created", name)
					End If
				Else
					Log.Message(LogType.[ERROR], "Account {0} already in database", name)
				End If
			End If
		End Sub
	End Class
End Namespace
