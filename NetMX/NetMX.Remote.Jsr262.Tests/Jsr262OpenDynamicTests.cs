using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class Jsr262OpenDynamicTests : OpenDynamicTests
   {
      protected override Uri GetUri()
      {
         return new Uri("http://localhost:13545/MBeanServer");
      }      
   }
}
