namespace RemoteManager; 

using Gtk;

public class RemoteManagerUI : Window {
    private Builder _builder;
    private RemoteManagerUI(Builder builder, IntPtr handle) : base(handle) {
        _builder = builder;

        builder.Autoconnect(this);
        
        DeleteEvent += delegate { Application.Quit(); };
    }

    public static RemoteManagerUI Create() {
        var builder = new Builder(null, "RemoteManager.resources.MainWindow.glade", null);
        return new RemoteManagerUI(builder, builder.GetObject("PrimaryWindow").Handle);
    }
}