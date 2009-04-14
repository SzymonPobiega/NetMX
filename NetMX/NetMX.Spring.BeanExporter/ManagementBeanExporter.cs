using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using NetMX.Spring.BeanExporter.Assembler;
using NetMX.Spring.BeanExporter.Naming;

namespace NetMX.Spring.BeanExporter
{
   public class ManagementBeanExporter : IInitializingObject, IObjectFactoryAware
   {
      private IListableObjectFactory _objectFactory;
      private IDictionary _beans = new Hashtable();
      private IObjectNamingStrategy _namingStrategy = new SimpleNamingStrategy();

      #region Dependency properties
      public bool Autodetect { private get; set; }
      public IMBeanInfoAssembler Assembler
      {
         private get;
         set;
      }
      public IObjectNamingStrategy NamingStrategy
      {
         set { _namingStrategy = value; }
      }
      public IDictionary Beans
      {
         set { _beans = value; }
      }
      public IMBeanServer BeanServer { private get; set; }
      #endregion

      #region IInitializingObject Members
      public void AfterPropertiesSet()
      {
         RegisterBeans();
      }
      #endregion

      #region IObjectFactoryAware Members
      public IObjectFactory ObjectFactory
      {
         set { _objectFactory = value as IListableObjectFactory; }
      }
      #endregion

      private void RegisterBeans()
      {
         if (Autodetect && _objectFactory != null)
         {
            AutodetectBeans((type, name) => NetMXUtils.IsMBean(type));
         }
         foreach (DictionaryEntry pair in _beans)
         {
            RegisterBeanInstanceOrName((string)pair.Key, pair.Value);
         }
      }

      private ObjectName RegisterBeanInstanceOrName(string key, object instance)
      {
         if (instance is string)
         {
            return null;
         }
         return RegisterBeanInstance(key, instance);
      }

      private ObjectName RegisterBeanInstance(string key, object instance)
      {
         ObjectName name = _namingStrategy.GetObjectName(instance, key);

         BeanServer.RegisterMBean(instance, name);

         return name;
      }

      private void AutodetectBeans(Func<Type, string, bool> shouldExportCallback)
      {
         IConfigurableListableObjectFactory configurableObjectFactory = _objectFactory as IConfigurableListableObjectFactory;

         foreach (string name in _objectFactory.GetObjectNamesForType(typeof(object), false, false))
         {
            Type beanType = _objectFactory.GetType(name);
            if (shouldExportCallback(beanType, name))
            {
               bool lazyInit = IsLazyInit(configurableObjectFactory, name);
               object instance = !lazyInit ? _objectFactory[name] : null;
               if (!_beans.Contains(name) 
                  && (instance == null || !BeansContainsInstance(instance)))
               {
                  _beans[name] = !lazyInit ? instance : name;
               }
            }
         }
      }

      private bool BeansContainsInstance(object candidate)
      {
         foreach (object bean in _beans.Values)
         {
            if (bean == candidate)
            {
               return true;
            }
         }
         return false;
      }

      private static bool IsLazyInit(IConfigurableListableObjectFactory configurableObjectFactory, string name)
      {
         if (configurableObjectFactory == null)
         {
            return false;
         }
         return configurableObjectFactory.GetObjectDefinition(name).IsLazyInit;
      }
   }
}
