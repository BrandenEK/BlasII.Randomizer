using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;

namespace BlasII.Randomizer.Patches;


/// <summary>
/// Skip the weapon select room
/// </summary>
[HarmonyPatch(typeof(DoorInteractable), nameof(DoorInteractable.ChangeScene))]
class DoorInteractable_ChangeScene_Patch
{
    public static void Prefix(DoorInteractable __instance)
    {
        string doorId = $"{CoreCache.Room.CurrentRoom.Name}[{__instance.entryId}]";
        ModLog.Info("Entering door: " + doorId);

        if (doorId == "Z0101[-]")
        {
            // Temp until all door transitions are handled through data
            ModLog.Info("Avoiding weapon room");
            __instance.destinationScene = "Z0102";
        }
    }
}
