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
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace DBC
	NotInheritable Class DBCReader
		Private Sub New()
		End Sub
		Public Shared Function ReadDBC(Of T As Structure)(strDict As Dictionary(Of UInteger, String), _fmt As String, FileName As String) As Dictionary(Of UInteger, T)
			Dim dict As New Dictionary(Of UInteger, T)()
			Try
				Dim path As String = WorldConfig.DataPath & "/dbc/" & FileName
				Using reader As New BinaryReader(New FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8)
					' read dbc header
					Dim header As DbcHeader = reader.ReadHeader(Of DbcHeader)()
					Dim size As Integer = Marshal.SizeOf(GetType(T))

					If Not header.IsDBC Then
						Logging.Log.Message(Logging.LogType.[ERROR], "{0} is not DBC File", FileName)
						Return Nothing
					End If

					If header.RecordSize <> _fmt.Length * 4 Then
						Logging.Log.Message(Logging.LogType.[ERROR], "Size of '{0}' setted by format string ({1}) not equal size of DBC structure ({2}).", FileName, _fmt.Length * 4, header.RecordSize)
						Return Nothing
					End If

					Dim structsize As Integer = Marshal.SizeOf(GetType(T))
					If structsize <> _fmt.GetFMTCount() Then
						Logging.Log.Message(Logging.LogType.[ERROR], "Size of '{0}' setted by format string ({1}) not equal size of C# Structure ({2}).", FileName, _fmt.GetFMTCount(), structsize)
						Return Nothing
					End If

					' read dbc data
					For r As Integer = 0 To header.RecordsCount - 1
						Dim key As UInteger = reader.ReadUInt32()
						reader.BaseStream.Position -= 4

						Dim T_entry As T = reader.ReadStruct(Of T)(_fmt)

						dict.Add(key, T_entry)
					Next

					' read dbc strings
					If strDict IsNot Nothing Then
						While reader.BaseStream.Position <> reader.BaseStream.Length
                            Dim offset As UInteger = CUInt(reader.BaseStream.Position - header.StartStringPosition)
                            Dim str As String = reader.ReadCString()
							strDict.Add(offset, str)
						End While
					End If
				End Using
			Catch generatedExceptionName As FileNotFoundException
				Logging.Log.Message(Logging.LogType.[ERROR], "Cant Find File {0}", FileName)
				Return Nothing
			End Try
			DBCStorage.DBCFileCount += 1
			Return dict
		End Function
	End Class
End Namespace
