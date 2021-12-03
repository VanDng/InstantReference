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
    partial class TriggerWindow
        : Window
    {
        public event TriggeredHandler OnTriggered;

        public delegate void TriggeredHandler(TriggerWindow triggerWindowHandle);

        private Timer _beginMovingTimer;
        private bool _isMovingState;
        private bool _isEnteredDragState;

        private bool _isLocationLoading;

        private double _opacityFocus = 1;
        private double _opacityNotFocus = 0.7;
        public TriggerWindow()
        {
            // Do not know why but these two line of setting make the option Window.SizeToContent work!!
            // Can't beleive it.
            //Width = 0;
            //Height = 0;

            _beginMovingTimer = new Timer(beginDragging, null, Timeout.Infinite, Timeout.Infinite);

            InitializeComponent();

            InitializeContextMenu();

            Loaded += SettingWindow_Loaded;
            LocationChanged += SettingWindow_LocationChanged;

            PreviewMouseLeftButtonDown += SettingWindow_PreviewMouseDown;
            PreviewMouseLeftButtonUp += SettingWindow_PreviewMouseUp;
            PreviewMouseMove += SettingWindow_PreviewMouseMove;

            //MouseUp += SettingWindow_PreviewMouseUp;
            MouseLeave += SettingWindow_MouseLeave;
            MouseEnter += TriggerWindow_MouseEnter;

            ContentRendered += TriggerWindow_ContentRendered;

            Opacity = 0;
        }

        private void TriggerWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            Opacity = _opacityFocus;
        }

        public void SetStatus(TriggerStatus triggerStatus)
        {
            string iconName = string.Empty;

            switch (triggerStatus)
            {
                case TriggerStatus.LookupInProgress:
                    iconName = "loading.gif";
                        break;

                case TriggerStatus.LookupCompleted:
                    iconName = "ok_4.gif";
                    break;

                default:
                    throw new Exception("Not supported trigger status");
            }

            string resourcePath = $"pack://application:,,,/Window/Icon/{iconName}";

            indicator.Dispatcher.InvokeAsync(() =>
            {
                AnimationBehavior.SetSourceUri(indicator, new Uri(resourcePath));
            });
        }
        private void TriggerWindow_ContentRendered(object sender, EventArgs e)
        {
            adjustCircleElement();

            Opacity = _opacityNotFocus;
        }

        private void beginDragging(object state)
        {
            draggingIndicator.Dispatcher.InvokeAsync(() =>
            {
                // Yes, Opacity solves the problem of display delaying.
                // It's there for a reason !
                //draggingIndicator.Visibility = Visibility.Visible;
                draggingIndicator.Opacity = 1;
            });

            _isMovingState = true;
        }

        private void stopDragging()
        {
            _beginMovingTimer.Change(Timeout.Infinite, Timeout.Infinite);

            if (_isMovingState || _isEnteredDragState)
            {
                draggingIndicator.Dispatcher.InvokeAsync(() =>
                {
                    //draggingIndicator.Visibility = Visibility.Collapsed;
                    draggingIndicator.Opacity = 0;
                });
            }

            _isMovingState = false;
            _isEnteredDragState = false;
        }

        private void SettingWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            Opacity = _opacityNotFocus;
            stopDragging();
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

            stopDragging();
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
            _beginMovingTimer.Change((int)(TimeSpan.FromSeconds(1).TotalMilliseconds), Timeout.Infinite);
        }

        private void adjustCircleElement()
        {
            /*
             * Haha if you are good at math and have a better solution, tell me.
             * For now, I use the try and error approach again haha
             */

            double actualDiagonal = Math.Sqrt(Math.Pow(indicator.ActualWidth, 2) + Math.Pow(indicator.ActualHeight, 2));

            double expecteddiagonal = circle.ActualWidth;

            double newWidth = indicator.ActualWidth;
            double newHeight = indicator.ActualHeight;

            double step = 0.1;

            if (expecteddiagonal >= actualDiagonal)
            {
                while (expecteddiagonal >= Math.Sqrt(Math.Pow(newWidth + step, 2) + Math.Pow(newHeight + step, 2)))
                {
                    newWidth += step;
                    newHeight += step;
                }
            }
            else if (expecteddiagonal <= actualDiagonal)
            {
                step = step * -1;
                while (expecteddiagonal <= Math.Sqrt(Math.Pow(newWidth + step, 2) + Math.Pow(newHeight + step, 2)))
                {
                    newWidth += step;
                    newHeight += step;
                }
            }

            indicator.Width = newWidth;
            indicator.Height = newHeight;

            draggingIndicator.Width = newWidth;
            draggingIndicator.Height = newHeight;
        }
    }
}
