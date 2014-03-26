//
//ScaleBar.cs
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ShapeDotNet.GIS
{
	/// <summary>
	/// Summary description for ScaleBar.
	/// </summary>
	public class ScaleBar
	{
		public ScaleBar( MapMetrics metrics )
		{			
			Graphics gr = Graphics.FromImage( metrics.Canvas );
			int dpi = (int) gr.DpiX;
			double x1 = 0.0;
			double y1 = 0.0;
			double x2 = dpi;
			double y2 = 0.0;

			metrics.Pixel2World( ref x1, ref y1 );
			metrics.Pixel2World( ref x2, ref y2 );

			double distance = x2 - x1;

			Pen p = new Pen( Color.Chartreuse, 4 );
			
			int scaleBarLeft = metrics.Canvas.Width - ( dpi + 20 );
			int scaleBarRight = metrics.Canvas.Width - 20;
			int scaleBarTop = metrics.Canvas.Height - 35;
			int scaleBarBottom = metrics.Canvas.Height - 15;

			gr.DrawLine( p, new Point( scaleBarLeft, scaleBarBottom ), 
				new Point( scaleBarRight, scaleBarBottom ) );

			p = new Pen( Color.Chartreuse, 1 );

			gr.DrawLine( p, new Point( scaleBarLeft, scaleBarBottom - 10 ), 
				new Point( scaleBarLeft, scaleBarBottom ) );
			gr.DrawLine( p, new Point( scaleBarRight, scaleBarBottom - 10 ), 
				new Point( scaleBarRight, scaleBarBottom ) );
			
			gr.DrawString( "1:" + distance.ToString("F4"), new Font("Arial", 8), 
				new SolidBrush( Color.Chartreuse ), scaleBarLeft+25, scaleBarTop+5 );
			
			gr.Dispose();			
		}
	}
}
