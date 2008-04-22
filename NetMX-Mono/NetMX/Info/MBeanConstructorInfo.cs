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
      #region PROPERTIES
      private ReadOnlyCollection<MBeanParameterInfo> _signature;
      /// <summary>
      /// Gets the list of parameters for this constructor.
      /// </summary>
		public IList<MBeanParameterInfo> Signature
		{
			get { return _signature; }
         protected set { _signature = value as ReadOnlyCollection<MBeanParameterInfo>; }
		}
      #endregion

      #region CONSTRUCTOR
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
      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>      
      /// <param name="name">Name of constructor</param>
      /// <param name="description">Description of constructor</param>
      /// <param name="signature">Parameters for this constructor.</param>
      /// <param name="dummy">A dummy parameter used to differenciate constructor signatures.</param>
      protected MBeanConstructorInfo(string name, string description, ReadOnlyCollection<MBeanParameterInfo> signature, bool dummy)
         : base(name, description)
      {
         _signature = signature;
      }
      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>
      /// <param name="info">Object describing CLR constructor.</param>      
      public MBeanConstructorInfo(ConstructorInfo info)
			: base(info.Name, InfoUtils.GetDescrition(info, info, "MBean constructor"))
		{			
			ParameterInfo[] paramInfos = info.GetParameters();
			List<MBeanParameterInfo> tmp = new List<MBeanParameterInfo>();			
			for (int i = 0; i < paramInfos.Length; i++)
			{
				tmp.Add(new MBeanParameterInfo(paramInfos[i]));
			}
			_signature = tmp.AsReadOnly();
		}
      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>
      /// <param name="info">Object describing CLR constructor.</param>      
      /// <param name="dummy">A dummy parameter used to differenciate constructor signatures.</param>
      public MBeanConstructorInfo(ConstructorInfo info, bool dummy)
         : base(info.Name, InfoUtils.GetDescrition(info, info, "MBean constructor"))
      {         
      }
      #endregion
   }
}
