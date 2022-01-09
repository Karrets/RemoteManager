namespace RemoteManager; 

using Gtk;

public class RemoteManagerUI : Window {
    //Instances of elements in gui
    #region Properties
    
    [Builder.Object]
    private Gtk.ListBox ModuleSelector = null!;
    
    #endregion

    private Builder _builder;
    private Module[] _availableModules;

    private RemoteManagerUI(Builder builder, IntPtr handle) : base(handle) {
        _builder = builder;

        builder.Autoconnect(this);

        _availableModules = RetrieveAvailableModules();
        EmployModules(_availableModules);
        
        DeleteEvent += delegate { Application.Quit(); }; //Simple anon method to close when exit button pressed.
    }

    private void EmployModules(IEnumerable<Module> modules) {
        foreach (var module in modules) {
            var newElem = new ListBoxRow();
            var label = new Label("Module");
            
            newElem.SetProperty("visible", new GLib.Value(true));
            label.SetProperty("visible", new GLib.Value(true));
            newElem.Add(label);
        
            var mSel = (ListBox) _builder.GetObject("ModuleSelector");
        
            mSel.Add(newElem);
        }
    }

    private static Module[] RetrieveAvailableModules() {
        return new Module[3];
        // return Array.Empty<Module>();
    }

    public static RemoteManagerUI Create() {
        var builder = new Builder(null, "RemoteManager.resources.MainWindow.glade", null);
        return new RemoteManagerUI(builder, builder.GetObject("PrimaryWindow").Handle);
    }
}