
namespace NetMX.OpenMBean
{
   /// <summary>
   /// Specifies the behavior of specific type of enumeration data objects which represent enumerations.
   /// </summary>
   public interface IEnumerationData
   {
      /// <summary>
      /// Gets the integer value of enumeration data.
      /// </summary>
      int Value { get; }
      /// <summary>
      /// Gets the enumeration type of this enumeration data instance.
      /// </summary>
      EnumerationType EnumerationType { get; }
   }
}
