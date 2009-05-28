namespace NetMX.Remote.Jsr262
{
   internal static class Schema
   {
      /// <summary>
      /// URI of namespece for the connector.
      /// </summary>
      public const string ConnectorNamespace = "http://jsr262.dev.java.net/jmxconnector";
      /// <summary>
      /// URI of resource representing MBean.
      /// </summary>
      public const string DynamicMBeanResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";
      /// <summary>
      /// URI of resource representing MBean server.
      /// </summary>
      public const string MBeanServerResourceUri = "http://jsr262.dev.java.net/MBeanServerResource";
      /// <summary>
      /// URI of JSR-262 invoke action.
      /// </summary>
      public const string InvokeAction = "http://jsr262.dev.java.net/DynamicMBeanResource/Invoke";
      /// <summary>
      /// URI of JSR-262 invoke action reposnse.
      /// </summary>
      public const string InvokeResponseAction = "http://jsr262.dev.java.net/DynamicMBeanResource/InvokeResponse";
      /// <summary>
      /// URI of JSR-262 check instance type action.
      /// </summary>
      public const string InstanceOfAction = "http://jsr262.dev.java.net/DynamicMBeanResource/IsInstanceOf";
      /// <summary>
      /// URI of JSR-262 check instance type action response.
      /// </summary>
      public const string InstanceOfResponseAction = "http://jsr262.dev.java.net/DynamicMBeanResource/IsInstanceOfResponse";
      /// <summary>
      /// URI of JSR-262 get MBean metadata action.
      /// </summary>
      public const string GetMBeanInfoAction = "http://jsr262.dev.java.net/DynamicMBeanResource/Metadata";
      /// <summary>
      /// URI of JSR-262 get MBean metadata action response.
      /// </summary>
      public const string GetMBeanInfoResponseAction = "http://jsr262.dev.java.net/DynamicMBeanResource/MetadataResponse";
      /// <summary>
      /// WS-Enumeration filter dialect for QueryMBeans method.
      /// </summary>
      public const string QueryMBeansDialect = @"http://jsr262.dev.java.net/DynamicMBeanResource/Filter/Query/Instance";
      /// <summary>
      /// WS-Enumeration filter dialect for QueryNames method.
      /// </summary>
      public const string QueryNamesDialect = @"http://jsr262.dev.java.net/DynamicMBeanResource/Filter/Query/Name";
   }
}