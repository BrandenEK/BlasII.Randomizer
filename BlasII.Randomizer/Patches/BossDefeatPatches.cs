using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Threading;
using Il2CppTGK.Game.Components.Animation.AnimatorManagement;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PlayerSpawn;
using UnityEngine;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// When attempting to load a post-boss room, reload the same room instead
/// </summary>
[HarmonyPatch(typeof(PlayerSpawnManager), nameof(PlayerSpawnManager.TeleportPlayer), typeof(SceneEntryID), typeof(bool), typeof(PlaybackKey))]
class PlayerSpawnManager_TeleportPlayer_Patch1
{
    public static void Prefix(ref SceneEntryID sceneEntry)
    {
        ModLog.Info($"Teleporting1 to: {sceneEntry.scene} ({sceneEntry.entryId})");
        string currentScene = CoreCache.Room.CurrentRoom?.Name;

        if (!sceneEntry.scene.StartsWith("Z15"))
            return;

        if (!Main.Randomizer.Data.TryGetBossTeleportInfo(currentScene, out BossTeleportInfo info))
            return;

        // Trying to teleport out of a boss room to a dream room
        Main.Randomizer.SetQuestValue("ST00", "DREAM_RETURN", true);
        sceneEntry = new SceneEntryID()
        {
            scene = info.EntryScene,
            entryId = info.EntryDoor
        };
    }
}
[HarmonyPatch(typeof(PlayerSpawnManager), nameof(PlayerSpawnManager.TeleportPlayer), typeof(SceneEntryID), typeof(float), typeof(float), typeof(bool), typeof(PlaybackKey))]
class PlayerSpawnManager_TeleportPlayer_Patch2
{
    public static void Prefix(ref SceneEntryID sceneEntry)
    {
        ModLog.Info($"Teleporting2 to: {sceneEntry.scene} ({sceneEntry.entryId})");
        string currentScene = CoreCache.Room.CurrentRoom?.Name;

        if (!sceneEntry.scene.StartsWith("Z15"))
            return;

        if (!Main.Randomizer.Data.TryGetBossTeleportInfo(currentScene, out BossTeleportInfo info))
            return;

        // Trying to teleport out of a boss room to a dream room
        Main.Randomizer.SetQuestValue("ST00", "DREAM_RETURN", true);
        sceneEntry = new SceneEntryID()
        {
            scene = info.EntryScene,
            entryId = info.EntryDoor
        };
    }
}

/// <summary>
/// When reloading a boss room after the fight, force deactivate it to prevent camera lock
/// </summary>
[HarmonyPatch(typeof(RoomManager), nameof(RoomManager.ChangeRoom))]
class Room_Change_Patch
{
    public static void Prefix(int roomHash, ref bool forceDeactivate)
    {
        if (!Main.Randomizer.Data.TryGetBossTeleportInfo(roomHash, out BossTeleportInfo info))
            return;

        ModLog.Info("Force deactivating boss room: " + info.ForceDeactivate);
        forceDeactivate = info.ForceDeactivate;
    }
}

/// <summary>
/// Always fade everything to block - prevents fade being locked to white after boss defeat
/// </summary>
[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeAsync), typeof(float), typeof(Action), typeof(Color), typeof(CancellationToken))]
class FadeWindowLogic_FadeAsync_Patch1
{
    public static void Prefix(ref Color targetColor)
    {
        targetColor = new Color(0, 0, 0, targetColor.a);
    }
}
[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeAsync), typeof(float), typeof(Action), typeof(Color))]
class FadeWindowLogic_FadeAsync_Patch2
{
    public static void Prefix(ref Color targetColor)
    {
        targetColor = new Color(0, 0, 0, targetColor.a);
    }
}
