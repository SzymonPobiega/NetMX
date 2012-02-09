using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NetMX
{
   /// <summary>
   /// Describes a constructor exposed by an MBean. Instances of this class are immutable. Subclasses may be 
   /// mutable but this is not recommended.
   /// </summary>
   [Serializable]   
   public class MBeanConstructorInfo : MBeanFeatureInfo
   {
      protected ReadOnlyCollection<MBeanParameterInfo> _signature;
      /// <summary>
      /// Gets the list of parameters for this constructor.
      /// </summary>
		public IList<MBeanParameterInfo> Signature
		{
			get { return _signature; }
		}

      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>      
      /// <param name="name">Name of constructor</param>
      /// <param name="description">Description of constructor</param>
      /// <param name="signature">Parameters for this constructor.</param>
      public MBeanConstructorInfo(string name, string description, IEnumerable<MBeanParameterInfo> signature)
			: base(name, description)
		{
         _signature = new List<MBeanParameterInfo>(signature).AsReadOnly();
		}

      public override bool Equals(object obj)
      {
         MBeanConstructorInfo other = obj as MBeanConstructorInfo;
         return other != null &&
                Name.Equals(other.Name) &&
                Description.Equals(other.Description) &&
                Descriptor.Equals(other.Descriptor) &&
                Signature.SequenceEqual(other.Signature);
      }

      public override int GetHashCode()
      {
         return _signature.Aggregate(Name.GetHashCode() ^ Description.GetHashCode() ^ Descriptor.GetHashCode(), 
               (hash, value) => hash ^ value.GetHashCode());           
      }
   }
}
