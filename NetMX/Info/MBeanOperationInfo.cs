#region USING
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
#endregion

namespace NetMX
{
   /// <summary>
   /// Represents metatada about MBean operation.
   /// </summary>
   [Serializable]
	public class MBeanOperationInfo : MBeanFeatureInfo
	{		
		private readonly string _returnType;
      /// <summary>
      /// Gets type of value returned by this operation.
      /// </summary>
		public string ReturnType
		{
			get { return _returnType; }
		}
      protected ReadOnlyCollection<MBeanParameterInfo> _signature;
      /// <summary>
      /// Gets a collection of this operation parameters.
      /// </summary>
		public IList<MBeanParameterInfo> Signature
		{
			get { return _signature; }
		}
      protected OperationImpact _impact;
      /// <summary>
      /// Gets impact of this operation.
      /// </summary>
		public OperationImpact Impact
		{
			get { return _impact; }
		}

      /// <summary>
      /// Creates new MBeanOperationInfo object.
      /// </summary>
      /// <param name="name">The name of the method.</param>
      /// <param name="description">A human readable description of the operation.</param>
      /// <param name="returnType">The type of the method's return value.</param>
      /// <param name="signature">MBeanParameterInfo objects describing the parameters(arguments) of the method. It should be an empty list if operation has no parameters.</param>
      /// <param name="impact">The impact of the method.</param>
		/// <param name="descriptor">Initial descriptor values.</param>
		public MBeanOperationInfo(string name, string description, string returnType, IEnumerable<MBeanParameterInfo> signature, OperationImpact impact, Descriptor descriptor)
			: base(name, description, descriptor)
		{
			_returnType = returnType;
		   _signature = new List<MBeanParameterInfo>(signature).AsReadOnly();
			_impact = impact;
		}

      /// <summary>
      /// Creates new MBeanOperationInfo object.
      /// </summary>
      /// <param name="name">The name of the method.</param>
      /// <param name="description">A human readable description of the operation.</param>
      /// <param name="returnType">The type of the method's return value.</param>
      /// <param name="signature">MBeanParameterInfo objects describing the parameters(arguments) of the method. It should be an empty list if operation has no parameters.</param>
      /// <param name="impact">The impact of the method.</param>
      public MBeanOperationInfo(string name, string description, string returnType, IEnumerable<MBeanParameterInfo> signature, OperationImpact impact)
         : this(name, description, returnType, signature, impact, new Descriptor())
      {
      }

      public override bool Equals(object obj)
      {
         MBeanOperationInfo other = obj as MBeanOperationInfo;
         return other != null &&
                Name.Equals(other.Name) &&
                Description.Equals(other.Description) &&
                Descriptor.Equals(other.Descriptor) &&
                ReturnType.Equals(other.ReturnType) &&
                Impact.Equals(other.Impact) &&
                Signature.SequenceEqual(other.Signature);
      }

      public override int GetHashCode()
      {
         return _signature.Aggregate(Name.GetHashCode() ^ Description.GetHashCode() ^ Descriptor.GetHashCode() ^ 
            ReturnType.GetHashCode() ^ Impact.GetHashCode(), (hash, value) => hash ^ value.GetHashCode());
      }
	}
}
