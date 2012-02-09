#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	public class NotificationSubscription
	{
		private NotificationCallback _callback;
		public NotificationCallback Callback
		{
			get { return _callback; }
		}
		private NotificationFilterCallback _filterCallback;
		public NotificationFilterCallback FilterCallback
		{
			get { return _filterCallback; }
		}
		private object _handback;
		public object Handback
		{
			get { return _handback; }
		}

		public NotificationSubscription(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{
			_callback = callback;
			_filterCallback = filterCallback;
			_handback = handback;
		}

		public override bool Equals(object obj)
		{
			NotificationSubscription other = obj as NotificationSubscription;
			if (other != null)
			{
				return this._callback.Equals(other._callback) &&
					 ((this._filterCallback == null && other._filterCallback == null) ||
					 (this._filterCallback != null && other._filterCallback != null && this._filterCallback.Equals(other._filterCallback))) &&
					 ((this._handback == null && other._handback == null) ||
					 (this._handback != null && other._handback != null && this._handback.Equals(other._handback)));
			}
			return false;
		}

		public override int GetHashCode()
		{
			int code = _callback.GetHashCode();
			if (_filterCallback != null)
			{
				code ^= _filterCallback.GetHashCode();
			}
			if (_handback != null)
			{
				code ^= _handback.GetHashCode();
			}
			return code;
		}
	}
}
