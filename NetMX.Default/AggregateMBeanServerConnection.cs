using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetMX.Server
{
    public class AggregateMBeanServerConnection : IMBeanServerConnection
    {
        private readonly Dictionary<string , IMBeanServerConnection> _aggregatedServers = new Dictionary<string, IMBeanServerConnection>();
        private readonly IMBeanServerConnection _defaultServer;

        public AggregateMBeanServerConnection(IMBeanServerConnection defaultServer)
        {
            _defaultServer = defaultServer;
        }

        public void AddChildServer(string domainPrefix, IMBeanServerConnection serverConnection)
        {
            _aggregatedServers.Add(domainPrefix, serverConnection);
        }

        public void RemoveChildServer(string domainPrefix)
        {
            _aggregatedServers.Remove(domainPrefix);
        }        

        private T Delegate<T>(ObjectName name, Func<IMBeanServerConnection, ObjectName, T> func)
        {
            var indexOfFirstDot = name.Domain.IndexOf(".");
            if (indexOfFirstDot < 0) //No prefix
            {
                return func(_defaultServer, name);
            }
            string prefix = name.Domain.Substring(0, indexOfFirstDot);
            IMBeanServerConnection delegateServer;
            if (_aggregatedServers.TryGetValue(prefix, out delegateServer))
            {
                var nameWithoutPrefix = new ObjectName(name.Domain.Substring(indexOfFirstDot + 1), name.KeyPropertyList);
                return func(delegateServer, nameWithoutPrefix);
            }
            return func(_defaultServer, name);
        }

        private void Delegate(ObjectName name, Action<IMBeanServerConnection, ObjectName> action)
        {
            var indexOfFirstDot = name.Domain.IndexOf(".");
            if (indexOfFirstDot < 0) //No prefix
            {
                action(_defaultServer, name);
            }
            else
            {
                string prefix = name.Domain.Substring(0, indexOfFirstDot);
                IMBeanServerConnection delegateServer;
                if (_aggregatedServers.TryGetValue(prefix, out delegateServer))
                {
                    var nameWithoutPrefix = new ObjectName(name.Domain.Substring(indexOfFirstDot + 1), name.KeyPropertyList);
                    action(delegateServer, nameWithoutPrefix);
                }
                else
                {
                    action(_defaultServer, name);
                }
            }
        }

        public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            Delegate(name, (s, n) => s.AddNotificationListener(n, callback, filterCallback, handback));
        }

        public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
        {
            Delegate(name, (s, n) => s.AddNotificationListener(n, listener, filterCallback, handback));
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            Delegate(name, (s, n) => s.RemoveNotificationListener(n, callback, filterCallback, handback));
        }

        public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
        {
            return Delegate(name, (s, n) => s.CreateMBean(className, n, arguments));
        }

        public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
        {
            Delegate(name, (s, n) => s.RemoveNotificationListener(n, listener, filterCallback, handback));
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
        {
            Delegate(name, (s, n) => s.RemoveNotificationListener(n, callback));
        }

        public void RemoveNotificationListener(ObjectName name, ObjectName listener)
        {
            Delegate(name, (s, n) => s.RemoveNotificationListener(n, listener));
        }

        public object Invoke(ObjectName name, string operationName, object[] arguments)
        {
            return Delegate(name, (s, n) => s.Invoke(n, operationName, arguments));
        }

        public void SetAttribute(ObjectName name, string attributeName, object value)
        {
            Delegate(name, (s, n) => s.SetAttribute(n, attributeName, value));
        }

        public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
        {
            return Delegate(name, (s, n) => s.SetAttributes(n, namesAndValues));
        }

        public object GetAttribute(ObjectName name, string attributeName)
        {
            return Delegate(name, (s, n) => s.GetAttribute(n, attributeName));
        }

        public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
        {
            return Delegate(name, (s, n) => s.GetAttributes(n, attributeNames));
        }

        public int GetMBeanCount()
        {
            return _aggregatedServers.Values.Sum(x => x.GetMBeanCount()) + _defaultServer.GetMBeanCount();
        }

        public MBeanInfo GetMBeanInfo(ObjectName name)
        {
            return Delegate(name, (s, n) => s.GetMBeanInfo(n));
        }

        public bool IsInstanceOf(ObjectName name, string className)
        {
            return Delegate(name, (s, n) => s.IsInstanceOf(n, className));
        }

        public bool IsRegistered(ObjectName name)
        {
            return Delegate(name, (s, n) => s.IsRegistered(n));
        }

        public IEnumerable<ObjectName> QueryNames(ObjectName name, IExpression<bool> query)
        {
            return _aggregatedServers
                .SelectMany(x => QueryNames(x.Value, x.Key, name, query))
                .Concat(_defaultServer.QueryNames(name, query));
        }

        private static IEnumerable<ObjectName> QueryNames(IMBeanServerConnection connection, string prefix, ObjectName name, IExpression<bool> query)
        {
            if (name != null)
            {
                ObjectName namePatternWithoutPrefix;
                return DomainPrefixMatches(name, prefix, out namePatternWithoutPrefix) 
                    ? connection.QueryNames(namePatternWithoutPrefix, query) 
                    : Enumerable.Empty<ObjectName>();
            }
            return connection.QueryNames(null, query);
        }

        private static bool DomainPrefixMatches(ObjectName namePattern, string domainPrefix, out ObjectName namePatternWithoutPrefix)
        {
            namePatternWithoutPrefix = null;
            var indexOfFirstDot = namePattern.Domain.IndexOf(".");
            if (indexOfFirstDot < 0)
            {
                return false;
            }
            var prefixPattern = namePattern.Domain.Substring(0, indexOfFirstDot);
            var patternRegex = new Regex(prefixPattern.Replace("?", ".").Replace("*", ".*"));
            return patternRegex.IsMatch(domainPrefix);
        }

        public void UnregisterMBean(ObjectName name)
        {
            Delegate(name, (s, n) => s.UnregisterMBean(n));
        }

        public string GetDefaultDomain()
        {
            return _defaultServer.GetDefaultDomain();
        }

        public IList<string> GetDomains()
        {
            return _aggregatedServers.Values
                .Concat(new[] {_defaultServer})
                .SelectMany(x => x.GetDomains())
                .ToList();
        }
    }
}