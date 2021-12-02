using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WinKey = System.Windows.Input.Key;

namespace InstanceReference
{
    static class ShortcutHelper
    {
        public static bool Shift
        {
            get
            {
                return Keyboard.IsKeyDown(WinKey.LeftShift) || Keyboard.IsKeyDown(WinKey.RightShift);
            }
        }

        public static bool Ctrl
        {
            get
            {
                return Keyboard.IsKeyDown(WinKey.LeftCtrl) || Keyboard.IsKeyDown(WinKey.RightCtrl);
            }
        }

        public static bool Atl
        {
            get
            {
                return Keyboard.IsKeyDown(WinKey.LeftAlt) || Keyboard.IsKeyDown(WinKey.RightAlt);
            }
        }

        public static bool Key(KeyCharactor keychar)
        {
            if (keychar.ToString() == WinKey.A.ToString()) return Keyboard.IsKeyDown(WinKey.A);

            return false;
        }
    }
}
