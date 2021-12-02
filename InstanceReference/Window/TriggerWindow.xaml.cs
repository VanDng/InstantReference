using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using XamlAnimatedGif;

namespace InstanceReference
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TriggerWindow
        : Window
    {
        public event TriggeredHandler OnTriggered;

        public delegate void TriggeredHandler(TriggerWindow triggerWindowHandle);

        private Timer _beginMovingTimer;
        private bool _isMovingState;
        private bool _isEnteredDragState;

        private bool _isLocationLoading;

        public TriggerWindow()
        {
            // Do not know why but these two line of setting make the option Window.SizeToContent work!!
            // Can't beleive it.
            Width = 0;
            Height = 0;

            _beginMovingTimer = new Timer(BeginMovingCallback, null, Timeout.Infinite, Timeout.Infinite);

            InitializeComponent();

            InitializeContextMenu();

            Loaded += SettingWindow_Loaded;
            LocationChanged += SettingWindow_LocationChanged;

            PreviewMouseLeftButtonDown += SettingWindow_PreviewMouseDown;
            PreviewMouseLeftButtonUp += SettingWindow_PreviewMouseUp;
            PreviewMouseMove += SettingWindow_PreviewMouseMove;

            //MouseUp += SettingWindow_PreviewMouseUp;
            MouseLeave += SettingWindow_MouseLeave;
        }

        private void BeginMovingCallback(object state)
        {
            indicator.Dispatcher.InvokeAsync(() =>
            {
                AnimationBehavior.SetSourceUri(indicator, new Uri("pack://application:,,,/Window/Icon/drag.gif"));
            });
            _isMovingState = true;
        }

        private void SettingWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            stopMoving();
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
            if (_isMovingState)
            {
                if (_isEnteredDragState == false)
                {
                    _isEnteredDragState = true;
                    DragMove();
                }
            }
        }

        private void SettingWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isMovingState == false)
            {
                Visibility = Visibility.Hidden;
                OnTriggered?.Invoke(this);
            }

            stopMoving();
        }

        private void stopMoving()
        {
            _beginMovingTimer.Change(Timeout.Infinite, Timeout.Infinite);

            if (_isMovingState || _isEnteredDragState)
            {
                AnimationBehavior.SetSourceUri(indicator, new Uri("pack://application:,,,/Window/Icon/check.gif"));
            }

            _isMovingState = false;
            _isEnteredDragState = false;
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
            _beginMovingTimer.Change((int)(TimeSpan.FromSeconds(2).TotalMilliseconds), Timeout.Infinite);
        }
    }
}
