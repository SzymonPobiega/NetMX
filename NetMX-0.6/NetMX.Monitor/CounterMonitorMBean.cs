using System;

namespace NetMX.Monitor
{
   /// <summary>
   /// Exposes the remote management interface of the counter monitor MBean.
   /// </summary>
   public interface CounterMonitorMBean : MonitorMBean
   {
      /// <summary>
      /// Gets the derived gauge for the specified MBean.
      /// </summary>
      /// <param name="objectName">The MBean for which the derived gauge is to be returned.</param>
      /// <returns>The derived gauge for the specified MBean.</returns>
      /// <exception cref="NotObservedObjectException">If specified MBean is not observed by this monitor.</exception>
      object GetDerivedGauge(ObjectName objectName);
      /// <summary>
      /// Gets the derived gauge timestamp for the specified MBean.
      /// </summary>
      /// <param name="objectName">The MBean for which the derived gauge timestamp is to be returned.</param>
      /// <returns>The derived gauge timestamp for the specified MBean.</returns>
      /// <exception cref="NotObservedObjectException">If specified MBean is not observed by this monitor.</exception>
      DateTime GetDerivedGaugeTimeStamp(ObjectName objectName);
      /// <summary>
      /// Gets or sets the difference mode flag value.
      /// </summary>
      bool DifferenceMode { get; set; }
      /// <summary>
      /// Gets or sets the initial threshold value common to all observed objects.
      /// </summary>
      object InitThreshold { get; set; }
      /// <summary>
      /// Gets or sets the modulus value.
      /// </summary>
      object Modulus { get; set; }
      /// <summary>
      /// Gets or sets the notification's on/off switch value.
      /// </summary>
      bool Notify { get; set; }
      /// <summary>
      /// Gets or sets the offset value.
      /// </summary>
      object Offset { get; set; }
      /// <summary>
      /// Gets the threshold value for the specified MBean.
      /// </summary>
      /// <param name="objectName">The MBean for which the threshold value is to be returned.</param>
      /// <returns>The threshold value for the specified MBean.</returns>
      /// <exception cref="NotObservedObjectException">If specified MBean is not observed by this monitor.</exception>
      object GetThreshold(ObjectName objectName);      
   }
}