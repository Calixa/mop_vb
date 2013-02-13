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
Imports Framework.Network.Packets
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Collections.Generic

Namespace Logging.PacketLogging
    Public Class PacketLog
        Shared logWriter As TextWriter
        Shared syncObj As New [Object]()

        Public Shared Sub WritePacket(clientInfo As String, Optional serverPacket As PacketWriter = Nothing, Optional clientPacket As PacketReader = Nothing)
            SyncLock syncObj
                Try
                    Dim sb As New StringBuilder()

                    If serverPacket IsNot Nothing Then
                        sb.AppendLine([String].Format("Client: {0}", clientInfo))
                        sb.AppendLine([String].Format("Time: {0}", DateTime.Now.ToString()))

                        If [Enum].IsDefined(GetType(LegacyMessage), serverPacket.Opcode) Then
                            sb.AppendLine("Type: LegacyMessage")
                            sb.AppendLine([String].Format("Name: {0}", [Enum].GetName(GetType(LegacyMessage), serverPacket.Opcode)))
                        ElseIf [Enum].IsDefined(GetType(JAMCMessage), serverPacket.Opcode) Then
                            sb.AppendLine("Type: JAMCMessage")
                            sb.AppendLine([String].Format("Name: {0}", [Enum].GetName(GetType(JAMCMessage), serverPacket.Opcode)))
                        ElseIf [Enum].IsDefined(GetType(Message), serverPacket.Opcode) Then
                            sb.AppendLine("Type: Message")
                            sb.AppendLine([String].Format("Name: {0}", [Enum].GetName(GetType(Message), serverPacket.Opcode)))
                        Else
                            sb.AppendLine("Type: JAMCCMessage")
                            sb.AppendLine([String].Format("Name: {0}", [Enum].GetName(GetType(JAMCCMessage), serverPacket.Opcode)))
                        End If

                        sb.AppendLine([String].Format("Value: 0x{0:X} ({1})", serverPacket.Opcode, serverPacket.Opcode))
                        sb.AppendLine([String].Format("Length: {0}", serverPacket.Size - 2))

                        sb.AppendLine("|----------------------------------------------------------------|")
                        sb.AppendLine("| 00  01  02  03  04  05  06  07  08  09  0A  0B  0C  0D  0E  0F |")
                        sb.AppendLine("|----------------------------------------------------------------|")
                        sb.Append("|")

                        If serverPacket.Size - 2 <> 0 Then
                            Dim data As List(Of Byte) = serverPacket.ReadDataToSend().ToList()
                            data.RemoveRange(0, 4)

                            For Each b As Byte In data
                                StringWriterSubFunction1(b, sb)
                            Next

                            '                            data.ForEach(StringWriterSubFunction1(b, sb))

                            sb.AppendLine("")
                            sb.AppendLine("|----------------------------------------------------------------|")
                        End If

                        sb.AppendLine("")
                    End If

                    If clientPacket IsNot Nothing Then
                        sb.AppendLine([String].Format("Client: {0}", clientInfo))
                        sb.AppendLine([String].Format("Time: {0}", DateTime.Now.ToString()))

                        sb.AppendLine("Type: ClientMessage")

                        If [Enum].IsDefined(GetType(ClientMessage), clientPacket.Opcode) Then
                            sb.AppendLine([String].Format("Name: {0}", clientPacket.Opcode))
                        Else
                            sb.AppendLine([String].Format("Name: {0}", "Unknown"))
                        End If

                        sb.AppendLine([String].Format("Value: 0x{0:X} ({1})", CUShort(clientPacket.Opcode), CUShort(clientPacket.Opcode)))
                        sb.AppendLine([String].Format("Length: {0}", clientPacket.Size))

                        sb.AppendLine("|----------------------------------------------------------------|")
                        sb.AppendLine("| 00  01  02  03  04  05  06  07  08  09  0A  0B  0C  0D  0E  0F |")
                        sb.AppendLine("|----------------------------------------------------------------|")
                        sb.Append("|")

                        If clientPacket.Size - 2 <> 0 Then
                            Dim data As List(Of Byte) = clientPacket.Storage.ToList()

                            Dim count As Integer = 0

                            For Each b As Byte In data
                                StringWriterSubFunction1(b, sb)
                            Next
                            'Function(b)

                            '    If b <= &HF Then
                            '        sb.Append([String].Format(" 0{0:X} ", b))
                            '    Else
                            '        sb.Append([String].Format(" {0:X} ", b))
                            '    End If

                            '    If count = 15 Then
                            '        sb.Append("|")
                            '        sb.AppendLine()
                            '        sb.Append("|")
                            '        count = 0
                            '    Else
                            '        count += 1
                            '    End If

                            'End Function
                            ')

                            sb.AppendLine()
                            sb.Append("|----------------------------------------------------------------|")
                        End If

                        sb.AppendLine("")
                    End If

                    logWriter = TextWriter.Synchronized(File.AppendText("Packet.log"))
                    logWriter.WriteLine(sb.ToString())
                    logWriter.Flush()
                Finally
                    logWriter.Close()
                End Try
            End SyncLock
        End Sub

        Shared Function StringWriterSubFunction1(ByRef b As Byte, ByRef sb As StringBuilder) As Integer
            Dim count As Integer = 0
            If b <= &HF Then
                sb.Append([String].Format(" 0{0:X} ", b))
            Else
                sb.Append([String].Format(" {0:X} ", b))
            End If

            If count = 15 Then
                sb.Append("|")
                sb.AppendLine()
                sb.Append("|")
                count = 0
            Else
                count += 1
            End If
            Return count
        End Function



    End Class
End Namespace
