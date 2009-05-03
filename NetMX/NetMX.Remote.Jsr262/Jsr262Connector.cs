using System;
using System.ServiceModel;


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
         ChannelFactory<IJsr262ServiceContract> factory = new ChannelFactory<IJsr262ServiceContract>(
            new WSHttpBinding(SecurityMode.None));         
         _connectionId = Guid.NewGuid();         
         _connection = new Jsr262MBeanServerConnection(new ProxyFactory(factory, _serviceUrl));
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
