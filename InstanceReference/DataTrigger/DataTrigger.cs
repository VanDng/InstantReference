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
                data = data.ToLower();

                OnDataArrived(data);
                _previousText = data;
            }
        }

        private bool IsNeedToTrigger(string text)
        {
            bool needToTrigger = false;

            if (text.Length > 0 &&
                text.Contains('.') == false &&
                text.Length < 50 &&
                text.Any(s => s >= 0 && s <= 9) == false)
            {
                if (text.Length == _previousText.Length && // Avoid large text comparison
                    _previousText == text)
                {
                }
                else
                {
                    needToTrigger = true;
                }
            }

            return needToTrigger;
        }
    }
}
