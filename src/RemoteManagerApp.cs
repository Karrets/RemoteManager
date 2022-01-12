using RemoteManager.ConnectionSystem;

namespace RemoteManager; 

using Gtk;

public static class RemoteManagerApp {
    private static Window? _instance;

    public static void Main(string[] args) {
        RemoteBridge.BeginSession();
        
        Application.Init();
        
        _instance = RemoteManagerUI.Create();
        _instance.Show();

        Application.Run();
    }
}