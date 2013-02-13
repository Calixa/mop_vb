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
Imports System.IO
Imports System.Text

Namespace Configuration
	Public Class Config
		Private ConfigContent As String()
		Public Property ConfigFile() As String
			Get
				Return m_ConfigFile
			End Get
			Set
				m_ConfigFile = Value
			End Set
		End Property
		Private m_ConfigFile As String

		Public Sub New(config__1 As String)
			ConfigFile = config__1

			If Not File.Exists(config__1) Then
				Log.Message(LogType.[ERROR], "{0} doesn't exist!", config__1)
				Environment.[Exit](0)
			Else
				ConfigContent = File.ReadAllLines(config__1, Encoding.UTF8)
			End If
		End Sub

		Public Function Read(Of T)(name As String, value As T, Optional hex As Boolean = False) As T
			Dim nameValue As String = Nothing
			Dim trueValue As T = DirectCast(Convert.ChangeType(value, GetType(T)), T)
			Dim lineCounter As Integer = 0

			Try
                For Each [option] As String In ConfigContent
                    Dim configOption As String() = [option].Split(New Char() {"="c}, StringSplitOptions.None)
                    If configOption(0).StartsWith(name, StringComparison.InvariantCulture) Then
                        If configOption(1).Trim().Equals("") Then
                            nameValue = value.ToString()
                        Else
                            nameValue = configOption(1).Replace("""", "").Trim()
                        End If
                    End If

                    If GetType(T) Is GetType(Boolean) AndAlso (nameValue <> "0" AndAlso nameValue <> "1") Then
                        Log.Message(LogType.[ERROR], "Error in {0} in line {1}", ConfigFile, lineCounter.ToString())
                        Log.Message(LogType.[ERROR], "Use default value for boolean config option: {0}. Default: {1}", name, value)
                    End If

                    lineCounter += 1
                Next
			Catch
				Log.Message(LogType.[ERROR], "Error in {0} in line {1}", ConfigFile, lineCounter.ToString())
			End Try

			If hex Then
				Return DirectCast(Convert.ChangeType(Convert.ToInt32(nameValue, 16), GetType(T)), T)
			End If

			Return DirectCast(Convert.ChangeType(nameValue, GetType(T)), T)
		End Function
	End Class
End Namespace
