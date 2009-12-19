using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
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

         _serviceHost.AddServiceEndpoint(typeof (IJsr262ServiceContract), binding, _serviceUrl);
         _serviceHost.AddServiceEndpoint(typeof (ITransferContract), binding, _serviceUrl);
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

   public class Soap12Addressing200408WSHttpBinding : WSHttpBinding
   {
      public Soap12Addressing200408WSHttpBinding(SecurityMode securityMode)
         : base(securityMode)
      { }

      public override BindingElementCollection CreateBindingElements()
      {
         BindingElementCollection elements = base.CreateBindingElements();
         TextMessageEncodingBindingElement txtenc = elements.Find<TextMessageEncodingBindingElement>();
         txtenc.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
         return elements;
      }
   }
}
