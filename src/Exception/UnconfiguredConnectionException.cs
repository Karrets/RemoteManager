namespace RemoteManager; 

//Used when a connection is attempted without first configuring in RemoteBridge.cs
public class UnconfiguredConnectionException : System.Exception {
    private readonly string _message;

    public UnconfiguredConnectionException(string message) {
        _message = message;
    }

    public override string ToString() {
        return _message;
    }
}