using System;

namespace NetMX.Monitor
{
   /// <summary>
   /// Defines a monitor MBean designed to observe the values of a counter attribute.
   ///
   /// A counter monitor sends a threshold notification when the value of the counter reaches or exceeds a 
   /// threshold known as the comparison level. The notify flag must be set to true.
   ///
   /// In addition, an offset mechanism enables particular counting intervals to be detected. If the offset 
   /// value is not zero, whenever the threshold is triggered by the counter value reaching a comparison level, 
   /// that comparison level is incremented by the offset value. This is regarded as taking place instantaneously, 
   /// that is, before the count is incremented. Thus, for each level, the threshold triggers an event 
   /// notification every time the count increases by an interval equal to the offset value.
   ///
   /// If the counter can wrap around its maximum value, the modulus needs to be specified. The modulus is the 
   /// value at which the counter is reset to zero. 
   /// 
   /// If the counter difference mode is used, the value of the derived gauge is calculated as the difference 
   /// between the observed counter values for two successive observations. If this difference is negative, 
   /// the value of the derived gauge is incremented by the value of the modulus. The derived gauge value 
   /// (V[t]) is calculated using the following method:
   /// 
   /// <list type="bullet">
   /// <item>if (counter[t] - counter[t-GP]) is positive then V[t] = counter[t] - counter[t-GP]</item>
   /// <item>if (counter[t] - counter[t-GP]) is negative then V[t] = counter[t] - counter[t-GP] + MODULUS </item>
   /// </list>
   /// </summary>
   public sealed class CounterMonitor : Monitor<CounterMonitorAttributeState>, CounterMonitorMBean
   {
      #region Const
      private static readonly Type[] SUPPORTED_TYPES = new Type[] {typeof(int), typeof(byte), typeof(short)};
      #endregion

      #region Fields
      private bool _differenceMode;
      private object _initThreshold;
      private object _modulus;
      private bool _notify;
      private object _offset;
      
      #endregion

      #region Overrides of Monitor      
      protected override void ProcessNewMeasurement(ObjectName objectName, CounterMonitorAttributeState state, string errorCode, string errorDescription)
      {         
         if (errorCode == null)
         {
            INumericUtil util = NumericUtils.GetUtil(state.FirstValue.GetType());
            IComparable derivedGauge = GetDerivedGauge(state);

            if (derivedGauge.CompareTo(state.Threshold) > 0 && Notify)
            {
               MonitorNotification notif = new MonitorNotification(MonitorNotification.ThresholdValueExceeded, this,
                                                                   -1, null, null, derivedGauge,
                                                                   GetThreshold(objectName), ObservedAttribute,
                                                                   objectName);
               SendNotification(notif);
            }
            if (Offset != null && util.Zero.CompareTo(Offset) != 0)            
            {                              
               while (derivedGauge.CompareTo(state.Threshold) > 0)
               {                  
                  state.Threshold = util.Add(state.Threshold, Offset);
                  if (util.Zero.CompareTo(Modulus) != 0)
                  {
                     if (Modulus != null && util.Zero.CompareTo(Modulus) != 0 && state.Threshold.CompareTo(Modulus) > 0)
                     {
                        state.Threshold = util.Sub(state.Threshold, Modulus);
                     }
                  }
               }               
            }
         }
         else if (Notify)
         {
            MonitorNotification notif = new MonitorNotification(errorCode, this, -1, errorDescription, null, null, null, ObservedAttribute, objectName);
            SendNotification(notif);
         }         
      }
      protected override Type[] SupportedTypes
      {
         get { return SUPPORTED_TYPES; }
      }
      protected override CounterMonitorAttributeState CreateNewAttributeState()
      {
         return new CounterMonitorAttributeState(InitThreshold);
      }
      #endregion

      #region Implementation of CounterMonitorMBean
      public object GetDerivedGauge(ObjectName objectName)
      {
         return GetDerivedGauge(base[objectName]);
      }
      public DateTime GetDerivedGaugeTimeStamp(ObjectName objectName)
      {
         return base[objectName].TimeStamp;
      }
      public bool DifferenceMode
      {
         get { return _differenceMode; }
         set { _differenceMode = value; }
      }
      public object InitThreshold
      {
         get { return _initThreshold; }
         set { _initThreshold = value; }
      }
      public object Modulus
      {
         get { return _modulus; }
         set { _modulus = value; }
      }
      public bool Notify
      {
         get { return _notify; }
         set { _notify = value; }
      }
      public object Offset
      {
         get { return _offset; }
         set { _offset = value; }
      }
      public object GetThreshold(ObjectName objectName)
      {
         return base[objectName].Threshold;
      }
      #endregion

      #region Utility
      private IComparable GetDerivedGauge(CounterMonitorAttributeState attributeState)
      {
         if (attributeState.FirstValue == null)
         {
            return null;
         }
         INumericUtil util = NumericUtils.GetUtil(attributeState.FirstValue.GetType());
         if (DifferenceMode)
         {
            if (attributeState.SecondValue == null)
            {
               return util.Zero;
            }
            IComparable diff = util.Sub(attributeState.FirstValue, attributeState.SecondValue);
            if (diff.CompareTo(util.Zero) < 0 )
            {
               return util.Add(diff, _modulus);
            }
            return diff;
         }
         return attributeState.FirstValue;
      }
      #endregion
   }
}