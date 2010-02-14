using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NetMX.Remote.Jsr262.Client;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Management;
using WSMan.NET.Transfer;


namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262Connector : INetMXConnector
   {
      private readonly Uri _serviceUrl;
      private readonly string _bindingConfiguration;
      private readonly int _enumerationMaxElements;
      private Jsr262MBeanServerConnection _connection;
      private bool _disposed;
      private Guid _connectionId;

      public Jsr262Connector(Uri serviceUrl, string bindingConfiguration, int enumerationMaxElements)
      {
         _serviceUrl = serviceUrl;
         _bindingConfiguration = bindingConfiguration;
         _enumerationMaxElements = enumerationMaxElements;
      }

      #region INetMXConnector Members
      public void Close()
      {
         _connection.Dispose();
         _connection = null;
      }
      public void Connect(object credentials)
      {
         Binding b = _bindingConfiguration != null 
            ? new Soap12Addressing200408WSHttpBinding(_bindingConfiguration) 
            : new Soap12Addressing200408WSHttpBinding(SecurityMode.None);         

         ChannelFactory<IJsr262ServiceContract> factory = new ChannelFactory<IJsr262ServiceContract>(b);         
         ChannelFactory<IWSTransferContract> transferFactory = new ChannelFactory<IWSTransferContract>(b);

         _connectionId = Guid.NewGuid();
         _connection = new Jsr262MBeanServerConnection(
            _enumerationMaxElements,
            new ProxyFactory(factory, _serviceUrl),
            new ManagementClient(_serviceUrl, transferFactory, MessageVersion.Soap12WSAddressingAugust2004),
            new EnumerationClient(true, _serviceUrl, b), 
            new EventingClient(_serviceUrl, b));
      }
      public string ConnectionId
      {
         get { return _connectionId.ToString(); }
      }
      public IMBeanServerConnection MBeanServerConnection
      {
         get {
            return _connection; }
      }
      #endregion

      #region IDisposable Members
      public void Dispose()
      {
         if (!_disposed)
         {
            try
            {
               if (_connection != null)
               {
                  _connection.Dispose();
                  _connection = null;
               }
            }
            finally
            {
               _disposed = true;
            }
         }
      }
      #endregion
   }   
}
