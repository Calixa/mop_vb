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


Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography

Namespace Cryptography
    Public Class SRP6
        Shared hashAlgorithm As HashAlgorithm = New SHA1Managed()
        Private BNA As IntPtr, BNb As IntPtr, _BNB As IntPtr, BNg As IntPtr, BNk As IntPtr, BNn As IntPtr, _
            BNS As IntPtr, BNU As IntPtr, BNv As IntPtr, BNx As IntPtr
        Public A As Byte(), K As Byte(), M2 As Byte(), N As Byte(), S As Byte(), salt As Byte(), _
            U As Byte()
        Public b1 As Byte() = New Byte(19) {}
        Public B2 As Byte() = New Byte(31) {}
        Public g As Byte() = New Byte() {&H7}
        Public _k As Byte() = New Byte() {&H3}
        Public Username As String, Password As String

        Public Sub New()
            N = New Byte() {&H89, &H4B, &H64, &H5E, &H89, &HE1, _
                &H53, &H5B, &HBD, &HAD, &H5B, &H8B, _
                &H29, &H6, &H50, &H53, &H8, &H1, _
                &HB1, &H8E, &HBF, &HBF, &H5E, &H8F, _
                &HAB, &H3C, &H82, &H87, &H2A, &H3E, _
                &H9B, &HB7}

            salt = New Byte() {&HAD, &HD0, &H3A, &H31, &HD2, &H71, _
                &H14, &H46, &H75, &HF2, &H70, &H7E, _
                &H50, &H26, &HB6, &HD2, &HF1, &H86, _
                &H59, &H99, &H76, &H2, &H50, &HAA, _
                &HB9, &H45, &HE0, &H9E, &HDD, &H2A, _
                &HA3, &H45}
        End Sub

        <DllImport("libeay32.dll")> _
        Private Shared Function BN_add(r As IntPtr, a As IntPtr, b As IntPtr) As Integer
        End Function
        <DllImport("libeay32.dll", EntryPoint:="BN_bin2bn")> _
        Private Shared Function BN_Bin2BN(ByteArrayIn As Byte(), length As Integer, [to] As IntPtr) As IntPtr
        End Function
        <DllImport("libeay32.dll")> _
        Private Shared Function BN_bn2bin(a As IntPtr, [to] As Byte()) As Integer
        End Function
        <DllImport("libeay32.dll", EntryPoint:="BN_CTX_free")> _
        Private Shared Function BN_ctx_free(a As IntPtr) As Integer
        End Function
        <DllImport("libeay32.dll", EntryPoint:="BN_CTX_new")> _
        Private Shared Function BN_ctx_new() As IntPtr
        End Function
        <DllImport("libeay32.dll")> _
        Private Shared Function BN_div(dv As IntPtr, r As IntPtr, a As IntPtr, b As IntPtr, ctx As IntPtr) As Integer
        End Function
        <DllImport("libeay32.dll", EntryPoint:="BN_free")> _
        Private Shared Sub BN_Free(r As IntPtr)
        End Sub
        <DllImport("libeay32.dll")> _
        Private Shared Function BN_mod_exp(res As IntPtr, a As IntPtr, p As IntPtr, m As IntPtr, ctx As IntPtr) As IntPtr
        End Function
        <DllImport("libeay32.dll")> _
        Private Shared Function BN_mul(r As IntPtr, a As IntPtr, b As IntPtr, ctx As IntPtr) As Integer
        End Function
        <DllImport("libeay32.dll", EntryPoint:="BN_new")> _
        Private Shared Function BN_New() As IntPtr
        End Function
        Public Sub CalculateB()
            RAND_bytes(Me.B2, 20)
            Dim res As IntPtr = BN_New()
            Dim r As IntPtr = BN_New()
            Dim ptr3 As IntPtr = BN_New()
            Me.BNb = BN_New()
            Dim ctx As IntPtr = BN_ctx_new()
            Array.Reverse(Me.B2)
            Me.BNb = BN_Bin2BN(Me.B2, Me.B2.Length, IntPtr.Zero)
            Array.Reverse(Me.B2)
            BN_mod_exp(res, Me.BNg, Me.BNb, Me.BNn, ctx)
            BN_mul(r, Me.BNk, Me.BNv, ctx)
            BN_add(ptr3, res, r)
            BN_div(IntPtr.Zero, Me.BNb, ptr3, Me.BNn, ctx)
            BN_bn2bin(Me.BNb, Me.b1)
            Array.Reverse(Me.b1)
            BN_ctx_free(ctx)
            BN_Free(ptr3)
            BN_Free(r)
            BN_Free(res)
        End Sub

        Public Sub CalculateK()
            Dim list As ArrayList = Split(Me.S)
            list(0) = hashAlgorithm.ComputeHash(DirectCast(list(0), Byte()))
            list(1) = hashAlgorithm.ComputeHash(DirectCast(list(1), Byte()))
            Me.K = Combine(DirectCast(list(0), Byte()), DirectCast(list(1), Byte()))
        End Sub

        Public Sub CalculateM2(m1 As Byte())
            Dim dst As Byte() = New Byte((Me.A.Length + m1.Length) + (Me.K.Length - 1)) {}
            Buffer.BlockCopy(Me.A, 0, dst, 0, Me.A.Length)
            Buffer.BlockCopy(m1, 0, dst, Me.A.Length, m1.Length)
            Buffer.BlockCopy(Me.K, 0, dst, Me.A.Length + m1.Length, Me.K.Length)
            Me.M2 = hashAlgorithm.ComputeHash(dst)
        End Sub

        Public Sub CalculateS()
            Dim res As IntPtr = BN_New()
            Dim r As IntPtr = BN_New()
            Me.BNS = BN_New()
            Dim ctx As IntPtr = BN_ctx_new()
            Me.S = New Byte(31) {}
            BN_mod_exp(res, Me.BNv, Me.BNU, Me.BNn, ctx)
            BN_mul(r, Me.BNA, res, ctx)
            BN_mod_exp(Me.BNS, r, Me.BNb, Me.BNn, ctx)
            BN_bn2bin(Me.BNS, Me.S)
            Array.Reverse(Me.S)
            Me.CalculateK()
            BN_ctx_free(ctx)
            BN_Free(r)
            BN_Free(res)
        End Sub

        Public Sub CalculateU(a As Byte())
            Me.A = a
            Dim dst As Byte() = New Byte(a.Length + (Me.b1.Length - 1)) {}
            Buffer.BlockCopy(a, 0, dst, 0, a.Length)
            Buffer.BlockCopy(Me.b1, 0, dst, a.Length, Me.b1.Length)
            Me.U = hashAlgorithm.ComputeHash(dst)
            Array.Reverse(Me.U)
            Me.BNU = BN_Bin2BN(Me.U, Me.U.Length, IntPtr.Zero)
            Array.Reverse(Me.U)
            Array.Reverse(Me.A)
            Me.BNA = BN_Bin2BN(Me.A, Me.A.Length, IntPtr.Zero)
            Array.Reverse(Me.A)
            Me.CalculateS()
        End Sub

        Public Sub CalculateV()
            Me.BNv = BN_New()
            Dim ctx As IntPtr = BN_ctx_new()
            BN_mod_exp(Me.BNv, Me.BNg, Me.BNx, Me.BNn, ctx)
            Me.CalculateB()
            BN_ctx_free(ctx)
        End Sub

        Public Sub CalculateX(username As Byte(), password As Byte())
            Dim src As Byte() = username
            Dim buffer2 As Byte() = password
            Dim dst As Byte() = New Byte((src.Length + buffer2.Length)) {}
            Dim buffer5 As Byte() = New Byte(Me.salt.Length + 19) {}
            Buffer.BlockCopy(src, 0, dst, 0, src.Length)
            dst(src.Length) = &H3A
            Buffer.BlockCopy(buffer2, 0, dst, src.Length + 1, buffer2.Length)
            Buffer.BlockCopy(hashAlgorithm.ComputeHash(dst, 0, dst.Length), 0, buffer5, Me.salt.Length, 20)
            Buffer.BlockCopy(Me.salt, 0, buffer5, 0, Me.salt.Length)
            Dim array__1 As Byte() = hashAlgorithm.ComputeHash(buffer5)
            Array.Reverse(array__1)
            Me.BNx = BN_Bin2BN(array__1, array__1.Length, IntPtr.Zero)
            Array.Reverse(Me.g)
            Me.BNg = BN_Bin2BN(Me.g, Me.g.Length, IntPtr.Zero)
            Array.Reverse(Me.g)
            Array.Reverse(Me.K)
            Me.BNk = BN_Bin2BN(Me.K, Me.K.Length, IntPtr.Zero)
            Array.Reverse(Me.K)
            Array.Reverse(Me.N)
            Me.BNn = BN_Bin2BN(Me.N, Me.N.Length, IntPtr.Zero)
            Array.Reverse(Me.N)
            Me.CalculateV()
        End Sub

        Private Shared Function Combine(b1 As Byte(), b2 As Byte()) As Byte()
            If b1.Length <> b2.Length Then
                Return Nothing
            End If
            Dim buffer As Byte() = New Byte(b1.Length + (b2.Length - 1)) {}
            Dim index As Integer = 0
            Dim num2 As Integer = 1
            For i As Integer = 0 To b1.Length - 1
                buffer(index) = b1(i)
                index += 1
                index += 1
            Next
            For j As Integer = 0 To b2.Length - 1
                buffer(num2) = b2(j)
                num2 += 1
                num2 += 1
            Next
            Return buffer
        End Function

        Protected Overrides Sub Finalize()
            Try
                BN_Free(Me.BNA)
                BN_Free(Me.BNb)
                BN_Free(Me.BNb)
                BN_Free(Me.BNg)
                BN_Free(Me.BNk)
                BN_Free(Me.BNn)
                BN_Free(Me.BNS)
                BN_Free(Me.BNU)
                BN_Free(Me.BNv)
                BN_Free(Me.BNx)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        <DllImport("libeay32.dll")> _
        Public Shared Function RAND_bytes(buf As Byte(), num As Integer) As Integer
        End Function
        Private Shared Function Split(bo As Byte()) As ArrayList
            Dim dst As Byte() = New Byte(bo.Length - 2) {}
            If ((bo.Length Mod 2) <> 0) AndAlso (bo.Length > 2) Then
                Buffer.BlockCopy(bo, 1, dst, 0, bo.Length)
            End If
            Dim buffer2 As Byte() = New Byte(bo.Length \ 2 - 1) {}
            Dim buffer3 As Byte() = New Byte(bo.Length \ 2 - 1) {}
            Dim index As Integer = 0
            Dim num2 As Integer = 1
            For i As Integer = 0 To buffer2.Length - 1
                buffer2(i) = bo(index)
                index += 1
                index += 1
            Next
            For j As Integer = 0 To buffer3.Length - 1
                buffer3(j) = bo(num2)
                num2 += 1
                num2 += 1
            Next
            Dim list As New ArrayList()
            list.Add(buffer2)
            list.Add(buffer3)
            Return list
        End Function
    End Class
End Namespace
