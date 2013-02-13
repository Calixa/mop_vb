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
Imports System.Globalization

Namespace Console
	Public Class CommandParser
		Public Shared Function Read(Of T)(args As String(), index As Integer) As T
			Try
				Return DirectCast(Convert.ChangeType(args(index), GetType(T), CultureInfo.GetCultureInfo("en-US").NumberFormat), T)
			Catch
				Log.Message(LogType.[ERROR], "Wrong arguments for the current command!!!")
			End Try

			Return Nothing
		End Function
	End Class
End Namespace
