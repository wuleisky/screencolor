//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Forms;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace ScreenColor
//{
//    /// <summary>
//    /// Window1.xaml 的交互逻辑
//    /// </summary>
//    public partial class ColorPickers : Window
//    {
//        PictureBox pictureBox;
//        Bitmap bmp;
//        Graphics g;
//        bool isDown = false;
//       // Point downPoint;
//       // private Panel pnlColorWindow;
//        private TrackBar tckZoomSize;
//        private PictureBox picZoom;
//        //private Label lblYValue;
//        //private Label lblYName;
//        //private Label lblXValue;
//        //private Label lblXName;
//        //private Label lblShowColor;
//        //private TextBox txtColorValue;
//        //private Label lblTitle;
//        System.Drawing.Rectangle rect;
//        System.Drawing.Pen pen;
//        Bitmap bmpZoom;
//        Graphics zoom;
//        int zoomSize = 0;
//        System.Drawing.Color selectedColor = System.Drawing.Color.White;

//        public System.Drawing.Color SelectedColor
//        {
//            get { return selectedColor; }
//            set { selectedColor = value; }
//        }

//        public ColorPickers()
//        {
//            InitializeComponent();
//            pictureBox = new PictureBox();
//            pictureBox.Dock = DockStyle.Fill;
//            pictureBox.BorderStyle = BorderStyle.None;
//            pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox_MouseUp);
//            pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox_MouseMove);
//            pictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
//           // string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,@"Image\Pen.png");          
//           // SetCursor(pictureBox, Bitmap.FromFile(path), new Point(0, 0));
//            this.ColorWindows.Child=pictureBox; 
//            //this.Size = new Size(0, 0);
//            //this.DoubleBuffered = true;
//            this.Loaded +=ColorPickers_Loaded; //new EventHandler(ColorPicker_Load);

//            bmpZoom = new Bitmap(picZoom.Width, picZoom.Height);
//            zoom = Graphics.FromImage(bmpZoom);
//            picZoom.Image = bmpZoom;

//            pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
//            pen.DashCap = DashCap.Round;
//            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

//            zoomSize = tckZoomSize.Value;
//        }

//            void ColorPickers_Loaded(object sender, RoutedEventArgs e)
//            {
//                throw new NotImplementedException();
//            }

//        //public  void SetCursor(Control control, Image cursor, Point hotPoint)
//        //{
//        //    int hotX = hotPoint.X;
//        //    int hotY = hotPoint.Y;
//        //    using (cursor)
//        //    using (Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY))
//        //    using (Graphics g = Graphics.FromImage(myNewCursor))
//        //    {
//        //        g.Clear(Color.FromArgb(0, 0, 0, 0));
//        //        g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width, cursor.Height);
//        //        IntPtr iptr = myNewCursor.GetHicon();
//        //        control.Cursor = new Cursor(iptr);
//        //    }
//        //}

//        void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            System.Windows.Point mousePoint = System.Windows.Forms.Control.MousePosition;
//            lblXValue.Text = mousePoint.X.ToString();
//            lblYValue.Text = mousePoint.Y.ToString();

//            Color color = bmp.GetPixel(mousePoint.X, mousePoint.Y);
//            lblShowColor.BackColor = color;
//            txtColorValue.Text = string.Format("#{0,2:X2}{1,2:X2}{2,2:X2}", color.R.ToString("X"), color.G.ToString("X"), color.B.ToString("X")).Replace(" ", "0");

//            zoom.InterpolationMode = InterpolationMode.NearestNeighbor;
//            zoom.DrawImage(bmp, new Rectangle(0, 0, bmpZoom.Width, bmpZoom.Height),
//                Control.MousePosition.X - bmpZoom.Width / (2 * zoomSize),
//                Control.MousePosition.Y - bmpZoom.Height / (2 * zoomSize),  
//                bmpZoom.Width / zoomSize, bmpZoom.Height / zoomSize,
//                GraphicsUnit.Pixel);
//            zoom.DrawLine(pen, picZoom.Width / 2 - 1, 0, picZoom.Width / 2 - 1, picZoom.Height);
//            zoom.DrawLine(pen, 0, picZoom.Height / 2 - 1, picZoom.Width, picZoom.Height / 2 - 1);

//            picZoom.Refresh();
//        }

//        void pictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            selectedColor = bmp.GetPixel(e.Location.X, e.Location.Y);

//            pen.Dispose();
//            bmp.Dispose();
//            bmpZoom.Dispose();
//            zoom.Dispose();
//            g.Dispose();

//            Close();
//        }

//        void ColorPicker_Load(object sender, EventArgs e)
//        {
//            rect = Screen.PrimaryScreen.Bounds;
//            bmp = new Bitmap(rect.Width, rect.Height);

//            g = Graphics.FromImage(bmp);
//            g.CopyFromScreen(0, 0, 0, 0, rect.Size);

//            pictureBox.Image = bmp;

//            this.Topmost = true;
//            //this.FormBorderStyle = FormBorderStyle.None;
//            this.WindowState = WindowState.Maximized;
//        }

//        //private void pnlColorWindow_MouseDown(object sender, MouseEventArgs e)
//        //{
//        //    if (e.Button == MouseButtons.Left)
//        //    {
//        //        isDown = true;
//        //        downPoint = e.Location;
//        //    }
//        //}

//        //private void pnlColorWindow_MouseMove(object sender, MouseEventArgs e)
//        //{
//        //    if (isDown)
//        //    {
//        //        pnlColorWindow.Left = Control.MousePosition.X - downPoint.X;
//        //        pnlColorWindow.Top = Control.MousePosition.Y - downPoint.Y;
//        //    }
//        //}

//        //private void pnlColorWindow_MouseUp(object sender, MouseEventArgs e)
//        //{
//        //    if (e.Button == MouseButtons.Left)
//        //    {
//        //        isDown = false;
//        //    }
//        //}

//        //private void tckZoomSize_ValueChanged(object sender, EventArgs e)
//        //{
//        //    zoomSize = tckZoomSize.Value;
//        //}
//        public ColorPickers()
//        {
//            InitializeComponent();
           
//        }
//    }
//}
