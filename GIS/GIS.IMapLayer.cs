//
//IMapLayer.cs
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
using System.Windows.Forms;
using System.Drawing;

namespace ShapeDotNet.GIS
{
	/// <summary>
	/// Describes the two basic types of map layers that will be present in
	/// SDN.
	/// </summary>
	public enum LayerType { Vector, Raster }

	/// <summary>
	/// Describes the specific file format of the currently loaded layer.
	/// Only Shapefile is supported right now.
	/// </summary>
	public enum LayerSubType { Shapefile, Mif, E00, GeoTIFF }

	
	/// <summary>
	/// The interface for all map layers.  All map layers must
	/// implement this interface.
	/// </summary>
	public interface IMapLayer
	{
		/// <summary>
		/// Draw the features of the layer to the MapMetrics.Canvas (bitmap).
		/// </summary>
		void Draw();
		
		/// <summary>
		/// Draw the labels for each feature of the layer to the MapMetrics.Canvas (bitmap).
		/// </summary>
		void DrawLabels();
		
		/// <summary>
		/// Determines whether or not to draw the labels.
		/// </summary>
		bool Labels
		{
			get;
			set;
		}

		/// <summary>
		/// Determines whether or not to draw the background of the labels.
		/// </summary>
		bool LabelBackground
		{
			get;
			set;
		}

		/// <summary>
		/// Determines the font of the labels.
		/// </summary>
		Font LabelFont
		{
			get;
			set;
		}
		
		/// <summary>
		/// Determines the fore color of the labels (for the font and label border).
		/// </summary>
		Color LabelForeColor
		{
			get;
			set;
		}

		/// <summary>
		/// Determines the fill color for the background of the label.
		/// </summary>
		Color LabelFillColor
		{
			get;
			set;
		}

		/// <summary>
		/// The source field in the db used for the labels.
		/// </summary>
		int LabelSourceField
		{
			get;
			set;
		}
		
		/// <summary>
		/// Get the extents of the layer.
		/// </summary>
		RectangleD Extents
		{
			get;
		}

		/// <summary>
		/// Returns the human readable names of the fields in the .dbf file.
		/// </summary>
		string[] FieldNames
		{
			get;
		}

		/// <summary>
		/// Gets/Sets the descriptive name of the layer. Usually set by the user
		/// at runtime.
		/// </summary>
		string LayerName
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the GIS.LayerType of the layer. ( bitmap or vector )
		/// </summary>
		/// <returns></returns>
		LayerType DrawingType
		{
			get;
		}

		/// <summary>
		/// Returns the GIS.LayerSubType of the layer. ( Specific file format )
		/// </summary>
		/// <returns></returns>
		LayerSubType DrawingSubType
		{
			get;
		}

		/// <summary>
		/// The pen to be used for drawing the features.
		/// </summary>
		Pen LayerPen
		{
			get;
			set;
		}
		
		/// <summary>
		/// The brush to be used for drawing the features.
		/// </summary>
		SolidBrush LayerBrush
		{
			get;
			set;
		}

		/// <summary>
		/// The index number of the feature to highlight.  -1 if no
		/// feature is highlighted.
		/// </summary>
		int HighlightedFeature
		{
			get;
			set;
		}
		
		/// <summary>
		/// Return the name/value associated with the geographic object
		/// located at x,y.
		/// </summary>
		/// <param name="x">Local X coordinate.</param>
		/// <param name="y">Local Y coordinate.</param>
		/// <returns>A string array of attributes represented by the object at x,y.</returns>
		string[] QueryPoint( int x, int y );
	}
}
