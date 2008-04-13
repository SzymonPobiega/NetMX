#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	public interface IMBeanServer : IMBeanServerConnection
	{
		void RegisterMBean(object bean, ObjectName name);		
	}
}
