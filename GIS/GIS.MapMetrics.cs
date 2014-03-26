//
//MapMetrics.cs
//

/// <license>
///  Copyright (C) 2002-2004  John Coleman ( john@k5jvc.com )
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
	/// This class holds all of the pertinent metrics of the entire
	/// map.  Think of it as a global class, there is only one.  The
	/// assumption is made that all layers are in the same units, datum,
	/// and projection.
	/// </summary>
	public class MapMetrics
	{
		RectangleD _extents;
		RectangleD _viewport;
		Bitmap _canvas;
		double _mapUnitsPerPixelX;
		double _mapUnitsPerPixelY;

		public MapMetrics()
		{
			_viewport = new RectangleD();
			_extents = new RectangleD();
		}

		/// <summary>
		/// The extents of the map. ( all layers )
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
		
		private void CalculateUnitsPerPixel()
		{			
			_mapUnitsPerPixelX = _viewport.DeltaX / _canvas.Width;
			_mapUnitsPerPixelY = _viewport.DeltaY / _canvas.Height;
		}
		
		/// <summary>
		/// Canvas is the internal bitmap that is drawn to.
		/// </summary>
		public Bitmap Canvas
		{
			get
			{
				return _canvas;
			}
			set
			{
				_canvas = value;
			}
		}
		
		/// <summary>
		/// Convert local pixel coordinates to geographic coordinates.
		/// </summary>
		/// @param dx local pixel X coordinate.
		/// @param dy local pixel Y coordinate.
		public void Pixel2World( ref double dx, ref double dy )
		{
			dx = _viewport.Xmin + ( dx * _mapUnitsPerPixelX );
			dy = _viewport.Ymax - ( dy * _mapUnitsPerPixelY );
		}

		/// <summary>
		/// Convert local pixel coordinates to geographic coordinates.
		/// </summary>
		/// @param point the local Drawing.Point to convert
		public PointD Pixel2World( Point point )
		{
			PointD tempPoint = new PointD();

			tempPoint.X = _viewport.Xmin + ( point.X * _mapUnitsPerPixelX );
			tempPoint.Y = _viewport.Ymax - ( point.Y * _mapUnitsPerPixelY );

			return tempPoint;
		}

		/// <summary>
		/// Update all of the metrics for the map.  Call before drawing anything.
		/// </summary>
		public void UpdateMetrics()
		{
			CalculateUnitsPerPixel();
		}

		/// <summary>
		/// The current geographic viewport on the map.
		/// </summary>
		public RectangleD Viewport
		{
			get
			{
				return _viewport;
			}
			set
			{
				_viewport = value;
			}
		}

		/// <summary>
		/// Convert geographic coordinates into local pixel coordinates.
		/// </summary>
		/// @param x Geographic X coordinate.
		/// @param y Geographic Y coordinate.
		public void World2Pixel( ref double x, ref double y )
		{
			x = ( x - _viewport.Xmin ) / _mapUnitsPerPixelX;
			y = ( _viewport.Ymax - y ) / _mapUnitsPerPixelY;
		}
		
		/// <summary>
		/// Convert geographic coordinates into local pixel coordinates.
		/// </summary>
		/// @param oldPoint PointD to convert to pixel coordinate
		/// @param newPoint the converted Point (output).
		public void World2Pixel( PointD oldPoint, ref Point newPoint )
		{			
			newPoint.X = (int) ( ( oldPoint.X - _viewport.Xmin ) / _mapUnitsPerPixelX );
			newPoint.Y = (int) ( ( _viewport.Ymax - oldPoint.Y ) / _mapUnitsPerPixelY );
		}
	}
}
