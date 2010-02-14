using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Remoting.Tests
{
   [TestFixture]
   public class RemotingOpenDynamicTests : OpenDynamicTests
   {
      protected override Uri GetUri()
      {
         return new Uri("tcp://localhost:1234/MBeanServer.tcp");
      }      
   }
}