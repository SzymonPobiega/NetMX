#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace RemotingServerDemo
{
	public class Sample : SampleMBean
	{
		#region MEMBERS
		private int _counter;
		private int _step;
		#endregion

		#region SampleMBean Members
		public int Step
		{
			get { return _step; }
			set { _step = value; }
		}
		public int Counter
		{
			get
			{
				return _counter;
			}			
		}
		public void Increment()
		{
			_counter += _step;
		}
		public void ResetCounter()
		{
			_counter = 0;
			//throw new ApplicationSpecificException();
		}
		public void AddAmount(int amount)
		{
			_counter += amount;
		}
		#endregion
	}
	[Serializable]
	public class ApplicationSpecificException : Exception
	{ }

	public interface SampleMBean
	{
		int Step { get; set; }
		int Counter { get; }
		void Increment();
		void ResetCounter();
		void AddAmount(int amount);
	}
}
