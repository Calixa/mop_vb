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


Namespace Constants.Authentication
	Public Enum ClientLink As Byte
		CMD_AUTH_LOGON_CHALLENGE = &H0
		CMD_AUTH_LOGON_PROOF = &H1
		CMD_AUTH_RECONNECT_CHALLENGE = &H2
		CMD_AUTH_RECONNECT_PROOF = &H3
		CMD_REALM_LIST = &H10
		CMD_XFER_INITIATE = &H30
		CMD_XFER_DATA = &H31
	End Enum

	Public Enum ServerLink As Byte
		CMD_GRUNT_AUTH_CHALLENGE = &H0
		CMD_GRUNT_AUTH_VERIFY = &H2
		CMD_GRUNT_CONN_PING = &H10
		CMD_GRUNT_CONN_PONG = &H11
		CMD_GRUNT_HELLO = &H20
		CMD_GRUNT_PROVESESSION = &H21
		CMD_GRUNT_KICK = &H24
		CMD_GRUNT_PCWARNING = &H29
		CMD_GRUNT_STRINGS = &H41
		CMD_GRUNT_SUNKENUPDATE = &H44
		CMD_GRUNT_SUNKEN_ONLINE = &H46
		CMD_GRUNT_CAISTIMEUPDATE = &H2c
	End Enum
End Namespace
