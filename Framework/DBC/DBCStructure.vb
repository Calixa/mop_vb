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

Namespace DBC
	Public Structure DbcHeader
		Public Signature As Integer
		Public RecordsCount As Integer
		Public FieldsCount As Integer
		Public RecordSize As Integer
		Public StringTableSize As Integer

		Public ReadOnly Property IsDBC() As Boolean
			Get
				Return Signature = &H43424457
			End Get
		End Property

		Public ReadOnly Property DataSize() As Long
			Get
				Return CLng(RecordsCount * RecordSize)
			End Get
		End Property

		Public ReadOnly Property StartStringPosition() As Long
			Get
				Return DataSize + CLng(Marshal.SizeOf(GetType(DbcHeader)))
			End Get
		End Property
	End Structure


	Public Structure ChrClasses
		Public ClassID As UInteger
		' 0        m_ID
		Public powerType As UInteger
		' 1        m_DisplayPower
		' 2        m_petNameToken
		Public _name As UInteger
		' 3        m_name_lang
		'char*       nameFemale;                               // 4        m_name_female_lang
		'char*       nameNeutralGender;                        // 5        m_name_male_lang
		'char*       capitalizedName                           // 6,       m_filename
		Public spellfamily As UInteger
		' 7        m_spellClassSet
		'uint32 flags2;                                        // 8        m_flags (0x08 HasRelicSlot)
		Public CinematicSequence As UInteger
		' 9        m_cinematicSequenceID
		Public expansion As UInteger
		' 10       m_required_expansion
		'uint32                                                // 11
		'uint32                                                // 12
		'uint32                                                // 13
		''' <summary>
		''' Return current Race Name
		''' </summary>
		Public ReadOnly Property ClassName() As String
			Get
				Return DBCStorage.ClassStrings.LookupByKey(_name)
			End Get
		End Property
	End Structure

	Public Structure ChrRaces
		Public RaceID As UInteger
		' 0
		' 1 unused
		Public FactionID As UInteger
		' 2 faction template id
		' 3 unused
		Public model_m As UInteger
		' 4
		Public model_f As UInteger
		' 5
		' 6 unused
		Public TeamID As UInteger
		' 7 (7-Alliance 1-Horde)
		' 8-11 unused
		Public CinematicSequence As UInteger
		' 12 id from CinematicSequences.dbc
		' 13 unused
		Public _name As UInteger
		' 14
		' 17
		' 16 
		' 17-18    m_facialHairCustomization[2]
		' 19       m_hairCustomization
		'uint32                                                // 20 (23 for worgens)
		'uint32                                                // 21 4.0.0
		'uint32                                                // 22 4.0.0
		''' <summary>
		''' Return current Race Name
		''' </summary>
		Public ReadOnly Property RaceName() As String
			Get
				Return DBCStorage.RaceStrings.LookupByKey(_name)
			End Get
		End Property
	End Structure

	Public Structure CharStartOutfit
		Public Mask As UInteger
		' Race, Class, Gender, ?
		<MarshalAs(UnmanagedType.ByValArray, SizeConst := 24)> _
		Public ItemId As UInteger()
	End Structure

	Public Structure NameGen
		Public Id As UInteger
		Public _name As UInteger
		Public Race As UInteger
		Public Gender As UInteger

		Public ReadOnly Property Name() As String
			Get
				Return DBCStorage.NameGenStrings.LookupByKey(_name)
			End Get
		End Property
	End Structure
End Namespace
