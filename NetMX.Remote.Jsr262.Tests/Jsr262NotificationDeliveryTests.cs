using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Remote.Tests;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
    [TestFixture]
    public class Jsr262NotificationDeliveryTests : NotificationDeliveryTests
    {
        protected override Uri GetUri()
        {
            return new Uri("http://localhost:13545/MBeanServer");
        }

        protected override INetMXConnectorServerFactory GetConnectorServerFactory()
        {
            return new Jsr262ConnectorServerFactory();
        }

        protected override INetMXConnectorFactory GetConnectorFactory()
        {
            return new Jsr262ConnectorFactory();
        }
    }
}