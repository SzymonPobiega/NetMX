#region USING
using System;
#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The SimpleType class is the open type class whose instances describe all open data values which 
   /// are neither arrays, nor <see cref="ICompositeData"/> values, nor <see cref="ITabularData"/> values. 
   /// It predefines all its possible instances as static fields, and has no public constructor.
   /// 
   /// Given a SimpleType instance describing values whose class is <see cref="OpenType.Representation"/>, the internal field 
   /// corresponding to the name of this SimpleType instance is set to this class full name (<see cref="Type.FullName"/>).   
   /// </summary>
   [Serializable]   
   public sealed class SimpleType : OpenType
   {
      #region MEMBERS
      /// <summary>
      /// Void type.
      /// </summary>
      public static readonly OpenType Void = new SimpleType(typeof (void), "Void type");
      /// <summary>
      /// Boolean type.
      /// </summary>
      public static readonly OpenType Boolean = new SimpleType(typeof (bool), "Boolean type");
      /// <summary>
      /// Character type (UTF-16).
      /// </summary>
      public static readonly OpenType Character = new SimpleType(typeof(char), "Character type (UTF-16)");
      /// <summary>
      /// Byte type (8 bit, unsigned).
      /// </summary>
      public static readonly OpenType Byte = new SimpleType(typeof(byte), "Byte type (8 bit, unsigned)");
      /// <summary>
      /// Short integer type (16 bit, signed).
      /// </summary>
      public static readonly OpenType Short = new SimpleType(typeof(short), "Short integer type (16 bit, signed)");
      /// <summary>
      /// Integer type (32 bit, signed).
      /// </summary>
      public static readonly OpenType Integer = new SimpleType(typeof(int), "Integer type (32 bit, signed)");
      /// <summary>
      /// Long integer type (64 bit, signed).
      /// </summary>
      public static readonly OpenType Long = new SimpleType(typeof(long), "Long integer type (64 bit, signed)");
      /// <summary>
      /// Float type.
      /// </summary>
      public static readonly OpenType Float = new SimpleType(typeof(float), "Float type");
      /// <summary>
      /// Double precision float type.
      /// </summary>
      public static readonly OpenType Double = new SimpleType(typeof(double), "Double precision float type");
      /// <summary>
      /// String type.
      /// </summary>
      public static readonly OpenType String = new SimpleType(typeof(string), "String type");
      /// <summary>
      /// Decimal (fixed-point) type.
      /// </summary>
      public static readonly OpenType Decimal = new SimpleType(typeof(decimal), "Decimal (fixed-point) type");
      /// <summary>
      /// Date and time type.
      /// </summary>
      public static readonly OpenType DateTime = new SimpleType(typeof(DateTime), "Date and time type");
      /// <summary>
      /// Time span type.
      /// </summary>
      public static readonly OpenType TimeSpan = new SimpleType(typeof(TimeSpan), "Time span type");
      /// <summary>
      /// ObjectName type.
      /// </summary>
      public static readonly OpenType ObjectName = new SimpleType(typeof(ObjectName), "ObjectName type");      
      #endregion
      
      #region CONSTRUCTOR
      private SimpleType(Type representation, string description)
         : base(representation, representation.FullName, description)
      {
      }
      #endregion

      #region Factory
      public static OpenType CreateFromType(Type t)
      {
         if (t == typeof(void))
         {
            return Void;
         }
         else if (t == typeof(bool))
         {
            return Boolean;
         }
         else if (t == typeof(char))
         {
            return Character;
         }
         else if (t == typeof(byte))
         {
            return Byte;
         }
         else if (t == typeof(short))
         {
            return Short;
         }
         else if (t == typeof(int))
         {
            return Integer;
         }
         else if (t == typeof(long))
         {
            return Long;
         }
         else if (t == typeof(float))
         {
            return Float;
         }
         else if (t == typeof(double))
         {
            return Double;
         }
         else if (t == typeof(string))
         {
            return String;
         }
         else if (t == typeof(decimal))
         {
            return Decimal;
         }
         else if (t == typeof(DateTime))
         {
            return DateTime;
         }
         else if (t == typeof(TimeSpan))
         {
            return TimeSpan;
         }
         else if (t == typeof(ObjectName))
         {
            return ObjectName;
         }
         else
         {
            throw new NotSupportedException("Not supported type: "+t);
         }
      }
      #endregion

      #region Overridden
      public override bool IsValue(object value)
      {
         return (value != null) && (value.GetType() == Representation);
      }
      public override OpenTypeKind Kind
      {
         get { return OpenTypeKind.SimpleType; }
      }
      public override bool Equals(object obj)
      {
         SimpleType other = obj as SimpleType;
         return other != null && Representation.Equals(other.Representation);
      }
      public override int GetHashCode()
      {
         return Representation.GetHashCode();
      }      
      #endregion
   }
}
