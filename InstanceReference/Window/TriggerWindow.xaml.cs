using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InstanceReference
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingWindow
        : Window
    {
        private MainWindow _mainWindow;

        private Stopwatch _mouseEnterWatch;
        private bool _isMoveDowned;
        private bool _isMouseMove;

        public SettingWindow(MainWindow mainWindow)
        {
            // Do not know why but these two line of setting make the option Window.SizeToContent work!!
            // Can't beleive it.
            Width = 0;
            Height = 0;

            _mouseEnterWatch = new Stopwatch();

            _mainWindow = mainWindow;
            InitializeComponent();

            this.Loaded += SettingWindow_Loaded;

            this.PreviewMouseDown += SettingWindow_PreviewMouseDown;
            this.PreviewMouseUp += SettingWindow_PreviewMouseUp;
            this.PreviewMouseMove += SettingWindow_PreviewMouseMove;

            MouseUp += SettingWindow_MouseUp;
        }

        private void SettingWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseEnterWatch.Elapsed.TotalSeconds < 2.0)
            {
                _mainWindow.HideWindow(false);
            }

            _mouseEnterWatch.Stop();
            _isMoveDowned = false;
        }

        private void SettingWindow_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMoveDowned == false) return;

            if (_isMouseMove == false && // Avoid stop watch evaluation pressure
                _mouseEnterWatch.Elapsed.TotalSeconds >= 2.0)
            {
                _isMouseMove = true;
            }
            else
            {
                _isMouseMove = false;
            }

            if (_isMouseMove)
            {
                DragMove();
            }
        }

        private void SettingWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseEnterWatch.Elapsed.TotalSeconds < 2.0)
            {
                _mainWindow.HideWindow(false);
            }

            _mouseEnterWatch.Stop();
            _isMoveDowned = false;
        }

        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var v = VisualTreeHelper.GetDpi(this);
            var w = ScreenHelper.Primary.WorkingArea.Width / v.DpiScaleY;
            var t = (int)(w - (int)Width);
            Left = t;
        }

        private void SettingWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMoveDowned = true;
            _mouseEnterWatch.Restart();
        }
    }
}
