namespace RemoteManagerApp.ConnectionSystem;

using Exception;
using Renci.SshNet;

public static class RemoteBridge {
    private static ConnectionInfo? _connectionInfo;
    private static SshClient? _curSession;

    public static void ConfigureSession(ConnectionConfiguration conf) {
        if (_curSession is {IsConnected: true}) {
            throw new ConfiguringActiveConnectionException(
                "The existing connection must be closed before it can be modified");
        }
        
        _connectionInfo = new ConnectionInfo(
            conf.Host,
            conf.Port,
            conf.Username,
            new PrivateKeyAuthenticationMethod(
                conf.Username,
                new PrivateKeyFile(conf.Keyfile) //TODO: Support encrypted private keys...
            )
        );
    }

    public static void BeginSession() {
        if (_connectionInfo == null)
            throw new UnconfiguredConnectionException("A connection was attempted with a null configuration");

        if (_curSession is {IsConnected: true})
            throw new ConnectionAlreadyEstablishedException("New connection attempted while one already established");

        _curSession = new SshClient(_connectionInfo);
        _curSession.Connect();

        // var r = _curSession.RunCommand("touch ~/test.xyz");
        // Console.WriteLine(r.Result);
    }

    public static void EndSession() {
        _curSession?.Disconnect();
    }
}