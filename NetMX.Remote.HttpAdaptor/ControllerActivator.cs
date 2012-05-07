using System;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NetMX.Remote.HttpAdaptor
{
    public class ControllerActivator : IHttpControllerActivator
    {
        private readonly IMBeanServerConnection _serverConnection;
        private readonly string _baseUrl;

        public ControllerActivator(IMBeanServerConnection serverConnection, string baseUrl)
        {
            _serverConnection = serverConnection;
            _baseUrl = baseUrl;
        }

        public IHttpController Create(HttpControllerContext controllerContext, Type controllerType)
        {
            return (IHttpController)Activator.CreateInstance(controllerType, _serverConnection, _baseUrl);
        }
    }
}