#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Default
{
	public class MBeanServer : IMBeanServer
	{
		#region MEMBERS
		private Dictionary<ObjectName, IDynamicMBean> _beans = new Dictionary<ObjectName, IDynamicMBean>();
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		#endregion

		#region UTILITY
		private void RegisterMBeanInternal(ObjectName name, IDynamicMBean bean)
		{
			IMBeanRegistration registration = new MBeanRegistrationHelper(bean as IMBeanRegistration);
			name = registration.PreRegister(this, name);

			string className = bean.GetMBeanInfo().ClassName;
			TestPermissions(className, null, name, MBeanPermissionAction.RegisterMBean);			
			if (_beans.ContainsKey(name))
			{
				registration.PostRegister(false);
				throw new InstanceAlreadyExistsException(name.ToString());
			}
			_beans[name] = bean;
			registration.PostRegister(true);
		}
		private INotficationEmitter GetEmitterMBean(ObjectName name, out IDynamicMBean bean)
		{
			bean = GetMBean(name);
			INotficationEmitter emitter = bean as INotficationEmitter;
			if (emitter != null)
			{
				return emitter;
			}
			throw new OperationsException(string.Format("Bean \"{0}\" is not a notification emitter.", name.ToString()));
		}
		private IDynamicMBean GetMBean(ObjectName name)
		{
			IDynamicMBean bean;
			if (_beans.TryGetValue(name, out bean))
			{
				return bean;
			}
			throw new InstanceNotFoundException(name.ToString());
		}
		private void TestPermissions(string className, string memberName, ObjectName name, MBeanPermissionAction action)
		{
			MBeanCASPermission casPerm = new MBeanCASPermission(className, memberName, name, action);
			casPerm.Demand();
			MBeanPermission perm = new MBeanPermission(className, memberName, name, action);
			perm.Demand();
		}
		#endregion

		#region IMBeanServer Members
		public void RegisterMBean(object bean, ObjectName name)
		{
			IDynamicMBean dynBean = bean as IDynamicMBean;
			if (dynBean != null)
			{
				RegisterMBeanInternal(name, dynBean);
			}
			else
			{
				Type beanType = bean.GetType();
				Type intfType = null;
				while (beanType != null)
				{
					intfType = beanType.GetInterface(beanType.Name + "MBean", false);
					if (intfType != null)
					{
						break;
					}
					beanType = beanType.BaseType;
				}				
				if (intfType == null)
				{
					throw new NotCompliantMBeanException(beanType.AssemblyQualifiedName);
				}
				StandardMBean stdBean = new StandardMBean(bean, intfType);
				RegisterMBeanInternal(name, stdBean);
			}
		}

		public object Invoke(ObjectName name, string operationName, object[] arguments)
		{
			IDynamicMBean bean = GetMBean(name);
			TestPermissions(bean.GetMBeanInfo().ClassName, operationName, name, MBeanPermissionAction.Invoke);
			try
			{				
				return bean.Invoke(operationName, arguments);
			}
			catch (Exception e)
			{
				throw new MBeanException("Exception during 'invoke' operation.", e);
			}
		}

		public void SetAttribute(ObjectName name, string attributeName, object value)
		{
			IDynamicMBean bean = GetMBean(name);
			TestPermissions(bean.GetMBeanInfo().ClassName, attributeName, name, MBeanPermissionAction.SetAttribute);	
			bean.SetAttribute(attributeName, value);
		}

		public object GetAttribute(ObjectName name, string attributeName)
		{
			IDynamicMBean bean = GetMBean(name);
			TestPermissions(bean.GetMBeanInfo().ClassName, attributeName, name, MBeanPermissionAction.GetAttribute);	
			return bean.GetAttribute(attributeName);
		}

		public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
		{
			IDynamicMBean bean = GetMBean(name);
			string className = bean.GetMBeanInfo().ClassName;
			List<AttributeValue> results = new List<AttributeValue>();
			foreach (string attributeName in attributeNames)
			{
				TestPermissions(className, attributeName, name, MBeanPermissionAction.GetAttribute);
				try
				{
					results.Add(new AttributeValue(attributeName, bean.GetAttribute(attributeName)));
				}
				catch (AttributeNotFoundException)
				{
				}
			}
			return results;
		}

		public MBeanInfo GetMBeanInfo(ObjectName name)
		{
			IDynamicMBean bean = GetMBean(name);
			MBeanInfo info = bean.GetMBeanInfo();
			TestPermissions(info.ClassName, null, name, MBeanPermissionAction.GetMBeanInfo);	
			return info;
		}
		public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{
			IDynamicMBean bean;
			INotficationEmitter emitter = GetEmitterMBean(name, out bean);
			TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.AddNotificationListener);
			emitter.AddNotificationListener(callback, filterCallback, handback);
		}

		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{
			IDynamicMBean bean;
			INotficationEmitter emitter = GetEmitterMBean(name, out bean);
			TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.RemoveNotificationListener);
			emitter.RemoveNotificationListener(callback, filterCallback, handback);
		}

		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
		{
			IDynamicMBean bean;
			INotficationEmitter emitter = GetEmitterMBean(name, out bean);
			TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.RemoveNotificationListener);
			emitter.RemoveNotificationListener(callback);
		}		
		public bool IsInstanceOf(ObjectName name, string className)
		{
			IDynamicMBean bean = GetMBean(name);
			MBeanInfo info = bean.GetMBeanInfo();
			TestPermissions(info.ClassName, null, name, MBeanPermissionAction.IsInstanceOf);			
			return info.ClassName == className;			
		}
		public bool IsRegistered(ObjectName name)
		{
			return _beans.ContainsKey(name);
		}
		public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
		{
			List<ObjectName> results = new List<ObjectName>(_beans.Keys);
			return results;
		}
		public void UnregisterMBean(ObjectName name)
		{
			IDynamicMBean bean = GetMBean(name);
			MBeanInfo info = bean.GetMBeanInfo();

			TestPermissions(info.ClassName, null, name, MBeanPermissionAction.UnregisterMBean);			

			IMBeanRegistration registration = new MBeanRegistrationHelper(bean as IMBeanRegistration);
			registration.PreDeregister();
			_beans.Remove(name);
			registration.PostDeregister();
		}
		#endregion
	}
}
