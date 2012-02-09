using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Monitor
{
   /// <summary>
   /// Provides definitions of the notifications sent by monitor MBeans.
   /// 
   /// The notification source and a set of parameters concerning the monitor MBean's state need to be 
   /// specified when creating a new object of this class. The list of notifications fired by the monitor 
   /// MBeans is the following: 
   /// 
   /// <list type="bullet">
   /// <item>Common to all kind of monitors:</item>
   ///   <list type="bullet">
   ///   <item>The observed object is not registered in the MBean server.</item>
   ///   <item>The observed attribute is not contained in the observed object.</item>
   ///   <item>The type of the observed attribute is not correct.</item>
   ///   <item>Any exception (except the cases described above) occurs when trying to get the value of the observed attribute. </item>
   ///   </list>
   /// </list>
   /// </summary>
   [Serializable]
   public sealed class MonitorNotification : Notification
   {
      #region Const
      /// <summary>
      /// Notification type denoting that the observed attribute is not contained in the observed object. 
      /// This notification is fired by all kinds of monitors.
      /// The value of this notification type is netmx.monitor.error.attribute.
      /// </summary>
      public const string ObservedAttributeError = "netmx.monitor.error.attribute";
      /// <summary>
      /// Notification type denoting that the type of the observed attribute is not correct. This notification is fired by all kinds of monitors.
      /// The value of this notification type is netmx.monitor.error.type.
      /// </summary>
      public const string ObservedAttributeTypeError = "netmx.monitor.error.type";
      /// <summary>
      /// Notification type denoting that the type of the thresholds, offset or modulus is not correct. This notification is fired by counter and gauge monitors.
      /// The value of this notification type is netmx.monitor.error.threshold.
      /// </summary>
      public const string ThresholdError = "netmx.monitor.error.threshold";
      /// <summary>
      /// Notification type denoting that a non-predefined error type has occurred when trying to get the value of the observed attribute. This notification is fired by all kinds of monitors.
      /// The value of this notification type is netmx.monitor.error.runtime.
      /// </summary>
      public const string RuntimeError = "netmx.monitor.error.runtime";
      /// <summary>
      /// Notification type denoting that the observed object is not registered in the MBean server. This notification is fired by all kinds of monitors.
      /// The value of this notification type is netmx.monitor.error.mbean.
      /// </summary>
      public const string ObservedObjectError = "netmx.monitor.error.mbean";      
      /// <summary>
      /// Notification type denoting that the observed attribute has reached the threshold value. This notification is only fired by counter monitors.
      /// The value of this notification type is netmx.monitor.counter.threshold.
      /// </summary>      
      public const string ThresholdValueExceeded = "netmx.monitor.counter.threshold";
      /// <summary>
      /// Notification type denoting that the observed attribute has exceeded the threshold high value. This notification is only fired by gauge monitors.
      /// The value of this notification type is netmx.monitor.gauge.high.
      /// </summary>
      public const string ThresholdHighValueExceeded = "netmx.monitor.gauge.high";
      /// <summary>
      /// Notification type denoting that the observed attribute has exceeded the threshold low value. This notification is only fired by gauge monitors.
      /// The value of this notification type is netmx.monitor.gauge.low.
      /// </summary>
      public const string ThresholdLowValueExceeded = "netmx.monitor.gauge.low";
      /// <summary>
      /// Notification type denoting that the observed attribute has matched the "string to compare" value. This notification is only fired by string monitors.
      /// The value of this notification type is netmx.monitor.string.matches.
      /// </summary>
      public const string StringToCompareValueMatched = "netmx.monitor.string.matches";
      /// <summary>
      /// Notification type denoting that the observed attribute has differed from the "string to compare" value. This notification is only fired by string monitors.
      /// The value of this notification type is netmx.monitor.string.differs.
      /// </summary>
      public const string StringToCompareValueDiffered = "netmx.monitor.string.differs";
      #endregion

      #region Fields
      private readonly object _derivedGauge;
      private readonly object _trigger;
      private readonly string _observedAttribute;
      private readonly ObjectName _observedObject;
      #endregion

      #region Properties
      /// <summary>
      /// Gets the derived gauge of this monitor notification.
      /// </summary>
      public object DerivedGauge
      {
         get { return _derivedGauge; }
      }
      /// <summary>
      /// Gets the threshold/string (depending on the monitor type) that triggered off this monitor notification.
      /// </summary>
      public object Trigger
      {
         get { return _trigger; }
      }
      /// <summary>
      /// Gets the observed attribute of this monitor notification.
      /// </summary>
      public string ObservedAttribute
      {
         get { return _observedAttribute; }
      }
      /// <summary>
      /// Gets the observed object of this monitor notification.
      /// </summary>
      public ObjectName ObservedObject
      {
         get { return _observedObject; }
      }
      #endregion

      #region Constructor
      /// <summary>
      /// Creates new <see cref="MonitorNotification"/> object.
		/// </summary>
		/// <param name="type">Notification type.</param>
		/// <param name="source">Notification source.</param>
		/// <param name="sequenceNumber">Sequence number.</param>
		/// <param name="message">Message.</param>
		/// <param name="userData">Used defined data.</param>      
      /// <param name="derivedGauge">The derived gauge of this monitor notification.</param>
      /// <param name="trigger">The threshold/string (depending on the monitor type) that triggered off this monitor notification.</param>
      /// <param name="observedAttribute">The observed attribute of this monitor notification.</param>
      /// <param name="observedObject">The observed object of this monitor notification.</param>
      public MonitorNotification(string type, object source, long sequenceNumber, string message, object userData, object derivedGauge, object trigger, string observedAttribute, ObjectName observedObject)
			: base(type, source, sequenceNumber, message, userData)
		{
         _derivedGauge = derivedGauge;
         _trigger = trigger;
         _observedAttribute = observedAttribute;
         _observedObject = observedObject;
		}
      #endregion
   }
}
