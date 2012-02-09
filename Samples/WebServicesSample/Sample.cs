#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NetMX;
#endregion

namespace WebServicesSample
{
	public class Sample : SampleMBean
	{
		#region MEMBERS
		private int _counter;
		private long _sequenceNumber = 0;
		#endregion

		#region SampleMBean Members
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
		#endregion

		private void OnCounterChanged()
		{
			if (CounterChanged != null)
			{
				CounterChanged(this, new NotificationEventArgs("Counter changed", _counter));
				_sequenceNumber++;
			}
		}
	}

	public interface SampleMBean
	{
		[Description("Counter value")]
		int Counter { get; set; }
		[Description("Sets counter value to 0")]
		void ResetCounter();
		[Description("Adds specified value to value of the counter")]
		void AddAmount(int amount);
		[Description("Raised when counter value gets changed")]
		[MBeanNotification("sample.counter")]
		event EventHandler<NotificationEventArgs> CounterChanged;
	}
}
