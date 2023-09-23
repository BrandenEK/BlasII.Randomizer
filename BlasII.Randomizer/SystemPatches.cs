using HarmonyLib;
using Il2CppPlaymaker.Player;
using Il2CppPlaymaker.Utils;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PlayerSpawn;
using Il2CppTGK.Game.Teleport;
using System.Reflection;
using UnityEngine;

namespace BlasII.Randomizer
{
    /// <summary>
    /// Log when a quest flag is being set
    /// </summary>
    [HarmonyPatch]
    class QuestManager_SetQuest_Patch
    {
        public static MethodInfo TargetMethod()
        {
            return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(bool));
        }

        public static void Postfix(int questId, int varId, bool value)
        {
            Main.Randomizer.LogWarning($"Setting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({value})");
        }
    }

    [HarmonyPatch(typeof(TeleportPlayerToAnotherScene), nameof(TeleportPlayerToAnotherScene.OnEnter))]
    class test
    {
        public static bool Prefix(TeleportPlayerToAnotherScene __instance)
        {
            Main.Randomizer.LogWarning("Teleport to another scene");
            Main.Randomizer.LogError(__instance.teleportData.Value.GetIl2CppType().ToString());

            //var data = __instance.teleportData.Cast<TeleportData>();
            //Main.Randomizer.Log("Exists: " + (data != null));
            //data.teleportData.sceneEntry = new Il2CppTGK.Game.PlayerSpawn.SceneEntryID()
            //{
            //    scene = "Z2304",
            //    entryId = 2
            //};

            //__instance.Finish();
            return true;
        }
    }

    [HarmonyPatch(typeof(ForceTPOAnimation), nameof(ForceTPOAnimation.OnEnter))]
    class test2
    {
        public static bool Prefix(ForceTPOAnimation __instance)
        {
            //Main.Randomizer.LogWarning("force anim");
            //__instance.Finish();
            return true;
        }
    }

    [HarmonyPatch(typeof(SetInputBlock), nameof(SetInputBlock.OnEnter))]
    class test3
    {
        public static bool Prefix(SetInputBlock __instance)
        {
            //Main.Randomizer.LogWarning("input block");
            //Main.Randomizer.Log(__instance.fsmComponent.name);

            string scene = CoreCache.Room.CurrentRoom?.Name;
            if (scene == "Z2304" && Main.Randomizer.GetQuestBool("Bosses", "BS04_DEAD"))
            {
                //Main.Randomizer.LogError("Destorying fsm");
                //Object.Destroy(__instance.fsmComponent);
                //__instance.Finish();
                //return false;
            }
            return true;
        }
    }

}
