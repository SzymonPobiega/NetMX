using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote
{
	public class NullSecurityProvider : NetMXSecurityProvider
	{
		public override void Authenticate(object credentials, out object subject, out object token)
		{
			subject = null;
			token = null;
		}

		public override INetMXPrincipal Authorize(object subject, object token)
		{
			return null;
		}
	}
}
