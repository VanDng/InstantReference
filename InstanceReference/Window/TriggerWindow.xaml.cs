using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        private bool _isLocationLoading;

        public SettingWindow(MainWindow mainWindow)
        {
            // Do not know why but these two line of setting make the option Window.SizeToContent work!!
            // Can't beleive it.
            Width = 0;
            Height = 0;

            _mouseEnterWatch = new Stopwatch();

            _mainWindow = mainWindow;
            InitializeComponent();

            InitializeContextMenu();

            Loaded += SettingWindow_Loaded;
            LocationChanged += SettingWindow_LocationChanged;

            PreviewMouseLeftButtonDown += SettingWindow_PreviewMouseDown;
            PreviewMouseLeftButtonUp += SettingWindow_PreviewMouseUp;
            PreviewMouseMove += SettingWindow_PreviewMouseMove;

            MouseUp += SettingWindow_PreviewMouseUp;
            MouseLeave += SettingWindow_MouseLeave;
        }

        private void SettingWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            _mouseEnterWatch.Stop();
            _isMoveDowned = false;
        }

        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(new MenuItem()
            {
                Header = "Exit"
            });
            contextMenu.PreviewMouseDown += (o, s) =>
            {
                Close();
            };

            ContextMenu = contextMenu;
        }

        private void SettingWindow_LocationChanged(object sender, System.EventArgs e)
        {
            if (_isLocationLoading == false)
            {
                var dim = Global.ConfigurationManager.Configuration.TriggerWindow_Dimension;
                dim.Left = Left;
                dim.Top = Top;
                dim.Height = Height;
                dim.Width = Width;
            }
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
            _isLocationLoading = true;

            var dim = Global.ConfigurationManager.Configuration.TriggerWindow_Dimension;

            if (dim.Left == 0)
            {
                var v = VisualTreeHelper.GetDpi(this);
                var w = ScreenHelper.Primary.WorkingArea.Width / v.DpiScaleY;
                var t = (int)(w - (int)Width);
                Left = t;
            }
            else
            {
                Left = dim.Left;
            }

            if (dim.Top == 0)
            {
                var v = VisualTreeHelper.GetDpi(this);
                var w = ScreenHelper.Primary.WorkingArea.Height / v.DpiScaleX;
                var t = (int)(w - (int)Height);
                Top = t / 2;
            }
            else
            {
                Top = dim.Top;
            }

            if (dim.Width != 0 && dim.Height != 0)
            {
                SizeToContent = SizeToContent.Manual;
                Width = dim.Width;
                Height = dim.Height;
            }
            else if (dim.Width != 0)
            {
                SizeToContent = SizeToContent.Height;
                Width = dim.Width;
            }
            else if (dim.Height != 0)
            {
                SizeToContent = SizeToContent.Width;
                Height = dim.Height;
            }
            else
            { }

            _isLocationLoading = false;
        }

        private void SettingWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMoveDowned = true;
            _mouseEnterWatch.Restart();
        }
    }
}
