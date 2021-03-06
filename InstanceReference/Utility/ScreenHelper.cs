using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace InstanceReference
{
    // Reference:
    // https://stackoverflow.com/questions/1927540/how-to-get-the-size-of-the-current-screen-in-wpf

    public class ScreenHelper
    {
        public static IEnumerable<ScreenHelper> AllScreens()
        {
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new ScreenHelper(screen);
            }
        }

        public static ScreenHelper GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            ScreenHelper ScreenHelper = new ScreenHelper(screen);
            return ScreenHelper;
        }

        public static ScreenHelper GetScreenFrom(System.Windows.Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            ScreenHelper ScreenHelper = new ScreenHelper(screen);

            return ScreenHelper;
        }

        public static ScreenHelper Primary
        {
            get { return new ScreenHelper(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        private readonly Screen screen;

        internal ScreenHelper(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }

        public Rect DeviceBounds
        {
            get { return this.GetRect(this.screen.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return this.GetRect(this.screen.WorkingArea); }
        }

        private Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public bool IsPrimary
        {
            get { return this.screen.Primary; }
        }

        public string DeviceName
        {
            get { return this.screen.DeviceName; }
        }
    }
}
