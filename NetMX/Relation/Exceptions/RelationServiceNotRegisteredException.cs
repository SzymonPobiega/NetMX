#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{   
   /// <summary>
   /// This exception is raised when an access is done to the Relation Service and that one is not registered.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class RelationServiceNotRegisteredException : RelationException
   {            
      public RelationServiceNotRegisteredException() : base() { }
      private RelationServiceNotRegisteredException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{			
		}		
   }
}
