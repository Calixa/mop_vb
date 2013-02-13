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


Imports Framework.Logging

Namespace DBC
    Public Class DBCLoader
        Public Shared Sub Init()
            Log.Message(LogType.NORMAL, "Loading DBCStorage...")
            DBCStorage.RaceStorage = DBCReader.ReadDBC(Of ChrRaces)(DBCStorage.RaceStrings, DBCFmt.ChrRacesEntryfmt, "ChrRaces.dbc")
            DBCStorage.ClassStorage = DBCReader.ReadDBC(Of ChrClasses)(Nothing, DBCFmt.ChrClassesEntryfmt, "ChrClasses.dbc")
            DBCStorage.CharStartOutfitStorage = DBCReader.ReadDBC(Of CharStartOutfit)(Nothing, DBCFmt.CharStartOutfitfmt, "CharStartOutfit.dbc")
            DBCStorage.NameGenStorage = DBCReader.ReadDBC(Of NameGen)(DBCStorage.NameGenStrings, DBCFmt.NameGenfmt, "NameGen.dbc")

            Log.Message(LogType.NORMAL, "Loaded {0} dbc files.", DBCStorage.DBCFileCount)
            Log.Message()
        End Sub
    End Class
End Namespace
