//
//Shapefile.cs
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

using ShapeDotNet.GIS.DbaseReader;

namespace ShapeDotNet.GIS.ShapefileReader
{
	
	
	
	/// <summary>
	/// Shapefile inherits from ShapeIndex and implements the IMapLayer
	/// interface. The handling of layers in this way enables us to 
	/// easily add new file type readers in the future.  See IMapLayer
	/// for more detailed information on what each method *must* do.
	/// </summary>
	public class Shapefile : ShapeIndex, IMapLayer 
	{
		protected Dbase _db;
		protected Pen _pen;
		protected SolidBrush _brush;
		protected bool _labels;
		protected int _labelSourceField;
		protected Color _labelForeColor;
		protected Color _labelFillColor;
		protected Font _labelFont;
		protected bool _labelBackground;
		protected int _highlightedFeature;
		protected string _layerName;
		
		private const int NUM_THREADS = 8;
		private RenderThread[] drawThreads;
		private Thread[] threads;
		private int threadBoundary;
		
		
		
		/// <summary>
		/// The constructor for the Shapefile class initializes the base class
		/// ShapeIndex and sets up the protected members of the class needed
		/// for implementing the IMapLayer interface.
		/// </summary>
		/// @param filename The filename the shapefile we want to load.
		/// @param metrics The MapMetrics object for this Map.
		public Shapefile( string filename, MapMetrics metrics ) : base( filename, metrics )
		{
			_db					= new Dbase( filename );
			_pen				= new Pen( Color.Black  );
			_brush				= new SolidBrush( Color.Tan );
			_labelSourceField	= 0;
			_labelForeColor		= Color.Black;
			_labelFillColor		= Color.White;
			_labelFont			= new Font( "Tahoma", 8 );
			_labelBackground	= true;
			_highlightedFeature	= -1;
			_layerName			= _Filename;

			drawThreads			= new RenderThread[ NUM_THREADS ];
			threads				= new Thread[ NUM_THREADS ];
			threadBoundary		= _features.Length / NUM_THREADS; // integer division, I know.
		}

		
		
		/// <summary>
		/// Calculates how many features each RenderThread will draw. Then each
		/// rendering thread is started and we wait for all of the threads to finish 
		/// before continuing on.
		/// </summary>
		public void Draw()
		{
			GenerateDrawList();
							
			for ( int a=0; a<NUM_THREADS; ++a )
			{
				if ( a == NUM_THREADS - 1 )
				{
					drawThreads[ a ] = new RenderThread( _mapMetrics, _features,
						_ShapeType, a * threadBoundary, _features.Length, _pen, _brush );
				}
				else
				{
					drawThreads[ a ] = new RenderThread( _mapMetrics, _features, 
						_ShapeType, a * threadBoundary, (a + 1) * threadBoundary, _pen, _brush );
				}

				threads[ a ] = new Thread( new ThreadStart( drawThreads[a].Start ) );
				threads[ a ].Start();
			}

			for ( int b=0; b<NUM_THREADS; ++b )
			{
				threads[b].Join();
			}

			// if a feature has been highlighted with a query
			
			if ( _highlightedFeature != -1 )
			{
				this.Highlight( _highlightedFeature, SystemColors.Highlight );
			}
		}



		/// <summary>
		/// Using the LabelSourceField property of this class, we label 
		/// each VectorFeature that is in view with the data contained
		/// in that field of the .dbf record.
		/// </summary>
		public void DrawLabels()
		{
			Graphics gr = Graphics.FromImage( _mapMetrics.Canvas );
			gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			ArrayList labelList = new ArrayList();

			
			if ( _ShapeType == ShapeType.Point )
			{					
				int posx;
				int posy;
				bool drawLabel;
				string tempLabel;

				for ( int a=0; a<_features.Length; a++ )
				{
					if ( _features[a].IsInViewport == false ) continue;
					
					drawLabel = true;
					tempLabel = _db[a][_labelSourceField];
					tempLabel.Trim();

					SizeF fontSize = gr.MeasureString( tempLabel, _labelFont );
					
					posx = (int)((_features[a][0][0].X + (_pen.Width /2)) - ( fontSize.Width / 2 ));
					posy = (int)(_features[a][0][0].Y - ( fontSize.Height ));

					Rectangle labelArea = new Rectangle( posx, posy,
						(int) fontSize.Width, (int) fontSize.Height );

					gr.ResetTransform();
					gr.TranslateTransform( (float) posx, (float) posy );

					labelArea.Inflate( 2, 2 );
					
					foreach ( Rectangle ll in labelList )
					{
						if ( ll.IntersectsWith( labelArea ) ) drawLabel = false;
					}
					
					if ( drawLabel == false ) 
					{
						continue;
					}
					else
					{
						labelList.Add( labelArea );
					}

					if ( _labelBackground == true && !tempLabel.StartsWith(" ") )
					{
						gr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, 
							fontSize.Width, fontSize.Height );
						gr.DrawRectangle( new Pen( _labelForeColor, 1f ), 0, 0, 
							fontSize.Width, fontSize.Height );
					}
				
					gr.DrawString( tempLabel, _labelFont, 
						new SolidBrush( _labelForeColor ), 0, 0 );
				}
			}


			if ( _ShapeType == ShapeType.Polygon ||
				  _ShapeType == ShapeType.Multipoint )
			{
				double centerx;
				double centery;
				bool drawLabel;
				string tempLabel;

				for ( int a=0; a<_features.Length; a++ )
				{
					if ( _features[a].IsInViewport == false ) continue;
					
					drawLabel = true;
					tempLabel = _db[a][_labelSourceField];
					tempLabel.Trim();

					SizeF fontSize = gr.MeasureString( tempLabel, _labelFont );
				
					centerx = _features[a].LabelAnchor.X;
					centery = _features[a].LabelAnchor.Y;
			
					_mapMetrics.World2Pixel( ref centerx, ref centery );

					centerx = (int)( centerx - ( fontSize.Width / 2 ));
					centery = (int)( centery - ( fontSize.Height / 2 ));

					Rectangle labelArea = new Rectangle( (int)centerx, (int)centery,
						(int)fontSize.Width, (int)fontSize.Height );

					gr.ResetTransform();
					gr.TranslateTransform( (float) centerx, (float) centery );

					labelArea.Inflate( 2, 2 );

					foreach ( Rectangle ll in labelList )
					{
						if ( ll.IntersectsWith( labelArea ) ) drawLabel = false;
					}
					
					if ( drawLabel == false ) 
					{
						continue;
					}
					else
					{
						labelList.Add( labelArea );
					}

					if ( _labelBackground == true && !tempLabel.StartsWith(" ") )
					{
						gr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, 
							fontSize.Width, fontSize.Height );
						gr.DrawRectangle( new Pen( _labelForeColor, 1f ), 0, 0, 
							fontSize.Width, fontSize.Height );
					}
			
					gr.DrawString( tempLabel, _labelFont, 
						new SolidBrush( _labelForeColor ), 0, 0 );
				}
			}

			
			if ( _ShapeType == ShapeType.Line )
			{
				double centerx;
				double centery;
				bool drawLabel;
				string tempLabel;
				
				for ( int a=0; a<_features.Length; a++ )
				{
					if ( _features[a].IsInViewport == false ) continue;
					
					drawLabel = true;
					tempLabel = _db[a][_labelSourceField];
					tempLabel.Trim();

					SizeF fontSize = gr.MeasureString( tempLabel, _labelFont );

					centerx = _features[a].LabelAnchor.X;
					centery = _features[a].LabelAnchor.Y;
			
					_mapMetrics.World2Pixel( ref centerx, ref centery );

					Rectangle labelArea = new Rectangle( (int)(centerx -fontSize.Width / 2), (int)(centery -fontSize.Width / 2),
						(int)fontSize.Width, (int)fontSize.Height );

					gr.ResetTransform();

					gr.TranslateTransform( (float)centerx, (float)centery );
					gr.RotateTransform( _features[a].FeatureAngle );
					gr.TranslateTransform( -fontSize.Width / 2, fontSize.Height * .20f );
					
					

					labelArea.Inflate( 10, 20 );
					
					foreach ( Rectangle ll in labelList )
					{
						if ( labelArea.IntersectsWith( ll ) ) 
						{
							drawLabel = false;
						}							
					}
					
					if ( drawLabel == false ) 
					{
						continue;
					}
					else
					{
						labelList.Add( labelArea );
					}

					if ( _labelBackground == true && !tempLabel.StartsWith(" ") )
					{
						gr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, 
							fontSize.Width, fontSize.Height );
						gr.DrawRectangle( new Pen( _labelForeColor, 1f ), 0, 0, 
							fontSize.Width, fontSize.Height );
					}

					gr.DrawString( tempLabel, _labelFont, 
						new SolidBrush( _labelForeColor ), 0, 0 );
				}			
			}                
				gr.Dispose();
		}



		/// <summary>
		/// Determine if a given feature in the VectorFeature array is currently
		/// in view and set its IsInViewport flag.
		/// </summary>
		private void GenerateDrawList()
		{				
			//If the viewport = extents of the map, we're drawing everything...
			if ( _mapMetrics.Viewport.Xmin == _mapMetrics.Extents.Xmin &&
				_mapMetrics.Viewport.Ymin == _mapMetrics.Extents.Ymin &&
				_mapMetrics.Viewport.Xmax == _mapMetrics.Extents.Xmax &&
				_mapMetrics.Viewport.Ymax == _mapMetrics.Extents.Ymax )
			{
				for ( int a=0; a<_FeatureCount; ++a )
				{
					_features[ a ].IsInViewport = true;
				}

				return;
			}
			
			if ( _ShapeType == ShapeType.Point )
			{
				for ( int a=0; a < _FeatureCount; ++a )
				{
					_features[ a ].IsInViewport = 
						_mapMetrics.Viewport.Contains( _features[ a ].GetPoint( 0, 0 ) );
				}
			}
			else
			{			
				for ( int a=0; a < _FeatureCount; ++a )
				{
					_features[ a ].IsInViewport = 
						_mapMetrics.Viewport.Intersects( _features[ a ].Extents );
				}
			}
		}


		
		/// <summary>
		/// Highlight a geographic object.
		/// </summary>
		/// @param record_number Array index of the record to be highlighted.
		/// @param c The color to use to highlight the record.
		private void Highlight(int record_number, Color c)
		{
			RenderThread drawThread;
			Pen tempPen = (Pen) _pen.Clone();
			tempPen.Color = c;
			
			if ( _ShapeType == ShapeType.Line )
			{
				tempPen.Width += 2;
				tempPen.StartCap = LineCap.RoundAnchor;
				tempPen.EndCap = LineCap.RoundAnchor;
			}

			HatchBrush tempBrush = new HatchBrush( HatchStyle.Percent70, c, Color.Transparent );

			drawThread = new RenderThread( _mapMetrics, _features,
				_ShapeType, record_number, record_number + 1, tempPen, tempBrush );

			drawThread.Start();
			
			tempPen.Dispose();
			tempBrush.Dispose();
		}

		
				
		/// <summary>
		/// Return the name/value associated with the geographic object
		/// located at the local x,y coordinate.
		/// </summary>
		/// @param x Local X coordinate.
		/// @param y Local Y coordinate.
		/// @returns A string array of attributes represented by the object at x,y
		public string[] QueryPoint( int x, int y )
		{
			int closestRecord = 0;
			double closestDistance = Double.MaxValue;
			double testDistance = 0;
			double dx = (double) x;
			double dy = (double) y;

			_mapMetrics.Pixel2World( ref dx, ref dy );
			
			for( int a=0; a<_FeatureCount; ++a )
			{
				if( !_features[a].IsInViewport ) continue;
				
				testDistance = _features[a].DistanceFromFeature( new PointD( dx, dy ) );
				
				if( testDistance < closestDistance )
				{
					closestDistance = testDistance;
					closestRecord = a;
				}
			}
			
			_highlightedFeature = closestRecord;
			return _db[ closestRecord ];
		}
		
		
				
		/// <summary>
		/// Returns the name of the shapefile currently in use.
		/// </summary>
		/// <returns></returns>
		public string LayerName
		{
			get
			{
				return _layerName;
			}

			set
			{
				_layerName = value;
			}
		}
		


		/// <summary>
		/// Boolean that determines if the labels will be drawn.
		/// </summary>
		public bool Labels
		{
			get
			{
				return _labels;
			}

			set
			{
				_labels = value;
			}
		}


		/// <summary>
		/// Boolean that determines if we draw the background of the label.
		/// </summary>
		public bool LabelBackground
		{
			get
			{
				return _labelBackground;
			}

			set
			{
				_labelBackground = value;
			}
		}


		/// <summary>
		/// The font to use for the labels.
		/// </summary>
		public Font LabelFont
		{
			get
			{
				return _labelFont;
			}
			
			set
			{
				_labelFont = value;
			}
		}

	
		/// <summary>
		/// The fore color of the labels ( font AND background border)
		/// </summary>
		public Color LabelForeColor
		{
			get
			{
				return _labelForeColor;
			}

			set
			{
				_labelForeColor = value;
			}
		}


		/// <summary>
		/// The backgorund color of the label (only if LabelBackground == true)
		/// </summary>
		public Color LabelFillColor
		{
			get
			{
				return _labelFillColor;
			}

			set
			{
				_labelFillColor = value;
			}
		}


		/// <summary>
		/// The source field of the .dbf file that is used to retreive the label data.
		/// </summary>
		public int LabelSourceField
		{
			get
			{
				return _labelSourceField;
			}

			set
			{
				_labelSourceField = value;
			}
		}


		/// <summary>
		/// Get the extents of the layer.
		/// </summary>
		public RectangleD Extents
		{
			get
			{
				return _extents;
			}
		}
		

		/// <summary>
		/// Get the human readable names of the fields in the database.
		/// </summary>
		public string[] FieldNames
		{
			get
			{
				return _db.FieldNames;
			}
		}

		
		/// <summary>
		/// Returns the basic GIS.LayerType of the layer. ( bitmap or vector )
		/// </summary>
		/// <returns></returns>
		public LayerType DrawingType
		{
			get
			{
				return LayerType.Vector;
			}
		}
		
		
		/// <summary>
		/// Returns the GIS.LayerSubType of the layer. ( Specific file format )
		/// </summary>
		/// <returns></returns>
		public LayerSubType DrawingSubType
		{
			get
			{
				return LayerSubType.Shapefile;
			}
		}

		
		/// <summary>
		/// The pen used for drawing this layer.
		/// </summary>
		public Pen LayerPen
		{
			get
			{
				return _pen;
			}
			set
			{
				_pen = value;
			}
		}
		
		
		/// <summary>
		/// The brush used for drawing this layer.
		/// </summary>
		public SolidBrush LayerBrush
		{
			get
			{
				return _brush;
			}
			set
			{
				_brush = value;
			}
		}

		
		/// <summary>
		/// The index number of the feature to highlight.
		/// </summary>
		public int HighlightedFeature
		{
			get
			{
				return _highlightedFeature;
			}
			set
			{
				_highlightedFeature = value;
			}
		}
	}
}
