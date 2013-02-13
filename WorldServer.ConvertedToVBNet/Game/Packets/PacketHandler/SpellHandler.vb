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
Imports WorldServer.Game.WorldEntities
Imports WorldServer.Network

Namespace Game.Packets.PacketHandler
	Public Class SpellHandler
		Inherits Globals
		Public Shared Sub HandleSendKnownSpells(ByRef session As WorldClass)
			Dim pChar As Character = session.Character

			Dim writer As New PacketWriter(JAMCMessage.SendKnownSpells)
			Dim BitPack As New BitPack(writer)

			BitPack.Write(Of UInteger)(CUInt(pChar.SpellList.Count), 24)
			BitPack.Write(1)
			BitPack.Flush()

            'pChar.SpellList.ForEach(Function(spell) writer.WriteUInt32(spell.SpellId))
            '         session.Send(writer)

            For Each spell As PlayerSpell In pChar.SpellList
                writer.WriteUInt32(spell.SpellId)
            Next
            session.Send(writer)

        End Sub
	End Class
End Namespace
