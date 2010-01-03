using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace NetMX.Remote.Remoting.Internal
{
	[Serializable]
	internal class NotificationFetcherConfig
	{
		private bool _proactive;
		public bool Proactive
		{
			get { return _proactive; }
		}
		private TimeSpan _fetchDelay;
		public TimeSpan FetchDelay
		{
			get { return _fetchDelay; }
		}
		private int _maxNotificationBatchSize = int.MaxValue;
		public int MaxNotificationBatchSize
		{
			get { return _maxNotificationBatchSize; }
		}

		public NotificationFetcherConfig(NameValueCollection config)
		{
			if (!string.IsNullOrEmpty(config["notificationFetchPolicy"]))
			{
				_proactive = string.Compare(config["notificationFetchPolicy"], "Proactive", true) == 0; 
				if (!string.IsNullOrEmpty(config["notificationFetchDelay"]))
				{
					_fetchDelay = TimeSpan.Parse(config["notificationFetchDelay"]);
				}
				else
				{
					_fetchDelay = new TimeSpan(0, 0, 5);
				}
			}
			if (!string.IsNullOrEmpty(config["maxNotificationBatchSize"]))
			{
				_maxNotificationBatchSize = int.Parse(config["maxNotificationBatchSize"]);
			}
		}		
	}
}
