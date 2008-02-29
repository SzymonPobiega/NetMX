#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace RemotingDemo
{
	public class Sample : SampleMBean
	{
		#region MEMBERS
		private int _counter;
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
			}
		}
		public void ResetCounter()
		{
			_counter = 0;
		}
		public void AddAmount(int amount)
		{
			_counter += amount;
		}
		#endregion
	}

	public interface SampleMBean
	{
		int Counter { get; set; }
		void ResetCounter();
		void AddAmount(int amount);
	}
}
