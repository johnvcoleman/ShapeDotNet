//
//Map.cs
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
	/// This is the core of the ShapeDotNet project.  The map class represnts
	/// the entire collection of individual map layers and is responsible for
	/// keeping track of the layers, drawing the appropriate portion of the 
	/// map to a bitmap, and handling queries.
	/// </summary>
	public class Map : System.Collections.CollectionBase
	{
		MapMetrics _mapMetrics = new MapMetrics();

		public Map()
		{
			//Just a placeholder until the first time Draw() is called.
			_mapMetrics.Canvas = new Bitmap( 10, 10 );
		}

		public Map( int width, int height )
		{
			_mapMetrics.Canvas = new Bitmap( width, height );
		}

/******************  Collection Implementation  ******************************/


		/// <summary>
		/// Returns the layer located at index in the map collection.
		/// </summary>
		/// @param index the index of the layer to return
		public IMapLayer this[ int index ]  
		{
			get  
			{
				return( (IMapLayer) List[index] );
			}
		}

		/// <summary>
		/// Add a new layer to the map.
		/// </summary>
		/// @param filename the filename of the layer to add
		/// @returns the index of the newly added layer
		public int Add( string filename )  
		{
			//TODO: Need file type detection here
			IMapLayer ml = new ShapefileReader.Shapefile( filename, _mapMetrics );
			List.Insert(0, ml); //always add the new layer to the "top"

			CalculateExtents();

			return 0;
		}

		/// <summary>
		/// Insert a IMaplayer object into the map collection at index.
		/// </summary>
		/// @param index the index to insert the layer at
		/// @param ml the layer to insert
		public void Insert( int index, IMapLayer ml )
		{
			List.Insert( index, ml );
			CalculateExtents();
		}
		
		/// <summary>
		/// Remove a layer from the map collection.
		/// </summary>
		/// @param index the index of the layer to remove
		public void Remove( int index )
		{
			List.RemoveAt( index );
			CalculateExtents();
		}



/******************  End Collection Implementation  **************************/

		
		/// <summary>
		/// Calculates the cumulative extents of all of the layers in the map.
		/// </summary>
		private void CalculateExtents()
		{
			if ( List.Count == 0 )
			{
				_mapMetrics.Extents.Xmin = 0d;
				_mapMetrics.Extents.Ymin = 0d;
				_mapMetrics.Extents.Xmax = 0d;
				_mapMetrics.Extents.Ymax = 0d;

				_mapMetrics.Viewport.Xmin = 0d;
				_mapMetrics.Viewport.Ymin = 0d;
				_mapMetrics.Viewport.Xmax = 0d;
				_mapMetrics.Viewport.Ymax = 0d;
				
				_mapMetrics.UpdateMetrics();
			}
			else
			{
			
				_mapMetrics.Extents.Xmin = Double.MaxValue;
				_mapMetrics.Extents.Ymin = Double.MaxValue;
				_mapMetrics.Extents.Xmax = Double.MinValue;
				_mapMetrics.Extents.Ymax = Double.MinValue;

			
				foreach ( IMapLayer layer in List )
				{
					if  ( layer.Extents.Xmin < _mapMetrics.Extents.Xmin )
					{
						_mapMetrics.Extents.Xmin = layer.Extents.Xmin;
					}
					if  ( layer.Extents.Ymin < _mapMetrics.Extents.Ymin )
					{
						_mapMetrics.Extents.Ymin = layer.Extents.Ymin;
					}

					if  ( layer.Extents.Xmax > _mapMetrics.Extents.Xmax )
					{
						_mapMetrics.Extents.Xmax = layer.Extents.Xmax;
					}
					if  ( layer.Extents.Ymax > _mapMetrics.Extents.Ymax )
					{
						_mapMetrics.Extents.Ymax = layer.Extents.Ymax;
					}
				}
			}

			// Initialize the viewport if it hasn't been already
			if ( _mapMetrics.Viewport.DeltaX == 0 &&
				 _mapMetrics.Viewport.DeltaY == 0 )
			{
				_mapMetrics.Extents.FixGeoAspect();

				_mapMetrics.Viewport.Xmin = _mapMetrics.Extents.Xmin;
				_mapMetrics.Viewport.Ymin = _mapMetrics.Extents.Ymin;
				_mapMetrics.Viewport.Xmax = _mapMetrics.Extents.Xmax;
				_mapMetrics.Viewport.Ymax = _mapMetrics.Extents.Ymax;
			}

			_mapMetrics.Extents.FixGeoAspect();
		}
		
		/// <summary>
		/// Move the local x,y to the center of the viewport.
		/// </summary>
		/// @param x Local X coordinate.
		/// @param y Local Y coordinate.
		public void Center( int x, int y )
		{
			double dx = (double) x;
			double dy = (double) y;
			
			_mapMetrics.Pixel2World( ref dx, ref dy );
			Center( dx, dy );
		}
		
		/// <summary>
		/// Move the geographic x,y to the center of the viewport.
		/// </summary>
		/// @param x Geographic X coordinate.
		/// @param y Geographic Y coordinate.
		public void Center( double x, double y )
		{
			double halfdeltax;
			double halfdeltay;
			
			halfdeltax = _mapMetrics.Viewport.DeltaX / 2;
			halfdeltay = _mapMetrics.Viewport.DeltaY / 2;
			
			_mapMetrics.Viewport.Xmin = x - halfdeltax;
			_mapMetrics.Viewport.Xmax = x + halfdeltax;
			
			_mapMetrics.Viewport.Ymin = y - halfdeltay;
			_mapMetrics.Viewport.Ymax = y + halfdeltay;
		}

		/// <summary>
		/// Drag the map "under" the viewport the specified distance
		/// calculated from the start point and end point.
		/// </summary>
		/// @param startx Starting local X coordinate.
		/// @param starty Starting local Y coordinate.
		/// @param endx Ending local X coordinate.
		/// @param endy Ending local Y coordinate.
		public void Drag( int startx, int starty, int endx, int endy )
		{
			double dstartx	= (double) startx;
			double dstarty	= (double) starty;
			double dendx	= (double) endx;
			double dendy	= (double) endy;
			double ddeltax	= 0;
			double ddeltay	= 0;
			
			_mapMetrics.Pixel2World( ref dstartx, ref dstarty );
			_mapMetrics.Pixel2World( ref dendx, ref dendy );
			
			ddeltax = dstartx - dendx;
			ddeltay = dstarty - dendy;
			
			if ( ddeltax != 0 || ddeltay != 0 )
			{
				_mapMetrics.Viewport.Xmin += ddeltax;
				_mapMetrics.Viewport.Xmax += ddeltax;
				_mapMetrics.Viewport.Ymin += ddeltay;
				_mapMetrics.Viewport.Ymax += ddeltay;
			}
		}

		/// <summary>
		/// Drag the map "under" the viewport the specified distance
		/// calculated from the start point and end point.
		/// </summary>
		/// @param startx Starting geographic X coordinate.
		/// @param starty Starting geographic Y coordinate.
		/// @param endx Ending geographic X coordinate.
		/// @param endy Ending geographic Y coordinate.
		public void Drag( double startx, double starty, double endx, double endy )
		{
			double ddeltax = 0;
			double ddeltay = 0;
			
			ddeltax = startx - endx;
			ddeltay = starty - endy;

			if ( ddeltax != 0 || ddeltay != 0 )
			{
				_mapMetrics.Viewport.Xmin += ddeltax;
				_mapMetrics.Viewport.Xmax += ddeltax;
				_mapMetrics.Viewport.Ymin += ddeltay;
				_mapMetrics.Viewport.Ymax += ddeltay;
			}
		}

		/// <summary>
		/// Draw the map to an external bitmap.
		/// </summary>
		/// @param bitmap Bitmap to draw the map on.
		public void Draw( Bitmap bitmap )
		{
			lock( this )
			{
				_mapMetrics.Canvas = bitmap;
				_mapMetrics.UpdateMetrics();
			
				for( int a=List.Count-1; a>-1; --a ) // draw in reverse order
				{
					this[a].Draw();
				}

				for( int a=List.Count-1; a>-1; --a ) // draw in reverse order
				{
					if ( this[a].Labels == true )
					{
						this[a].DrawLabels();
					}
				}

				//ScaleBar sb = new ScaleBar( _mapMetrics );
			}
		}

		/// <summary>
		/// Get the extents of the Map.
		/// </summary>
		RectangleD Extents
		{
			get
			{
				return _mapMetrics.Extents;
			}
		}

		/// <summary>
		/// Set the viewport to the specified coordinates.
		/// </summary>
		/// @param sx Local upper-left X coordinate.
		/// @param sy Local upper-left Y coordinate.
		/// @param ex Local lower-right X coordinate.
		/// @param ey Local lower-right Y coordinate.
		public void ZoomArea( int sx, int sy, int ex, int ey )
		{
			double dsx = (double) sx;
			double dsy = (double) sy;
			double dex = (double) ex;
			double dey = (double) ey;

			if ( sx != ex || sy != ey ) // catch a single click
			{
				_mapMetrics.Pixel2World( ref dsx, ref dsy );
				_mapMetrics.Pixel2World( ref dex, ref dey );

				_mapMetrics.Viewport.Xmin = dsx;
				_mapMetrics.Viewport.Xmax = dex;
			
				_mapMetrics.Viewport.Ymin = dey;
				_mapMetrics.Viewport.Ymax = dsy;

				_mapMetrics.Viewport.FixGeoAspect();
			}
		}

		/// <summary>
		/// Set the viewport to the specified coordinates.
		/// </summary>
		/// @param rect Geographic rectangle for the viewport.
		public void ZoomArea( RectangleD rect )
		{
			_mapMetrics.Viewport = rect;
		}

		/// <summary>
		/// Zoom to the extents of the layer.
		/// </summary>
		public void ZoomExtents()
		{
			_mapMetrics.Viewport.Xmin = _mapMetrics.Extents.Xmin;
			_mapMetrics.Viewport.Ymin = _mapMetrics.Extents.Ymin;
			_mapMetrics.Viewport.Xmax = _mapMetrics.Extents.Xmax;
			_mapMetrics.Viewport.Ymax = _mapMetrics.Extents.Ymax;
		}

		/// <summary>
		/// Zoom the viewport in on the current center of the viewport.
		/// </summary>
		public void ZoomIn()
		{
			double quarterdeltax;
			double quarterdeltay;
			
			quarterdeltax = _mapMetrics.Viewport.DeltaX / 4;
			quarterdeltay = _mapMetrics.Viewport.DeltaY / 4;
			
			_mapMetrics.Viewport.Xmin += quarterdeltax;
			_mapMetrics.Viewport.Xmax -= quarterdeltax;
			
			_mapMetrics.Viewport.Ymin += quarterdeltay;
			_mapMetrics.Viewport.Ymax -= quarterdeltay;
		}

		/// <summary>
		/// Zoom the viewport in after centering on x,y. 
		/// </summary>
		/// @param x Local X coordinate.
		/// @param y Local Y coordinate.
		public void ZoomIn( int x, int y )
		{
			Center( x, y );
			ZoomIn();
		}

		/// <summary>
		/// Zoom the viewport in after centering on x,y.
		/// </summary>
		/// @param x Geographic X coordinate.
		/// @param y Geographic Y coordinate.
		public void ZoomIn( double x, double y )
		{
			Center( x, y );
			ZoomIn();
		}

		/// <summary>
		/// Zoom the viewport out on the current center of the viewport.
		/// </summary>
		public void ZoomOut()
		{
			double thirddeltax;
			double thirddeltay;
			
			thirddeltax = _mapMetrics.Viewport.DeltaX / 3;
			thirddeltay = _mapMetrics.Viewport.DeltaY / 3;
			
			_mapMetrics.Viewport.Xmin -= thirddeltax;
			_mapMetrics.Viewport.Xmax += thirddeltax;
			
			_mapMetrics.Viewport.Ymin -= thirddeltay;
			_mapMetrics.Viewport.Ymax += thirddeltay;
			
			//m_LastQueryResult = 0;
		}

		/// <summary>
		/// Zoom the viewport out after centering on x,y.
		/// </summary>
		/// @param x Local X coordinate.
		/// @param y Local Y coordinate.
		public void ZoomOut( int x, int y )
		{
			Center( x, y );
			ZoomOut();
		}

		/// <summary>
		/// Zoom the viewport out after centering on x,y.
		/// </summary>
		/// @param x Geographic X coordinate.
		/// @param y Geographic Y coordinate.
		public void ZoomOut( double x, double y )
		{
			Center( x, y );
			ZoomOut();
		}

		/// <summary>
		/// Get the names of all the layers.
		/// </summary>
		/// @returns A String array containing the name of each layer
		public string[] GetLayerNames()
		{
			string[] names = new string[ List.Count ];

			for ( int a=0; a<List.Count; ++a )
			{
				names[a] = this[a].LayerName;
			}

			return names;
		}

		/// <summary>
		/// Get/Set the GIS.MapMetrics object for this collection of
		/// layers (Map)
		/// </summary>
		/// @returns GIS.MapMetrics
		public MapMetrics Metrics
		{
			get
			{
				return _mapMetrics;
			}
			set
			{
				_mapMetrics = value;
			}
		}
	}
}
