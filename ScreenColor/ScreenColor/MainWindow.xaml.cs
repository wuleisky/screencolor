using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenColor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScreenColorPicker.ScreenColorPicker c = new ScreenColorPicker.ScreenColorPicker();
            c.CallColorEvent = new Action<string>(this.ReturnColorMethod);
            c.ShowDialog();
        }

        public void ReturnColorMethod(string colorName)
        {
            this.bd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorName));
            tb.Text = colorName;
        }
    }
}
