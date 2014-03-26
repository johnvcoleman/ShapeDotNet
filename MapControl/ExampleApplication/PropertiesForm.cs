using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ShapeDotNet.GIS;

namespace SDNViewer
{
	/// <summary>
	/// Summary description for PropertiesForm.
	/// </summary>
	public class PropertiesForm : System.Windows.Forms.Form
	{
		IMapLayer _ml;
		Color _foreColor;
		Color _fillColor;
		int _featureAlpha;
		SolidBrush _fillBrush;
		float _penWidth;
		
		Font _labelFont;
		Color _labelForeColor;
		Color _labelFillColor;
		int _labelAlpha;

		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown lineWidthUpDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button fillColorButton;
		private System.Windows.Forms.Button foreColorButton;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox showLabelsCheckBox;
		private System.Windows.Forms.ComboBox labelSourceComboBox;
		private System.Windows.Forms.Label FilenameLabel;
		private System.Windows.Forms.Button labelFontButton;
		private System.Windows.Forms.FontDialog fontDialog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TrackBar featureOpacityTrackBar;
		private System.Windows.Forms.TrackBar fontOpacityTrackBar;
		private System.Windows.Forms.CheckBox showLabelsBGCheckBox;
		private System.Windows.Forms.Button labelFillColorButton;
		private System.Windows.Forms.Button labelForeColorButton;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PropertiesForm( IMapLayer ml )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			FilenameLabel.Text = ml.ToString();

			_ml = ml;
			_foreColor = ml.LayerPen.Color;
			_fillColor = ml.LayerBrush.Color;
			_featureAlpha = ml.LayerPen.Color.A;
			_penWidth = ml.LayerPen.Width;
			_labelFont = _ml.LabelFont;
			_labelForeColor = _ml.LabelForeColor;
			_labelFillColor = _ml.LabelFillColor;
			_labelAlpha = ml.LabelForeColor.A;

			showLabelsCheckBox.Checked = _ml.Labels;
			showLabelsBGCheckBox.Checked = _ml.LabelBackground;
			labelSourceComboBox.Items.AddRange( _ml.FieldNames );
			labelSourceComboBox.SelectedIndex = _ml.LabelSourceField;
			labelFontButton.Font = _ml.LabelFont;
			featureOpacityTrackBar.Enabled = false;
			featureOpacityTrackBar.Value = _featureAlpha;
			featureOpacityTrackBar.Enabled = true;
			fontOpacityTrackBar.Enabled = false;
			fontOpacityTrackBar.Value = _labelAlpha;
			fontOpacityTrackBar.Enabled = true;
			
			Bitmap foreColorImg = new Bitmap( 35, 24 );
			Bitmap fillColorImg = new Bitmap( 35, 24 );
			Bitmap labelForeColorImg = new Bitmap( 35, 24 );
			Bitmap labelFillColorImg = new Bitmap( 35, 24 );

			Graphics foreGr = Graphics.FromImage( foreColorImg );
			Graphics fillGr = Graphics.FromImage( fillColorImg );
			Graphics labelForeGr = Graphics.FromImage( labelForeColorImg );
			Graphics labelFillGr = Graphics.FromImage( labelFillColorImg );

			foreGr.FillRectangle( new SolidBrush( _foreColor ), 0, 0, foreColorImg.Width, foreColorImg.Height );
			fillGr.FillRectangle( new SolidBrush( _fillColor ), 0, 0, fillColorImg.Width, fillColorImg.Height );
			labelForeGr.FillRectangle( new SolidBrush( _labelForeColor ), 0, 0, labelForeColorImg.Width, labelForeColorImg.Height );
			labelFillGr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, labelFillColorImg.Width, labelFillColorImg.Height );

			foreColorButton.Image = foreColorImg;
			fillColorButton.Image = fillColorImg;
			labelForeColorButton.Image = labelForeColorImg;
			labelFillColorButton.Image = labelFillColorImg;

			lineWidthUpDown.Value = (decimal) ml.LayerPen.Width;

			foreGr.Dispose();
			fillGr.Dispose();
			labelForeGr.Dispose();
			labelFillGr.Dispose();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.FilenameLabel = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.featureOpacityTrackBar = new System.Windows.Forms.TrackBar();
			this.label2 = new System.Windows.Forms.Label();
			this.lineWidthUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.fillColorButton = new System.Windows.Forms.Button();
			this.foreColorButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelFillColorButton = new System.Windows.Forms.Button();
			this.showLabelsBGCheckBox = new System.Windows.Forms.CheckBox();
			this.fontOpacityTrackBar = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.labelForeColorButton = new System.Windows.Forms.Button();
			this.labelFontButton = new System.Windows.Forms.Button();
			this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
			this.labelSourceComboBox = new System.Windows.Forms.ComboBox();
			this.fontDialog = new System.Windows.Forms.FontDialog();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.featureOpacityTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lineWidthUpDown)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fontOpacityTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// colorDialog
			// 
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(136, 428);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 5;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(220, 428);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.FilenameLabel});
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(292, 52);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filename";
			// 
			// FilenameLabel
			// 
			this.FilenameLabel.Location = new System.Drawing.Point(4, 16);
			this.FilenameLabel.Name = "FilenameLabel";
			this.FilenameLabel.Size = new System.Drawing.Size(284, 32);
			this.FilenameLabel.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.featureOpacityTrackBar,
																					this.label2,
																					this.lineWidthUpDown,
																					this.label1,
																					this.fillColorButton,
																					this.foreColorButton});
			this.groupBox2.Location = new System.Drawing.Point(8, 68);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(292, 144);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Features";
			// 
			// featureOpacityTrackBar
			// 
			this.featureOpacityTrackBar.LargeChange = 20;
			this.featureOpacityTrackBar.Location = new System.Drawing.Point(68, 88);
			this.featureOpacityTrackBar.Maximum = 255;
			this.featureOpacityTrackBar.Name = "featureOpacityTrackBar";
			this.featureOpacityTrackBar.Size = new System.Drawing.Size(216, 42);
			this.featureOpacityTrackBar.TabIndex = 10;
			this.featureOpacityTrackBar.TickFrequency = 20;
			this.featureOpacityTrackBar.Value = 255;
			this.featureOpacityTrackBar.Scroll += new System.EventHandler(this.featureOpacityTrackBar_Scroll);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Opacity:";
			// 
			// lineWidthUpDown
			// 
			this.lineWidthUpDown.Location = new System.Drawing.Point(148, 56);
			this.lineWidthUpDown.Maximum = new System.Decimal(new int[] {
																			50,
																			0,
																			0,
																			0});
			this.lineWidthUpDown.Name = "lineWidthUpDown";
			this.lineWidthUpDown.Size = new System.Drawing.Size(44, 20);
			this.lineWidthUpDown.TabIndex = 8;
			this.lineWidthUpDown.Value = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.lineWidthUpDown.ValueChanged += new System.EventHandler(this.lineWidthUpDown_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 60);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Line Width / Point Size";
			// 
			// fillColorButton
			// 
			this.fillColorButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.fillColorButton.Location = new System.Drawing.Point(156, 24);
			this.fillColorButton.Name = "fillColorButton";
			this.fillColorButton.Size = new System.Drawing.Size(128, 24);
			this.fillColorButton.TabIndex = 6;
			this.fillColorButton.Text = "Fill Color";
			this.fillColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.fillColorButton.Click += new System.EventHandler(this.fillColorButton_Click);
			// 
			// foreColorButton
			// 
			this.foreColorButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.foreColorButton.Location = new System.Drawing.Point(12, 24);
			this.foreColorButton.Name = "foreColorButton";
			this.foreColorButton.Size = new System.Drawing.Size(128, 24);
			this.foreColorButton.TabIndex = 5;
			this.foreColorButton.Text = "Fore Color";
			this.foreColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.foreColorButton.Click += new System.EventHandler(this.foreColorButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.labelFillColorButton,
																					this.showLabelsBGCheckBox,
																					this.fontOpacityTrackBar,
																					this.label3,
																					this.labelForeColorButton,
																					this.labelFontButton,
																					this.showLabelsCheckBox,
																					this.labelSourceComboBox});
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(8, 220);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(292, 192);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Labels";
			// 
			// labelFillColorButton
			// 
			this.labelFillColorButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelFillColorButton.Location = new System.Drawing.Point(148, 104);
			this.labelFillColorButton.Name = "labelFillColorButton";
			this.labelFillColorButton.Size = new System.Drawing.Size(128, 24);
			this.labelFillColorButton.TabIndex = 16;
			this.labelFillColorButton.Text = "Fill Color";
			this.labelFillColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelFillColorButton.Click += new System.EventHandler(this.labelFillColorButton_Click);
			// 
			// showLabelsBGCheckBox
			// 
			this.showLabelsBGCheckBox.Location = new System.Drawing.Point(132, 28);
			this.showLabelsBGCheckBox.Name = "showLabelsBGCheckBox";
			this.showLabelsBGCheckBox.Size = new System.Drawing.Size(152, 24);
			this.showLabelsBGCheckBox.TabIndex = 15;
			this.showLabelsBGCheckBox.Text = "Show Label Background";
			// 
			// fontOpacityTrackBar
			// 
			this.fontOpacityTrackBar.LargeChange = 20;
			this.fontOpacityTrackBar.Location = new System.Drawing.Point(68, 140);
			this.fontOpacityTrackBar.Maximum = 255;
			this.fontOpacityTrackBar.Name = "fontOpacityTrackBar";
			this.fontOpacityTrackBar.Size = new System.Drawing.Size(216, 42);
			this.fontOpacityTrackBar.TabIndex = 14;
			this.fontOpacityTrackBar.TickFrequency = 20;
			this.fontOpacityTrackBar.Value = 255;
			this.fontOpacityTrackBar.Scroll += new System.EventHandler(this.fontOpacityTrackBar_Scroll);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Opacity:";
			// 
			// labelForeColorButton
			// 
			this.labelForeColorButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelForeColorButton.Location = new System.Drawing.Point(12, 104);
			this.labelForeColorButton.Name = "labelForeColorButton";
			this.labelForeColorButton.Size = new System.Drawing.Size(128, 24);
			this.labelForeColorButton.TabIndex = 12;
			this.labelForeColorButton.Text = "Fore Color";
			this.labelForeColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelForeColorButton.Click += new System.EventHandler(this.labelForeColorButton_Click);
			// 
			// labelFontButton
			// 
			this.labelFontButton.Location = new System.Drawing.Point(12, 72);
			this.labelFontButton.Name = "labelFontButton";
			this.labelFontButton.Size = new System.Drawing.Size(128, 24);
			this.labelFontButton.TabIndex = 11;
			this.labelFontButton.Text = "Label Font";
			this.labelFontButton.Click += new System.EventHandler(this.labelFontButton_Click);
			// 
			// showLabelsCheckBox
			// 
			this.showLabelsCheckBox.Location = new System.Drawing.Point(12, 28);
			this.showLabelsCheckBox.Name = "showLabelsCheckBox";
			this.showLabelsCheckBox.TabIndex = 10;
			this.showLabelsCheckBox.Text = "Show Labels";
			// 
			// labelSourceComboBox
			// 
			this.labelSourceComboBox.Location = new System.Drawing.Point(148, 72);
			this.labelSourceComboBox.Name = "labelSourceComboBox";
			this.labelSourceComboBox.Size = new System.Drawing.Size(132, 21);
			this.labelSourceComboBox.TabIndex = 9;
			this.labelSourceComboBox.Text = "Source Field";
			// 
			// PropertiesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(306, 458);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox3,
																		  this.groupBox2,
																		  this.groupBox1,
																		  this.cancelButton,
																		  this.okButton});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PropertiesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Layer Properties";
			this.TopMost = true;
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.featureOpacityTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lineWidthUpDown)).EndInit();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fontOpacityTrackBar)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void foreColorButton_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = _foreColor;
			
			if ( colorDialog.ShowDialog() == DialogResult.OK )
			{
				_foreColor = Color.FromArgb( _featureAlpha, colorDialog.Color );
				
				Bitmap foreBmp = new Bitmap( foreColorButton.Image.Width, foreColorButton.Image.Height );
				Graphics foreGr = Graphics.FromImage( foreBmp );
				foreGr.FillRectangle( new SolidBrush( _foreColor ), 0, 0, foreBmp.Width, foreBmp.Height );

				foreColorButton.Image = foreBmp;

				foreGr.Dispose();
			}
		}

		private void fillColorButton_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = _fillColor;
			
			if ( colorDialog.ShowDialog() == DialogResult.OK )
			{
				_fillColor = Color.FromArgb( _featureAlpha, colorDialog.Color );
				_fillBrush = new SolidBrush( _fillColor );

				Bitmap fillBmp = new Bitmap( fillColorButton.Image.Width, fillColorButton.Image.Height );
				Graphics fillGr = Graphics.FromImage( fillBmp );
				fillGr.FillRectangle( _fillBrush, 0, 0, fillBmp.Width, fillBmp.Height );

				fillColorButton.Image = fillBmp;

				fillGr.Dispose();
			}
		}

		private void labelFontButton_Click(object sender, System.EventArgs e)
		{
			if ( fontDialog.ShowDialog() == DialogResult.OK )
			{
				_labelFont = fontDialog.Font;
				labelFontButton.Font = _labelFont;
			}
		}

		private void labelForeColorButton_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = _labelForeColor;

			if ( colorDialog.ShowDialog() == DialogResult.OK )
			{
				_labelForeColor = Color.FromArgb( _labelAlpha, colorDialog.Color );
				
				Bitmap labelBmp = new Bitmap( labelForeColorButton.Image.Width, labelForeColorButton.Image.Height );
				Graphics labelGr = Graphics.FromImage( labelBmp );
				labelGr.FillRectangle( new SolidBrush( _labelForeColor ), 0, 0, labelBmp.Width, labelBmp.Height );

				labelForeColorButton.Image = labelBmp;

				labelGr.Dispose();
			}
		}

		private void labelFillColorButton_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = _labelFillColor;

			if ( colorDialog.ShowDialog() == DialogResult.OK )
			{
				_labelFillColor = Color.FromArgb( _labelAlpha, colorDialog.Color );
				
				Bitmap labelBmp = new Bitmap( labelFillColorButton.Image.Width, labelFillColorButton.Image.Height );
				Graphics labelGr = Graphics.FromImage( labelBmp );
				labelGr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, labelBmp.Width, labelBmp.Height );

				labelFillColorButton.Image = labelBmp;

				labelGr.Dispose();
			}
		}
		
		private void okButton_Click(object sender, System.EventArgs e)
		{
			_ml.LayerPen = new Pen( Color.FromArgb( _featureAlpha, _foreColor ), _penWidth );
			_ml.LayerBrush = new SolidBrush( Color.FromArgb( _featureAlpha, _fillColor ) );
			_ml.Labels = showLabelsCheckBox.Checked;
			_ml.LabelBackground = showLabelsBGCheckBox.Checked;
			_ml.LabelSourceField = labelSourceComboBox.SelectedIndex;
			_ml.LabelForeColor = Color.FromArgb( _labelAlpha, _labelForeColor );
			_ml.LabelFillColor = Color.FromArgb( _labelAlpha, _labelFillColor );
			_ml.LabelFont = _labelFont;
			
			this.DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void lineWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			_penWidth = (float) lineWidthUpDown.Value;
		}

		private void featureOpacityTrackBar_Scroll(object sender, System.EventArgs e)
		{
			_featureAlpha = featureOpacityTrackBar.Value;

			_foreColor = Color.FromArgb( _featureAlpha, _foreColor );
			_fillColor = Color.FromArgb( _featureAlpha, _fillColor );
				
			Bitmap foreBmp = new Bitmap( foreColorButton.Image.Width, foreColorButton.Image.Height );
			Graphics foreGr = Graphics.FromImage( foreBmp );
			foreGr.FillRectangle( new SolidBrush( _foreColor ), 0, 0, foreBmp.Width, foreBmp.Height );

			foreColorButton.Image = foreBmp;

			foreGr.Dispose();
			
			_fillColor = Color.FromArgb( _featureAlpha, _fillColor );
			_fillBrush = new SolidBrush( _fillColor );

			Bitmap fillBmp = new Bitmap( fillColorButton.Image.Width, fillColorButton.Image.Height );
			Graphics fillGr = Graphics.FromImage( fillBmp );
			fillGr.FillRectangle( _fillBrush, 0, 0, fillBmp.Width, fillBmp.Height );

			fillColorButton.Image = fillBmp;

			fillGr.Dispose();		
		}

		private void fontOpacityTrackBar_Scroll(object sender, System.EventArgs e)
		{
			_labelAlpha = fontOpacityTrackBar.Value;

			_labelForeColor = Color.FromArgb( _labelAlpha, _labelForeColor );
			_labelFillColor = Color.FromArgb( _labelAlpha, _labelFillColor );
				
			Bitmap labelBmp = new Bitmap( labelForeColorButton.Image.Width, labelForeColorButton.Image.Height );
			Graphics labelGr = Graphics.FromImage( labelBmp );
			labelGr.FillRectangle( new SolidBrush( _labelForeColor ), 0, 0, labelBmp.Width, labelBmp.Height );

			labelForeColorButton.Image = labelBmp;


			labelBmp = new Bitmap( labelFillColorButton.Image.Width, labelFillColorButton.Image.Height );
			labelGr = Graphics.FromImage( labelBmp );
			labelGr.FillRectangle( new SolidBrush( _labelFillColor ), 0, 0, labelBmp.Width, labelBmp.Height );

			labelFillColorButton.Image = labelBmp;

			labelGr.Dispose();
		}
	}
}
