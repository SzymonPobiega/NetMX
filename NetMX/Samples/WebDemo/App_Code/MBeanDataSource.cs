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
using System.ComponentModel;
#endregion

namespace Controls
{
	public class MBeanDataSource : DataSourceControl
	{
		#region MEMBERS
		private MBeanInfo _info;
		private MBeanAttributesDataSourceView _attributesView;
		private MBeanOperationsDataSourceView _operationsView;
		#endregion

		#region PROPERTIES
		private string _mBeanServerProxyID;
		/// <summary>
		/// ID of MBeanServerProxy control
		/// </summary>
		public string MBeanServerProxyID
		{
			get { return _mBeanServerProxyID; }
			set { _mBeanServerProxyID = value; }
		}
		private ObjectName _objectName;
		/// <summary>
		/// ObjectName of displayed/edited MBean
		/// </summary>
		public string ObjectName
		{
			get { return _objectName.ToString(); }
			set { _objectName = new ObjectName(value); }
		}
		private MBeanServerProxy _proxy;
		private MBeanServerProxy Proxy
		{
			get
			{
				if (_proxy == null)
				{
					Control container = this.NamingContainer;
					while (container != null)
					{
						_proxy = (MBeanServerProxy)container.FindControl(MBeanServerProxyID);
						if (_proxy != null)
						{
							_info = _proxy.ServerConnection.GetMBeanInfo(_objectName);
							break;
						}
						else
						{
							container = container.NamingContainer;
						}
					}										
				}
				return _proxy;
			}
		}
		#endregion

		#region INTERFACE
		//public void GetDescriptionAndClassName(ObjectName objectName, out string description, out string className)
		//{
		//   IMBeanServerConnection remoteServer = _connector.MBeanServerConnection;
		//   MBeanInfo beanInfo = remoteServer.GetMBeanInfo(objectName);
		//   description = beanInfo.Description;
		//   className = beanInfo.ClassName;
		//}
		#endregion

		protected override DataSourceView GetView(string viewName)
		{
			if (_attributesView == null)
			{
				_attributesView = new MBeanAttributesDataSourceView(Proxy.ServerConnection, _info, _objectName, this, "Attributes");
				_operationsView = new MBeanOperationsDataSourceView(Proxy.ServerConnection, _info, _objectName, this, "Operations");
			}
			if (viewName == "Attributes")
			{
				return _attributesView;
			}
			else if (viewName == "Operations")
			{
				return _operationsView;
			}
			else
			{
				throw new NotSupportedException("Not supported view: " + viewName);
			}
		}

		#region ATTRIBUTES VIEW
		private class MBeanAttributesDataSourceView : DataSourceView
		{
			private IMBeanServerConnection _connection;
			private MBeanInfo _info;			
			private ObjectName _objectName;

			public MBeanAttributesDataSourceView(IMBeanServerConnection connection, MBeanInfo info, ObjectName objectName, IDataSource owner, string viewName)
				: base(owner, viewName)
			{
				_connection = connection;
				_info = info;
				_objectName = objectName;
			}

			protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
			{				
				List<MBeanAttribute> results = new List<MBeanAttribute>();
				foreach (MBeanAttributeInfo attributeInfo in _info.Attributes)
				{
					string value = null;
					if (attributeInfo.Readable)
					{
						object o = _connection.GetAttribute(_objectName, attributeInfo.Name);
						if (o != null)
						{
							value = o.ToString();
						}
						else
						{
							value = string.Empty;
						}
					}
					results.Add(new MBeanAttribute(attributeInfo, value));
				}
				return results;
			}			
			protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
			{
				string attributeName = (string)keys["Name"];
				foreach (MBeanAttributeInfo attribute in _info.Attributes)
				{
					if (attribute.Name == attributeName)
					{
						TypeConverter tc = TypeDescriptor.GetConverter(Type.GetType(attribute.Type, true));
						_connection.SetAttribute(_objectName, attributeName, tc.ConvertFromString((string)values["Value"]));
						return 1;
					}
				}				
				return 0;
			}
		}
		#endregion

		#region OPERATIONS VIEW
		private class MBeanOperationsDataSourceView : DataSourceView
		{
			private IMBeanServerConnection _connection;
			private MBeanInfo _info;
			private ObjectName _objectName;

			public MBeanOperationsDataSourceView(IMBeanServerConnection connection, MBeanInfo info, ObjectName objectName, IDataSource owner, string viewName)
				: base(owner, viewName)
			{
				_connection = connection;
				_info = info;
				_objectName = objectName;
			}
			protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
			{
				return _info.Operations;
			}
			protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
			{
				string operationName = (string)keys["Name"];
				foreach (MBeanOperationInfo operation in _info.Operations)
				{
					if (operation.Name == operationName)
					{
						List<object> argumentList = new List<object>();
						foreach (MBeanParameterInfo param in operation.Signature)
						{
							TypeConverter tc = TypeDescriptor.GetConverter(Type.GetType(param.Type, true));
							argumentList.Add(tc.ConvertFromString((string)values[param.Name]));
						}
						_connection.Invoke(_objectName, operationName, argumentList.ToArray());
						return 1;
					}
				}
				return 0;
			}
		}
		#endregion
	}
}