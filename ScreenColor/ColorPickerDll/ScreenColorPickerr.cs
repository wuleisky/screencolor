﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenColorPicker
{
    public class ScreenColorPicker : Form
    {
        PictureBox pictureBox;
        Bitmap bmp;
        Graphics g;
        bool isDown = false;
        Point downPoint;
        private Panel pnlColorWindow;
        private TrackBar tckZoomSize;
        private PictureBox picZoom;
        private Label lblYValue;
        private Label lblYName;
        private Label lblXValue;
        private Label lblXName;
        private Label lblShowColor;
        private TextBox txtColorValue;
        private Label lblTitle;
        Rectangle rect;
        Pen pen;
        Bitmap bmpZoom;
        Graphics zoom;
        int zoomSize = 0;
        Color selectedColor = Color.White;

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public Action<string> CallColorEvent;
        public ScreenColorPicker()
        {
            InitializeComponent();
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BorderStyle = BorderStyle.None;
            pictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
            pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
            pictureBox.Cursor = Cursors.Cross;
           // string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,@"Image\Pen.png");          
           // SetCursor(pictureBox, Bitmap.FromFile(path), new Point(0, 0));
            this.Controls.Add(pictureBox);
            this.Size = new Size(0, 0);
            this.DoubleBuffered = true;
            this.Load += new EventHandler(ColorPicker_Load);

            bmpZoom = new Bitmap(picZoom.Width, picZoom.Height);
            zoom = Graphics.FromImage(bmpZoom);
            picZoom.Image = bmpZoom;

            pen = new Pen(Color.Black, 1);
            pen.DashCap = DashCap.Round;
            pen.DashStyle = DashStyle.Dash;

            zoomSize = tckZoomSize.Value;
        }

        public  void SetCursor(Control control, Image cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            using (cursor)
            using (Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY))
            using (Graphics g = Graphics.FromImage(myNewCursor))
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
                g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width, cursor.Height);
                IntPtr iptr = myNewCursor.GetHicon();
                control.Cursor = new Cursor(iptr);
            }
        }

        void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = Control.MousePosition;
            lblXValue.Text = mousePoint.X.ToString();
            lblYValue.Text = mousePoint.Y.ToString();

            Color color = bmp.GetPixel(mousePoint.X, mousePoint.Y);
            lblShowColor.BackColor = color;
            txtColorValue.Text = string.Format("#{0,2:X2}{1,2:X2}{2,2:X2}", color.R.ToString("X"), color.G.ToString("X"), color.B.ToString("X")).Replace(" ", "0");

            zoom.InterpolationMode = InterpolationMode.NearestNeighbor;
            zoom.DrawImage(bmp, new Rectangle(0, 0, bmpZoom.Width, bmpZoom.Height),
                Control.MousePosition.X - bmpZoom.Width / (2 * zoomSize),
                Control.MousePosition.Y - bmpZoom.Height / (2 * zoomSize),  
                bmpZoom.Width / zoomSize, bmpZoom.Height / zoomSize,
                GraphicsUnit.Pixel);
            zoom.DrawLine(pen, picZoom.Width / 2 - 1, 0, picZoom.Width / 2 - 1, picZoom.Height);
            zoom.DrawLine(pen, 0, picZoom.Height / 2 - 1, picZoom.Width, picZoom.Height / 2 - 1);

            picZoom.Refresh();
        }

        void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            selectedColor = bmp.GetPixel(e.Location.X, e.Location.Y);
            string colorname = string.Format("#{0,2:X2}{1,2:X2}{2,2:X2}", selectedColor.R.ToString("X"), selectedColor.G.ToString("X"), selectedColor.B.ToString("X")).Replace(" ", "0");                
            pen.Dispose();
            bmp.Dispose();
            bmpZoom.Dispose();
            zoom.Dispose();
            g.Dispose();       
            if (CallColorEvent != null)
            {
                CallColorEvent(colorname);
            }
            Close();
                   
           
        }

        void ColorPicker_Load(object sender, EventArgs e)
        {
            rect = Screen.PrimaryScreen.Bounds;
            bmp = new Bitmap(rect.Width, rect.Height);

            g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, rect.Size);

            pictureBox.Image = bmp;

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void pnlColorWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDown = true;
                downPoint = e.Location;
            }
        }

        private void pnlColorWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                pnlColorWindow.Left = Control.MousePosition.X - downPoint.X;
                pnlColorWindow.Top = Control.MousePosition.Y - downPoint.Y;
            }
        }

        private void pnlColorWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDown = false;
            }
        }

        private void tckZoomSize_ValueChanged(object sender, EventArgs e)
        {
            zoomSize = tckZoomSize.Value;
        }

        private void InitializeComponent()
        {
            this.pnlColorWindow = new System.Windows.Forms.Panel();
            this.lblYValue = new System.Windows.Forms.Label();
            this.lblXValue = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtColorValue = new System.Windows.Forms.TextBox();
            this.lblShowColor = new System.Windows.Forms.Label();
            this.picZoom = new System.Windows.Forms.PictureBox();
            this.lblYName = new System.Windows.Forms.Label();
            this.lblXName = new System.Windows.Forms.Label();
            this.tckZoomSize = new System.Windows.Forms.TrackBar();
            this.pnlColorWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tckZoomSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlColorWindow
            // 
            this.pnlColorWindow.BackColor = System.Drawing.Color.White;
            this.pnlColorWindow.Controls.Add(this.lblYValue);
            this.pnlColorWindow.Controls.Add(this.lblXValue);
            this.pnlColorWindow.Controls.Add(this.lblTitle);
            this.pnlColorWindow.Controls.Add(this.txtColorValue);
            this.pnlColorWindow.Controls.Add(this.lblShowColor);
            this.pnlColorWindow.Controls.Add(this.picZoom);
            this.pnlColorWindow.Controls.Add(this.lblYName);
            this.pnlColorWindow.Controls.Add(this.lblXName);
            this.pnlColorWindow.Controls.Add(this.tckZoomSize);
            this.pnlColorWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnlColorWindow.Location = new System.Drawing.Point(51, 61);
            this.pnlColorWindow.Name = "pnlColorWindow";
            this.pnlColorWindow.Size = new System.Drawing.Size(198, 97);
            this.pnlColorWindow.TabIndex = 0;
            this.pnlColorWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlColorWindow_MouseDown);
            this.pnlColorWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlColorWindow_MouseMove);
            this.pnlColorWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlColorWindow_MouseUp);
            // 
            // lblYValue
            // 
            this.lblYValue.Location = new System.Drawing.Point(86, 66);
            this.lblYValue.Name = "lblYValue";
            this.lblYValue.Size = new System.Drawing.Size(33, 15);
            this.lblYValue.TabIndex = 7;
            this.lblYValue.Text = "1000";
            // 
            // lblXValue
            // 
            this.lblXValue.Location = new System.Drawing.Point(24, 66);
            this.lblXValue.Name = "lblXValue";
            this.lblXValue.Size = new System.Drawing.Size(33, 15);
            this.lblXValue.TabIndex = 5;
            this.lblXValue.Text = "1000";
            this.lblXValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(9, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(65, 12);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "颜色选取器";
            // 
            // txtColorValue
            // 
            this.txtColorValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColorValue.Location = new System.Drawing.Point(49, 34);
            this.txtColorValue.Name = "txtColorValue";
            this.txtColorValue.ReadOnly = true;
            this.txtColorValue.Size = new System.Drawing.Size(70, 21);
            this.txtColorValue.TabIndex = 9;
            // 
            // lblShowColor
            // 
            this.lblShowColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblShowColor.Location = new System.Drawing.Point(12, 34);
            this.lblShowColor.Name = "lblShowColor";
            this.lblShowColor.Size = new System.Drawing.Size(30, 20);
            this.lblShowColor.TabIndex = 2;
            // 
            // picZoom
            // 
            this.picZoom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picZoom.Location = new System.Drawing.Point(130, 6);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(60, 56);
            this.picZoom.TabIndex = 0;
            this.picZoom.TabStop = false;
            // 
            // lblYName
            // 
            this.lblYName.AutoSize = true;
            this.lblYName.Location = new System.Drawing.Point(67, 67);
            this.lblYName.Name = "lblYName";
            this.lblYName.Size = new System.Drawing.Size(17, 12);
            this.lblYName.TabIndex = 6;
            this.lblYName.Text = "Y:";
            // 
            // lblXName
            // 
            this.lblXName.AutoSize = true;
            this.lblXName.Location = new System.Drawing.Point(8, 67);
            this.lblXName.Name = "lblXName";
            this.lblXName.Size = new System.Drawing.Size(17, 12);
            this.lblXName.TabIndex = 4;
            this.lblXName.Text = "X:";
            // 
            // tckZoomSize
            // 
            this.tckZoomSize.AutoSize = false;
            this.tckZoomSize.Cursor = System.Windows.Forms.Cursors.Default;
            this.tckZoomSize.Location = new System.Drawing.Point(118, 63);
            this.tckZoomSize.Margin = new System.Windows.Forms.Padding(0);
            this.tckZoomSize.Maximum = 8;
            this.tckZoomSize.Minimum = 1;
            this.tckZoomSize.Name = "tckZoomSize";
            this.tckZoomSize.Size = new System.Drawing.Size(77, 23);
            this.tckZoomSize.TabIndex = 0;
            this.tckZoomSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tckZoomSize.Value = 4;
            this.tckZoomSize.ValueChanged += new System.EventHandler(this.tckZoomSize_ValueChanged);
            // 
            // ScreenColorPicker
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.pnlColorWindow);
            this.Name = "ScreenColorPicker";
            this.ShowInTaskbar = false;
            this.pnlColorWindow.ResumeLayout(false);
            this.pnlColorWindow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tckZoomSize)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
