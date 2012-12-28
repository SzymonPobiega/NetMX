using System;
using System.Collections.Generic;
using System.Linq;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanServerController : BaseController
    {
        public MBeanServerController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public MBeanServerResource Get()
        {
            var beans = _serverConnection.QueryNames(null, null);
            var delegateBeanProxy = _serverConnection.CreateDynamicProxy(MBeanServerDelegate.ObjectName);

            var rootDomain = new MBeanDomain();

            foreach (var bean in beans)
            {
                var nameParts = bean.Domain.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                var domainParts = nameParts.Take(nameParts.Length - 1).ToArray();
                var currentDomain = EnsureSubdomain(rootDomain, domainParts);
                currentDomain.Beans.Add(new Resources.MBeanInfo
                                            {
                                                ObjectName = bean.CanonicalName,
                                                HRef = GetResourceUrl("bean", new {objectName = bean.CanonicalName})
                                            });
            }

            var resource = new MBeanServerResource
                               {
                                   RootDomain = rootDomain,
                                   InstanceName = delegateBeanProxy.MBeanServerId,
                                   Version = delegateBeanProxy.ImplementationVersion,
                                   StaticViewHref = GetResourceUrl("server", new {}),
                                   DynamicViewHref = GetResourceUrl("ui", new { contentFile = "mbeanserver.htm" })
                               };

            return resource;
        }

        private static MBeanDomain EnsureSubdomain(MBeanDomain currentDomain, string[] domainParts)
        {
            foreach (var domainPartName in domainParts)
            {
                var subdomain = currentDomain.Subdomains.FirstOrDefault(x => x.Name == domainPartName);
                if (subdomain == null)
                {
                    subdomain = new MBeanDomain()
                                    {
                                        Name = domainPartName
                                    };
                    currentDomain.Subdomains.Add(subdomain);
                }
                currentDomain = subdomain;
            }
            return currentDomain;
        }
    }
}