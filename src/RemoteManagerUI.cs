using System.Runtime.CompilerServices;

namespace RemoteManager; 

using ModuleSystem;
using Gtk;

public class RemoteManagerUI : Window {
    //Instances of elements in gui
    #region Properties

    [Builder.Object] private ListBox? ModuleSelector = null;
    [Builder.Object] private Box? ModuleContent = null;
    
    #endregion

    private Builder _builder;
    private List<Module> _availableModules;

    private RemoteManagerUI(Builder builder, IntPtr handle) : base(handle) {
        _builder = builder;

        builder.Autoconnect(this);
        //^^-TODO: Instead of using auto-connect, maybe do it manually as to avoid compiler warnings.

        _availableModules = RetrieveAvailableModules();
        EmployModules(_availableModules);
        
        ModuleSelector.AddSignalHandler("row-activated", ModuleSelectionEvent);
        DeleteEvent += delegate { Application.Quit(); }; //Simple anon method to close when exit button pressed.
    }

    private void EmployModules(IEnumerable<Module> modules) {
        foreach (var module in modules) {
            var newElem = new ListBoxRow();
            var label = new Label(module.GetDisplayModuleName());
            
            newElem.Visible = true;
            newElem.Activatable = true;
            newElem.Data.Add("assocModule", module);
            label.Visible = true;
            
            newElem.Add(label);
            
            ModuleSelector.Add(newElem);
        }
    }

    private static List<Module> RetrieveAvailableModules() {
        return Directory.GetFiles("modules").Select(Module.DefineModuleFromPath).ToList();
    }

    public static RemoteManagerUI Create() {
        var builder = new Builder(null, "RemoteManager.resources.MainWindow.glade", null);
        return new RemoteManagerUI(builder, builder.GetObject("PrimaryWindow").Handle);
    }

    private void ModuleSelectionEvent(ListBox listBox, EventArgs args) {
        var row = listBox.SelectedRow;
        
        Module? module = null;

        if(row.Data["assocModule"] is Module) {
            module = (Module?) row.Data["assocModule"];
        }

        if(module is null) {
            throw new MalformedModuleException("The selected module had a malformed or non module data attatched!");
        }
        
        BeginModuleSession(module);
    }
    private void BeginModuleSession(Module m) {
        var mGui = m.GetModuleGui();
        
        mGui.Visible = true;
        mGui.ChildVisible = true;

        foreach (var child in mGui.Children) {
            child.Visible = true;
        }
        ModuleContent.Add(mGui);
    }
}