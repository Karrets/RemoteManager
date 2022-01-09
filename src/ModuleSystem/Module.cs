using System.IO.Compression;
using System.Text.Json;

namespace RemoteManager.ModuleSystem; 

using Gtk;

public class Module {
    private Builder _builder;
    private Box _internalGUI;
    private JsonDocument _configFile;
    private string _internalName;
    private ZipArchive _modArchive;
    
    
    private Module(Builder builder, Box internalGUI, JsonDocument configFile, string internalName, ZipArchive modArchive) {
        _builder = builder;
        _internalGUI = internalGUI;
        _configFile = configFile;
        _internalName = internalName;
        _modArchive = modArchive;
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
            if(fileEntry.Name == "config.json") {
                configFile = JsonDocument.Parse(
                    new StreamReader(fileEntry.Open()).ReadToEnd()
                );
            } else if(fileEntry.Name == "gui.glade") {
                builder = new Builder(
                    null,
                    new StreamReader(fileEntry.Open()).ReadToEnd(),
                    null);

                try { mainGuiElem = (Box) builder.GetObject("PrimaryInterface"); }
                catch (InvalidCastException e) {
                    throw new MalformedModuleException(
                        "Primary interface is of incorrect type or does not exist!", e
                    );
                }
            }
        }

        if(builder is null || mainGuiElem is null || configFile is null) {
            throw new MalformedModuleException(
                "Either the gui element or config file is missing from the module: " + internalName
            );
        }

        return new Module(builder, mainGuiElem, configFile, internalName, zip);
    }

    public Box GetModuleGui() {
        return _internalGUI;
    }
}