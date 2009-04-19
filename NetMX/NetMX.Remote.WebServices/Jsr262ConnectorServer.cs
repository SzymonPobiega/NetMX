using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using NetMX.Remote.WebServices.Jsr262;

namespace NetMX.Remote.WebServices
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
         _serviceHost = new ServiceHost(new NetMXWSServiceImpl(_server) );
         WSHttpBinding binding = new WSHttpBinding(SecurityMode.None);
         ServiceEndpoint endpoint = _serviceHost.AddServiceEndpoint(typeof (INetMXWSService), binding, _serviceUrl);         
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
