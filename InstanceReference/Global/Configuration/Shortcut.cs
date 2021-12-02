using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstanceReference
{
    class ShortcutCollection
    {
        public Shortcut TriggerWindow_Resize { get; set; } = new Shortcut();
        public Shortcut TriggerWindow_Move { get; set; } = new Shortcut();
        public Shortcut TriggerWindow_Trigger { get; set; } = new Shortcut();

        public Shortcut DictionaryWindow_Resize { get; set; } = new Shortcut();
        public Shortcut DictionaryWindow_ChangeWindowMode { get; set; } = new Shortcut();
        public Shortcut DictionaryWindow_Close { get; set; } = new Shortcut();

        public bool IsValid(Shortcut targetShortcut)
        {
            bool isValid = true;

            var tmpShortcutType = new Shortcut();

            var shortcuts = this.GetType()
                                .GetProperties()
                                .Where(w => w.PropertyType == tmpShortcutType.GetType())
                                .Select(s => s.GetValue(this))
                                .Cast<Shortcut>();

            foreach(var shortcut in shortcuts)
            {
                if (targetShortcut == shortcut)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }

    class Shortcut
    {
        public bool Ctrl { get; set; }
        public bool Atl { get; set; }
        public bool Shift { get; set; }
        public KeyCharactor KeyCharactor { get; set; }
        public MouseAction MouseAction { get; set; }

        public Shortcut()
        {
            Ctrl = false;
            Atl = false;
            Shift = false;
            KeyCharactor = KeyCharactor.None;
            MouseAction = MouseAction.None;
        }
    }

    enum KeyCharactor
    {
        None,
        A, B, C, D, E, F, G, H, I, J, K, L, M,
        N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    enum MouseAction
    {
        None,
        Left_DoubleClick,
        Right_DoubleClick,
        Left_SingleClick,
        Right_SingleClick,
        Hover,
        Leave
    }
}
