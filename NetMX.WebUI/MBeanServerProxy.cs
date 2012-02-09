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

namespace NetMX.WebUI.WebControls
{
	public sealed class MBeanServerProxy : CompositeControl
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
