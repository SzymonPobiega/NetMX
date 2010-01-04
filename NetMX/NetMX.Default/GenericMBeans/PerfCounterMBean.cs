#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NetMX.OpenMBean;
using System.Globalization;

#endregion

namespace NetMX.Server.GenericMBeans
{
   public class PerfCounterMBean : IDynamicMBean, IMBeanRegistration
   {
      #region MEMBERS
      private bool _beanInfoDirty = true;
      private OpenMBeanInfoSupport _beanInfo;
      private ObjectName _thisName;
      private readonly string _perfObjectName;
      private readonly string _perfInstanceName;
      private readonly Dictionary<string, PerformanceCounter> _counters = new Dictionary<string, PerformanceCounter>();
      private readonly PerformanceCounterCategory _category;
      #endregion

      #region PROPERTIES
      #endregion

      #region CONSTRUCTOR
      public PerfCounterMBean(string perfObjectName, string perfInstanceName, IEnumerable<string> perfCounterNames, bool useProcessInstanceName, bool useAllCounters)
      {
         _perfObjectName = perfObjectName;
         if (useProcessInstanceName)
         {
            Process p = Process.GetCurrentProcess();
            _perfInstanceName = p.ProcessName;
         }
         else
         {
            _perfInstanceName = perfInstanceName ?? "";
         }
         _category = new PerformanceCounterCategory(_perfObjectName);
         if (useAllCounters)
         {
            foreach (PerformanceCounter counter in _category.GetCounters(_perfInstanceName))
            {
               _counters[counter.CounterName] = counter;
            }
         }
         else if (perfCounterNames != null)
         {
            foreach (string counterName in perfCounterNames)
            {
               PerformanceCounter counter;
               counter = new PerformanceCounter(_perfObjectName, counterName, _perfInstanceName, true);
               _counters.Add(counterName, counter);
            }
         }
      }
      public PerfCounterMBean(string perfObjectName, bool useProcessInsatnceName, IEnumerable<string> counterNames)
         : this(perfObjectName, null, counterNames, useProcessInsatnceName, false)
      {
      }
      public PerfCounterMBean(string perfObjectName, bool useProcessInsatnceName)
         : this(perfObjectName, null, null, useProcessInsatnceName, true)
      {
      }
      #endregion

      #region Interface
      public bool RemovePerformanceCounter(string counterName)
      {
         if (!_category.CounterExists(counterName))
         {
            return false;
         }
         if (!_counters.ContainsKey(counterName))
         {
            return true;
         }
         _counters[counterName].Dispose();
         _counters.Remove(counterName);
         _beanInfoDirty = true;
         return true;
      }
      public bool AddPerformanceCounter(string counterName)
      {
         if (!_category.CounterExists(counterName))
         {
            return false;
         }
         if (_counters.ContainsKey(counterName))
         {
            return true;
         }
         PerformanceCounter counter = new PerformanceCounter(_perfObjectName, counterName, _perfInstanceName, true);
         _counters[counter.CounterName] = counter;
         _beanInfoDirty = true;
         return true;
      }
      #endregion

      #region IDynamicMBean Members
      public MBeanInfo GetMBeanInfo()
      {
         if (_beanInfoDirty)
         {
            string description;
            if (_perfInstanceName != null)
            {
               description = string.Format(CultureInfo.CurrentCulture, "Performance counter MBean for {0} of {1}.", _perfObjectName, _perfInstanceName);
            }
            else
            {
               description = string.Format(CultureInfo.CurrentCulture, "Performance counter MBean for {0}.", _perfObjectName);
            }
            List<IOpenMBeanAttributeInfo> attributes = new List<IOpenMBeanAttributeInfo>();
            List<IOpenMBeanConstructorInfo> constructors = new List<IOpenMBeanConstructorInfo>();
            List<IOpenMBeanOperationInfo> operations = new List<IOpenMBeanOperationInfo>();
            List<MBeanNotificationInfo> notifications = new List<MBeanNotificationInfo>();

            List<object> legalCountersToCreate = new List<object>();
            List<object> legalCountersToRemove = new List<object>();

            foreach (PerformanceCounter counter in _category.GetCounters(_perfInstanceName))
            {
               if (_counters.ContainsKey(counter.CounterName))
               {
                  legalCountersToRemove.Add(counter.CounterName);
               }
               else
               {
                  legalCountersToCreate.Add(counter.CounterName);
               }
            }

            OpenMBeanParameterInfoSupport addParam =
               new OpenMBeanParameterInfoSupport("counterName", "Name of new counter", SimpleType.String, null, legalCountersToCreate );
            operations.Add(new OpenMBeanOperationInfoSupport("AddPerformanceCounter", "Adds new performance counter", SimpleType.Boolean, 
                                                             new IOpenMBeanParameterInfo[] { addParam}, OperationImpact.Action));

            OpenMBeanParameterInfoSupport removeParam =
               new OpenMBeanParameterInfoSupport("counterName", "Name of new counter", SimpleType.String, null, legalCountersToRemove);
            operations.Add(new OpenMBeanOperationInfoSupport("RemovePerformanceCounter", "Removes existing performance counter", SimpleType.Boolean,
                                                             new IOpenMBeanParameterInfo[] { removeParam }, OperationImpact.Action));

            foreach (PerformanceCounter counter in _counters.Values)
            {
               attributes.Add(new OpenMBeanAttributeInfoSupport(counter.CounterName,
                                                                string.Format(CultureInfo.CurrentCulture, "Raw counter value for counter {0}.", counter.CounterName), SimpleType.Float, true, false));
            }

            _beanInfo = new OpenMBeanInfoSupport(typeof(PerfCounterMBean).AssemblyQualifiedName, description, attributes, constructors, operations, notifications);
            _beanInfoDirty = false;
         }
         return _beanInfo;
      }
      public object GetAttribute(string attributeName)
      {
         PerformanceCounter counter;
         if (_counters.TryGetValue(attributeName, out counter))
         {
            return CounterSample.Calculate(counter.NextSample());
         }
         else
         {
            throw new AttributeNotFoundException(attributeName, _thisName,
                                                 typeof(PerfCounterMBean).AssemblyQualifiedName);
         }
      }
      public void SetAttribute(string attributeName, object value)
      {
         throw new NotImplementedException();
      }
      public object Invoke(string operationName, object[] arguments)
      {
         if (operationName == "AddPerformanceCounter")
         {
            return AddPerformanceCounter((string) arguments[0]);
         }
         else if (operationName == "RemovePerformanceCounter")
         {
            return RemovePerformanceCounter((string)arguments[0]);
         }
         else throw new OperationNotFoundException(operationName, _thisName, typeof(PerfCounterMBean).AssemblyQualifiedName);
      }
      #endregion

      #region IMBeanRegistration Members
      public void PostDeregister()
      {
         foreach (PerformanceCounter counter in _counters.Values)
         {
            counter.Dispose();
         }
      }
      public void PostRegister(bool registrationDone)
      {
      }
      public void PreDeregister()
      {
      }
      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         _thisName = name;
         return name;
      }
      #endregion
   }
}