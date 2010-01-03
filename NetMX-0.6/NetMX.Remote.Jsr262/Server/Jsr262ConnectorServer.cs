using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using NetMX.Remote.Jsr262.Server;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Transfer;

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
         Soap12Addressing200408WSHttpBinding binding = new Soap12Addressing200408WSHttpBinding(SecurityMode.None);

         _serviceHost = new ServiceHost(new Jsr262ServiceImplementation(_server));
         ServiceBehaviorAttribute behavior = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.AddressFilterMode = AddressFilterMode.Any;
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;
         behavior.IncludeExceptionDetailInFaults = true;
         behavior.ValidateMustUnderstand = false;

         _serviceHost.AddServiceEndpoint(typeof(IJsr262ServiceContract), binding, _serviceUrl);
         _serviceHost.AddServiceEndpoint(typeof(IWSTransferContract), binding, _serviceUrl);
         _serviceHost.AddServiceEndpoint(typeof(IWSEventingContract), binding, _serviceUrl);
         _serviceHost.AddServiceEndpoint(typeof(IWSEnumerationContract), binding, _serviceUrl);
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
