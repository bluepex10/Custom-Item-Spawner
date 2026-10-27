<div align="center">
  <img src="https://cdn.discordapp.com/attachments/1308883033130008738/1563987807960174733/CIS_2.png?ex=6adfa0a6&is=6ade4f26&hm=00de8ddc0319f5b4fa7132fc51629761cddfd6e4003d53fcb0d76ee52d28f784&" alt="CIS Banner" width="100%" />

  <h1>CustomItemSpawner (CIS)</h1>
</div>

A [SCP: Secret Laboratory](https://store.steampowered.com/app/700330/SCP_Secret_Laboratory/) plugin allowing to spawn and serialize custom items `& Default Items`.

## Installation

You will need the latest **LabAPI** version installed on your server.

1. Put your `CustomItemSpawner.dll` file in the `LabAPI/Plugins` directory.
2. Restart your server to generate the configuration files.

[Full CIS User Manual](https://drive.google.com/file/d/1vPWedmnaTaDFKUD-q1-Urqn8hpCTui4b/view?usp=sharing)

<p align="center">
  <img src="https://github.com/user-attachments/assets/ccc9f6a8-b4af-4a9e-a937-bd620b0de011" alt="CustomItemSpawner Demo GIF" width="100%" />
</p>

---

## Developer Integration (Custom Items)

To integrate your own Custom Items with **CustomItemSpawner** without adding a hard dependency to your project, include the open-source `SpawnerClient.cs` helper in your custom item plugin:

1. Copy the `SpawnerClient.cs` file into your Visual Studio project.
2. Change the namespace in `SpawnerClient.cs` to match your plugin's namespace.
3. Register and unregister your custom item upon plugin startup and shutdown:

### Example Usage:

```csharp
using System;
using LabApi.Features.Wrappers;
using UnityEngine;
using YourPluginNamespace; // Where SpawnerClient is located

public class MyCustomItemPlugin
{
    public void Enable()
    {
        // Register your custom item with CustomItemSpawner
        SpawnerClient.Register(
            id: "my_custom_item_id",
            displayName: "My Custom Item Name",
            
            // Spawn delegate: returns the spawned Pickup object
            spawn: (position, rotation) =>
            {
                Pickup customPickup = MyItemManager.SpawnItem("my_custom_item_id", position, rotation);
                return customPickup;
            },
            
            // Optional despawn delegate: handles plugin-specific cleanup when the spawner removes the item
            despawn: (pickup) =>
            {
                MyItemManager.CleanupItemData(pickup);
            }
        );
    }

    public void Disable()
    {
        // Unregister the item when your plugin shuts down
        SpawnerClient.Unregister("my_custom_item_id");
    }
}
