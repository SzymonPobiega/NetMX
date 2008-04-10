using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean
{
	[Serializable]
	public sealed class CompositeType : OpenType
	{
		public CompositeType(string typeName, string description, IEnumerable<string> itemNames,
			IEnumerable<string> itemDescriptions, IEnumerable<string> itemTypes)
			: base(OpenTypeRepresentation.Composite, typeName, description)
		{
		}

		public override bool IsValue(object value)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
