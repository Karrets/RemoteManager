namespace RemoteManagerApp.ConnectionSystem;

using Exception;
using Renci.SshNet;

public static class RemoteBridge {
    private static ConnectionConfiguration? _curConfig;
    private static SshClient? _curSession;

    public static void ConfigureSession(ConnectionConfiguration configuration) {
        _curConfig = configuration;
    }

    public static void BeginSession() {
        if (_curConfig == null)
            throw new UnconfiguredConnectionException("A connection was attempted with a null configuration");

        if (_curSession != null)
            throw new ConnectionAlreadyEstablishedException("New connection attempted while one already established");

        _curSession = new SshClient("localhost", "kodi86", @"***REMOVED***");

        _curSession.Connect();

        var r = _curSession.RunCommand("touch ~/test.xyz");
        Console.WriteLine(r.Result);
    }

    public static void EndSession() {
        _curSession = null;
    }
}