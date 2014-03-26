//
//Form1.cs
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using ShapeDotNet;
using ShapeDotNet.GIS;

namespace SDNViewer
{
	/// <summary>
	/// Example application using ShapeDotNet.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private int queryField;
		
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.StatusBarPanel statusBarPanel3;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuOpenFile;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuHelpAbout;

		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuSaveViewTo;
		private System.Windows.Forms.MenuItem menuItemSaveToImage;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private ShapeDotNet.MapControl mapControl;
		private System.Windows.Forms.ToolBarButton pointerButton;
		private System.Windows.Forms.ToolBarButton zoomButton;
		private System.Windows.Forms.ToolBarButton zoomAreaButton;
		private System.Windows.Forms.ToolBarButton dragButton;
		private System.Windows.Forms.ToolBarButton centerButton;
		private System.Windows.Forms.ToolBarButton queryButton;
		private System.Windows.Forms.ToolBarButton extentsButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolBar mainToolBar;
		private System.Windows.Forms.ListBox layerListBox;
		private System.Windows.Forms.ToolBar layerToolBar;
		private System.Windows.Forms.ToolBarButton addLayerButton;
		private System.Windows.Forms.ToolBarButton removeLayerButton;
		private System.Windows.Forms.ToolBarButton toolBarSeparator1;
		private System.Windows.Forms.ToolBarButton moveLayerUpButton;
		private System.Windows.Forms.ToolBarButton moveLayerDownButton;
		private System.Windows.Forms.ToolBarButton layerPropertiesButton;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button clearSelectionButton;
		private System.Windows.Forms.ComboBox queryComboBox;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mapControl.OnQueryResult += new QueryResultEventHandler( this.mapControl_QueryResult );
			
			if ( Environment.GetCommandLineArgs().Length > 1 )
			{
				mapControl.AddLayer( Environment.GetCommandLineArgs()[ 1 ] );
				UpdateLayerList();
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuOpenFile = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuSaveViewTo = new System.Windows.Forms.MenuItem();
			this.menuItemSaveToImage = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuHelpAbout = new System.Windows.Forms.MenuItem();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel3 = new System.Windows.Forms.StatusBarPanel();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.queryComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.layerToolBar = new System.Windows.Forms.ToolBar();
			this.addLayerButton = new System.Windows.Forms.ToolBarButton();
			this.removeLayerButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarSeparator1 = new System.Windows.Forms.ToolBarButton();
			this.moveLayerUpButton = new System.Windows.Forms.ToolBarButton();
			this.moveLayerDownButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.layerPropertiesButton = new System.Windows.Forms.ToolBarButton();
			this.layerListBox = new System.Windows.Forms.ListBox();
			this.mainToolBar = new System.Windows.Forms.ToolBar();
			this.pointerButton = new System.Windows.Forms.ToolBarButton();
			this.zoomButton = new System.Windows.Forms.ToolBarButton();
			this.zoomAreaButton = new System.Windows.Forms.ToolBarButton();
			this.dragButton = new System.Windows.Forms.ToolBarButton();
			this.centerButton = new System.Windows.Forms.ToolBarButton();
			this.queryButton = new System.Windows.Forms.ToolBarButton();
			this.extentsButton = new System.Windows.Forms.ToolBarButton();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mapControl = new ShapeDotNet.MapControl();
			this.clearSelectionButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).BeginInit();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuHelp});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuOpenFile,
																					  this.menuItem3,
																					  this.menuSaveViewTo,
																					  this.menuItem6,
																					  this.menuExit});
			this.menuItem1.Text = "&File";
			// 
			// menuOpenFile
			// 
			this.menuOpenFile.Index = 0;
			this.menuOpenFile.Text = "&Open Shape File";
			this.menuOpenFile.Click += new System.EventHandler(this.menuOpenFile_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuSaveViewTo
			// 
			this.menuSaveViewTo.Index = 2;
			this.menuSaveViewTo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItemSaveToImage});
			this.menuSaveViewTo.Text = "&Save View As...";
			// 
			// menuItemSaveToImage
			// 
			this.menuItemSaveToImage.Index = 0;
			this.menuItemSaveToImage.Text = "Image";
			this.menuItemSaveToImage.Click += new System.EventHandler(this.menuItemSaveToImage_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 3;
			this.menuItem6.Text = "-";
			// 
			// menuExit
			// 
			this.menuExit.Index = 4;
			this.menuExit.Text = "E&xit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Index = 1;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuHelpAbout});
			this.menuHelp.Text = "&Help";
			// 
			// menuHelpAbout
			// 
			this.menuHelpAbout.Index = 0;
			this.menuHelpAbout.Text = "&About";
			this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 302);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1,
																						  this.statusBarPanel2,
																						  this.statusBarPanel3});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(500, 22);
			this.statusBar1.TabIndex = 2;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarPanel1.MinWidth = 200;
			this.statusBarPanel1.Width = 200;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarPanel2.MinWidth = 150;
			this.statusBarPanel2.Width = 150;
			// 
			// statusBarPanel3
			// 
			this.statusBarPanel3.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.statusBarPanel3.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel3.MinWidth = 60;
			this.statusBarPanel3.Width = 134;
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.White;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.groupBox2,
																				 this.groupBox1,
																				 this.mainToolBar});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.DockPadding.Left = 3;
			this.panel1.DockPadding.Right = 3;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(152, 302);
			this.panel1.TabIndex = 5;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.clearSelectionButton,
																					this.queryComboBox});
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(3, 188);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(146, 76);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Query Field";
			// 
			// queryComboBox
			// 
			this.queryComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.queryComboBox.Location = new System.Drawing.Point(3, 20);
			this.queryComboBox.Name = "queryComboBox";
			this.queryComboBox.Size = new System.Drawing.Size(140, 21);
			this.queryComboBox.TabIndex = 6;
			this.queryComboBox.Text = "Query Source Field";
			this.queryComboBox.SelectionChangeCommitted += new System.EventHandler(this.queryComboBox_SelectionChangeCommitted);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.layerToolBar,
																					this.layerListBox});
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(3, 47);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(146, 141);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Layers";
			// 
			// layerToolBar
			// 
			this.layerToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							this.addLayerButton,
																							this.removeLayerButton,
																							this.toolBarButton1,
																							this.toolBarSeparator1,
																							this.moveLayerUpButton,
																							this.moveLayerDownButton,
																							this.toolBarButton2,
																							this.layerPropertiesButton});
			this.layerToolBar.ButtonSize = new System.Drawing.Size(23, 22);
			this.layerToolBar.DropDownArrows = true;
			this.layerToolBar.ImageList = this.imageList1;
			this.layerToolBar.Location = new System.Drawing.Point(3, 111);
			this.layerToolBar.Name = "layerToolBar";
			this.layerToolBar.ShowToolTips = true;
			this.layerToolBar.Size = new System.Drawing.Size(140, 25);
			this.layerToolBar.TabIndex = 11;
			this.layerToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.layerToolBar_ButtonClick);
			// 
			// addLayerButton
			// 
			this.addLayerButton.ImageIndex = 8;
			this.addLayerButton.ToolTipText = "Add a New Layer";
			// 
			// removeLayerButton
			// 
			this.removeLayerButton.ImageIndex = 9;
			this.removeLayerButton.ToolTipText = "Remove the currently selected layer";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarSeparator1
			// 
			this.toolBarSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// moveLayerUpButton
			// 
			this.moveLayerUpButton.ImageIndex = 11;
			this.moveLayerUpButton.ToolTipText = "Move the currently selected layer \'up\'";
			// 
			// moveLayerDownButton
			// 
			this.moveLayerDownButton.ImageIndex = 12;
			this.moveLayerDownButton.ToolTipText = "Move the currently selected layer \'down\'";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// layerPropertiesButton
			// 
			this.layerPropertiesButton.ImageIndex = 10;
			this.layerPropertiesButton.ToolTipText = "Show the properties of the currently selected layer";
			// 
			// layerListBox
			// 
			this.layerListBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.layerListBox.Location = new System.Drawing.Point(3, 16);
			this.layerListBox.Name = "layerListBox";
			this.layerListBox.Size = new System.Drawing.Size(140, 95);
			this.layerListBox.TabIndex = 10;
			this.layerListBox.SelectedValueChanged += new System.EventHandler(this.layerListBox_SelectedValueChanged);
			// 
			// mainToolBar
			// 
			this.mainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.mainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.pointerButton,
																						   this.zoomButton,
																						   this.zoomAreaButton,
																						   this.dragButton,
																						   this.centerButton,
																						   this.queryButton,
																						   this.extentsButton});
			this.mainToolBar.DropDownArrows = true;
			this.mainToolBar.ImageList = this.imageList1;
			this.mainToolBar.Location = new System.Drawing.Point(3, 0);
			this.mainToolBar.Name = "mainToolBar";
			this.mainToolBar.ShowToolTips = true;
			this.mainToolBar.Size = new System.Drawing.Size(146, 47);
			this.mainToolBar.TabIndex = 3;
			this.mainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// pointerButton
			// 
			this.pointerButton.ImageIndex = 0;
			this.pointerButton.Pushed = true;
			this.pointerButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.pointerButton.ToolTipText = "Select";
			// 
			// zoomButton
			// 
			this.zoomButton.ImageIndex = 1;
			this.zoomButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.zoomButton.ToolTipText = "Zoom In/Out";
			// 
			// zoomAreaButton
			// 
			this.zoomAreaButton.ImageIndex = 2;
			this.zoomAreaButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.zoomAreaButton.ToolTipText = "Zoom Area";
			// 
			// dragButton
			// 
			this.dragButton.ImageIndex = 3;
			this.dragButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.dragButton.ToolTipText = "Drag";
			// 
			// centerButton
			// 
			this.centerButton.ImageIndex = 4;
			this.centerButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.centerButton.ToolTipText = "Center On Click";
			// 
			// queryButton
			// 
			this.queryButton.ImageIndex = 5;
			this.queryButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.queryButton.ToolTipText = "Query";
			// 
			// extentsButton
			// 
			this.extentsButton.ImageIndex = 6;
			this.extentsButton.ToolTipText = "Zoom To Extents";
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(152, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 302);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// mapControl
			// 
			this.mapControl.BackColor = System.Drawing.SystemColors.Window;
			this.mapControl.Cursor = System.Windows.Forms.Cursors.Default;
			this.mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl.Location = new System.Drawing.Point(155, 0);
			this.mapControl.Name = "mapControl";
			this.mapControl.PointerBehavior = ShapeDotNet.PointerMode.Select;
			this.mapControl.QueryLayer = -1;
			this.mapControl.Size = new System.Drawing.Size(345, 302);
			this.mapControl.TabIndex = 7;
			this.mapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapControl_MouseMove);
			// 
			// clearSelectionButton
			// 
			this.clearSelectionButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.clearSelectionButton.Location = new System.Drawing.Point(4, 48);
			this.clearSelectionButton.Name = "clearSelectionButton";
			this.clearSelectionButton.Size = new System.Drawing.Size(136, 23);
			this.clearSelectionButton.TabIndex = 7;
			this.clearSelectionButton.Text = "Clear All Selections";
			this.clearSelectionButton.Click += new System.EventHandler(this.clearSelectionButton_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(500, 324);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mapControl,
																		  this.splitter1,
																		  this.panel1,
																		  this.statusBar1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Shape.Net Viewer";
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapControl_MouseMove);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).EndInit();
			this.panel1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run( new Form1() );
		}

		public void Draw()
		{
			mapControl.Draw();
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(mainToolBar.Buttons.IndexOf(e.Button))
			{
				case 0: //Pointer - no action
					mainToolBar.Buttons[0].Pushed = true;
					mainToolBar.Buttons[1].Pushed = false;
					mainToolBar.Buttons[2].Pushed = false;
					mainToolBar.Buttons[3].Pushed = false;
					mainToolBar.Buttons[4].Pushed = false;
					mainToolBar.Buttons[5].Pushed = false;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.Select;
					break;

				case 1: //Pointer - zoom
					mainToolBar.Buttons[0].Pushed = false;
					mainToolBar.Buttons[1].Pushed = true;
					mainToolBar.Buttons[2].Pushed = false;
					mainToolBar.Buttons[3].Pushed = false;
					mainToolBar.Buttons[4].Pushed = false;
					mainToolBar.Buttons[5].Pushed = false;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.Zoom;
					break;

				case 2: //Pointer - zoom area
					mainToolBar.Buttons[0].Pushed = false;
					mainToolBar.Buttons[1].Pushed = false;
					mainToolBar.Buttons[2].Pushed = true;
					mainToolBar.Buttons[3].Pushed = false;
					mainToolBar.Buttons[4].Pushed = false;
					mainToolBar.Buttons[5].Pushed = false;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.ZoomArea;
					break;

				case 3: //Pointer - drag
					mainToolBar.Buttons[0].Pushed = false;
					mainToolBar.Buttons[1].Pushed = false;
					mainToolBar.Buttons[2].Pushed = false;
					mainToolBar.Buttons[3].Pushed = true;
					mainToolBar.Buttons[4].Pushed = false;
					mainToolBar.Buttons[5].Pushed = false;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.Drag;
					break;

				case 4: //Pointer - center
					mainToolBar.Buttons[0].Pushed = false;
					mainToolBar.Buttons[1].Pushed = false;
					mainToolBar.Buttons[2].Pushed = false;
					mainToolBar.Buttons[3].Pushed = false;
					mainToolBar.Buttons[4].Pushed = true;
					mainToolBar.Buttons[5].Pushed = false;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.Center;
					break;

				case 5: //Pointer - query
					mainToolBar.Buttons[0].Pushed = false;
					mainToolBar.Buttons[1].Pushed = false;
					mainToolBar.Buttons[2].Pushed = false;
					mainToolBar.Buttons[3].Pushed = false;
					mainToolBar.Buttons[4].Pushed = false;
					mainToolBar.Buttons[5].Pushed = true;
					mapControl.PointerBehavior = ShapeDotNet.PointerMode.Query;
					break;

				case 6: // Zoom to Extents
					mapControl.MapCollection.ZoomExtents();
					mapControl.Draw();
					break;

			}
	
		}

		private void menuOpenFile_Click(object sender, System.EventArgs e)
		{			
			mapControl.AddLayer();
			UpdateLayerList();
		}

		private void menuHelpAbout_Click(object sender, System.EventArgs e)
		{
			string msg = "A small ESRI Shapefile Viewer built with\n";
			msg += mapControl.ProductName + " v" + mapControl.ProductVersion + " (BETA 2)\n\n";
			msg += "(c)2002-2004 John Coleman <k5jvc at netscape.net>";
			
			MessageBox.Show(msg , "About Shape.Net...");
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void mapControl_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
		{
			statusBar1.Panels[1].Text = mapControl.MouseGeoX.ToString("F6") + " x " + 
										mapControl.MouseGeoY.ToString("F6");
		}

		private void mapControl_QueryResult( object sender, string[] result )
		{
			if ( result != null && queryField < result.Length )
			{
				statusBar1.Panels[0].Text = result[ queryField ].Trim();
			}
		}

		private void menuItemSaveToImage_Click(object sender, System.EventArgs e)
		{
			mapControl.SaveViewToImage();
		}

		private void layerToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(layerToolBar.Buttons.IndexOf(e.Button))
			{
				case 0:
					mapControl.AddLayer();
					UpdateLayerList();
					break;
				
				case 1:
					if ( layerListBox.SelectedIndex != -1 )
					{
						mapControl.RemoveLayer( layerListBox.SelectedIndex );
						UpdateLayerList();
					}
					break;
				
				case 4:
					if ( layerListBox.SelectedIndex != -1 )
					{
						mapControl.MoveLayerUp( layerListBox.SelectedIndex );
						UpdateLayerList();
					}
					break;
				
				case 5:
					if ( layerListBox.SelectedIndex != -1 )
					{
						mapControl.MoveLayerDown( layerListBox.SelectedIndex );
						UpdateLayerList();
					}
					break;
				
				case 7:
					if ( layerListBox.SelectedIndex != -1 )
					{
						PropertiesForm pf = new PropertiesForm( 
							mapControl.MapCollection[ layerListBox.SelectedIndex ] );
						
						if ( pf.ShowDialog()== DialogResult.OK )
						{
							Draw();	
						}
					}
					break;
			}

		}

		private void UpdateLayerList()
		{
			queryComboBox.Items.Clear();
			
			string seperator = "\\";
			char[] delimiter = seperator.ToCharArray() ;
			layerListBox.Items.Clear();

			foreach ( string s in mapControl.MapCollection.GetLayerNames() )
			{
				string[] ss = s.Split( delimiter, 100 );
				layerListBox.Items.Add( ss[ ss.Length - 1] );
			}
		}

		private void layerListBox_SelectedValueChanged(object sender, System.EventArgs e)
		{
			IMapLayer ml = mapControl.MapCollection[ layerListBox.SelectedIndex ];
			
			queryComboBox.Items.Clear();
			queryComboBox.Items.AddRange( ml.FieldNames );
			queryComboBox.SelectedIndex = 0;
			queryField = 0;

			mapControl.QueryLayer = layerListBox.SelectedIndex;
		}

		private void queryComboBox_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			queryField = queryComboBox.SelectedIndex;
		}

		private void clearSelectionButton_Click(object sender, System.EventArgs e)
		{
			for ( int a=0; a<layerListBox.Items.Count; ++a )
			{
				mapControl.MapCollection[a].HighlightedFeature = -1;
			}

			mapControl.Draw();
		}
	}
}
