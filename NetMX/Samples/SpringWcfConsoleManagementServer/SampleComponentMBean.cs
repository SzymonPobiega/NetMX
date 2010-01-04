using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfSpringConsoleDemo
{
   public interface SampleComponentMBean
   {
      void Start();
      void Stop();
      int Count { get; }
      void IntOperation(int value);
      void StringAndIntOperation(string stringValue, int intValue);
   }
}