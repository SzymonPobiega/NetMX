using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSample
{
   public class SampleComponent : SampleComponentMBean
   {
      #region SampleComponentMBean Members
      public void Start()
      {
      }
      public void Stop()
      {
      }
      public int Count
      {
         get { return 0; }
      }      
      public void IntOperation(int value)
      {
         Console.WriteLine("Int value: {0}", value);
      }
      public void StringAndIntOperation(string stringValue, int intValue)
      {
         Console.WriteLine("Int value: {0}, string value: {1}", intValue, stringValue);
      }
      #endregion
   }
}
