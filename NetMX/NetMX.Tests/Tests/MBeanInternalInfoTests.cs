using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NetMX.Server.InternalInfo;

namespace NetMX.Tests
{
	/// <summary>
	/// Summary description for MBeanInternalInfoTests
	/// </summary>
	[TestFixture]
	public class MBeanInternalInfoTests
	{		
		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[Test]
		public void TestCreateMBeanInfo()
		{
			MBeanInternalInfo info = MBeanInternalInfo.GetCached(typeof(TestMBean));
			int a = 2;
		}		
	}
	[MBeanResource("NetMX.Tests.TestMBeanResource")]
	public interface TestMBean
	{
		[System.ComponentModel.Description("Counter value")]
		int Counter { get; set; }
		[System.ComponentModel.Description("Sets counter value to 0")]
		void ResetCounter();
		//[System.ComponentModel.Description("Adds specified value to value of the counter")]
		void AddAmount(int amount);
		//[System.ComponentModel.Description("Raised when counter value gets changed")]
		[MBeanNotification("sample.counter")]
		event EventHandler<NotificationEventArgs> CounterChanged;
	}
}
