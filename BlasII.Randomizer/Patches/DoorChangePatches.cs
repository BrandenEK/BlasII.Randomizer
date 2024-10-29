using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;

namespace BlasII.Randomizer.Patches;

[HarmonyPatch(typeof(DoorInteractable), nameof(DoorInteractable.ChangeScene))]
class Door_Use_Patch
{
    public static void Prefix(DoorInteractable __instance)
    {
        string doorId = $"{CoreCache.Room.CurrentRoom.Name}[{__instance.entryId}]";
        Main.Randomizer.Log("Entering door: " + doorId);
    }
}
