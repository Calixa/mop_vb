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


Imports Framework.Constants
Imports System.Collections.Generic
Imports System.Reflection
Imports Framework.Network.Packets
Imports WorldServer.Network

Namespace Game.Packets
	Public Class PacketManager
		Inherits Globals
		Shared OpcodeHandlers As New Dictionary(Of ClientMessage, HandlePacket)()
		Private Delegate Sub HandlePacket(ByRef packet As PacketReader, ByRef session As WorldClass)

		Public Shared Sub DefineOpcodeHandler()
			Dim currentAsm As Assembly = Assembly.GetExecutingAssembly()
            For Each type As Type In currentAsm.GetTypes()
                For Each methodInfo As MethodInfo In type.GetMethods()
                    Dim opcodeAttr As OpcodeAttribute = methodInfo.GetCustomAttribute(Of OpcodeAttribute)()

                    If opcodeAttr IsNot Nothing Then
                        OpcodeHandlers(opcodeAttr.Opcode) = DirectCast([Delegate].CreateDelegate(GetType(HandlePacket), methodInfo), HandlePacket)
                    End If
                Next
            Next
		End Sub

		Public Shared Function InvokeHandler(ByRef reader As PacketReader, session As WorldClass, opcode As ClientMessage) As Boolean
			If session.Character IsNot Nothing Then
				Dim charGuid As ULong = session.Character.Guid

				If WorldMgr.Sessions.ContainsKey(charGuid) Then
					WorldMgr.Sessions(charGuid) = session
				End If
			End If

			If OpcodeHandlers.ContainsKey(opcode) Then
				OpcodeHandlers(opcode).Invoke(reader, session)
				Return True
			Else
				Return False
			End If
		End Function
	End Class
End Namespace
