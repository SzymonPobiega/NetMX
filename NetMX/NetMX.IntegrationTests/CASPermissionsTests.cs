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

   [MBeanCASPermission(SecurityAction.Deny, Access = MBeanPermissionAction.RegisterMBean)]
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
      public int IntValue
      {
         get { return 0; }
         set { }
      }
      public void DoSomething()
      {
      }
   }

   public interface DummyMBean
   {
      int IntValue { get; set; }
      void DoSomething();
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

      [TestMethod]
      public void GetAttributePermissionTest()
      {
         IMBeanServer server = null;
         DoPermissionTest(
            delegate()
            {
               server = new MBeanServer();
               server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
            },
            delegate()
            {
               object o = server.GetAttribute(new ObjectName("dummy:"), "IntValue");
            }, "IntValue", "DoSomething", MBeanPermissionAction.GetAttribute, MBeanPermissionAction.SetAttribute);
      }

      [TestMethod]
      public void SetAttributePermissionTest()
      {
         IMBeanServer server = null;
         DoPermissionTest(
            delegate()
            {
               server = new MBeanServer();
               server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
            },
            delegate()
            {
               server.SetAttribute(new ObjectName("dummy:"), "IntValue", 5);
            }, "IntValue", "DoSomething", MBeanPermissionAction.SetAttribute, MBeanPermissionAction.GetAttribute);
      }

      [TestMethod]
      public void RegisterMBeanPermissionTest()
      {
         DoPermissionTest(
            null,
            delegate()
            {
               IMBeanServer server = new MBeanServer();
               server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
            }, null, null, MBeanPermissionAction.RegisterMBean, MBeanPermissionAction.UnregisterMBean);
      }
      [TestMethod]
      public void UnregisterMBeanPermissionTest()
      {
         IMBeanServer server = null;
         DoPermissionTest(
            delegate()
            {
               server = new MBeanServer();
               server.RegisterMBean(new Dummy(), new ObjectName("dummy:"));
            },
            delegate()
            {
               server.UnregisterMBean(new ObjectName("dummy:"));
            }, null, null, MBeanPermissionAction.UnregisterMBean, MBeanPermissionAction.RegisterMBean);
      }

      #region Utility
      private void DoPermissionTest(ThreadStart delSetup, ThreadStart del, string memberName, string otherMember, MBeanPermissionAction requiredRight, MBeanPermissionAction otherRight)
      {
         Assert.IsTrue(DoDenied(delSetup, del, null, null, null, requiredRight));
         Assert.IsTrue(DoDenied(delSetup, del, null, null, null, MBeanPermissionAction.All));
         Assert.IsFalse(DoDenied(delSetup, del, null, null, null, otherRight));

         Assert.IsFalse(DoDenied(delSetup, del, null, null, new ObjectName("dummy:type=ble"), MBeanPermissionAction.All));
         Assert.IsTrue(DoDenied(delSetup, del, null, null, new ObjectName("dummy:"), MBeanPermissionAction.All));

         if (memberName != null)
         {
            Assert.IsFalse(DoDenied(delSetup, del, null, otherMember, null, MBeanPermissionAction.All));
            Assert.IsTrue(DoDenied(delSetup, del, null, memberName, null, MBeanPermissionAction.All));
         }

         Assert.IsFalse(DoDenied(delSetup, del, typeof(int).AssemblyQualifiedName, null, null, MBeanPermissionAction.All));
         Assert.IsTrue(DoDenied(delSetup, del, typeof(DummyMBean).AssemblyQualifiedName, null, null, MBeanPermissionAction.All));
      }
      private bool DoDenied(ThreadStart delSetup, ThreadStart del, string denyClassName, string denyMemberName, ObjectName denyObjectName, MBeanPermissionAction denyAction)
      {
         if (delSetup != null)
         {
            delSetup();
         }
         MBeanCASPermission perm = new MBeanCASPermission(denyClassName, denyMemberName, denyObjectName, denyAction);
         try
         {
            perm.Deny();
            del();
            return false;
         }
         catch (SecurityException ex)
         {
            return true;
         }
         finally
         {
            CodeAccessPermission.RevertDeny();
         }
      }
      #endregion
   }
}
