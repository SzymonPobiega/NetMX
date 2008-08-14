using System;

namespace NetMX.Monitor
{
   public abstract class AttributeState
   {
      private DateTime _timeStamp = DateTime.MinValue;
      /// <summary>
      /// Gets the time when last value was obtained. 
      /// </summary>
      public DateTime TimeStamp
      {
         get { return _timeStamp; }
      }

      /// <summary>
      /// Registers current value of attribute and it's timestamp.
      /// </summary>
      /// <param name="currentValue">Current value.</param>
      /// <param name="timeStamp">Time when value was obtained.</param>
      public void Rotate(IComparable currentValue, DateTime timeStamp)
      {
         _timeStamp = timeStamp;
         Rotate(currentValue);
      }

      /// <summary>
      /// Registers current value of attribute.
      /// </summary>
      /// <param name="currentValue">Current value.</param>     
      protected abstract void Rotate(IComparable currentValue);
      /// <summary>
      /// Gets the current value of attribute.
      /// </summary>
      public abstract IComparable CurrentValue { get; }
   }
   public sealed class StringAttributeState : AttributeState
   {
      private string _currentValue;

      protected override void Rotate(IComparable currentValue)
      {
         _currentValue = currentValue as String;
         if (_currentValue == null)
         {
            throw new ArgumentException();
         }
      }

      public override IComparable CurrentValue
      {
         get { return _currentValue; }
      }
   }
   /// <summary>
   /// Represents state of one numeric attribute of one MBean. Holds values of two subsequent measurements of the
   /// attribute.
   /// </summary>
   public class NumericAttributeState : AttributeState
   {
      #region Fields
      private IComparable _thirdValue;
      private IComparable _secondValue;
      private IComparable _firstValue;
      #endregion

      /// <summary>
      /// Gets the third (last) of three subsequent values.
      /// </summary>
      public IComparable ThirdValue
      {
         get { return _thirdValue; }
      }

      /// <summary>
      /// Gets the second (middle) of three subsequent values.
      /// </summary>
      public IComparable SecondValue
      {
         get { return _secondValue; }
      }      

      /// <summary>
      /// Gets the first (latest) of three subsequent values.
      /// </summary>
      public IComparable FirstValue
      {
         get { return _firstValue; }
      }

      protected override void Rotate(IComparable currentValue)
      {
         _thirdValue = _secondValue;         
         _secondValue = _firstValue;
         _firstValue = currentValue;
      }
      public override IComparable CurrentValue
      {
         get { return FirstValue; }
      }
   }

   /// <summary>
   /// Represents state of attribute monitored by counter monitor. Adds information about current counter
   /// threshold for the particular monitored MBean.
   /// </summary>
   public sealed class CounterMonitorAttributeState : NumericAttributeState
   {
      private IComparable _threshold;
      /// <summary>
      /// Gets or sets current value of counter threshold.
      /// </summary>
      public IComparable Threshold
      {
         get { return _threshold; }
         set { _threshold = value; }
      }
      /// <summary>
      /// Creates new <see cref="CounterMonitorAttributeState"/> instance.
      /// </summary>
      public CounterMonitorAttributeState()
      {         
      }
      /// <summary>
      /// Creates new <see cref="CounterMonitorAttributeState"/> instance.
      /// </summary>
      /// <param name="initalThreshold">Initial value of threshold.</param>
      public CounterMonitorAttributeState(object initalThreshold)
      {
         _threshold = (IComparable) initalThreshold;
      }
   }
}