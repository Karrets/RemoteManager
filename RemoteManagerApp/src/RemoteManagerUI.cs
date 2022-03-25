namespace RemoteManagerApp;

using ModuleSystem;
using Exception;
using Gtk;

public class RemoteManagerUI : Window {
    //Instances of elements in gui
    #region Properties

    // ReSharper disable InconsistentNaming FieldCanBeMadeReadOnly.Local MemberInitializerValueIgnored
    
    // Comments disable warnings about non initialization, as your IDE is unlikely to be aware of
    // the gtk# auto-initializer...
    
    [Builder.Object] private SearchEntry ModuleSearch = null!;
    [Builder.Object] private ListBox ModuleSelector = null!;
    [Builder.Object] private Paned AllContent = null!;
    
    // ReSharper restore InconsistentNaming FieldCanBeMadeReadOnly.Local MemberInitializerValueIgnored

    #endregion

    private Builder _builder;
    private ToolbarManager _toolbarManager;
    private List<Module> _availableModules;

    private RemoteManagerUI(Builder builder, IntPtr handle) : base(handle) {
        _builder = builder;
        _toolbarManager = new ToolbarManager();

        builder.Autoconnect(this);
        builder.Autoconnect(_toolbarManager);

        _availableModules = RetrieveAvailableModules();
        EmployModules(_availableModules);

        ModuleSelector.AddSignalHandler("row-activated", ModuleSelectionEvent);

        ModuleSearch.AddSignalHandler(
            "search-changed",
            delegate(SearchEntry _, EventArgs _) { ModuleSelector?.InvalidateFilter(); }
        );

        ModuleSelector.FilterFunc = ModuleSearchFilter;

        DeleteEvent += delegate { Application.Quit(); }; //Simple anon method to close when exit button pressed.
    }

    private static int CalcStringDifference(string s, string t) {
        //https://stackoverflow.com/a/6944095 Thank you to the Stack-Overflow community and the answerer 'Marty Neal'
        //I trust their implementation of the Damereau-Levenshein Distance algorithm.

        if(string.IsNullOrEmpty(s)) {
            return string.IsNullOrEmpty(t) ? 0 : t.Length;
        }

        if(string.IsNullOrEmpty(t)) { return s.Length; }

        var n = s.Length;
        var m = t.Length;
        var d = new int[n + 1, m + 1];

        // initialize the top and right of the table to 0, 1, 2, ...
        for(var i = 0; i <= n; d[i, 0] = i++) {};
        for(var j = 1; j <= m; d[0, j] = j++) {};

        for (var i = 1; i <= n; i++) {
            for (var j = 1; j <= m; j++) {
                var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                var min1 = d[i - 1, j] + 1;
                var min2 = d[i, j - 1] + 1;
                var min3 = d[i - 1, j - 1] + cost;
                d[i, j] = Math.Min(Math.Min(min1, min2), min3);
            }
        }

        return d[n, m];
    }

    private bool ModuleSearchFilter(ListBoxRow row) {
        if(ModuleSearch.Text is null or "") return true;

        Module? module;

        try { module = (Module?) row.Data["assocModule"]; }
        catch (InvalidCastException) {
            throw new InvalidDataException("Module for given row \"" + row.Name + "\" is of incorrect type.");
        }

        if(module is null) { throw new MissingMemberException("No associated module for row: " + row.Name); }

        var query = ModuleSearch.Text;
        var moduleDName = module.GetDisplayName();
        var moduleIName = module.GetInternalName();
        var moduleDescription = module.GetDescription();

        var distance = new[] {
            CalcStringDifference(query, moduleDName),
            (CalcStringDifference(query, moduleIName) * 2),
            (CalcStringDifference(query, moduleDescription) / 5)
        }.Min();
        //Some basic weighting, internal module name is half as relevant, where as the description is 5 times more!
        //This should be tweaked and tuned until it feels right.

        Console.WriteLine(distance);
        return (distance <= 10); //Maximum 'distance' value that the given option can be before it is discarded!
    }

    private void EmployModules(IEnumerable<Module> modules) {
        foreach (var module in modules) {
            var newElem = new ListBoxRow();
            var label = new Label(module.GetDisplayName());

            newElem.Visible = true;
            newElem.Activatable = true;
            newElem.Data.Add("assocModule", module);
            label.Visible = true;

            newElem.Add(label);

            ModuleSelector?.Add(newElem);
        }
    }

    private static List<Module> RetrieveAvailableModules() {
        return Directory.GetFiles("modules").Select(Module.DefineModuleFromPath).ToList();
    }

    public static RemoteManagerUI Create() {
        var builder = new Builder(null, "RemoteManagerApp.resources.MainWindow.glade", null);
        return new RemoteManagerUI(builder, builder.GetObject("PrimaryWindow").Handle);
    }

    private void ModuleSelectionEvent(ListBox listBox, EventArgs args) {
        var row = listBox.SelectedRow;

        Module? module = null;

        if(row.Data["assocModule"] is Module) { module = (Module?) row.Data["assocModule"]; }

        if(module is null) {
            throw new MalformedModuleException("The selected module is malformed or module was not parsed correctly!");
        }

        BeginModuleSession(module);
    }

    private void BeginModuleSession(Module m) {
        var mGui = m.GetModuleGui();

        // mGui.PreInit()? May be needed later if certain modules need to be prepped or something...

        if (AllContent.Child2 is not null) {
            AllContent.Remove(AllContent.Child2);
        }
        AllContent.Add2(mGui);
    }
}