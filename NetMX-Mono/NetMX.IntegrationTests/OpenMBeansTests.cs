using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMX.Default;
using NetMX.OpenMBean;

namespace NetMX.IntegrationTests
{
   [OpenMBean]
   public interface OpenMBean
   {
      [OpenMBeanAttributeAttribute(DefaultValue = 3, MinValue = 1, MaxValue = 5)]
      int OpenAttribute { get; set; }
      [OpenMBeanAttributeAttribute()]
      string[] OpenArrayAttribute { get; set; }
      [OpenMBeanAttributeAttribute(LegalValues = new object[] { 1, 2, 3 })]
      int OpenDiscreteValuesAttribute { get; set; }      
   }
   public class Open : OpenMBean
   {
      private int _openAttribute;
      private string[] _openArrayAttribute;
      private int _openDiscreteValuesAttribute;

      #region OpenMBean Members
      public int OpenAttribute
      {
         get { return _openAttribute; }
         set { _openAttribute = value; }
      }
      public string[] OpenArrayAttribute
      {
         get { return _openArrayAttribute; }
         set { _openArrayAttribute = value; }
      }
      public int OpenDiscreteValuesAttribute
      {
         get { return _openDiscreteValuesAttribute; }
         set { _openDiscreteValuesAttribute = value; }
      }
      #endregion
   }

   [TestClass]
   public class OpenMBeansTests
   {      
      [TestMethod]
      public void TestGetMBeanInfo()
      {
         IMBeanServer server = new MBeanServer();
         server.RegisterMBean(new Open(), "Sample:");
         MBeanInfo info = server.GetMBeanInfo("Sample:");
         Assert.IsTrue(info is IOpenMBeanInfo);         
         IList<MBeanAttributeInfo> attributes = info.Attributes;
         foreach (MBeanAttributeInfo attribute in attributes)
         {
            Assert.IsTrue(attribute is IOpenMBeanAttributeInfo);
            IOpenMBeanAttributeInfo openAttr = (IOpenMBeanAttributeInfo) attribute;
            
         }
      }
   }
}
