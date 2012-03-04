using System;
using System.ComponentModel;
using NetMX;

namespace Jsr262Demo
{
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