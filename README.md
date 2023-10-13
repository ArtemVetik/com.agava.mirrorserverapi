
<br/>
<p align="center">
  <h3 align="center">Mirror Server Api</h3>

  <p align="center">
    Unity Package. Provides API for creating and connecting to Mirror servers using JoinCode!
    <br/>
    <br/>
  </p>
</p>



## About

Servers are created using REST requests to WebApi. See [here](https://github.com/ArtemVetik/docker-gameserver-factory.git) WebApi implementation with server creation methods.


### Installation

Make sure you have standalone Git installed first. Reboot after installation.
In Unity, open "Window" -> "Package Manager".
Click the "+" sign on top left corner -> "Add package from git URL..."
Paste this: `https://github.com/ArtemVetik/com.agava.mirrorserverapi.git#1.0.0`
See minimum required Unity version in the package.json file.
Find "Samples" in the package window and click the "Import" button. Use it as a guide.
To update the package, simply add it again while using a different version tag.

## Usage

To create a server build, you must use `AgavaNetworkManager` instead of the standard NetworkManager. This will automatically remove servers when they are empty.

To create a server or connect to a server, use the `MirrorServerApi` class. Before you start, you must call the `MirrorServerApi.Initialize(...)` method.

An example of server creation:
```cs
var joinCode = await MirrorServerApi.CreateServer();
await MirrorServerApi.Connect(joinCode);
```
After calling `MirrorServerApi.Connect(joinCode)`, the `NetworkManager.singleton.StartClient()` method will be called and you will automatically be connected to the server as a client.

