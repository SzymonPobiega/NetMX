using System;
using System.Dynamic;

namespace NetMX
{
    ///<summary>
    /// A dynamic object that proxies a remote MBean.
    ///</summary>
    public class DynamicMBeanProxy : DynamicObject
    {
        private readonly ObjectName _objectName;
        private readonly IMBeanServerConnection _serverConnection;

        ///<summary>
        /// Creates new instance of <see cref="DynamicMBeanProxy"/>.
        ///</summary>
        ///<param name="objectName"></param>
        ///<param name="serverConnection"></param>
        public DynamicMBeanProxy(ObjectName objectName, IMBeanServerConnection serverConnection)
        {
            _objectName = objectName;
            _serverConnection = serverConnection;
        }

        ///<summary>
        /// Forwards method invocation as operation invocation to remote server via a connection.
        ///</summary>
        ///<param name="binder"></param>
        ///<param name="args"></param>
        ///<param name="result"></param>
        ///<returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = _serverConnection.Invoke(_objectName, binder.Name, args);
            return true;
        }

        ///<summary>
        /// Forwards property get action as attribute value retrieval to remote server via a connection.
        ///</summary>
        ///<param name="binder"></param>
        ///<param name="result"></param>
        ///<returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _serverConnection.GetAttribute(_objectName, binder.Name);
            return true;
        }

        /// <summary>
        /// Forwards property set action as attribute value update to remote server via a connection.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _serverConnection.SetAttribute(_objectName, binder.Name, value);
            return true;
        }

    }
}