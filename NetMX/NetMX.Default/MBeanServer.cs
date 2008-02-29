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
        private void RegisterMBeanInternal(ObjectName name, IDynamicMBean bean, IMBeanRegistration beanRegistrationController)
        {
            IMBeanRegistration registration = new MBeanRegistrationHelper(beanRegistrationController);
            name = registration.PreRegister(this, name);

            string className = bean.GetMBeanInfo().ClassName;
            MBeanCASPermission casPerm = new MBeanCASPermission(className, null, name, MBeanPermissionAction.RegisterMBean);
            casPerm.Demand();
            MBeanPermission perm = new MBeanPermission(className, null, name, MBeanPermissionAction.RegisterMBean);
            perm.Demand();

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
                RegisterMBeanInternal(name, dynBean, dynBean as IMBeanRegistration);
			}
			else
			{
				Type beanType = bean.GetType();
				Type intfType = beanType.GetInterface(beanType.Name+"MBean", false);
				if (intfType == null)
				{
					throw new NotCompliantMBeanException(beanType.AssemblyQualifiedName);
				}
				StandardMBean stdBean = new StandardMBean(bean, intfType);
                RegisterMBeanInternal(name, stdBean, stdBean);
			}
		}
        
		public object Invoke(ObjectName name, string operationName, object[] arguments)
		{            
			IDynamicMBean bean = GetMBean(name);

            MBeanCASPermission casPerm = new MBeanCASPermission(bean.GetMBeanInfo().ClassName, operationName, name, MBeanPermissionAction.Invoke);
            casPerm.Demand();
            MBeanPermission perm = new MBeanPermission(bean.GetMBeanInfo().ClassName, operationName, name, MBeanPermissionAction.Invoke);
            perm.Demand();            

			return bean.Invoke(operationName, arguments);			
		}

		public void SetAttribute(ObjectName name, string attributeName, object value)
		{
			IDynamicMBean bean = GetMBean(name);

            MBeanPermission perm = new MBeanPermission(bean.GetMBeanInfo().ClassName, attributeName, name, MBeanPermissionAction.SetAttribute);
            perm.Demand();            

			bean.SetAttribute(attributeName, value);
		}

		public object GetAttribute(ObjectName name, string attributeName)
		{
			IDynamicMBean bean = GetMBean(name);

            MBeanPermission perm = new MBeanPermission(bean.GetMBeanInfo().ClassName, attributeName, name, MBeanPermissionAction.GetAttribute);
            perm.Demand();

			return bean.GetAttribute(attributeName);
		}

		public MBeanInfo GetMBeanInfo(ObjectName name)
		{
			IDynamicMBean bean = GetMBean(name);

            MBeanPermission perm = new MBeanPermission(bean.GetMBeanInfo().ClassName, null, name, MBeanPermissionAction.GetMBeanInfo);
            perm.Demand();

			return bean.GetMBeanInfo();
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

        #endregion
    }
}
