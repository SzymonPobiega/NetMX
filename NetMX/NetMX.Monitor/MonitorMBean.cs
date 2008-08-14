using System;
using System.Collections;
using System.Collections.Generic;

namespace NetMX.Monitor
{
   /// <summary>
   /// Exposes the remote management interface of monitor MBeans.
   /// </summary>
   public interface MonitorMBean
   {
      /// <summary>
      /// Adds the specified object in the set of observed MBeans.
      /// </summary>
      /// <param name="objectName">The object to observe.</param>      
      void AddObservedObject(ObjectName objectName);
      /// <summary>
      /// Tests whether the specified object is in the set of observed MBeans.
      /// </summary>
      /// <param name="objectName">The object to observe.</param>
      /// <returns></returns>
      bool ContainsObservedObject(ObjectName objectName);
      /// <summary>
      /// Gets or sets the granularity period.
      /// </summary>
      TimeSpan GranularityPeriod { get; set; }
      /// <summary>
      /// Gets or sets the attribute being observed.
      /// </summary>
      string ObservedAttribute { get; set; }
      /// <summary>
      /// Returns a collection containing the objects being observed.
      /// </summary>
      /// <returns>The objects being observed.</returns>
      IEnumerable<ObjectName> GetObservedObjects();
      /// <summary>
      /// Tests if the monitor MBean is active. A monitor MBean is marked active when the <see cref="Start"/> method is 
      /// called. It becomes inactive when the <see cref="Stop"/> method is called.
      /// </summary>
      bool IsActive { get; }
      /// <summary>
      /// Removes the specified object from the set of observed MBeans.
      /// </summary>
      /// <param name="objectName">The object to remove.</param>
      void RemoveObservedObject(ObjectName objectName);
      /// <summary>
      /// Starts the monitor.
      /// </summary>
      void Start();
      /// <summary>
      /// Stops the monitor.
      /// </summary>
      void Stop();
   }
}