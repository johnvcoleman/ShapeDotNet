//
//ShapeIndex.cs
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

#region Using
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace ShapeDotNet.GIS.ShapefileReader
{
	/// <summary>
	/// The types of shapefiles that *ARE SUPPORTED* by SDN.
	/// </summary>
	public enum ShapeType 
	{
		Null		= 0,
		Point		= 1,
		Line		= 3,
		Polygon		= 5,
		Multipoint	= 8 
	};

	
	
	/// <summary>
	/// Reads and generates the indexing structures used to read the 
	/// Shapefile.  This is an Abstract class and is not directly 
	/// instantiated, it is inherited by the Shapefile class.
	/// </summary>
	abstract public class ShapeIndex
	{				
		protected int 				_Version;
		protected ShapeType 		_ShapeType;
		protected string 			_Filename;
		protected int 				_IndexFileSize;
		protected RectangleD		_extents;

		protected int[]				_OffsetOfRecord;
		protected int[]				_LengthOfRecord;
		protected int				_FeatureCount;
		
		protected VectorFeature[]	_features;
		protected MapMetrics		_mapMetrics;
		
		
		
		/// <summary>
		/// Constructor of the ShapIndex class.  It reads
		/// the .shp & .shx files and sets up the feature data structures. It 
		/// parses the header of the .shx file, reads the entire .shx file and 
		/// then reads the .shp file.
		/// </summary>
		/// @param filename The filename the shapefile we want to load.
		/// @param metrics The MapMetrics object for this Map.
		public ShapeIndex( string filename, MapMetrics metrics )
		{			
			_Filename		= filename;
			_mapMetrics		= metrics;
			_extents		= new RectangleD();
			
			// 1.. 2.. 3.. 4..  8^)
			ParseHeader( filename );// .shx file
			ReadIndex( filename ); // .shx file
			LoadFile(); // .shp file
			SetupLabels();
		}
		
		
		
		/// <summary>
		/// Loads the VectorFeature array with the features from the
		/// shapefile.
		/// </summary>
		private void LoadFile()
		{			
			FileStream fs		= new FileStream( _Filename, FileMode.Open );
			BinaryReader br		= new BinaryReader( fs );
			VectorFeature tempFeature;
			int[] segments;
			int segmentPosition;
			int pointsInSegment;
			PointD[] tempPoints;
			PointD[] segmentPoints;

			_features = new VectorFeature[ _FeatureCount ];


			if ( _ShapeType == ShapeType.Point )
			{
				for ( int a=0; a < _FeatureCount; ++a ) 
				{
					// Point types don't have parts (segments) / one point per feature
					tempFeature = new VectorFeature( 1, _mapMetrics );
					tempPoints = new PointD[ 1 ];

					fs.Seek( _OffsetOfRecord[ a ], 0 );

					br.ReadInt32(); //Record number (not needed)
					br.ReadInt32(); //Content length (not needed)
					br.ReadInt32(); //Shape type (not needed)
					tempPoints[ 0 ] = new PointD( br.ReadDouble(), br.ReadDouble() );
					
					tempFeature.AddSegment( tempPoints );
					_features[ a ] = tempFeature;
				}
			}
			else
			{			
				for ( int a=0; a < _FeatureCount; ++a ) 
				{
					fs.Seek( _OffsetOfRecord[ a ] + 44, 0 );
					
					// Read the number of parts (segments) and create a new VectorFeature
					tempFeature = new VectorFeature( br.ReadInt32(), _mapMetrics ); 

					fs.Seek( _OffsetOfRecord[ a ], 0 );
					
					br.ReadInt32(); //Record number (not needed)
					br.ReadInt32(); //Content length (not needed)
					br.ReadInt32(); //Shape type (not needed)
					tempFeature.Extents.Xmin = br.ReadDouble();
                    tempFeature.Extents.Ymin = br.ReadDouble();
					tempFeature.Extents.Xmax = br.ReadDouble();
					tempFeature.Extents.Ymax = br.ReadDouble();
					br.ReadInt32(); // Number of parts (segments) gotten earlier
					tempPoints = new PointD[ br.ReadInt32() ]; // Number of points

					segments = new int[ tempFeature.SegmentCount + 1 ];

					//Read in the segment indexes
					for ( int b=0; b<tempFeature.SegmentCount; ++b )
					{
						segments[ b ] = br.ReadInt32();
					}
					
					//Read in *ALL* of the points in the feature
					for ( int c=0; c<tempPoints.Length; ++c )
					{
						tempPoints[ c ] = new PointD( br.ReadDouble(), br.ReadDouble() );
					}

					//Add in an ending point for the inner loop that follows (e) 
					segments[ tempFeature.SegmentCount ] = tempPoints.Length;

					//Watch your step...
					for ( int d=0; d<tempFeature.SegmentCount; ++d )
					{
						pointsInSegment = segments[ d+1 ] - segments[ d ];
						segmentPoints = new PointD[ pointsInSegment ];
						segmentPosition = 0;

						for ( int e=segments[ d ]; e<segments[ d+1 ]; ++e )
						{
							segmentPoints[ segmentPosition ] = tempPoints[ e ];
							++segmentPosition;
						}

						tempFeature.AddSegment( segmentPoints );
					}

					_features[ a ] = tempFeature;
				}
			}
			
			_OffsetOfRecord	= null;  //don't need the info anymore
			_LengthOfRecord	= null;

			GC.Collect();

			br.Close();
			fs.Close();
		}
		
		
		
		/// <summary>
		/// Reads the header of the .shx index file and extracts the 
		/// information that is needed to read the remainder of the
		/// .shx file and the .shp file.
		/// </summary>
		/// @param filename The name of the Shapefile. (ie. "blah.shp")
		private void ParseHeader( string filename )
		{
			filename			= filename.Remove( filename.Length - 4, 4 );
			filename			+= ".shx";

			FileStream fs		= new FileStream( filename, FileMode.Open );
			BinaryReader br		= new BinaryReader( fs );
			
			int i;
			int[] header_a 		= new int[ 9 ];
			double[] header_b 	= new double[ 8 ];

			//Read the first part of the header as integers
			for ( i=0; i<9; ++i ) 
			{
				header_a[ i ]	= br.ReadInt32();
			}
			
			//Read the second part of the header as doubles
			for ( i=0; i<8; ++i ) 
			{
				header_b[ i ]	= br.ReadDouble();
			}
			
			br.Close();
			fs.Close();

			//File size is reported in the header as a big endian
			//16 bit word.
			//Translating the size to 8 bit little endian.
			_IndexFileSize 		= 2* SwitchByteOrder( header_a[ 6 ] );
			_Version 			= header_a[ 7 ];
			_ShapeType 			= ( ShapeType ) header_a[ 8 ]; //cast int to enum
			_extents.Xmin 		= header_b[ 0 ];
			_extents.Xmax 		= header_b[ 2 ];
			_extents.Ymin 		= header_b[ 1 ];
			_extents.Ymax 		= header_b[ 3 ];		
		}
		
		
		
		/// <summary>
		/// Reads the record offset and length from the .shx index file and
		/// places the information into arrays (_OffsetOfRecord &
		/// _LengthOfRecord)
		/// </summary>
		/// @param filename The name of the Shapefile. (ie. "blah.shp")
		private void ReadIndex( string filename )
		{
			filename			= filename.Remove( filename.Length - 4, 4 );
			filename			+= ".shx";
			
			FileStream fs		= new FileStream( filename, FileMode.Open );
			BinaryReader br		= new BinaryReader( fs );
			int ibuffer;
			
			_FeatureCount		= ( _IndexFileSize - 100 ) / 8;
			
			_OffsetOfRecord		= new int[ _FeatureCount ];
			_LengthOfRecord		= new int[ _FeatureCount ];
			
			fs.Seek( 100, 0 );  //seek past the file header
			
			for ( int x=0; x < _FeatureCount; ++x ) 
			{
				//value must be multiplied by 2 to convert from the native
				//16 bit value (word) to an 8 bit value, and convert the
				//format from big endian to little endian.
				ibuffer = br.ReadInt32(); 		
				_OffsetOfRecord[ x ] = 2* SwitchByteOrder( ibuffer );

				//Add 8 bytes to the length to compensate for the
				//header at the beginning of each record in the shp file.
				ibuffer = br.ReadInt32();				
				_LengthOfRecord[ x ] = ( 2* SwitchByteOrder( ibuffer ) ) + 8;
			}

			br.Close();
			fs.Close();
		}



		/// <summary>
		/// Determines the optimal (?) geographic placement of the yet to be
		/// created labels and if the shapefile is the line type it also
		/// determines the angle of the line at the point of label placement
		/// so the label can be rotated to match the line.
		/// </summary>
		private void SetupLabels()
		{
			double minx;
			double maxx;
			double miny;
			double maxy;
			PointD[] tmpPoints;
			int midPoint;
			int endPoint;
			int segToDraw =0;
			int maxSegPoints =0;
			
			if ( _ShapeType == ShapeType.Polygon ||
				_ShapeType == ShapeType.Multipoint )
			{
				for ( int a=0; a< _features.Length; ++a )
				{
					// Optimal situation
					if ( _features[a].SegmentCount == 1 )
					{
						_features[a].LabelAnchor = new PointD( 
							_features[a].Extents.CenterX, 
							_features[a].Extents.CenterY );

						continue;
					} 
					else 
					{
						for ( int b=0; b<_features[a].SegmentCount; ++b )
						{
							tmpPoints = _features[a].GetPoints(b);
							
							if ( tmpPoints.Length > maxSegPoints )
							{
								maxSegPoints = tmpPoints.Length;
								segToDraw = b;
							}
						}
							tmpPoints = _features[ a ].GetPoints( segToDraw );

							PointD p1 = tmpPoints[ 0 ];
							PointD p2 = tmpPoints[ (int)tmpPoints.Length /2 ];
					
							if ( p1.X < p2.X )
							{
								minx = p1.X;
								maxx = p2.X;
							} 
							else 
							{
								minx = p2.X;
								maxx = p1.X;
							}

					
							if ( p1.Y < p2.Y )
							{
								miny = p1.Y;
								maxy = p2.Y;
							} 
							else 
							{
								miny = p2.Y;
								maxy = p1.Y;
							}

							_features[a].LabelAnchor = new PointD( minx + (( maxx - minx)/2), miny + (( maxy - miny)/2) );
							segToDraw =0;
							maxSegPoints =0;
					}
				}
			}

			if ( _ShapeType == ShapeType.Line )
			{
				double dx, dy;
				double tmpx, tmpy;
				double angle = 0.0;

				for ( int a=0; a< _features.Length; ++a )
				{										
					tmpPoints = _features[a].GetPoints(0);
					endPoint = tmpPoints.Length - 1;
					
					// Figure out which point to use as an anchor for the label					
					midPoint = ( tmpPoints.Length -1 ) / 2;
					dx = tmpPoints[midPoint +1].X - tmpPoints[midPoint].X;
					dy = tmpPoints[midPoint +1].Y - tmpPoints[midPoint].Y;
					
					tmpx = tmpPoints[midPoint].X + ( dx / 2 );
					tmpy = tmpPoints[midPoint].Y + ( dy / 2 );
					
					_features[a].LabelAnchor = new PointD( tmpx, tmpy );
					
					// calculate angle of line					
					angle = -Math.Atan(dy / dx) + Math.PI / 2d;
					angle *= (180d / Math.PI); // convert radians to degrees

					_features[a].FeatureAngle = (float) angle - 90; // -90 text orientation
				}
			}
		}

		
		
		///<summary>
		///Reverses the byte order of an integer (int32 only) 
		///(big ->little or little ->big)
		///</summary>
		///@param i The int32 integer to byte swap
		protected int SwitchByteOrder (int i) 
		{			
			byte[] buffer 	= new byte[4];
			buffer 			= BitConverter.GetBytes(i);
			Array.Reverse( buffer, 0, buffer.Length );
			
			return BitConverter.ToInt32(buffer, 0);
		}
		//#endregion
	}
}
