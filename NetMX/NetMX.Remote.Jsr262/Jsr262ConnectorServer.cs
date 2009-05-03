using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;

namespace NetMX.Remote.Jsr262
{
   internal sealed class Jsr262ConnectorServer : INetMXConnectorServer
   {
      private readonly Uri _serviceUrl;
      private readonly IMBeanServer _server;
      private ServiceHost _serviceHost;

      public Jsr262ConnectorServer(Uri serviceUrl, IMBeanServer server)
      {
         _serviceUrl = serviceUrl;
         _server = server;
      }

      public void Dispose()
      {
         if (_serviceHost != null)
         {
            Stop();
         }
      }

      public IMBeanServer MBeanServer
      {
         get { return _server; }
      }

      public void Start()
      {
         if (_serviceHost != null)
         {
            throw new InvalidOperationException("Server is already started.");
         }
         _serviceHost = new ServiceHost(new Jsr262ServiceImplementation(_server) );
         WSHttpBinding binding = new WSHttpBinding(SecurityMode.None);
         ServiceEndpoint endpoint = _serviceHost.AddServiceEndpoint(typeof (IJsr262ServiceContract), binding, _serviceUrl);         
         _serviceHost.Open();
      }

      public void Stop()
      {
         if (_serviceHost == null)
         {
            throw new InvalidOperationException("Server is already stopped.");
         }
         _serviceHost.Close();
         _serviceHost = null;
      }
   }
}
