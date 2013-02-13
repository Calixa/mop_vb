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
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Namespace Framework.Network.Realm
	Public Class RealmNetwork
		Public listenSocket As Boolean = True
		Private listener As TcpListener

		Public Function Start(host As String, port As Integer) As Boolean
			Try
				listener = New TcpListener(IPAddress.Parse(host), port)
				listener.Start()

				Return True
			Catch e As Exception
				Log.Message(LogType.[ERROR], "{0}", e.Message)
				Log.Message()

				Return False
			End Try
		End Function

		Public Sub AcceptConnectionThread()
            'New Thread(AddressOf AcceptConnection).Start()
            Dim thread As New Thread(AddressOf AcceptConnection)
            thread.Start()
		End Sub

        Private Sub AcceptConnection()
            While listenSocket
                Thread.Sleep(1)
                If listener.Pending() Then
                    Dim realmClient As New RealmClass()
                    realmClient.clientSocket = listener.AcceptSocket()
                    '			New Thread(realmClient.Recieve).Start()
                    Dim thread As New Thread(
                      Sub()
                          realmClient.Recieve()
                      End Sub
                    )
                    thread.Start()
                End If
            End While
        End Sub
	End Class
End Namespace
