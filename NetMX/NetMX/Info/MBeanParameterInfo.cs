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
   }
}
