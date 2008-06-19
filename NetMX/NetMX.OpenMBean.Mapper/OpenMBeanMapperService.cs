using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
   public class OpenMBeanMapperService : OpenMBeanMapperServiceMBean, IMBeanRegistration, INotificationListener
   {
      #region Members
      private IMBeanServer _server;
      private ObjectName _ownName;
      private ObjectName[] _beansToMapPatterns;
		private string _proxyIndicatorProperty = "OpenMBeanProxy";


      private readonly SortedList<int, ITypeMapper> _mappers = new SortedList<int, ITypeMapper>();
      private readonly Dictionary<ObjectName, int> _externalMappers = new Dictionary<ObjectName, int>();
      private readonly Dictionary<ObjectName, ObjectName> _mappedBeans = new Dictionary<ObjectName, ObjectName>();
      #endregion

      #region Constructors
      public OpenMBeanMapperService()
      { 
         _mappers.Add(int.MaxValue, new PlainNetTypeMapper());
         _mappers.Add(int.MaxValue - 1, new SimpleTypeMapper());
         _mappers.Add(int.MaxValue - 2, new CollectionTypeMapper());         
      }
      public OpenMBeanMapperService(IEnumerable<ObjectName> beansToMapPatterns)
         : this()
      {
         
      }
      #endregion

      #region Utility
      //private OpenType CreateOpenType(Type plainNetType)
      //{

      //}
      #endregion

      #region IMBeanRegistration Members
      public void PostDeregister()
      {         
      }
      public void PostRegister(bool registrationDone)
      {
         _server.AddNotificationListener(MBeanServerDelegate.ObjectName, _ownName, null, null);
      }
      public void PreDeregister()
      {
         _server.RemoveNotificationListener(MBeanServerDelegate.ObjectName, _ownName);
      }
      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         _ownName = name;
         _server = server;
         return name;
      }
      #endregion

      #region INotificationListener Members
      public void HandleNotification(Notification notification, object handback)
      {
         MBeanServerNotification serverNotification = notification as MBeanServerNotification;
         if (serverNotification != null)            
         {
            if (serverNotification.Type == MBeanServerNotification.RegistrationNotification)
            {
               if (ShouldMapBean(serverNotification.ObjectName))
               {
						MapBean(serverNotification.ObjectName);
               }
            }
            else if (serverNotification.Type == MBeanServerNotification.UnregistrationNotification)
            {
               if (_externalMappers.ContainsKey(serverNotification.ObjectName))
               {
                  _mappers.Remove(_externalMappers[serverNotification.ObjectName]);
                  _externalMappers.Remove(serverNotification.ObjectName);
               }
               else if (_mappedBeans.ContainsKey(serverNotification.ObjectName))
               {
                  _server.UnregisterMBean(_mappedBeans[serverNotification.ObjectName]);
                  _mappedBeans.Remove(serverNotification.ObjectName);
               }
            }
         }
      }
		private void MapBean(ObjectName originalBeanName)
		{
			Dictionary<string, string> props = new Dictionary<string,string>(originalBeanName.KeyPropertyList);
			props.Add(_proxyIndicatorProperty, "true");
			ObjectName proxyName = new ObjectName(originalBeanName.Domain, props);

		}
      private bool ShouldMapBean(ObjectName newBeanName)
      {         
         foreach (ObjectName name in _beansToMapPatterns)
         {
            if (name.Apply(newBeanName))
            {
               return true;
            }
         }
         return false;
      }
      #endregion

      #region OpenMBeanMapperServiceMBean Members
      public ObjectName[] BeansToMapPatterns
      {
         get
         {
            return _beansToMapPatterns;
         }
         set
         {            
            _beansToMapPatterns = value;
         }
      }
      public void RefreshMappings()
      {
         
      }
      public void FlushMappedTypeCache()
      {
         
      }
      #endregion
   }
}
