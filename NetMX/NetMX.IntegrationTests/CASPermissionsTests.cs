using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Security.Permissions;
using NetMX.Default;
using System.Security;

namespace NetMX.IntegrationTests
{
    public class Trusted : TrustedMBean, IMBeanRegistration
    {
        private IMBeanServer _server;

        #region IMBeanRegistration Members
        public void PostDeregister()
        {
        }

        public void PostRegister(bool registrationDone)
        {
            _server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
        }

        public void PreDeregister()
        {
        }

        public ObjectName PreRegister(IMBeanServer server, ObjectName name)
        {
            _server = server;
            return name;
        }
        #endregion
    }

    public interface TrustedMBean
    {
    }

    [MBeanCASPermission(SecurityAction.Deny, Access=MBeanPermissionAction.RegisterMBean)]
    public class Untrusted : UntrustedMBean, IMBeanRegistration
    {
        private IMBeanServer _server;

        #region IMBeanRegistration Members
        public void PostDeregister()
        {
        }

        public void PostRegister(bool registrationDone)
        {
            _server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
        }

        public void PreDeregister()
        {
        }

        public ObjectName PreRegister(IMBeanServer server, ObjectName name)
        {
            _server = server;
            return name;
        }
        #endregion
    }

    public interface UntrustedMBean
    {
    }

    public class Dummy : DummyMBean
    {
    }

    public interface DummyMBean
    {
    }

    [TestClass]
    public class CASPermissionsTests
    {
        [TestMethod]
        public void RegisterMBeanSuccessTest()
        {
            IMBeanServer server = new MBeanServer();
            server.RegisterMBean(new Trusted(), new ObjectName("trusted:"));
        }
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void RegisterMBeanFailureTest()
        {
            IMBeanServer server = new MBeanServer();
            server.RegisterMBean(new Untrusted(), new ObjectName("trusted:"));
        }
    }
}
