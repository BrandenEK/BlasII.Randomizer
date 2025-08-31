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

        string nextDoor = $"{__instance.destinationScene}[{__instance.destinationEntryId}]";

        ModLog.Info($"Entering door: {doorId} --> {nextDoor}");

        // Temp until all door transitions are handled through data
        if (doorId == "Z0101[-]")
        {
            ModLog.Info("Avoiding weapon room");
            __instance.destinationScene = "Z0102";
        }
        else if (doorId == "Z1324[DOOR_W_63-60]")
        {
            ModLog.Info("Avoiding susona dream room");
            __instance.destinationScene = "Z1328";
        }

        //string newId = $"{CoreCache.Room.CurrentRoom.Name}.d{__instance.transform.GetSiblingIndex()}";
        //ModLog.Error("Entering door id: " + newId);
        //ModLog.Error($"Destination: {__instance.destinationScene}[{__instance.destinationEntryId}]");

        //foreach (var door in UnityEngine.Object.FindObjectsOfType<DoorInteractable>(true))
        //{
        //    string scene = door.gameObject.scene.name;
        //    int index = door.transform.GetSiblingIndex();

        //    if (door.name == "ELEVATOR_DESTINATION")
        //    {
        //        ModLog.Info(door.transform.parent.name);
        //        index = door.transform.parent.GetSiblingIndex();
        //    }

        //    ModLog.Warn($"Found door: {scene}.d{index}  {door.name}");
        //}
    }
}
