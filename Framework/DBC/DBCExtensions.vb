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
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace DBC
    Public Module DBCExtensions
        Sub New()
        End Sub
        <System.Runtime.CompilerServices.Extension> _
        Public Function ReadStruct(Of T As Structure)(reader As BinaryReader, fmt As String) As T
            Dim rawData As Byte() = New Byte(Marshal.SizeOf(GetType(T)) - 1) {}

            Dim index As Integer = 0
            For Each c As Char In fmt
                If c.ToString().Equals("x") Then
                    reader.BaseStream.Position += 4
                Else
                    reader.Read(rawData, index, 4)
                    index += 4
                End If
            Next

            Dim handle As GCHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned)
            Dim returnObject As T = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)

            handle.Free()

            Return returnObject
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function ReadHeader(Of T As Structure)(reader As BinaryReader) As T
            Dim rawData As Byte() = reader.ReadBytes(Marshal.SizeOf(GetType(T)))

            Dim handle As GCHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned)
            Dim returnObject As T = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)

            handle.Free()

            Return returnObject
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function ReadCString(reader As BinaryReader) As String
            Dim num As Byte
            Dim temp As New List(Of Byte)()

            While (InlineAssignHelper(num, reader.ReadByte())) <> 0
                temp.Add(num)
            End While

            Return Encoding.UTF8.GetString(temp.ToArray())
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function LookupByKey(Of T)(dictionary As Dictionary(Of UInteger, T), key As UInteger) As T
            Dim value As T
            dictionary.TryGetValue(key, value)
            Return value
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function GetFMTCount(fmt As String) As Integer
            Dim count As Integer = 0
            For Each c As Char In fmt
                If Not c.Equals("x"c) Then
                    count += 1
                End If
            Next
            Return count * 4
        End Function
        Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

    End Module
End Namespace
