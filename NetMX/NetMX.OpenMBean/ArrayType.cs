using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean
{
	[Serializable]
	public sealed class ArrayType : OpenType
	{
		public ArrayType(int dimension, OpenType elementType)
			: base(elementType.Representation, "Array", "Array")
		{
		}

		public override bool IsValue(object value)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
