# RemoteManager
A tool to manage servers with ssh capabilities, easily! Use a completely local GUI in tandem with simple to define modules to control a server. Almost no configuration on the remote required, just an SSH connection, and you're ready to go.

The following libraries are used,
 - [SSH.NET](https://github.com/sshnet/SSH.NET)
Used for connecting to remote servers, and running administrative commands.

 - [GTK#](https://github.com/GtkSharp/GtkSharp)
Used in tandem with glade to create all the GUI elements.

You can compile this program using MSBuild and NuGet. Presently the program is in an early beta, and non functional. Feature requests and similar are greatly appreciated, and will be considered when the application's basic feature set is fully implemented.

## Troubleshooting
### Permission denied (publickey).
This occurs when the server does not find your SSH key to be valid for any number of reasons.
First ensure that the key you are attempting to use is in fact valid. If the issue persists,
evaluate the logs produced by the SSH Daemon, this may require you to enable more verbose log outputs.
A common issue may be that the server is not configured to accept a key of the type you are supplying.

--Thanks!
