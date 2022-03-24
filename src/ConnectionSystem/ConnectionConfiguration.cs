namespace RemoteManager.ConnectionSystem;

public struct ConnectionConfiguration
{
    private string host { get; set; }
    private string username { get; set; }
    private string keyfile { get; set; }
    private int port { get; set; }

    public ConnectionConfiguration(string host, string username, string keyfile, int port) {
        this.host = host;
        this.username = username;
        this.keyfile = keyfile;
        this.port = port;
    }
}