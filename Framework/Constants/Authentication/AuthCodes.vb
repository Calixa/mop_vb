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
	Public Enum AuthCodes As Byte
		AUTH_OK = &Hc
		AUTH_FAILED = &Hd
		AUTH_REJECT = &He
		AUTH_BAD_SERVER_PROOF = &Hf
		AUTH_UNAVAILABLE = &H10
		AUTH_SYSTEM_ERROR = &H11
		AUTH_BILLING_ERROR = &H12
		AUTH_BILLING_EXPIRED = &H13
		AUTH_VERSION_MISMATCH = &H14
		AUTH_UNKNOWN_ACCOUNT = &H15
		AUTH_INCORRECT_PASSWORD = &H16
		AUTH_SESSION_EXPIRED = &H17
		AUTH_SERVER_SHUTTING_DOWN = &H18
		AUTH_ALREADY_LOGGING_IN = &H19
		AUTH_LOGIN_SERVER_NOT_FOUND = &H1a
		AUTH_WAIT_QUEUE = &H1b
		AUTH_BANNED = &H1c
		AUTH_ALREADY_ONLINE = &H1d
		AUTH_NO_TIME = &H1e
		AUTH_DB_BUSY = &H1f
		AUTH_SUSPENDED = &H20
		AUTH_PARENTAL_CONTROL = &H21
	End Enum
End Namespace
