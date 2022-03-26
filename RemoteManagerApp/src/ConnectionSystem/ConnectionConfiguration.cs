namespace RemoteManagerApp.ConnectionSystem;

public struct ConnectionConfiguration
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Keyfile { get; set; }
    public int Port { get; set; }

    public ConnectionConfiguration(string host, string username, string keyfile, int port) {
        Host = host;
        Username = username;
        Keyfile = keyfile;
        Port = port;
    }
}