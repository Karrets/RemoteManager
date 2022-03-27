using System.Text;

namespace RemoteManagerApp.ConnectionSystem;

using Exception;
using Renci.SshNet;

public class RemoteBridge {
    private ConnectionInfo? _connectionInfo;
    private SshClient? _client;

    public void ConfigureSession(ConnectionConfiguration conf) {
        if (_client is {IsConnected: true}) {
            throw new ConfiguringActiveConnectionException(
                "The existing connection must be closed before it can be modified");
        }
        
        _connectionInfo = new ConnectionInfo(
            conf.Host,
            conf.Port,
            conf.Username,
            new PrivateKeyAuthenticationMethod(
                conf.Username,
                new PrivateKeyFile(conf.Keyfile)
                //TODO: Support encrypted private keys...
            )
        );
    }

    public void BeginSession() {
        if (_connectionInfo == null)
            throw new UnconfiguredConnectionException(
                "A connection was attempted with a null configuration");

        if (_client is {IsConnected: true})
            throw new ConnectionAlreadyEstablishedException(
                "New connection attempted while one already established");

        _client = new SshClient(_connectionInfo);

        _client.Connect();
        //TODO: Create GUI Helper for making dialogues, then produce error dialogs in the case of an auth failure...

        // var r = _curSession.RunCommand("touch ~/test.xyz");
        // Console.WriteLine(r.Result);
    }

    public void EndSession() {
        _client?.Disconnect();
    }
}