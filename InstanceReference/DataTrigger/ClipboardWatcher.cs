using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private AutoResetEvent _clipboardCheckingEvent;
        private Timer _clipboardCheckingTimer;

        private Thread _threadWatcher;
        private bool _stopped;

        public ClipboardWatcher()
        {
            _stopped = false;

            _clipboardCheckingEvent = new AutoResetEvent(false);
            _clipboardCheckingTimer = new Timer((state) => {
                _clipboardCheckingEvent.Set();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(250));

            _threadWatcher = new Thread(clipboardWatching);
            _threadWatcher.SetApartmentState(ApartmentState.STA);
            _threadWatcher.IsBackground = true;
            _threadWatcher.Start();
        }

        ~ClipboardWatcher()
        {
            _clipboardCheckingEvent.Set();
            _stopped = true;
        }

        private void clipboardWatching()
        {
            string lastText = string.Empty;

            while (true)
            {
                if (_stopped) break;

                _clipboardCheckingEvent.WaitOne();

                //Debug.WriteLine(DateTime.Now.ToString());

                if (_stopped) break;

                string text = string.Empty;
                try
                {
                    text = Clipboard.GetText(TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

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
            }
        }
    }
}
