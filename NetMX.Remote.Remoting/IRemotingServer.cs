namespace NetMX.Remote.Remoting
{
	public interface IRemotingServer
	{		
		IRemotingConnection NewClient(object credentials, out object token);
		IRemotingConnection Reconnect(string connectionId);
	}
}
