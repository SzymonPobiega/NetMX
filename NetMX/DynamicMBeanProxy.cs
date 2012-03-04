using System;
using System.Dynamic;

namespace NetMX
{
    public class DynamicMBeanProxy : DynamicObject
    {
        private readonly ObjectName _objectName;
        private readonly IMBeanServerConnection _serverConnection;

        public DynamicMBeanProxy(ObjectName objectName, IMBeanServerConnection serverConnection)
        {
            _objectName = objectName;
            _serverConnection = serverConnection;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = _serverConnection.Invoke(_objectName, binder.Name, args);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _serverConnection.GetAttribute(_objectName, binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _serverConnection.SetAttribute(_objectName, binder.Name, value);
            return true;
        }

    }
}