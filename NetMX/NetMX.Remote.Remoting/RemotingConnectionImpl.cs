#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.Security.Principal;
#endregion

namespace NetMX.Remote.Remoting
{
	public sealed class RemotingConnectionImpl : MarshalByRefObject, IRemotingConnection
	{
		#region MEMBERS
		private IMBeanServer _server;
        private object _subject;
        private string _secutityProvider;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnectionImpl(IMBeanServer server, object subject, string securityProvider)
		{
			_server = server;
            _subject = subject;
            _secutityProvider = securityProvider;
		}
		#endregion

		#region IRemotingConnection Members
		public object Invoke(ObjectName name, string operationName, object[] arguments)
		{
            IPrincipal p = Thread.CurrentPrincipal;
            try
            {
                Thread.CurrentPrincipal = NetMXSecurityService.Authorize(_secutityProvider, _subject);
                return _server.Invoke(name, operationName, arguments);
            }
            finally
            {
                Thread.CurrentPrincipal = p;
            }
		}
		public void SetAttribute(ObjectName name, string attributeName, object value)
		{
			_server.SetAttribute(name, attributeName, value);
		}
		public object GetAttribute(ObjectName name, string attributeName)
		{
			return _server.GetAttribute(name, attributeName);
		}
		public MBeanInfo GetMBeanInfo(ObjectName name)
		{
			return _server.GetMBeanInfo(name);
		}
		public void Close()
		{
			RemotingServices.Disconnect(this);
		}
		#endregion        
	}
}
