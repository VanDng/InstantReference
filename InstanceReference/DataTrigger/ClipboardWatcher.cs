using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace InstanceReference
{
    partial class ClipboardWatcher
    {
        public class ClipBoardText
        {
            public string Value { get; set; }
        }

        public event DataArrivedHandler OnTextArrived;

        public delegate void DataArrivedHandler(ClipBoardText clipBoardText);
    }

    partial class ClipboardWatcher
    {
        private TimeSpan _pollingInterval = TimeSpan.FromSeconds(0.5);

        private Thread _threadWatcher;
        private bool _stopped;

        public ClipboardWatcher()
        {
            _stopped = false;

            _threadWatcher = new Thread(clipboardWatching);
            _threadWatcher.SetApartmentState(ApartmentState.STA);
            _threadWatcher.IsBackground = true;
            _threadWatcher.Start();
        }

        ~ClipboardWatcher()
        {
            _stopped = true;
        }

        private void clipboardWatching()
        {
            string lastText = string.Empty;

            while (_stopped == false)
            {
                string text = string.Empty;
                try
                {
                    text = Clipboard.GetText(TextDataFormat.Text);
                }
                catch
                { }

                if (text.Length > 0 &&
                    text.Length < 50)
                {
                    if (lastText != text)
                    {
                        OnTextArrived(new ClipBoardText()
                        {
                            Value = text
                        });

                        lastText = text;
                    }
                }

                Thread.Sleep((int)_pollingInterval.TotalMilliseconds);
            }
        }
    }
}
