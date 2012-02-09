using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetMX.Remote.Remoting.Internal
{	
	internal class NotificationFetcher : IDisposable
	{
		private bool _disposed;		
		private Timer _timer;
		private IRemotingConnection _connection;
		private RemotingMBeanServerConnection _serverConnection;
		private int _maxBatchSize;

		private int _nextPendingNotification;
		public int NextPendingNotification
		{
			get { return _nextPendingNotification; }
		}		
		
		public NotificationFetcher(NotificationFetcherConfig fetcherConfig, IRemotingConnection connection, RemotingMBeanServerConnection serverConnection)
		{
			_connection = connection;
			_serverConnection = serverConnection;
			_maxBatchSize = fetcherConfig.MaxNotificationBatchSize;
			if (fetcherConfig.Proactive)
			{
				_timer = new Timer(FetchNotifications, null, TimeSpan.Zero, fetcherConfig.FetchDelay);
			}
			else
			{
				FetchNotifications(null);
			}
		}

		private void FetchNotifications(object state)
		{
			NotificationResult result = _connection.FetchNotifications(_nextPendingNotification, _maxBatchSize);
			_nextPendingNotification = result.NextSequenceNumber;
			foreach (TargetedNotification notif in result.TargetedNotifications)
			{
				_serverConnection.Notify(notif);
			}
		}

		#region IDisposable Members
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (_timer != null)
					{
						using (EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset))
						{
							_timer.Dispose(handle);
							handle.WaitOne();
						}
					}
				}
				_disposed = true;
			}
		}
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
