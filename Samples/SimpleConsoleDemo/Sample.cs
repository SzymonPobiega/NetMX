#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using System.ComponentModel;
#endregion

namespace SimpleConsoleDemo
{
	public class Counter : CounterMBean
	{
		private int _counter;

		public int Value
		{
			get { return _counter; }
			set
			{
				_counter = value;
				OnCounterChanged();
			}
		}
		public void Reset()
		{
			_counter = 0;
			OnCounterChanged();
		}
		public void Add(int amount)
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
			}
		}
	}

	public interface CounterMBean
	{
		[Description("Counter value")]
		int Value { get; set; }
		[Description("Sets counter value to 0")]
		void Reset();
		[Description("Adds specified value to value of the counter")]
		void Add(int amount);
		[Description("Raised when counter value gets changed")]
		[MBeanNotification("sample.counter")]
		event EventHandler<NotificationEventArgs> CounterChanged;
	}
}
