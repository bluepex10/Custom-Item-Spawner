https://media.discordapp.net/attachments/1308883033130008738/1563971576548958239/E2838DD3-F75E-40F8-881C-CBDB58E726AC.png?ex=6adf9189&is=6ade4009&hm=4dc2e087c52393fb2995c887332772afa7756905bf90983bdf826d72f99a420d&=&format=webp&quality=lossless

# CustomItemSpawner (CIS)

A [SCP: Secret Laboratory](https://store.steampowered.com/app/700330/SCP_Secret_Laboratory/) plugin allowing to spawn and serialize custom items `& Default Items`.

## Installation

You will need the latest **LabAPI** version installed on your server.

1. Put your `CustomItemSpawner.dll` file in the `LabAPI/Plugins` directory.
2. Restart your server to generate the configuration files.

[Full CIS User Manual](https://docs.google.com/document/d/1PYJnn3K8GMBtaKWdQbcsYq1RRu7EK-9xLU8VXjNNWlg/edit?usp=sharing)

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
