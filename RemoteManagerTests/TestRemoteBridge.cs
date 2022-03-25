namespace RemoteManagerTests;

using NUnit.Framework;
using RemoteManagerApp.ConnectionSystem;
using RemoteManagerApp.Exception;

public class TestRemoteBridge {
    [Test]
    public void TestConnectionAlreadyRunningThrowsException() {
        var config = new ConnectionConfiguration("host","username","keyfile",22);
        RemoteBridge.ConfigureSession(config);
        RemoteBridge.BeginSession();
        Assert.Throws<ConnectionAlreadyEstablishedException>(RemoteBridge.BeginSession);
    }

    [Test]
    public void TestNullConfigurationThrowsException() {
        Assert.Throws<UnconfiguredConnectionException>(RemoteBridge.BeginSession);
    }

    [Test]
    public void TestWorkingConnection() {
        //Fail until we actually write this test
        Assert.Equals(2, (2 - 2));
    }
}