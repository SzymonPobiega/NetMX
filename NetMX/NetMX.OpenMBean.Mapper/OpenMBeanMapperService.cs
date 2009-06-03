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


      private readonly OpenTypeCache _typeCache = new OpenTypeCache();
      private readonly Dictionary<ObjectName, int> _externalMappers = new Dictionary<ObjectName, int>();
      private readonly Dictionary<ObjectName, ObjectName> _mappedBeans = new Dictionary<ObjectName, ObjectName>();
      #endregion

      #region Constructors
      /// <summary>
      /// Creates new <see cref="OpenMBeanMapperService"/> instance with default type mappers
      /// (for collection types, simple types, enums and PONOs).
      /// </summary>
      public OpenMBeanMapperService()
      {
         _typeCache.AddTypeMapper(new PlainNetTypeMapper(), null, int.MaxValue);
         _typeCache.AddTypeMapper(new EnumTypeMapper(), null, int.MaxValue - 10);
         _typeCache.AddTypeMapper(new SimpleTypeMapper(), null, int.MaxValue - 20);
         _typeCache.AddTypeMapper(new CollectionTypeMapper(), null, int.MaxValue - 30);         
      }
      /// <summary>
      /// Creates new <see cref="OpenMBeanMapperService"/> instance providing patterns for MBeans to map.
      /// </summary>
      /// <param name="beansToMapPatterns">Patterns of MBean names of objects to be mapped.</param>
      public OpenMBeanMapperService(ObjectName[] beansToMapPatterns)
         : this()
      {
         _beansToMapPatterns = beansToMapPatterns;
      }
      #endregion

      #region Utility
      private void UnregisterAllProxies()
      {
         foreach (ObjectName name in _mappedBeans.Values)
         {
            _server.UnregisterMBean(name);
         }
      }
      private void MapBean(ObjectName originalBeanName)
      {
         Dictionary<string, string> props = new Dictionary<string, string>(originalBeanName.KeyPropertyList);
         props.Add(_proxyIndicatorProperty, "true");
         ObjectName proxyName = new ObjectName(originalBeanName.Domain, props);

         MBeanInfo originalInfo = _server.GetMBeanInfo(originalBeanName);
         if (!(originalInfo is NetMX.OpenMBean.IOpenMBeanInfo))
         {
            ProxyBean proxyBean = new ProxyBean(originalInfo, originalBeanName, _typeCache);
            _server.RegisterMBean(proxyBean, proxyName);
         }
      }
      private bool ShouldMapBean(ObjectName newBeanName)
      {
         if (_beansToMapPatterns != null)
         {
            if (newBeanName.KeyPropertyList.ContainsKey(_proxyIndicatorProperty))
            {
               return false;
            }
            foreach (ObjectName name in _beansToMapPatterns)
            {
               if (name.Apply(newBeanName))
               {
                  return true;
               }
            }
         }
         return false;
      }
      #endregion
      
      #region IMBeanRegistration Members
      public void PostDeregister()
      {         
      }
      public void PostRegister(bool registrationDone)
      {
         _server.AddNotificationListener(MBeanServerDelegate.ObjectName, _ownName, null, null);
         RefreshMappings();
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
                  _typeCache.RemoveTypeMapper(_externalMappers[serverNotification.ObjectName]);
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
      #endregion

      #region OpenMBeanMapperServiceMBean Members
      public string ProxyIndicatorProperty
      {
         get { return _proxyIndicatorProperty;  }
         set { _proxyIndicatorProperty = value; }
      }
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
         UnregisterAllProxies();
         if (_beansToMapPatterns != null)
         {
            foreach (ObjectName pattern in _beansToMapPatterns)
            {
               foreach (ObjectName name in _server.QueryNames(pattern, null))
               {
                  MapBean(name);
               }
            }
         }
      }
      public void FlushMappedTypeCache()
      {
         _typeCache.FlushCache();
      }
      #endregion
   }
}
