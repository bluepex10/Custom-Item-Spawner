using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers;
using UnityEngine;

// ZMIEŃ namespace na namespace swojego pluginu z custom itemami (np. TacticalSNav, TacticalNightVision).
namespace YourPluginNamespace
{
    /// <summary>
    /// Kopiuj ten plik do każdego pluginu z custom itemami. Nie wymaga referencji do pluginu CustomItemSpawner
    /// ani żadnej wspólnej biblioteki: oba pluginy rozmawiają przez wspólny słownik w AppDomain (kontrakt v1).
    ///
    /// Użycie w Enable() pluginu (po utworzeniu Managera):
    ///
    ///   SpawnerClient.Register("snav300", "S-Nav 300",
    ///       (pos, rot) => Manager.SpawnSnavInWorld(pos, rot, SnavType.Standard300),
    ///       pickup => Manager.DespawnPickup(pickup));
    ///
    /// W Disable():
    ///
    ///   SpawnerClient.Unregister("snav300");
    ///
    /// spawn   - tworzy przedmiot w podanym miejscu i zwraca Pickup (np. Twoja istniejąca metoda SpawnXInWorld).
    /// despawn - opcjonalne: sprzątanie stanu pluginu (światła, wpisy w słownikach), gdy spawner usuwa przedmiot
    ///           (edytor: remove / show). Sam Pickup spawner niszczy po wywołaniu tej akcji.
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
