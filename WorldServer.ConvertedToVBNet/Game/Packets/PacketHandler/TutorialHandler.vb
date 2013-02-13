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
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class TutorialHandler
		Public Shared Sub HandleTutorialFlags(ByRef session As WorldClass)
			Dim tutorialFlags As New PacketWriter(LegacyMessage.TutorialFlags)
			For i As Integer = 0 To 7
				tutorialFlags.WriteUInt32(0)
			Next

			session.Send(tutorialFlags)
		End Sub
	End Class
End Namespace
