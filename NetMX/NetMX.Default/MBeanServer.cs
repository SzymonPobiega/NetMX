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
      private string _defaultDomain = "root";
		private Dictionary<ObjectName, IDynamicMBean> _beans = new Dictionary<ObjectName, IDynamicMBean>();
      private Dictionary<string, bool> _domainSet = new Dictionary<string, bool>();
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		#endregion

		#region UTILITY
      private ObjectInstance RegisterMBeanExternal(object bean, ObjectName name)
      {
         IDynamicMBean dynBean = bean as IDynamicMBean;
         if (dynBean != null)
         {
            return RegisterMBeanInternal(name, dynBean);
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
            return RegisterMBeanInternal(name, stdBean);
         }
      }
		private ObjectInstance RegisterMBeanInternal(ObjectName name, IDynamicMBean bean)
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
         _domainSet[name.Domain] = true;
         return new ObjectInstance(name, bean.GetMBeanInfo().ClassName);
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
      private INotificationListener GetListenerMBean(ObjectName name, out IDynamicMBean bean)
      {
         bean = GetMBean(name);
         INotificationListener listner = bean as INotificationListener;
         if (listner != null)
         {
            return listner;
         }
         throw new OperationsException(string.Format("Bean \"{0}\" is not a notification listener.", name.ToString()));
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
      public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
      {
         object instance = Activator.CreateInstance(Type.GetType(className), arguments);
         return RegisterMBeanExternal(instance, name);
      }
		public void RegisterMBean(object bean, ObjectName name)
		{
         RegisterMBeanExternal(bean, name);
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
         _domainSet.Remove(name.Domain);
		}		
      public int GetMBeanCount()
      {
         return _beans.Count;
      }
      public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         IDynamicMBean bean;
         INotficationEmitter emitter = GetEmitterMBean(name, out bean);
         TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.AddNotificationListener);
         INotificationListener listenerBean = GetListenerMBean(name, out bean);
         NotificationCallback callback = new NotificationCallback(listenerBean.HandleNotification);
         emitter.AddNotificationListener(callback, filterCallback, handback);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         IDynamicMBean bean;
         INotficationEmitter emitter = GetEmitterMBean(name, out bean);
         TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.RemoveNotificationListener);
         INotificationListener listenerBean = GetListenerMBean(name, out bean);
         NotificationCallback callback = new NotificationCallback(listenerBean.HandleNotification);
         emitter.RemoveNotificationListener(callback, filterCallback, handback);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener)
      {
         IDynamicMBean bean;
         INotficationEmitter emitter = GetEmitterMBean(name, out bean);
         TestPermissions(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.RemoveNotificationListener);
         INotificationListener listenerBean = GetListenerMBean(listener, out bean);
         NotificationCallback callback = new NotificationCallback(listenerBean.HandleNotification);
         emitter.RemoveNotificationListener(callback);
      }
      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         IDynamicMBean bean = GetMBean(name);
         string className = bean.GetMBeanInfo().ClassName;
         List<AttributeValue> results = new List<AttributeValue>();
         foreach (AttributeValue nameAndValue in namesAndValues)
         {
            TestPermissions(className, nameAndValue.Name, name, MBeanPermissionAction.SetAttribute);
            try
            {
               bean.SetAttribute(nameAndValue.Name, nameAndValue.Value);
               results.Add(new AttributeValue(nameAndValue.Name, nameAndValue.Value));
            }
            catch (AttributeNotFoundException)
            {
               //Best-effort
            }
         }
         return results;
      }
      public string GetDefaultDomain()
      {
         return _defaultDomain;
      }
      public IList<string> GetDomains()
      {
         return new List<string>(_domainSet.Keys);
      }
      #endregion
   }
}