//
//PointD.cs
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
	/// The most basic object in all VectorFeature objects, an X and Y coordinate
	/// of the type Double.  Don't confuse this class with PointF!  PointF belongs
	/// to GDI+.
	/// </summary>
	public class PointD
	{
		private double _X;
		private double _Y;
		
		public PointD()
		{
			_X = 0.0d;
			_Y = 0.0d;
		}

		public PointD( double X, double Y )
		{
			_X = X;
			_Y = Y;
		}

		public PointD( PointD P )
		{
			_X = P.X;
			_Y = P.Y;
		}
		
		/// <summary>
		/// Used to see if the lvalue point is geographically right and below
		/// the rvalue point.
		/// </summary>
		/// @param lvalue the point to test
		/// @param rvalue the point to test against.
		/// @returns true if the lvalue is geographically right and below rvalue
		public static bool operator >( PointD lvalue, PointD rvalue )
		{
			if ( lvalue.X >= rvalue.X &&
				 lvalue.Y <= rvalue.Y )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Used to see if the lvalue point is geographically left and above
		/// the rvalue point.
		/// </summary>
		/// @param lvalue the point to test
		/// @param rvalue the point to test against.
		/// @returns true if the lvalue is geographically left and above rvalue
		public static bool operator <( PointD lvalue, PointD rvalue )
		{
			if ( lvalue.X <= rvalue.X &&
				 lvalue.Y >= rvalue.Y )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Gets/Sets the X coordinate of this point.
		/// </summary>
		public double X
		{
			get
			{
				return _X;
			}
			set
			{
				_X = value;
			}
		}

		/// <summary>
		/// Gets/Sets the Y coordinate of this point.
		/// </summary>
		public double Y
		{
			get
			{
				return _Y;
			}
			set
			{
				_Y = value;
			}
		}
	}
}
