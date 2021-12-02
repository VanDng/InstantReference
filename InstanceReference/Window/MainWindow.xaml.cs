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
using System.Diagnostics;

namespace InstanceReference
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch _visibilityMonitor;

        public MainWindow()
        {
            InitializeComponent();

            _visibilityMonitor = new Stopwatch();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
            MouseLeave += MainWindow_MouseLeave;

            IsVisibleChanged += MainWindow_IsVisibleChanged;

            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
            {
                _visibilityMonitor.Restart();
            }

            UpdateWindow();
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            // There's the problem that MouseLeave events fire immediately after the window shown up, sometimes.
            // The monitor prevents that problem.
            if (_visibilityMonitor.Elapsed.TotalMilliseconds >= 300)
            {
                Visibility = Visibility.Hidden;
            }
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            UpdateWindow();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //Make the window an appbar:
            //AppBarFunctions.SetAppBar(this, ABEdge.None);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void UpdateWindow()
        {
            var v = VisualTreeHelper.GetDpi(this);

            var h = ScreenHelper.Primary.WorkingArea.Height / v.DpiScaleX;
            var w = ScreenHelper.Primary.WorkingArea.Width / v.DpiScaleY;

            Top = 0;
            Height = h;

            if (Visibility == Visibility.Hidden)
            {
                Left = w;
            }
            else
            {
                Left = w - Width;
            }
        }
    }
}
