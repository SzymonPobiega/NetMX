using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NetMX.Remote.HttpAdaptor.Tests
{
    [TestFixture]
    public class SimpleStandardTests
    {
        [Test]
        public void DoTest()
        {
            var adaptor = new SelfHostingHttpAdaptor("http://localhost:12345/adaptor");
            adaptor.Start();

            Thread.Sleep(100000);
        }
    }
}
