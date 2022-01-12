namespace RemoteManager.ConnectionSystem; 

using Renci.SshNet;

public static class RemoteBridge {
        private static ConnectionConfiguration? curConfig;
        private static SshClient? curSession;
        
        public static void BeginSession() {
            curSession = new SshClient("localhost", "kodi86", @"***REMOVED***");
            
            curSession.Connect();
            
            var r = curSession.RunCommand("touch /home/kodi86/test.xyz");
            Console.WriteLine(r.Result);
        }
        public static void EndSession() {
                curSession = null;
        }
}