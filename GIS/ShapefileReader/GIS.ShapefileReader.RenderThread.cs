//
//RenderThread.cs
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace ShapeDotNet.GIS.ShapefileReader
{
	/// <summary>
	/// This is where the rubber meets the road. RenderThread is given
	/// a beginning and ending point in the VectorFeature array and it
	/// draws all of the features between those two points to the 
	/// MapMetrics.Canvas.  Of course VectorFeatures that are marked as
	/// not being in view are skipped.
	/// </summary>
	public class RenderThread
	{
		private MapMetrics _mapMetrics;
		private ShapeType _shapeType;
		private VectorFeature[] _features;
		private Graphics _gr;
		private int _beginningFeature;
		private int _endingFeature;
		private Pen _pen;
		private Brush _brush;

		/// <summary>
		/// The constructor for the RenderThread. Sets up the private members
		/// of the class, but DOES NOT start the rendering process.
		/// </summary>
		/// @param metrics The MapMetrics object for this Map
		/// @param features The VectorFeature array
		/// @param type The type of shapefile we're going to draw
		/// @param beginning The array index of the VectorFeature to begin drawing with
		/// @param ending The array index of the VectorFeature to end drawing with
		/// @param pen The pen to draw the features with
		/// @param brush The brush to fill the features with
		public RenderThread( MapMetrics metrics, 
							 VectorFeature[] features,
							 ShapeType type,
							 int beginning,
							 int ending,
							 Pen pen,
							 Brush brush )
		{
			_mapMetrics = metrics;
			_features = features;
			_shapeType = type;
			_beginningFeature = beginning;
			_endingFeature = ending;
			_pen = (Pen) pen.Clone();
			_brush = (Brush) brush.Clone();
			_gr = Graphics.FromImage( _mapMetrics.Canvas );
		}
		
		
		/// <summary>
		/// If the shapefile is a point type, this method is used to
		/// draw the features.
		/// </summary>
		private void DrawPoints() 
		{			
			for ( int a =_beginningFeature; a<_endingFeature; ++a )
			{
				VectorFeature vf = _features[a];

				if ( vf.IsInViewport == false ) continue; // Don't draw this feature

				for ( int segment=0; segment < vf.SegmentCount; ++segment )
				{
					for ( int point=0; point < vf[segment].Length; ++point )
					{
						_gr.FillEllipse( _brush, vf[segment][point].X,
							vf[segment][point].Y, _pen.Width, _pen.Width );

						_gr.DrawEllipse( new Pen( _pen.Color, 1 ), vf[segment][point].X, 
							vf[segment][point].Y, _pen.Width, _pen.Width );
					}
				}
			}
	
			_gr.Dispose();
		}
		
		
		/// <summary>
		/// If the shapefile is a polygon type, this method is used to
		/// draw the features.  A GraphicsPath is used instead of graphics
		/// object so that the "negation" of polygons inside polygons can be 
		/// achieved with a minimum of fuss.  See page 21 in the ESRI
		/// whitepaper for a complete explanation.
		/// </summary>
		private void DrawPolygons() 
		{			
			GraphicsPath gp = new GraphicsPath();

			for ( int a =_beginningFeature; a<_endingFeature; ++a )
			{
				VectorFeature vf = _features[a];

				if ( vf.IsInViewport == false ) continue; // Don't draw this feature

				for ( int segment=0; segment < vf.SegmentCount; ++segment )
				{
					gp.AddPolygon( vf[segment] );
					gp.StartFigure();
				}

				gp.CloseFigure();
			}
			
			_gr.FillPath( _brush, gp );
			_gr.DrawPath( _pen, gp );

			gp.Dispose();
			_gr.Dispose();
		}

		
		/// <summary>
		/// If the shapefile is a polyline type, this method is used to
		/// draw the features.
		/// </summary>
		private void DrawPolyLines() 
		{			
			for ( int a =_beginningFeature; a<_endingFeature; ++a )
			{
				VectorFeature vf = _features[a];

				if ( vf.IsInViewport == false ) continue; // Don't draw this feature

				for ( int segment=0; segment < vf.SegmentCount; ++segment )
				{
					_gr.DrawLines( _pen, vf[segment] );
				}
			}

			_gr.Dispose();
		}
		
		
		/// <summary>
		/// If the shapefile is a multipoint type, this method is used to
		/// draw the features. Multipoint types have not been well tested!
		/// </summary>
		private void DrawMultiPoints() 
		{
			for ( int a =_beginningFeature; a<_endingFeature; ++a )
			{
				VectorFeature vf = _features[a];

				if ( vf.IsInViewport == false ) continue; // Don't draw this feature

				for ( int segment=0; segment < vf.SegmentCount; ++segment )
				{
					for ( int point=0; point < vf[segment].Length; ++point )
					{
						_gr.FillEllipse( _brush, vf[segment][point].X,
							vf[segment][point].Y, _pen.Width *3, _pen.Width *3 );

						_gr.DrawEllipse( new Pen( _pen.Color, 1 ), vf[segment][point].X, 
							vf[segment][point].Y, _pen.Width, _pen.Width );
					}
				}
			}
	
			_gr.Dispose();
		}

		
		/// <summary>
		/// The method that gets called when the Thread.start() is called.
		/// </summary>
		public void Start()
		{
			switch ( _shapeType ) 
			{
				case ShapeType.Point:
					DrawPoints();
					break;
				case ShapeType.Line:
					DrawPolyLines();
					break;
				case ShapeType.Polygon:
					DrawPolygons();
					break;
				case ShapeType.Multipoint:
					DrawMultiPoints();
					break;
				default:
					Console.WriteLine("\n\t\t**Danger Will Robinson!!**\n");
					Console.WriteLine("\t\tUnsupported Shape Type (Type: " + 
						_shapeType + ")");
					break;
			}
		}
	}
}


/*                    *** Code parking lot ***
 

					-> from DrawPolyLines() <-
					for ( int point=0; point < vf[segment].Length - 1; ++point )
					{
						_gr.DrawLine( new Pen( Color.Black ), 
							vf[segment][point].X, vf[segment][point].Y,	
							vf[segment][point+1].X, vf[segment][point+1].Y );
					}
					
*/