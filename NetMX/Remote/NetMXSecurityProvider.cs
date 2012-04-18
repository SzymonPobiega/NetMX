
namespace NetMX.Remote
{
    public interface INetMXSecurityProvider
    {
        void Authenticate(object credentials, out object subject, out object token);
        INetMXPrincipal Authorize(object subject, object token);
    }
}
