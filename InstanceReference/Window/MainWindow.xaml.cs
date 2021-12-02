using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppBar;
using Microsoft.Win32;

namespace InstanceReference
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingWindow _triggerWindow;

        public MainWindow()
        {
            InitializeComponent();

            _triggerWindow = new SettingWindow(this);
            _triggerWindow.Visibility = Visibility.Hidden;

            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
            MouseLeave += MainWindow_MouseLeave;

            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            HideWindow(true);
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            UpdateWindow(Visibility);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
        
            //Make the window an appbar:
            //AppBarFunctions.SetAppBar(this, ABEdge.None);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HideWindow(true);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void UpdateWindow(Visibility visibility)
        {
            var v = VisualTreeHelper.GetDpi(this);

            var h = ScreenHelper.Primary.WorkingArea.Height / v.DpiScaleX;
            var w = ScreenHelper.Primary.WorkingArea.Width / v.DpiScaleY;

            Top = 0;
            Height = h;

            if (visibility == Visibility.Hidden)
            {
                Left = w;
            }
            else
            {
                Left = w - Width;
            }
        }

        public void HideWindow(bool isHidden)
        {
            if (isHidden)
            {
                _triggerWindow.Visibility = Visibility.Visible;
                
                UpdateWindow(Visibility.Hidden);
                Visibility = Visibility.Hidden;
            }
            else
            {
                _triggerWindow.Visibility = Visibility.Hidden;

                UpdateWindow(Visibility.Visible);
                Visibility = Visibility.Visible;
            }
        }
    }
}
