using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.PrieDieu;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Skip giving a prie dieu upgrade when talking to the sisters
/// </summary>
[HarmonyPatch(typeof(UpgradePrieDieu), nameof(UpgradePrieDieu.OnEnter))]
class UpgradePrieDieu_OnEnter_Patch
{
    public static bool Prefix(UpgradePrieDieu __instance)
    {
        string upgradeName = __instance.prieDieuUpgrade.Value.name;

        if (upgradeName == "TeleportToHUBUpgrade" ||
            upgradeName == "FervourFillUpgrade" ||
            upgradeName == "TeleportToAnotherPrieuDieuUpgrade")
        {
            ModLog.Warn("Skipping upgrade for " + upgradeName);
            __instance.Finish();
            return false;
        }

        return true;
    }
}

/// <summary>
/// Change what they are checking from owning the upgrade to the flag status
/// </summary>
[HarmonyPatch(typeof(IsPrieDieuUpgraded), nameof(IsPrieDieuUpgraded.OnEnter))]
class IsPrieDieuUpgraded_OnEnter_Patch
{
    public static bool Prefix(IsPrieDieuUpgraded __instance)
    {
        string upgradeName = __instance.prieDieuUpgrade.Value.name;
        bool unlocked;

        // Instead of checking the prie dieu upgrade, check the flag
        if (upgradeName == "TeleportToHUBUpgrade")
        {
            unlocked = Main.Randomizer.GetQuestBool("ST25", "UPGRADE1_UNLOCKED");
        }
        else if (upgradeName == "FervourFillUpgrade")
        {
            unlocked = Main.Randomizer.GetQuestBool("ST25", "UPGRADE2_UNLOCKED");
        }
        else if (upgradeName == "TeleportToAnotherPrieuDieuUpgrade")
        {
            unlocked = Main.Randomizer.GetQuestBool("ST25", "UPGRADE3_UNLOCKED");
        }
        else
        {
            return true;
        }

        // If it was one of these three, do special finish
        if (unlocked)
        {
            __instance.Fsm.Event(__instance.yesEvent);
            __instance.Finish();
            return false;
        }
        else
        {
            __instance.Fsm.Event(__instance.noEvent);
            __instance.Finish();
            return false;
        }
    }
}
