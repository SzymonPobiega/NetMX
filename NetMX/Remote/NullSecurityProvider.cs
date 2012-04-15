namespace NetMX.Remote
{
	public class NullSecurityProvider : INetMXSecurityProvider
	{
		public void Authenticate(object credentials, out object subject, out object token)
		{
			subject = null;
			token = null;
		}

		public INetMXPrincipal Authorize(object subject, object token)
		{
			return null;
		}
	}
}
