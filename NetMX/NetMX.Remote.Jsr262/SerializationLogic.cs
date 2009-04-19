using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMX.Remote.Jsr262
{
   public interface IDeserializable
   {
      object Deserialize();
   }

   public partial class NamedGenericValueType
   {
      public NamedGenericValueType()
      {
      }
      public NamedGenericValueType(string name, object value)
         : base(value)
      {
         this.name = name;
      }
   }

   public partial class GenericValueType : IDeserializable
   {
      public GenericValueType()
      {
      }

      public GenericValueType(object value)
      {
         if (value == null)
         {            
            Item = new NullType();
            ItemElementName = ItemChoiceType.Null;
         }
         else
         {
            Type valueType = value.GetType();
            if (valueType == typeof(byte[]))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Base64Binary;               
            }
            else if (valueType  == typeof(bool))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Boolean;               
            }
            else if (valueType == typeof(byte))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Byte;
            }
            else if (valueType == typeof(ushort))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Character;
            }
            else if (valueType == typeof(DateTime))
            {
               Item = value;
               ItemElementName = ItemChoiceType.DateTime;
            }
            else if (valueType == typeof(float))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Float;
            }
            else if (valueType == typeof(int))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Int;
            }            
            else if (valueType == typeof(long))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Long;
            }
            else if (valueType == typeof(short))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Short;
            }
            else if (valueType == typeof(string))
            {
               Item = value;
               ItemElementName = ItemChoiceType.String;
            }
            else if (valueType == typeof(ICollection))
            {
               Item = new MultipleValueType((ICollection) valueType);
               ItemElementName = ItemChoiceType.List;
            }
         }
      }
      public object Deserialize()
      {
         if (ItemElementName == ItemChoiceType.Null)
         {
            return null;
         }
         IDeserializable deserializable = Item as IDeserializable;
         if (deserializable != null)
         {
            return deserializable.Deserialize();
         }
         return Item;
      }
   }

   public partial class MultipleValueType : IDeserializable
   {
      public MultipleValueType()
      {
      }

      public MultipleValueType(ICollection values)
      {
         List<GenericValueType> valueTypes = new List<GenericValueType>();
         foreach (object value in values)
         {
            valueTypes.Add(new GenericValueType(value));
         }
         Value = valueTypes.ToArray();
      }
      public object Deserialize()
      {
         ArrayList results = new ArrayList();
         foreach (GenericValueType valueType in Value)
         {
            results.Add(valueType.Deserialize());
         }
         return results;
      }
   }

   //public partial class ArrayDataType_Type
   //{
   //   public ArrayDataType_Type(ArrayType arrayType)
   //   {
   //      dimension = arrayType.Dimension;
   //      Description = arrayType.Description;
   //      dimensionSpecified = true;
   //   }
   //}
}
