using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace InstanceReference
{
    class WindowDimension
    {
        private Rect _rect;

        public double Left { get; set; }
        public double Top { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        public WindowDimension()
        {
            _rect.X = 0;
            _rect.Y = 0;
            _rect.Width = 0;
            _rect.Height = 0;
        }
    }
}
