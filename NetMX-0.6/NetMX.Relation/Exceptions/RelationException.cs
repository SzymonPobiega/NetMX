#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{   
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public class RelationException : NetMXException
   {            
      public RelationException() : base() { }
      public RelationException(string message) : base(message) { }
      protected RelationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{			
		}		     
   }
}
