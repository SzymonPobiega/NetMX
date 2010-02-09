#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
#endregion

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
   }
}
