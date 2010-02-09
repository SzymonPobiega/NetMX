using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.OpenMBean;
using NUnit.Framework;

namespace NetMX.Remote.Tests
{
   [TestFixture]
   public abstract class OpenDynamicTests
   {            
      [Test]
      public void TestQueryNamesNoRestrictions()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames(null, null);

         Assert.AreEqual(2, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Tests:key=value")));
         Assert.IsTrue(names.Contains(new ObjectName("NetMXImplementation:type=MBeanServerDelegate")));
      }

      [Test]
      public void TestQueryNamesObjectNameRestriction()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames("Tests:key=value", null);

         Assert.AreEqual(1, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Tests:key=value")));
      }

      [Test]
      public void TestGetMBeanInfo()
      {
         MBeanInfo info = _remoteServer.GetMBeanInfo("Tests:key=value");

         Assert.IsTrue(info.IsOpen());
         IOpenMBeanInfo openInfo = info.AsOpen();
         IOpenMBeanInfo referenceInfo = _server.GetMBeanInfo("Tests:key=value").AsOpen();

         IOpenMBeanAttributeInfo firstDeserializedAttriute = openInfo.Attributes.First(x => x.Name == "Attribute");
         IOpenMBeanAttributeInfo firstAttribute = referenceInfo.Attributes.First(x => x.Name == "Attribute");
         Assert.IsTrue(firstDeserializedAttriute.Readable);
         Assert.IsTrue(firstDeserializedAttriute.Writable);
         Assert.AreEqual(firstAttribute.OpenType, firstDeserializedAttriute.OpenType);

         IOpenMBeanAttributeInfo secondDeserializedAttribute = openInfo.Attributes.First(x => x.Name == "NestedTableAttribute");
         IOpenMBeanAttributeInfo secondAttribute = referenceInfo.Attributes.First(x => x.Name == "NestedTableAttribute");
         Assert.IsTrue(secondDeserializedAttribute.Readable);
         Assert.IsTrue(secondDeserializedAttribute.Writable);
         Assert.AreEqual(secondAttribute.OpenType, secondDeserializedAttribute.OpenType);

         IOpenMBeanOperationInfo deserializedOperation = openInfo.Operations.First(x => x.Name == "DoSomething");
         IOpenMBeanOperationInfo referenceOperation = referenceInfo.Operations.First(x => x.Name == "DoSomething");
         Assert.AreEqual(referenceOperation.ReturnOpenType, deserializedOperation.ReturnOpenType);         
      }

      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         OpenDynamic o = new OpenDynamic();
         ObjectName name = new ObjectName("Tests:key=value");
         _server.RegisterMBean(o, name);
         Uri serviceUrl = GetUri();

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = NetMXConnectorFactory.Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      protected abstract Uri GetUri();

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}