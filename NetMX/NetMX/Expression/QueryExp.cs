using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
	[Serializable]
	public abstract class QueryExp
	{
      public abstract bool Match(IQueryEvaluationContext context);
	}
}
