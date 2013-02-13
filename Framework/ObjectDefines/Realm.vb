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
    Public Class Realm
        Public Property Id() As UInteger
            Get
                Return m_Id
            End Get
            Set(value As UInteger)
                m_Id = value
            End Set
        End Property
        Private m_Id As UInteger
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = value
            End Set
        End Property
        Private m_Name As String
        Public Property IP() As String
            Get
                Return m_IP
            End Get
            Set(value As String)
                m_IP = value
            End Set
        End Property
        Private m_IP As String
        Public Property Port() As UInteger
            Get
                Return m_Port
            End Get
            Set(value As UInteger)
                m_Port = value
            End Set
        End Property
        Private m_Port As UInteger
    End Class
End Namespace
