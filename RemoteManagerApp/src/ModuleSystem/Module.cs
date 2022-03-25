namespace RemoteManagerApp.ModuleSystem; 

using System.IO.Compression;
using System.Text.Json;
using Gtk;

using Exception;

public class Module {
    private readonly Box _internalGUI;
    private readonly JsonDocument _configFile;
    private readonly string _internalName;
    private ZipArchive _modArchive;
    private static Dictionary<string, Module> modules = new Dictionary<string, Module>();


    private Module(Box internalGUI, JsonDocument configFile, string internalName, ZipArchive modArchive) {
        _internalGUI = internalGUI;
        _configFile = configFile;
        _internalName = internalName;
        _modArchive = modArchive;
        
        modules.Add(internalName, this);
    }

    public static Module DefineModuleFromPath(string path) {
        Builder? builder = null;
        Box? mainGuiElem = null;
        JsonDocument? configFile = null;
        var internalName = Path.GetFileNameWithoutExtension(path);

        var zip = new ZipArchive(
            new FileStream(path, FileMode.Open),
            ZipArchiveMode.Read
        );

        foreach (var fileEntry in zip.Entries) {
            switch (fileEntry.Name) {
                case "conf.json":
                case "config.json":
                    configFile = JsonDocument.Parse(
                        new StreamReader(fileEntry.Open()).ReadToEnd()
                    );
                    break;
                case "gui.glade":
                    builder = new Builder();
                    builder.AddFromString(new StreamReader(fileEntry.Open()).ReadToEnd());

                    try {
                        mainGuiElem = (Box) builder.GetObject("PrimaryInterface");
                        mainGuiElem.Unparent();
                    }
                    catch (InvalidCastException e) {
                        throw new MalformedModuleException(
                            "Primary interface is of incorrect type or does not exist!", e
                        );
                    }

                    break;
            }
        }

        if(builder is null || mainGuiElem is null || configFile is null) {
            throw new MalformedModuleException(
                "Either the gui element or config file is missing from the module: " + internalName
            );
        }

        return new Module(mainGuiElem, configFile, internalName, zip);
    }

    public string GetDisplayName() {
        return _configFile.RootElement.GetProperty("name").GetString() ?? throw new NullReferenceException(
            "Module has no given display name!"
        );
    }
    
    public string GetInternalName() {
        return _internalName;
    }
    
    public Box GetModuleGui() {
        return _internalGUI;
    }

    public string GetDescription() {
        return _configFile.RootElement.GetProperty("description").GetString() ?? throw new NullReferenceException(
            "Module has no given description!"
        );
    }
}