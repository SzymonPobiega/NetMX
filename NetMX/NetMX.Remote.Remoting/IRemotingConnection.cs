using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote.Remoting
{
	public interface IRemotingConnection
	{
		object Invoke(ObjectName name, string operationName, object[] arguments);

		void SetAttribute(ObjectName name, string attributeName, object value);
		object GetAttribute(ObjectName name, string attributeName);

		MBeanInfo GetMBeanInfo(ObjectName name);

		void Close();
	}
}
