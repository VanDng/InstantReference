using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstanceReference
{
    partial class DataTrigger
    {
        public event DataArrivedHandler OnDataArrived;

        public delegate void DataArrivedHandler(string text);
    }

    partial class DataTrigger
    {
        private string _previousText;

        public DataTrigger()
        {
            _previousText = string.Empty;
        }

        public void Push(string data)
        {
            if (IsNeedToTrigger(data))
            {
                OnDataArrived(data);
                _previousText = data;
            }
        }

        private bool IsNeedToTrigger(string text)
        {
            bool needToTrigger = false;

            text = text.ToLower()
                       .Trim();

            if (text.Length > 0 &&
                text.Length < 50 &&
                text.Any(s => (s < 'a' || s > 'z') && (s != ' ')) == false)
            {
                if (_previousText != text)
                {
                    needToTrigger = true;
                }
            }

            return needToTrigger;
        }
    }
}
