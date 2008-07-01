#region USING
using System;
using System.Collections.Generic;

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
   public class SimpleType : OpenType
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

      #region Static type dictionary
      private readonly static Dictionary<Type, OpenType> _typeDictionary = new Dictionary<Type, OpenType>();
      static SimpleType()
      {
         _typeDictionary[typeof (void)] = Void;
         _typeDictionary[typeof (bool)] = Boolean;
         _typeDictionary[typeof (char)] = Character;
         _typeDictionary[typeof (byte)] = Byte;
         _typeDictionary[typeof (short)] = Short;
         _typeDictionary[typeof (int)] = Integer;
         _typeDictionary[typeof (long)] = Long;
         _typeDictionary[typeof (float)] = Float;
         _typeDictionary[typeof (Double)] = Double;
         _typeDictionary[typeof (string)] = String;
         _typeDictionary[typeof (decimal)] = Decimal;
         _typeDictionary[typeof (DateTime)] = DateTime;
         _typeDictionary[typeof (TimeSpan)] = TimeSpan;
         _typeDictionary[typeof (ObjectName)] = ObjectName;
      }
      #endregion

      #region CONSTRUCTOR
      private SimpleType(Type representation, string description)
         : base(representation, representation.FullName, description)
      {
      }
      #endregion

      #region Factory
      /// <summary>
      /// Creates new <see cref="SimpleType"/> instance based on CLR type.
      /// </summary>
      /// <param name="t">CLR simple type.</param>
      /// <returns></returns>
      public static OpenType CreateSimpleType(Type t)
      {
         OpenType type;
         if (_typeDictionary.TryGetValue(t, out type))
         {
            return type;
         }
         throw new NotSupportedException("Not supported type: "+t);
      }
      /// <summary>
      /// Tests if provided CLR type maps to a <see cref="SimpleType"/>.
      /// </summary>
      /// <param name="t"></param>
      /// <returns></returns>
      public static bool IsSimpleType(Type t)
      {
         return _typeDictionary.ContainsKey(t);
      }
      #endregion

      #region Overridden
      public sealed override void Visit(OpenTypeVisitor visitor)
      {
         visitor.VisitSimpleType(this);
      }
      public sealed override bool IsValue(object value)
      {
         return (value != null) && (value.GetType() == Representation);
      }
      public sealed override OpenTypeKind Kind
      {
         get { return OpenTypeKind.SimpleType; }
      }
      public sealed override bool Equals(object obj)
      {
         SimpleType other = obj as SimpleType;
         return other != null && Representation.Equals(other.Representation);
      }
      public sealed override int GetHashCode()
      {
         return Representation.GetHashCode();
      }      
      #endregion
   }
}
