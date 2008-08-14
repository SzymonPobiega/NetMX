using System;
using System.Collections.Generic;
using System.Text;
using NetMX.Timer;

namespace NetMX.Monitor
{
   /// <summary>
   /// Defines the common part to all monitor MBeans. A monitor MBean monitors values of an attribute common 
   /// to a set of observed MBeans. The observed attribute is monitored at intervals specified by the 
   /// granularity period. A gauge value (derived gauge) is derived from the values of the observed attribute.
   /// </summary>
   public abstract class Monitor<T> : NotificationEmitterSupport, MonitorMBean, IMBeanRegistration
      where T : AttributeState, new()
   {
      #region Const
      protected const string NOTIFICATION_TYPE = "Monitor";
      protected const string NOTIFICATION_MESSAGE = "MonitorMessage";
      #endregion

      #region Fields
      private IMBeanServer _server;
      private TimerMBean _timer;
      private bool _isActive;
      private int _notificationId;
      private TimeSpan _granularityPeriod;
      private string _observedAttribute;

      private readonly Dictionary<ObjectName, T> _observedObjects =
         new Dictionary<ObjectName, T>();
      #endregion

      #region Subclass contract
      protected TimerMBean Timer
      {
         get { return _timer; }
      }
      protected IMBeanServer MBeanServer
      {
         get { return _server; }
      }
      protected T this[ObjectName objectName] 
      {
         get
         {
            if (!_observedObjects.ContainsKey(objectName))
            {
               throw new NotObservedObjectException(objectName);
            }
            return _observedObjects[objectName];
         }
      }
      protected virtual T CreateNewAttributeState()
      {
         return new T();
      } 
      protected abstract Type[] SupportedTypes { get; }
      protected abstract void ProcessNewMeasurement(ObjectName objectName, T state, string errorCode, string errorDescription);
      protected IComparable GetObservedValue(ObjectName objectName, Type desiredType, out string errorType, out string errorDescription)
      {
         object result = null;
         errorType = null;
         errorDescription = null;
         try
         {
            result = _server.GetAttribute(objectName, _observedAttribute);
            if (result == null || 
               ( (desiredType == null && Array.IndexOf(SupportedTypes, result.GetType()) == -1) ||
                 (desiredType != null && desiredType != result.GetType())))
            {
               errorType = MonitorNotification.ObservedAttributeTypeError;
               result = null;
            }
         }
         catch (InstanceNotFoundException)
         {
            errorType = MonitorNotification.ObservedObjectError;
         }
         catch (AttributeNotFoundException)
         {
            errorType = MonitorNotification.ObservedAttributeError;
         }
         catch (Exception ex)
         {
            errorType = MonitorNotification.RuntimeError;
            errorDescription = ex.Message;
         }
         return (IComparable) result;
      }
      #endregion

      #region Implementation of MonitorMBean
      public void AddObservedObject(ObjectName objectName)
      {
         if (!_observedObjects.ContainsKey(objectName))
         {
            _observedObjects.Add(objectName, CreateNewAttributeState());
         }
      }

      public bool ContainsObservedObject(ObjectName objectName)
      {
         return _observedObjects.ContainsKey(objectName);
      }

      public TimeSpan GranularityPeriod
      {
         get { return _granularityPeriod; }
         set
         {
            _granularityPeriod = value;
            Stop();
            Start();
         }
      }

      public string ObservedAttribute
      {
         get { return _observedAttribute; }
         set { _observedAttribute = value; }
      }

      public IEnumerable<ObjectName> GetObservedObjects()
      {
         return new List<ObjectName>(_observedObjects.Keys).AsReadOnly();
      }

      public bool IsActive
      {
         get { return _isActive; }
      }

      public void RemoveObservedObject(ObjectName objectName)
      {
         _observedObjects.Remove(objectName);
      }

      public void Start()
      {
         if (!_isActive)
         {
            _isActive = true;
            _timer.Start();
            _notificationId = _timer.AddNotification2(NOTIFICATION_TYPE, NOTIFICATION_MESSAGE, null, DateTime.Now, _granularityPeriod);
         }
      }

      public void Stop()
      {
         if (_isActive)
         {
            _isActive = false;
            _timer.Stop();
            _timer.RemoveNotification(_notificationId);
         }
      }

      #endregion

      #region Implementation of IMBeanRegistration
      public void PostDeregister()
      {
      }

      public void PostRegister(bool registrationDone)
      {
      }

      public void PreDeregister()
      {
      }

      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         _server = server;
         Timer.Timer timer = new Timer.Timer();
         IDictionary<string, string> props = name.KeyPropertyList;
         props.Add("EmbeddedTimer", "true");
         ObjectName timerName = new ObjectName(name.Domain, props);
         server.RegisterMBean(timer, timerName);
         _timer = NetMX.NewMBeanProxy<TimerMBean>(_server, timerName);
         _server.AddNotificationListener(timerName, OnTimerEvent, null, null);
         return name;
      }
      #endregion

      private void OnTimerEvent(Notification notification, object handback)
      {
         foreach (ObjectName observedObject in _observedObjects.Keys)
         {
            string errorType;
            string errorDescription;
            T state = _observedObjects[observedObject];
            Type desiredType = state.CurrentValue != null ? state.CurrentValue.GetType() : null;
            IComparable currentValue = GetObservedValue(observedObject, desiredType, out errorType, out errorDescription);
            if (errorType == null)
            {
               state.Rotate(currentValue, notification.Timestamp);
            }
            ProcessNewMeasurement(observedObject, state, errorType, errorDescription);
         }
      }
   }
}
