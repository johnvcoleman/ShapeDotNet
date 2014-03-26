//
//RectangleD.cs
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

namespace ShapeDotNet.GIS
{
	/// <summary>
	/// RectangleD is a geographic rectangle represented by the upper left
	/// and lower right GIS.PointD.  This representation is *NOT* like the
	/// one used by GDI+ where the rectangle is expressed by its upper left
	/// point, its width, and height.  Don't confuse this class with RectangleF!
	/// </summary>
	public class RectangleD
	{
		private PointD _UL;
		private PointD _LR;
		
		public RectangleD()
		{
			_UL = new PointD( 0.0d, 0.0d );
			_LR = new PointD( 0.0d, 0.0d );
		}
		
		public RectangleD( double ul_x, double ul_y, double lr_x, double lr_y )
		{
			_UL = new PointD( ul_x, ul_y );
			_LR = new PointD( lr_x, lr_y );
		}

		public RectangleD( PointD ul, PointD lr )
		{
			_UL = ul;
			_LR = lr;
		}

		public RectangleD( RectangleD rect )
		{
			_UL = rect.UL;
			_LR = rect.LR;
		}
		
		
		/// <summary>
		/// The geographic center of this rectangle on the X plane.
		/// </summary>
		public double CenterX
		{
			get
			{
				return Xmin + ( DeltaX / 2 );
			}
		}

		/// <summary>
		/// The geographic center of this rectangle on the Y plane.
		/// </summary>
		public double CenterY
		{
			get
			{
				return Ymin + ( DeltaY / 2 );
			}
		}

		/// <summary>
		/// Checks to see if the geographic point X,Y in inside this rectangle
		/// </summary>
		/// @param X the X coordinate of the point to test
		/// @param Y the Y coordinate of the point to test
		/// @returns true if the point is inside this rectangle
		public bool Contains( double X, double Y )
		{
			return Contains( new PointD( X, Y ) );
		}
		
		/// <summary>
		/// Checks to see if the GIS.PointD object is in inside this rectangle
		/// </summary>
		/// @param point the point to test
		/// @returns true if the point is inside this rectangle
		public bool Contains( PointD point )
		{
			if ( point > _UL && point < _LR ) // overloads in PointD.cs
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Determines if the GIS.RectangleD object intersects this rectangle
		/// </summary>
		/// @param rect the rectangle to test
		/// @returns true if any part of the rectangles intersect
		public bool Intersects( RectangleD rect )
		{
			// It's not pretty, but it works.  
			// Is there a mathematician in the house?

			if ( Contains( rect.UL ) )
			{
				return true;
			}

			if ( Contains( rect.LR ) )
			{
				return true;
			}
			
			if ( rect.Xmin <= this.Xmax &&
				 rect.Xmax >= this.Xmax )
			{
				if ( this.Ymax >= rect.Ymax &&
					this.Ymin <= rect.Ymax )
				{
					return true;
				}

				if ( this.Ymax >= rect.Ymin &&
					this.Ymin <= rect.Ymin )
				{
					return true;
				}
			}

			if ( rect.Xmin <= this.Xmin &&
				rect.Xmax >= this.Xmin )
			{
				if ( this.Ymax >= rect.Ymax &&
					this.Ymin <= rect.Ymax )
				{
					return true;
				}

				if ( this.Ymax >= rect.Ymin &&
					this.Ymin <= rect.Ymin )
				{
					return true;
				}
			}

			if ( rect.Ymin <= this.Ymax &&
				rect.Ymax >= this.Ymax )
			{
				if ( this.Xmax >= rect.Xmax &&
					this.Xmin <= rect.Xmax )
				{
					return true;
				}

				if ( this.Xmax >= rect.Xmin &&
					this.Xmin <= rect.Xmin )
				{
					return true;
				}
			}

			if ( rect.Ymin <= this.Ymin &&
				rect.Ymax >= this.Ymin )
			{
				if ( this.Xmax >= rect.Xmax &&
					this.Xmin <= rect.Xmax )
				{
					return true;
				}

				if ( this.Xmax >= rect.Xmin &&
					this.Xmin <= rect.Xmin )
				{
					return true;
				}
			}
			
			if ( rect.UL < this.UL &&
				rect.LR > this.LR )
			{
				return true;
			}

			return false;
		}


		/// <summary>
		/// Geographic aspect voodoo.  Tries to keep the geographic aspect
		/// at a 1:1 ratio.  Used by GIS.Map only.
		/// </summary>
		public void FixGeoAspect()
		{
			if ( this.DeltaX > this.DeltaY )
			{
				double tmpCenter = this.CenterY;
				this.Ymin = tmpCenter - ( ( this.DeltaX ) / 2 );
				this.Ymax = tmpCenter + ( ( this.DeltaX ) / 2 );
			}
			else
			{
				double tmpCenter = this.CenterX;
				this.Xmin = tmpCenter - ( ( this.DeltaY ) / 2 );
				this.Xmax = tmpCenter + ( ( this.DeltaY ) / 2 );
			}
		}

		/// <summary>
		/// The upper left point of this rectangle.
		/// </summary>
		public PointD UL
		{
			get
			{
				return _UL;
			}
			set
			{
				_UL = value;
			}
		}
		
		/// <summary>
		/// The lower right point of this rectangle.
		/// </summary>
		public PointD LR
		{
			get
			{
				return _LR;
			}
			set
			{
				_LR = value;
			}
		}

		/// <summary>
		/// The distance covered by this rectangle on the X plane.
		/// </summary>
		public double DeltaX
		{
			get
			{
				return _LR.X - _UL.X;
			}
		}

		/// <summary>
		/// The distance covered by this rectangle on the Y plane.
		/// </summary>
		public double DeltaY
		{
			get
			{
				return _UL.Y - _LR.Y;
			}
		}

		/// <summary>
		/// The Minimum value on the X plane.
		/// </summary>
		public double Xmin
		{
			get
			{
				return _UL.X;
			}
			set
			{
				_UL.X = value;
			}
		}

		/// <summary>
		/// The Maximum value on the X plane.
		/// </summary>
		public double Xmax
		{
			get
			{
				return _LR.X;
			}
			set
			{
				_LR.X = value;
			}
		}

		/// <summary>
		/// The Minimum value on the Y plane.
		/// </summary>
		public double Ymin
		{
			get
			{
				return _LR.Y;
			}
			set
			{
				_LR.Y = value;
			}
		}
		
		/// <summary>
		/// The Maximum value on the Y plane.
		/// </summary>
		public double Ymax
		{
			get
			{
				return _UL.Y;
			}
			set
			{
				_UL.Y = value;
			}
		}
	}
}