using HarmonyLib;
using Il2CppTGK.Game.Managers;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Hash the zone id when saving, and unhash it when loading
/// This messes up vanilla saves in rando, and rando saves in vanilla
/// </summary>
[HarmonyPatch(typeof(MapManager.MapPersistenceData_v3))]
class MapPersistenceData_v3_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapManager.MapPersistenceData_v3.WriteDataToStream))]
    public static void WriteDataToStream_Patch(MapManager.MapPersistenceData_v3 __instance)
    {
        // Encrypt zone
        __instance.storedZoneId ^= SAVE_MASK;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapManager.MapPersistenceData_v3.ReadDataFromStream))]
    public static void ReadDataFromStream_Patch(MapManager.MapPersistenceData_v3 __instance)
    {
        // Decrypt zone
        __instance.storedZoneId ^= SAVE_MASK;
    }

    private const int SAVE_MASK = 0x72341904;
}
