using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace NetMX.Remote.Jsr262
{
    public class FragmentHeaderAttribute : Attribute, IServiceBehavior
    {
        public class MessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            {
                MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
                request = buffer.CreateMessage();
                Message originalMessage = buffer.CreateMessage();

                foreach (var header in request.Headers)
                {
                    if (header.Namespace == "http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd")
                    {
                        request.Headers.UnderstoodHeaders.Add(header);
                        Debug.WriteLine(string.Format("{0}:{1}", header.Namespace, header.Name));
                    }
                }
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
               if (reply == null)
               {
                  return;
               }
               ConnectionIdHeader header = new ConnectionIdHeader();
               int i = reply.Headers.FindHeader(header.Name, header.Namespace);
               if (-1 >= i)
               {
                  reply.Headers.Add(header);
               }
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var inspector = new MessageInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {}

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {}

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            for (int i = 0; i < serviceHostBase.ChannelDispatchers.Count; i++)
            {
                ChannelDispatcher channelDispatcher = serviceHostBase.ChannelDispatchers[i] as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        var inspector = new MessageInspector();
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                    }
                }
            }
        }
    }
}