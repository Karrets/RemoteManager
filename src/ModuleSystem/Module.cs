using System.IO.Compression;
using System.Text.Json;

namespace RemoteManager.ModuleSystem; 

using Gtk;

public class Module {
    private Box _internalGUI;
    private JsonDocument _configFile;
    private string _internalName;
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
                    uint handle = builder.AddFromString(new StreamReader(fileEntry.Open()).ReadToEnd());

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

    public string? GetDisplayModuleName() {
        return _configFile.RootElement.GetProperty("name").GetString();
    }
    
    public string? GetInternalModuleName() {
        return _internalName;
    }
    
    public Box GetModuleGui() {
        return _internalGUI;
    }
}