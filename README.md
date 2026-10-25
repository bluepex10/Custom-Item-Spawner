# CustomItemSpawner (CIS)

A [SCP: Secret Laboratory](https://store.steampowered.com/app/700330/SCP_Secret_Laboratory/) plugin allowing to spawn and serialize custom items.

## Installation

You will need the latest **EXILED** or **LabAPI** version installed on your server.

1. Put your `CustomItemSpawner.dll` file in the `EXILED/Plugins` or `LabAPI/Plugins` directory.
2. Restart your server to generate the configuration files.

---

## Developer Integration (Custom Items)

To integrate your own Custom Items with the spawner, include the open-source `Client.cs` interface in your Visual Studio project:

1. Download [`Client.cs`](https://github.com/TwojNick/TwojeRepo/blob/main/Client.cs) and add it to your project.
2. Link your item instance upon creation:

```csharp
using YourCustomItemNamespace;

public void OnItemSpawned(Exiled.API.Features.Items.Item exiledItem)
{
    Client spawnerClient = new Client(exiledItem);

    if (spawnerClient.ValidateForSpawner())
    {
        // Item is ready for JSON serialization and spawner profiles
    }
}
