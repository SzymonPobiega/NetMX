#region USING
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetMX;
using NetMX.Remote;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Controls
{
	public class MBeanServerProxy : WebControl
	{
		#region MEMBERS
		private INetMXConnector _connector;
		#endregion

		#region PROPERTIES
		private string _serviceUrl;
		/// <summary>
		/// URL of remote server connector.
		/// </summary>
		public string ServiceUrl
		{
			get { return _serviceUrl; }
			set { _serviceUrl = value; }
		}
		public IMBeanServerConnection ServerConnection
		{
			get { return _connector.MBeanServerConnection; }
		}
		#endregion

		#region INTERFACE
		public void GetDescriptionAndClassName(ObjectName objectName, out string description, out string className)
		{
			IMBeanServerConnection remoteServer = _connector.MBeanServerConnection;
			MBeanInfo beanInfo = remoteServer.GetMBeanInfo(objectName);
			description = beanInfo.Description;
			className = beanInfo.ClassName;
		}
		//public IEnumerable GetOperations(ObjectName objectName)
		//{
		//   IMBeanServerConnection remoteServer = _connector.MBeanServerConnection;
		//   MBeanInfo beanInfo = remoteServer.GetMBeanInfo(objectName);
		//   if (beanInfo != null)
		//   {
		//      return beanInfo.Operations;
		//   }
		//   return null;
		//}
		//public void SetAttribute(ObjectName objectName, string attributeName, object value)
		//{
		//   IMBeanServerConnection remoteServer = _connector.MBeanServerConnection;
		//   remoteServer.SetAttribute(objectName, attributeName, value);
		//}
		public void Invoke(ObjectName objectName, string operationName, object[] arguments)
		{
			IMBeanServerConnection remoteServer = _connector.MBeanServerConnection;
			remoteServer.Invoke(objectName, operationName, arguments);
		}
		#endregion

		#region OVERRIDDEN
      protected override void OnInit(EventArgs e)
      {
         base.OnInit(e);
         _connector = NetMXConnectorFactory.Connect(new Uri(ServiceUrl), null);			
      }		
		public override void Dispose()
		{
			base.Dispose();
			if (_connector != null)
			{
				_connector.Dispose();
			}
		}		
		#endregion		
	}
}
