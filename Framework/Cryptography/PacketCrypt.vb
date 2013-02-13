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


Imports System.Security.Cryptography

Namespace Cryptography
	Public Class PacketCrypt
		Public Property IsInitialized() As Boolean
			Get
				Return m_IsInitialized
			End Get
			Set
				m_IsInitialized = Value
			End Set
		End Property
		Private m_IsInitialized As Boolean

		Shared ReadOnly ServerEncryptionKey As Byte() = {&H8, &Hf1, &H95, &H9f, &H47, &He5, _
			&Hd2, &Hdb, &Ha1, &H3d, &H77, &H8f, _
			&H3f, &H3e, &He7, &H0}
		Shared ReadOnly ServerDecryptionKey As Byte() = {&H40, &Haa, &Hd3, &H92, &H26, &H71, _
			&H43, &H47, &H3a, &H31, &H8, &Ha6, _
			&He7, &Hdc, &H98, &H2a}

		Private SARC4Encrypt As SARC4, SARC4Decrypt As SARC4
		Private DecryptSHA1 As HMACSHA1, EncryptSHA1 As HMACSHA1

		Public Sub New()
			IsInitialized = False
		End Sub

		Public Sub Initialize(sessionKey As Byte())
			If IsInitialized Then
				Throw New InvalidOperationException("PacketCrypt already initialized!")
			End If

			SARC4Encrypt = New SARC4()
			SARC4Decrypt = New SARC4()

			DecryptSHA1 = New HMACSHA1(ServerDecryptionKey)
			EncryptSHA1 = New HMACSHA1(ServerEncryptionKey)

			SARC4Encrypt.PrepareKey(EncryptSHA1.ComputeHash(sessionKey))
			SARC4Decrypt.PrepareKey(DecryptSHA1.ComputeHash(sessionKey))

			Dim PacketEncryptionDummy As Byte() = New Byte(1023) {}
			Dim PacketDecryptionDummy As Byte() = New Byte(1023) {}

			SARC4Encrypt.ProcessBuffer(PacketEncryptionDummy, PacketEncryptionDummy.Length)
			SARC4Decrypt.ProcessBuffer(PacketDecryptionDummy, PacketDecryptionDummy.Length)

			IsInitialized = True
		End Sub

		Public Sub Encrypt(data As Byte())
			If Not IsInitialized Then
				Throw New InvalidOperationException("PacketCrypt not initialized!")
			End If

			SARC4Encrypt.ProcessBuffer(data, 4)
		End Sub

		Public Sub Decrypt(data As Byte())
			If Not IsInitialized Then
				Throw New InvalidOperationException("PacketCrypt not initialized!")
			End If

			SARC4Decrypt.ProcessBuffer(data, 4)
		End Sub
	End Class
End Namespace
