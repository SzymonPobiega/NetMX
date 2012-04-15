using System;

namespace NetMX.Remote.Remoting.Internal
{
	[Serializable]
	public class NotificationFetcherConfig
	{
		private readonly bool _proactive;
		public bool Proactive
		{
			get { return _proactive; }
		}
		private readonly TimeSpan _fetchDelay;
		public TimeSpan FetchDelay
		{
			get { return _fetchDelay; }
		}

	    private readonly int _maxNotificationBatchSize;

	    public int MaxNotificationBatchSize
		{
			get { return _maxNotificationBatchSize; }
		}

        public NotificationFetcherConfig(bool proactive, TimeSpan fetchDelay)
            : this(proactive, fetchDelay, int.MaxValue)
        {
        }

	    public NotificationFetcherConfig(bool proactive, TimeSpan fetchDelay, int maxNotificationBatchSize)
        {
            _proactive = proactive;
            _fetchDelay = fetchDelay;
            _maxNotificationBatchSize = maxNotificationBatchSize;
        }
	}
}
