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


Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential)> _
Friend Structure EVP_CTX
	Private cipher As IntPtr
	Private Engine As IntPtr
	Private encrypt As Integer
	Private buflen As Integer
	<MarshalAs(UnmanagedType.ByValArray, SizeConst := 16)> _
	Private oiv As Byte()
	<MarshalAs(UnmanagedType.ByValArray, SizeConst := 16)> _
	Private iv As Byte()
	<MarshalAs(UnmanagedType.ByValArray, SizeConst := 32)> _
	Private buf As Byte()
	Private num As Integer
	Private app_data As IntPtr
	Private key_len As Integer
	Private flags As UInteger
	Private cipher_data As IntPtr
	Private final_used As Integer
	Private block_mask As Integer
	<MarshalAs(UnmanagedType.ByValArray, SizeConst := 32)> _
	Private final As Byte()
End Structure

<StructLayout(LayoutKind.Sequential)> _
Friend Structure EVP_MD_CTX
	Private digest As IntPtr
	Private Engine As IntPtr
	Private flags As UInteger
	Private md_data As IntPtr
End Structure

<StructLayout(LayoutKind.Sequential)> _
Friend Structure HMAC_CTX
	Private md As IntPtr
	Private md_ctx As EVP_MD_CTX
	Private i_ctx As EVP_MD_CTX
	Private o_ctx As EVP_MD_CTX
	Private key_length As UInteger
	<MarshalAs(UnmanagedType.ByValArray, SizeConst := 128)> _
	Private key As Byte()
End Structure

Namespace Cryptography
	Public Class SARC4
		Implements IDisposable
		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Sub EVP_CIPHER_CTX_init(ByRef ctx As EVP_CTX)
		End Sub

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Sub EVP_EncryptInit_ex(ByRef ctx As EVP_CTX, Cipher As IntPtr, Engine As IntPtr, key As Byte(), iv As Byte())
		End Sub

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Function EVP_CIPHER_CTX_set_key_length(ByRef ctx As EVP_CTX, keylen As Integer) As Integer
		End Function

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Function EVP_CIPHER_CTX_cleanup(ByRef ctx As EVP_CTX) As Integer
		End Function

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Function EVP_EncryptUpdate(ByRef ctx As EVP_CTX, <Out> outp As Byte(), ByRef outL As Integer, <[In]> inp As Byte(), inplen As Integer) As Integer
		End Function

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Function EVP_EncryptFinal_ex(ByRef ctx As EVP_CTX, <Out> output As Byte(), ByRef outL As Integer) As Integer
		End Function

		<DllImport("Libeay32.dll", CallingConvention := CallingConvention.Cdecl)> _
		Private Shared Function EVP_rc4() As IntPtr
		End Function

		Private context As EVP_CTX

		Public Sub New()
			context = New EVP_CTX()
			EVP_CIPHER_CTX_init(context)
			EVP_EncryptInit_ex(context, EVP_rc4(), IntPtr.Zero, Nothing, Nothing)
			EVP_CIPHER_CTX_set_key_length(context, 20)
		End Sub

		Public Sub Dispose() Implements IDisposable.Dispose
			EVP_CIPHER_CTX_cleanup(context)
		End Sub

		Public Sub PrepareKey(seed As Byte())
			EVP_EncryptInit_ex(context, IntPtr.Zero, IntPtr.Zero, seed, Nothing)
		End Sub

		Public Sub ProcessBuffer(data As Byte(), len As Integer)
			Dim outLen As Integer = 0
			EVP_EncryptUpdate(context, data, outLen, data, len)
			EVP_EncryptFinal_ex(context, data, outLen)
		End Sub

		Private mCryptBuffer As Byte() = New Byte(257) {}
	End Class
End Namespace
