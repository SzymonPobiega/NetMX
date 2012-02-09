using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class Jsr262NotificationTests : NotificationTests
   {
      protected override Uri GetUri()
      {
         return new Uri("http://localhost:13545/MBeanServer");
      }
   }
}