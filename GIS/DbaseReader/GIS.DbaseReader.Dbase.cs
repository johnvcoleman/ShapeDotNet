//
//Dbase.cs
//

/// <license>
///  ShapeDotNet - Reads ESRI shapefiles.
///  Copyright (C) 2002-2004  John Coleman ( k5jvc at netscape.net )
///  
///  This program is free software; you can redistribute it and/or
///  modify it under the terms of the GNU General Public License
///  as published by the Free Software Foundation; either version 2
///  of the License, or (at your option) any later version.
///  
///  This program is distributed in the hope that it will be useful,
///  but WITHOUT ANY WARRANTY; without even the implied warranty of
///  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
///  GNU General Public License for more details.
///  
///  You should have received a copy of the GNU General Public License
///  along with this program; if not, write to the Free Software
///  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
/// 
/// </license>

using System;
using System.IO;

namespace ShapeDotNet.GIS.DbaseReader
{
	/// <summary>
	/// The class that provides access to the .dbf database file.
	/// </summary>
	public class Dbase : DbaseIndex
	{

		public Dbase( string filename ) : base( filename )
		{
		}

		~Dbase()
		{
		}

		
		/// <summary>
		/// Get a string array that represents a record, one string for
		/// each field in the record.
		/// </summary>
		/// @return A string array that represents one record.
		public string[] this[ int index ] 
		{	
			get 
			{
				return GetRecord( index );
			}
		}

		public void Dispose()
		{
		} 

		/// <summary>
		/// Get a whole record from the db file.  TODO: handle value types
		/// other than characters correctly.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private string[] GetRecord( int index )
		{
			FileStream fs = new FileStream( _Filename, FileMode.Open, FileAccess.Read );
			BinaryReader br	= new BinaryReader( fs );
			
			string[] buffer = new string[ _FieldCount ];

			if ( index < _RecordCount )
			{
				fs.Seek( _RecordStart + ( index * _RecordLength ), 0 );
			}
			else
			{
				return null;
			}

			br.ReadByte();  // delete flag

			for ( int a=0; a < _FieldCount; ++a )
			{
				char[] chars;

				switch( _FieldTypes[ a ] )
				{
					case FieldType.C:
					case FieldType.c:
						//buffer[ a ] = new string( br.ReadChars( _FieldLengths[ a ] ) );
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;

					case FieldType.D:
					case FieldType.d:
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;

					case FieldType.F:
					case FieldType.f:
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;

					case FieldType.L:
					case FieldType.l:
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;

					case FieldType.N:
					case FieldType.n:
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;

					default:
						chars = new char[_FieldLengths[ a ]];
						for(int i = 0; i < _FieldLengths[ a ]; i++) 
						{
							chars[i] = (char)br.ReadByte();
						}
						buffer[ a ] = new string(chars);
						break;
				}
			}

			br.Close();
			fs.Close();

			return buffer;
		}
	}
}
