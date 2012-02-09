#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote.Remoting
{
	internal class RemotingConnectionImplConfig
	{
		#region MEMBERS
		private string _securityProvider;
		/// <summary>
		/// Gets or sets name of security provider to use by this connection.
		/// </summary>
		public string SecurityProvider
		{
			get { return _securityProvider; }
			set { _securityProvider = value; }
		}
		private int _bufferSize;
		/// <summary>
		/// Gets or sets size of notification buffer to use with this connection.
		/// </summary>
		public int BufferSize
		{
			get { return _bufferSize; }
			set { _bufferSize = value; }
		}
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		#endregion
	}
}
