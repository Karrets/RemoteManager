namespace RemoteManager; 

using Gtk;

public class ToolbarManager {
    //Instances of elements in gui
    #region Properties

    // ReSharper disable InconsistentNaming FieldCanBeMadeReadOnly.Local MemberInitializerValueIgnored

    [Builder.Object] private MenuItem? SettingsOption = null!;
    [Builder.Object] private ApplicationWindow? SettingsDialog = null!;

    // ReSharper restore InconsistentNaming FieldCanBeMadeReadOnly.Local MemberInitializerValueIgnored

    #endregion

    
    
}