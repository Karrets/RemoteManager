namespace RemoteManager.Exception; 

public class ConnectionAlreadyEstablishedException : System.Exception {
    private readonly string _message;

    public ConnectionAlreadyEstablishedException(string message) {
        _message = message;
    }

    public override string ToString() {
        return _message;
    }
}