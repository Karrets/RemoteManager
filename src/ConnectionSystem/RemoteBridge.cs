namespace RemoteManager.ConnectionSystem; 

using Renci.SshNet;

public static class RemoteBridge {
        private static ConnectionConfiguration? _curConfig;
        private static SshClient? _curSession;
        
        public static void BeginSession() {
            _curSession = new SshClient("localhost", "kodi86", @"***REMOVED***");
            
            _curSession.Connect();
            
            var r = _curSession.RunCommand("touch ~/test.xyz");
            Console.WriteLine(r.Result);
        }
        public static void EndSession() {
                _curSession = null;
        }
}