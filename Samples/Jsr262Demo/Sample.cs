#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX;

#endregion

namespace Jsr262Demo
{
    public class Sample : SampleMBean
    {
        private int _counter;
        private long _sequenceNumber = 0;

        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
                OnCounterChanged();
            }
        }
        public void ResetCounter()
        {
            _counter = 0;
            OnCounterChanged();
        }
        public void AddAmount(int amount)
        {
            _counter += amount;
            OnCounterChanged();
        }
        public event EventHandler<NotificationEventArgs> CounterChanged;

        private void OnCounterChanged()
        {
            if (CounterChanged != null)
            {
                CounterChanged(this, new NotificationEventArgs("Counter changed", _counter));
                _sequenceNumber++;
            }
        }
    }
}