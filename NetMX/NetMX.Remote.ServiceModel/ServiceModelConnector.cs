using System;
using System.ServiceModel;

namespace NetMX.Remote.ServiceModel
{
   internal sealed class ServiceModelConnector : INetMXConnector
   {
      private readonly Uri _serviceUrl;
      private readonly string _configurationName;
      private IMBeanServerContract _proxy;
      private ServiceModelMBeanServerConnection _connection;
      private bool _disposed;
      private Guid _connectionId;

      internal ServiceModelConnector(string configurationName, Uri serviceUrl)
      {
         _serviceUrl = serviceUrl;
         _configurationName = configurationName;
      }

      #region INetMXConnector Members
      public void Close()
      {
         ICommunicationObject co = (ICommunicationObject) _proxy;
         if (co != null)
         {
            try
            {
               if (co.State != CommunicationState.Faulted)
               {
                  co.Close();
               }
               else
               {
                  co.Abort();
               }
            }
            catch (CommunicationException)
            {
               co.Abort();
            }
            catch (TimeoutException)
            {
               co.Abort();
            }
            catch (Exception)
            {
               co.Abort();
               throw;
            }
            finally
            {
               _proxy = null;
               _connection = null;
            }
         }
      }
      public void Connect(object credentials)
      {
         ChannelFactory<IMBeanServerContract> factory = new ChannelFactory<IMBeanServerContract>(
            _configurationName,
            new EndpointAddress(_serviceUrl));         
         _proxy = factory.CreateChannel();
         _connectionId = Guid.NewGuid();
         _connection = new ServiceModelMBeanServerConnection(_proxy);
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
               Close();
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
