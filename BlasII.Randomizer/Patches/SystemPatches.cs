using HarmonyLib;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.Misc;
using Il2CppTGK.Game.DialogSystem;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;
using System.Reflection;

namespace BlasII.Randomizer.Patches
{
    /// <summary>
    /// Log when a quest flag is being set
    /// </summary>
    [HarmonyPatch]
    class QuestManager_SetQuestBool_Patch
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

    /// <summary>
    /// Logs the id whenever a dialog is started
    /// </summary>
    [HarmonyPatch(typeof(DialogManager), nameof(DialogManager.ShowDialogWithObject))]
    class Dialog_Show_Patch
    {
        public static void Prefix(Dialog dialog)
        {
            Main.Randomizer.Log("Starting dialog: " + dialog.name);
        }
    }

    /// <summary>
    /// Always allow upgrading weapons, even without lance
    /// </summary>
    [HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.HasItem))]
    class Inventory_HasItem_Patch
    {
        public static void Postfix(ItemID itemID, ref bool __result)
        {
            __result = __result || itemID.name == "QI70" && Main.Randomizer.GetQuestBool("ST00", "WEAPON_EVENT");
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
            foreach (int room in bossRooms)
            {
                if (roomHash == room)
                {
                    Main.Randomizer.Log("Force deactivating boss room");
                    forceDeactivate = true;
                }
            }
        }

        private static readonly int[] bossRooms =
        {
            129108797,
            -1436975306,
            129108570,
            // 1574233179, Causes missing fog vfx
            // Benedicta changes rooms
            -133013164,
            1433070649,
            1433070681,
        };
    }
}
