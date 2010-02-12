#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Represents metadata about MBean operation or constructor parameter.
   /// </summary>
   [Serializable]
   public class MBeanParameterInfo : MBeanFeatureInfo
   {
      private readonly string _type;

      /// <summary>
      /// Gets type of this parameter.
      /// </summary>
      public string Type
      {
         get { return _type; }
      }

      ///<summary>
      /// Creates new <see cref="MBeanParameterInfo"/> object.
      ///</summary>
      ///<param name="name">Name of this parameter.</param>
      ///<param name="description">Description.</param>
      ///<param name="type">Type of accepted value.</param>
      ///<param name="descriptor">Initial descriptor values.</param>
      public MBeanParameterInfo(string name, string description, string type, Descriptor descriptor)
         : base(name, description, descriptor)
      {
         _type = type;
      }

      ///<summary>
      /// Creates new <see cref="MBeanParameterInfo"/> object.
      ///</summary>
      ///<param name="name">Name of this parameter.</param>
      ///<param name="description">Description.</param>
      ///<param name="type">Type of accepted value.</param>
      public MBeanParameterInfo(string name, string description, string type)
         : this (name, description, type, new Descriptor())
      {
      }

      public override bool Equals(object obj)
      {
         MBeanParameterInfo other = obj as MBeanParameterInfo;
         return other != null &&
                Name.Equals(other.Name) &&
                Description.Equals(other.Description) &&
                Descriptor.Equals(other.Descriptor) &&
                _type.Equals(other._type);
      }

      public override int GetHashCode()
      {
         return Name.GetHashCode() ^
                Description.GetHashCode() ^
                Descriptor.GetHashCode() ^
                _type.GetHashCode();
      }
   }
}
