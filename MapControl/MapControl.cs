//
//MapControl.cs
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Threading;

using ShapeDotNet.GIS;

namespace ShapeDotNet
{
	public delegate void QueryResultEventHandler( object sender, string[] result );

	/// <summary>
	/// Determines the behavior of the pointer (mouse) within the map.
	/// </summary>
	public enum PointerMode { Select, Zoom, ZoomArea, Drag, Center, Query };

	/// <summary>
	/// The Windows Forms mapping control. This is the GUI front end of
	/// the ShapeDotNet project that interacts with the backend GIS 
	/// namespace. MapControl is a composite control consisting of a
	/// PictureBox drawn over the top of the actual UserControl.  All 
	/// user interaction is with the PictureBox not the UserControl per se.
	/// </summary>
	[ ToolboxBitmap( typeof( ShapeDotNet.MapControl ), "SDN.bmp" ),
	  Description("Shape.Net Map Control") ] 
	public class MapControl : System.Windows.Forms.UserControl
	{	
		/// <summary>
		/// The event the MapControl raises when the result of a query is
		/// ready to be returned.
		/// </summary>
		public event QueryResultEventHandler OnQueryResult;

		protected Brush _DragBrush;
		protected Pen _DragPen;
		protected Graphics _Gr;
		protected Map _map;
		protected double _MouseGeoX;
		protected double _MouseGeoY;
		protected int _MouseActionBeginX;
		protected int _MouseActionBeginY;
		protected Point _MouseActionBeginScreen;
		protected bool _ButtonDown;
		protected PointerMode _PointerMode;
		protected int _queryLayer;
		
		private System.Windows.Forms.PictureBox Canvas;
		private System.Windows.Forms.ToolTip queryTip;
		private System.Windows.Forms.SaveFileDialog saveBitmapDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.ComponentModel.IContainer components;

		public MapControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			_map = new Map();
			
			this.ResizeRedraw = true;
			Canvas.Size = this.Size;
			
			_DragPen = new Pen( SystemColors.Highlight, 2f );
			_DragPen.EndCap = LineCap.SquareAnchor;
			_DragPen.StartCap = LineCap.SquareAnchor;
			_DragBrush = new SolidBrush( Color.FromArgb( 64, SystemColors.Highlight ) );
			_queryLayer = -1;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Canvas = new System.Windows.Forms.PictureBox();
			this.queryTip = new System.Windows.Forms.ToolTip(this.components);
			this.saveBitmapDialog = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// Canvas
			// 
			this.Canvas.BackColor = System.Drawing.Color.Transparent;
			this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Canvas.Name = "Canvas";
			this.Canvas.Size = new System.Drawing.Size(136, 128);
			this.Canvas.TabIndex = 0;
			this.Canvas.TabStop = false;
			this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
			this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
			this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
			// 
			// queryTip
			// 
			this.queryTip.Active = false;
			// 
			// saveBitmapDialog
			// 
			this.saveBitmapDialog.FileName = "map";
			this.saveBitmapDialog.Filter = "Windows Bitmap (*.bmp)|*.bmp|Portable Network Graphics (*.png)|*.png|Graphics Int" +
				"erchange Format (*.gif)|*.gif|Tagged Image File Format (*.tiff)|*.tiff|Enhanced " +
				"Windows Metafile (*.emf)|*.emf";
			this.saveBitmapDialog.Title = "Save map to...";
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "shp";
			this.openFileDialog.Filter = "ESRI Shapefiles (*.shp)|*.shp|All files (*.*)|*.*";
			this.openFileDialog.Title = "Open Shapefile...";
			// 
			// MapControl
			// 
			this.BackColor = System.Drawing.Color.RoyalBlue;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Canvas});
			this.Name = "MapControl";
			this.Resize += new System.EventHandler(this.MapControl_Resize);
			this.Load += new System.EventHandler(this.MapControl_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Executes when the control is loaded in a design-time enviroment.
		/// </summary>
		/// @param sender
		/// @param e
		private void MapControl_Load(object sender, System.EventArgs e)
		{
			Canvas.Size = this.Size;
		}
		
		/// <summary>
		/// Keep the Canvas (PictureBox) in sync with the UserControl.
		/// </summary>
		/// @param sender
		/// @param e
		private void MapControl_Resize(object sender, System.EventArgs e)
		{
			//The resize event gets fired on minimize.  Logical, but a pain.
			if( this.ParentForm != null && 
				this.ParentForm.WindowState != FormWindowState.Minimized )
			{
				Canvas.Size = this.Size;
				Draw();
			}
		}

		/// <summary>
		/// Catch the PictureBox.MouseDown event, process the map according to the current
		/// PointerMode and then raise the MouseDown event for the developer
		/// to catch in their app.
		/// </summary>
		/// @param sender
		/// @param e
		private void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch( _PointerMode )
			{
				case PointerMode.Select:
					break;
			
				case PointerMode.Zoom:
					break;

				case PointerMode.ZoomArea:
					_Gr = Canvas.CreateGraphics();
					_MouseActionBeginScreen = Canvas.PointToScreen( new Point( e.X, e.Y ) );
					_MouseActionBeginX = e.X;
					_MouseActionBeginY = e.Y;
					_ButtonDown = true;
					break;
			
				case PointerMode.Drag:
					_Gr = Canvas.CreateGraphics();
					_MouseActionBeginScreen = Canvas.PointToScreen( new Point( e.X, e.Y ) );
					_MouseActionBeginX = e.X;
					_MouseActionBeginY = e.Y;
					_ButtonDown = true;
					break;
			
				case PointerMode.Center:
					_map.Center( e.X, e.Y );
					Draw();
					break;

				case PointerMode.Query:
					break;
			}
			
			OnMouseDown( e );
		}

		/// <summary>
		/// Catch the PictureBox.MouseMove event, process the map according to the current
		/// PointerMode and then raise the MouseMove event for the developer
		/// to catch in their app.
		/// </summary>
		/// @param sender
		/// @param e
		private void Canvas_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{			
			double tempx = (double) e.X;
			double tempy = (double) e.Y;

			_map.Metrics.Pixel2World( ref tempx, ref tempy );
			
			_MouseGeoX = tempx;
			_MouseGeoY = tempy;
		
			switch( _PointerMode )
			{
				case PointerMode.Select:
					break;
		
				case PointerMode.Zoom:
					break;

				case PointerMode.ZoomArea:
					if ( _ButtonDown == true )
					{	
						Canvas.Refresh();
						
						int zwidth = Math.Abs( e.X - _MouseActionBeginX );
						int zheight = Math.Abs( e.Y - _MouseActionBeginY );

						_Gr.DrawRectangle( _DragPen, _MouseActionBeginX - zwidth, 
							_MouseActionBeginY - zheight, zwidth *2, zheight *2 );

						_Gr.FillRectangle( _DragBrush, _MouseActionBeginX - zwidth, 
							_MouseActionBeginY - zheight, zwidth *2, zheight *2 );
			
						_Gr.DrawRectangle( Pens.White, _MouseActionBeginX - zwidth +1, 
							_MouseActionBeginY - zheight +1, zwidth *2 -2, zheight *2 -2 );
					}
					break;
		
				case PointerMode.Drag:
					if ( _ButtonDown == true )
					{
						Canvas.Refresh();
						
						_Gr.DrawLine( new Pen( Color.White, 4f ), _MouseActionBeginX, _MouseActionBeginY,
							e.X, e.Y );
						_Gr.DrawEllipse( Pens.White, _MouseActionBeginX -5, _MouseActionBeginY -5,
							10, 10 );
						_Gr.DrawEllipse( Pens.White, e.X -5, e.Y -5,
							10, 10 );

						_Gr.DrawLine( _DragPen, _MouseActionBeginX, _MouseActionBeginY,
							e.X, e.Y );
						_Gr.FillEllipse( new SolidBrush( _DragPen.Color ), _MouseActionBeginX -5, _MouseActionBeginY -5,
							10, 10 );									
						_Gr.FillEllipse( new SolidBrush( _DragPen.Color ), e.X -5, e.Y -5,
							10, 10 );
					}
					break;
		
				case PointerMode.Center:
					break;

				case PointerMode.Query:
					break;
			}
			
			OnMouseMove( e );
		}
		
		/// <summary>
		/// Catch the PictureBox.MouseUp event, process the map according to the current
		/// PointerMode and then raise the MouseUp event for the developer
		/// to catch in their app.
		/// </summary>
		/// @param sender
		/// @param e
		private void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_ButtonDown = false;
			
			switch( _PointerMode )
			{
				case PointerMode.Select:
					break;
			
				case PointerMode.Zoom:					
					if ( e.Button == MouseButtons.Left )
					{
						_map.ZoomIn( e.X, e.Y );
						Draw();
					} 
					else 
					{
						_map.ZoomOut( e.X, e.Y );
						Draw();
					}
					
					break;
			
				case PointerMode.ZoomArea:
					_ButtonDown = false;
					int zwidth = Math.Abs( e.X - _MouseActionBeginX );
					int zheight = Math.Abs( e.Y - _MouseActionBeginY );
					int zleft = _MouseActionBeginX - zwidth;
					int ztop = _MouseActionBeginY - zheight;

					_map.ZoomArea( zleft, ztop, zleft + (zwidth *2), ztop + (zheight *2) );
					_Gr.Dispose();
					Draw();
					break;

				case PointerMode.Drag:
					_ButtonDown = false;
					_map.Drag( _MouseActionBeginX, _MouseActionBeginY, e.X, e.Y );
					_Gr.Dispose();
					Draw();
					break;
			
				case PointerMode.Center:
					break;
			
				case PointerMode.Query:
					if ( e.Button == MouseButtons.Left )
					{
						Query( e.X, e.Y );
					} 
					break;
			}

			OnMouseUp( e );
		}
		
		/// <summary>
		/// Force the map to be redrawn.
		/// </summary>
		public void Draw()
		{
			this.Cursor = Cursors.WaitCursor;

			if ( _map.Count > 0 )
			{					
				Bitmap bmp = new Bitmap( Canvas.Width, Canvas.Height );				
				
				_map.Draw( bmp );				
				Canvas.Image = bmp;
			}
			else
			{
				Bitmap bmp = new Bitmap( Canvas.Width, Canvas.Height );
				
				Canvas.Image = bmp;
				Canvas.Refresh();
			}

			this.Cursor = Cursors.Default;
		}
		
		/// <summary>
		/// Add a new layer to the map.  Overloaded.  If you pass it a filename,
		/// that file will be loaded.  Otherwise, a file selection box will 
		/// be presented to the user for them to select a file to load.
		/// </summary>
		public void AddLayer()
		{
			if ( openFileDialog.ShowDialog() == DialogResult.OK ) 
			{				
				this.Cursor = Cursors.AppStarting;
				_map.Add( openFileDialog.FileName );
				this.Cursor = Cursors.Default;				
				Draw();
			}
		}

		/// <summary>
		/// Add a new layer to the map.  Overloaded.  If you pass it a filename,
		/// that file will be loaded.  Otherwise, a file selection box will 
		/// be presented to the user for them to select a file to load.
		/// </summary>
		public void AddLayer( string filename )
		{			
			this.Cursor = Cursors.AppStarting;
			_map.Add( filename );
			this.Cursor = Cursors.Default;				
			Draw();
		}

		/// <summary>
		/// Remove a layer from the map.
		/// </summary>
		/// @param index the index of the layer to remove
		public void RemoveLayer( int index )
		{
			if ( index > -1 && index < _map.Count )
			{
				_map.Remove( index );
				Draw();
			}
		}

		/// <summary>
		/// Move a layer "up" in the drawing order.
		/// </summary>
		/// @param index the index of the layer to move "up"
		public void MoveLayerUp( int index )
		{
			if ( _map.Count > 0 && index != 0 )
			{
				IMapLayer tmpLayer = _map[ index ];
				_map.Insert( index - 1, tmpLayer );
				_map.Remove( index + 1 );
				Draw();
			}
		}

		/// <summary>
		/// Move a layer "down" in the drawing order.
		/// </summary>
		/// @param index the index of the layer to move "down"
		public void MoveLayerDown( int index )
		{
			if ( _map.Count > 0 && index != _map.Count - 1)
			{
				IMapLayer tmpLayer = _map[ index ];
				_map.Insert( index + 2, tmpLayer );
				_map.Remove( index );
				Draw();
			}
		}
		
		/// <summary>
		/// Save the current map image to file. Presents a file selection dialog
		/// to the user for them to select the file name and image type.
		/// </summary>
		public void SaveViewToImage()
		{
			if ( saveBitmapDialog.ShowDialog() == DialogResult.OK )
			{
				Bitmap bmp = new Bitmap( this.Width, this.Height );
				Graphics gr = Graphics.FromImage( bmp );
				gr.FillRectangle( new SolidBrush( this.BackColor ), 0, 0, this.Width, this.Height );
				
				_map.Draw( bmp );
				
				switch ( saveBitmapDialog.FilterIndex )
				{
					case 1:
						bmp.Save( saveBitmapDialog.FileName, ImageFormat.Bmp );
						break;
					case 2:
						bmp.Save( saveBitmapDialog.FileName, ImageFormat.Png );
						break;
					case 3:
						bmp.Save( saveBitmapDialog.FileName, ImageFormat.Gif );
						break;
					case 4:
						bmp.Save( saveBitmapDialog.FileName, ImageFormat.Tiff );
						break;
					case 5:
						bmp.Save( saveBitmapDialog.FileName, ImageFormat.Emf );
						break;
				}

				gr.Dispose();
			}

		}
		
		/// <summary>
		/// The current Geographic position of the mouse on the X plane.
		/// </summary>
		public double MouseGeoX
		{
			get
			{
				return _MouseGeoX;
			}
		}

		/// <summary>
		/// The current Geographic position of the mouse on the Y plane.
		/// </summary>
		public double MouseGeoY
		{
			get
			{
				return _MouseGeoY;
			}
		}
		
		/// <summary>
		/// Get/Set the behavior of the mouse inside this MapControl.  Uses the
		/// PointerMode enumeration.
		/// </summary>
		public PointerMode PointerBehavior
		{
			get
			{
				return _PointerMode;
			}
			set
			{
				_PointerMode = value;
				Canvas.Refresh();
			}
		}

		/// <summary>
		/// The layer that will be queried.
		/// </summary>
		public int QueryLayer
		{
			get
			{
				return _queryLayer;
			}
			set
			{
				_queryLayer = value;
			}
		}

		/// <summary>
		/// This is the method that gets called for the querying of features
		/// in the MapControl.
		/// </summary>
		/// param x the local x coodinate in pixels
		/// param y the local y coodinate in pixels
		private void Query( int x, int y )
		{
			if ( _queryLayer != -1 )
			{
				IMapLayer ml = _map[ _queryLayer ];
				string[] result = ml.QueryPoint( x, y );
				this.Draw();

				OnQueryResult( this, result );
			}
		}

		/// <summary>
		/// Get the PictureBox object that is used as the canvas in this
		/// compound user control.
		/// </summary>
		/// @returns System.Windows.Forms.PictureBox
		public PictureBox MapCanvas
		{
			get
			{
				return Canvas;
			}
		}

		/// <summary>
		/// Get the GIS.Map layer collection associated with this control.
		/// </summary>
		/// @returns GIS.Map
		public Map MapCollection
		{
			get
			{
				return _map;
			}
		}
	}
}
