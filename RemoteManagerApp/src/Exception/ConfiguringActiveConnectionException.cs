namespace RemoteManagerApp.Exception; 

//Thrown when attempting to configure a session, while it is already active.
public class ConfiguringActiveConnectionException : System.Exception {
    private readonly string _message;

    public ConfiguringActiveConnectionException(string message) {
        _message = message;
    }

    public override string ToString() {
        return _message;
    }
}