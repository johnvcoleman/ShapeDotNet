//
//VectorFeature.cs
//

/// <license>
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
using System.Drawing;

namespace ShapeDotNet.GIS
{	
	/// <summary>
	/// This class holds the payload of each feature in a layer.  It's
	/// modeled after the construction of data in a ESRI Shapefile where one
	/// feature can contain multiple sub-features (parts)  See pages
	/// 7-10 in the ESRI Shapefile specification for more details.
	/// </summary>
	public class VectorFeature
	{
		// PointD[ part ][ points ]
		private PointD[][] _segment;
		private MapMetrics _mapMetrics;
		private RectangleD _extents;
		private bool _isInViewport;
		int _segmentCounter;
		float _featureAngle;
		PointD _labelAnchor;

		
		
		/// <summary>
		/// The only constructor requires that you provide the number
		/// of parts in this feature so that the first dimention of 
		/// the array can be created.
		/// </summary>
		/// @param numOfParts the number of sub-features in this feature
		/// @param mapMetrics the GIS.MapMetrics object for this map
		public VectorFeature( int numOfParts, MapMetrics mapMetrics )
		{
			if ( numOfParts == 1094233352 )
			{
				numOfParts = 5;
			}
			_segment		= new PointD[ numOfParts ][];
			_mapMetrics		= mapMetrics;
			_extents		= new RectangleD();
			_segmentCounter = 0;
			_featureAngle	= 0;
		}

		
		
		/// <summary>
		/// Add a new segment (part) to this feature.
		/// </summary>
		/// @param points the array of GIS.PointD objects that make up this segment
		public void AddSegment( PointD[] points )
		{
			_segment[ _segmentCounter ] = points;
			++_segmentCounter;
		}



		/// <summary>
		/// Determines the distance from the GIS.PointD object to the center of
		/// this feature if it has more than one point.  If this feature only has
		/// one point then the distance is calculated from that first point.
		/// </summary>
		/// @param point the point to measure to
		/// @returns the distance between the center of this rectangle to the point
		public double DistanceFromFeature( PointD point )
		{			
			double tmpx;
			double tmpy;

			if ( this[0].Length == 1 ) // It's a point feature
			{
				tmpx = Math.Pow( Math.Abs( this.GetPoint(0, 0).X - point.X ), 2 ); 
				tmpy = Math.Pow( Math.Abs( this.GetPoint(0, 0).Y - point.Y ), 2 );
			}
			else  // otherwise it's a multi-point feature
			{
				tmpx = Math.Pow( Math.Abs( this.Extents.CenterX - point.X ), 2 ); 
				tmpy = Math.Pow( Math.Abs( this.Extents.CenterY - point.Y ), 2 );
			}
			
			return Math.Sqrt( ( tmpx + tmpy ) ); 
		}



		/// <summary>
		/// The extents of this feature.
		/// </summary>
		public RectangleD Extents
		{
			get
			{
				return _extents;
			}
			set
			{
				_extents = value;
			}
		}

		
		
		/// <summary>
		/// Get a single GIS.PointD from this feature.
		/// </summary>
		/// @param part the index of the part or segment of the feature
		/// @param point the index of the point within the segment
		/// <returns></returns>
		public PointD GetPoint( int part, int point )
		{
			return _segment[ part ][ point ];
		}

		
		
		/// <summary>
		/// Get a single part or segment from this feature
		/// </summary>
		/// @param part the index of the part or segment to to return 
		/// @returns an array of GIS.PointD objects that represent the part or segment
		public PointD[] GetPoints( int part )
		{
			return _segment[ part ];
		}



		/// <summary>
		/// The angle of this feature. Really only used for line types as
		/// a way to communicate the angle needed to label the line.
		/// </summary>
		public float FeatureAngle
		{
			get
			{
				return _featureAngle;
			}
			set
			{
				_featureAngle = value;
			}
		}



		/// <summary>
		/// The anchor point for the label of this feature.
		/// </summary>
		public PointD LabelAnchor
		{
			get
			{
				return _labelAnchor;
			}
			set
			{
				_labelAnchor = value;
			}
		}
		
		
		
		/// <summary>
		/// Flags this feature as being in the current GIS.MapMetrics.Viewport.
		/// </summary>
		public bool IsInViewport
		{
			get
			{
				return _isInViewport;
			}
			set
			{
				_isInViewport = value;
			}
		}

		
		
		/// <summary>
		/// The number of parts or segments in this feature.
		/// </summary>
		public int SegmentCount
		{
			get
			{
				return _segment.GetLength( 0 );
			}
		}
		


		/// <summary>
		/// Returns an array of Point objects that represent a part or
		/// segment that has been converted to local coordinates so
		/// they can be drawn to the canvas.
		/// </summary>
		/// @param part the index of the part or segment in this feature
		public Point[] this[ int part ]
		{
			get
			{
				Point[] tempPoints = new Point[ _segment[ part ].Length ]; 

				for ( int point=0; point < _segment[ part ].Length; ++point )
				{
					tempPoints[ point ] = new Point( 0, 0 );
					_mapMetrics.World2Pixel( _segment[ part ][ point ], ref tempPoints[ point ] );
				}

				return tempPoints;	
			}
		}
	}
}
