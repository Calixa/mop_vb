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



Namespace ObjectDefines
	Public Class Quaternion
		Public ReadOnly X As Single
		Public ReadOnly Y As Single
		Public ReadOnly Z As Single
		Public ReadOnly W As Single

		Const multiplier As Single = 9.536743E-07F

		Public Sub New(compressedQuaternion As Long)
			Dim c As Long = compressedQuaternion

			X = (c >> 42) * 4.768372E-07F
			Y = (c >> 21) * multiplier
			Z = c * multiplier

			W = (X * X) + (Y * Y) + (Z * Z)

			If Math.Abs(W - 1F) >= multiplier Then
				W = CSng(Math.Sqrt(1F - W))
			Else
				W = 0
			End If
		End Sub

		Public Shared Function GetCompressed(orientation As Single) As Long
			Dim z As Single = CSng(Math.Sin(orientation / 1.9999945))
			Return CLng(Math.Truncate(z / Math.Atan(Math.Pow(2, -20))))
		End Function
	End Class
End Namespace
