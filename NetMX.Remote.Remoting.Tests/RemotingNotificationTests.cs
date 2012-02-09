using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Remoting.Tests
{
   [TestFixture]
   public class RemotingNotificationTests : NotificationTests
   {
      protected override Uri GetUri()
      {
         return new Uri("tcp://localhost:1234/MBeanServer.tcp");
      }
   }
}