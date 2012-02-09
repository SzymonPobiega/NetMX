
namespace NetMX.Spring.BeanExporter.Assembler
{
   /// <summary>
   /// Represents objects which can return MBean metadata based on object which is being exported
   /// as MBean and its Spring container key.
   /// </summary>
   public interface IMBeanInfoAssembler
   {
      /// <summary>
      /// Returns MBean metadata object based on exported object instance (<paramref name="beanInstance"/>)
      /// and its Spring container key (<paramref name="beanKey"/>).
      /// </summary>
      /// <param name="beanInstance">Instance of object being exported.</param>
      /// <param name="beanKey">Spring container key.</param>
      /// <returns>Managment bean metadata object.</returns>
      MBeanInfo GetMBeanInfo(object beanInstance, string beanKey);
   }
}
