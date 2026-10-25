using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers;
using UnityEngine;

// CHANGE the namespace to your custom item plugin's namespace (e.g., TacticalSNav, TacticalNightVision).
namespace YourPluginNamespace
{
    /// <summary>
    /// Copy this file into every plugin adding custom items. It requires no reference to the CustomItemSpawner plugin
    /// or any shared assembly: both plugins communicate through a shared dictionary in AppDomain (v1 contract).
    ///
    /// Usage in your plugin's Enable() method (after initializing your Manager):
    ///
    ///   SpawnerClient.Register("snav300", "S-Nav 300",
    ///       (pos, rot) => Manager.SpawnSnavInWorld(pos, rot, SnavType.Standard300),
    ///       pickup => Manager.DespawnPickup(pickup));
    ///
    /// Usage in Disable():
    ///
    ///   SpawnerClient.Unregister("snav300");
    ///
    /// spawn   - Creates the item at the given position/rotation and returns a Pickup (e.g., your existing SpawnXInWorld method).
    /// despawn - Optional: cleans up plugin state (lights, dictionary entries) when the spawner removes the item
    ///           (editor: remove / show). The spawner itself destroys the Pickup after invoking this action.
    /// </summary>
    public static class SpawnerClient
    {
        private const string Key = "CustomItemSpawner.Registry.v1";

        public static void Register(string id, string displayName, Func<Vector3, Quaternion, Pickup> spawn, Action<Pickup> despawn = null)
        {
            if (string.IsNullOrWhiteSpace(id) || spawn == null)
                return;

            Dictionary<string, object[]> raw = GetRaw();
            lock (raw)
            {
                raw[id] = new object[] { displayName ?? id, spawn, despawn };
            }
        }

        public static void Unregister(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            Dictionary<string, object[]> raw = GetRaw();
            lock (raw)
            {
                raw.Remove(id);
            }
        }

        private static Dictionary<string, object[]> GetRaw()
        {
            Dictionary<string, object[]> raw = AppDomain.CurrentDomain.GetData(Key) as Dictionary<string, object[]>;
            if (raw == null)
            {
                raw = new Dictionary<string, object[]>(StringComparer.OrdinalIgnoreCase);
                AppDomain.CurrentDomain.SetData(Key, raw);
            }

            return raw;
        }
    }
}
