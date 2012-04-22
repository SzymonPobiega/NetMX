using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace NetMX.Remote.Tests
{
   [TestFixture]
   public abstract class NotificationTests
   {      
      [Test]
      public void When_adding_remote_notification_listener_new_notification_listener_is_added_to_the_server()
      {
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, null);

         _server.AssertWasCalled(x => x.AddNotificationListener("Tests:key=value", OnNotificaion, null, null),
            x => x.IgnoreArguments().Constraints(Is.Equal(new ObjectName("Tests:key=value")), Is.Anything(), Is.Anything(), Is.Anything()));         
      }

      [Test]
      public void When_removing_specific_remote_notification_listener_only_this_listener_is_removed_from_the_server()
      {
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, 1);
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, 2);
         _remoteServer.RemoveNotificationListener("Tests:key=value", OnNotificaion, null, 1);

         //Force wait for possibly asynchornous Remove to complete.
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, null);

         _server.AssertWasCalled(x => x.RemoveNotificationListener("Tests:key=value", OnNotificaion, null, null),
            x => x.IgnoreArguments()
               .Constraints(Is.Equal(new ObjectName("Tests:key=value")), Is.Anything(), Is.Anything(), Is.Anything())
               .Repeat.Once());
      }

      [Test]
      public void When_removing_non_specific_remote_notification_listener_all_listeners_are_removed_from_the_server()
      {
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, 1);
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, 2);

         _remoteServer.RemoveNotificationListener("Tests:key=value", OnNotificaion);

         //Force wait for possibly asynchornous Remove to complete.
         _remoteServer.AddNotificationListener("Tests:key=value", OnNotificaion, null, null);

         _server.AssertWasCalled(x => x.RemoveNotificationListener("Tests:key=value", OnNotificaion, null, null),
            x => x.IgnoreArguments()
               .Constraints(Is.Equal(new ObjectName("Tests:key=value")), Is.Anything(), Is.Anything(), Is.Anything())
               .Repeat.Twice());
         
      } 
    
      private void OnNotificaion(Notification notification, object handback)
      {
         _notificationFlag.Set();
      }

      private static bool FilterNotificaion(Notification notification)
      {
         return true;
      }

      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;
      private ManualResetEvent _notificationFlag;

      [SetUp]
      public void SetUp()
      {
         _notificationFlag = new ManualResetEvent(false);
         _server = MockRepository.GenerateMock<IMBeanServer>();         
         Uri serviceUrl = GetUri();

         _connectorServer = GetConnectorServerFactory().NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = GetConnectorFactory().Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      protected abstract Uri GetUri();
      protected abstract INetMXConnectorServerFactory GetConnectorServerFactory();
      protected abstract INetMXConnectorFactory GetConnectorFactory();

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}