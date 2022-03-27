using System;

namespace RemoteManagerTests;

using NUnit.Framework;
using RemoteManagerApp.ConnectionSystem;
using RemoteManagerApp.Exception;

public class TestRemoteBridge {
    [Test]
    public void TestConnectionAlreadyRunningThrowsException() {
        var config = new ConnectionConfiguration(
            "dummyvm.hostname.local",
            "username",
            "/home/user/.ssh/id_rsa",
            22
        );
        
        var bridge = new RemoteBridge();
        
        bridge.ConfigureSession(config);
        bridge.BeginSession();
        Assert.Throws<ConnectionAlreadyEstablishedException>(bridge.BeginSession);
    }

    [Test]
    public void TestNullConfigurationThrowsException() {
        var bridge = new RemoteBridge();
        Assert.Throws<UnconfiguredConnectionException>(bridge.BeginSession);
    }

    [Test]
    public void TestWorkingConnection() {
        var config = new ConnectionConfiguration(
            "dummyvm.hostname.local",
            "username",
            "/home/user/.ssh/id_rsa",
            22
        );
        
        var bridge = new RemoteBridge();
        
        bridge.ConfigureSession(config);
        bridge.BeginSession();
        System.Threading.Thread.Sleep(100);
        bridge.EndSession();
    }
}