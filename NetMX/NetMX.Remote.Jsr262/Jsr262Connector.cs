using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Management;
using WSMan.NET.Transfer;


namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262Connector : INetMXConnector
   {
      private readonly Uri _serviceUrl;            
      private Jsr262MBeanServerConnection _connection;
      private bool _disposed;
      private Guid _connectionId;

      internal Jsr262Connector(Uri serviceUrl)
      {
         _serviceUrl = serviceUrl;
      }

      #region INetMXConnector Members
      public void Close()
      {
         _connection.Dispose();
         _connection = null;
      }
      public void Connect(object credentials)
      {
         Binding b = new Soap12Addressing200408WSHttpBinding(SecurityMode.None);

         ChannelFactory<IJsr262ServiceContract> factory = new ChannelFactory<IJsr262ServiceContract>(b);         
         ChannelFactory<ITransferContract> transferFactory = new ChannelFactory<ITransferContract>(b);

         _connectionId = Guid.NewGuid();         
         _connection = new Jsr262MBeanServerConnection(
            new ProxyFactory(factory, _serviceUrl),
            new ManagementClient(_serviceUrl, transferFactory, MessageVersion.Soap12WSAddressingAugust2004));
      }
      public string ConnectionId
      {
         get { return _connectionId.ToString(); }
      }
      public IMBeanServerConnection MBeanServerConnection
      {
         get { return _connection; }
      }
      #endregion

      #region IDisposable Members
      public void Dispose()
      {
         if (!_disposed)
         {
            try
            {
               _connection.Dispose();
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
