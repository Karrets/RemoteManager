namespace RemoteManager; 

using Gtk;

public class RemoteManagerApp {
    private static RemoteManagerApp? _instance;

    private RemoteManagerApp() {
        var window = RemoteManagerUI.Create();
        window.Show();
        _instance = this;
    }
    
    public static void Main(string[] args) {
        Application.Init();
        
        _instance = new RemoteManagerApp();

        Application.Run();
    }
}