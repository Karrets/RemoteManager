﻿namespace RemoteManagerApp; 

using Gtk;

public static class RemoteManagerApp {
    private static Window? _instance;

    public static void Main(string[] args) {
        Application.Init();
        
        _instance = RemoteManagerUI.Create();
        _instance.Show();

        Application.Run();
    }
}