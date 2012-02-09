using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
    [Flags]
    public enum MBeanPermissionAction
    {
        AddNotificationListener = 1,
        GetAttribute = 2,
        GetDomains = 4,
        GetMBeanInfo = 8,
        GetObjectInstance = 16,
        Instantiate = 32,
        Invoke = 64,
        IsInstanceOf = 128,
        QueryMBeans = 256,
        QueryNames = 512,
        RegisterMBean = 1024,
        RemoveNotificationListener = 2048,
        SetAttribute = 4096,
        UnregisterMBean = 8192,
        All = 16383
    }
}
