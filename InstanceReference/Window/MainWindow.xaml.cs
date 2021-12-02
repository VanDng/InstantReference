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
        private Timer _visibilityTimer;
        private bool _SureToBeClosing;

        public MainWindow()
        {
            InitializeComponent();

            _visibilityTimer = new Timer((s) =>
            {
                _SureToBeClosing = true;
            }, null, Timeout.Infinite, Timeout.Infinite);

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
                _SureToBeClosing = false;
                _visibilityTimer.Change(300, Timeout.Infinite);
            }
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            // There's the problem that MouseLeave events fire immediately after the window shown up, sometimes.
            // A monitor prevents that problem.
            if (_SureToBeClosing)
            {
                ChangeVisibility(Visibility.Hidden);
            }
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

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        public void ChangeVisibility(Visibility visibility)
        {
            // The order of callings is important.
            if (visibility == Visibility.Visible)
            {
                UpdateWindow(visibility);
                Visibility = visibility;
            }
            else
            {
                Visibility = visibility;
                UpdateWindow(visibility);
            }
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
    }
}
