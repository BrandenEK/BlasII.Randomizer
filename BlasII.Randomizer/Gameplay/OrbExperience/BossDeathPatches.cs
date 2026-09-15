using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Helpers;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using System.Linq;

namespace BlasII.Randomizer.Gameplay.OrbExperience;

/// <summary>
/// Gives some marks when certain bosses are defeated, based on your settings
/// </summary>
[HarmonyPatch(typeof(BossDeathEffectManager), nameof(BossDeathEffectManager.DeactivateColorGrad))]
class BossDeathEffectManager_DeactivateColorGrad_Patch
{
    public static void Postfix()
    {
        if (Main.Randomizer.CurrentSettings.MartyrdomExperience != 3)
            return;

        if (!BOSS_ROOMS.Contains(SceneHelper.CurrentScene))
            return;

        ModLog.Info("Giving boss defeat marks");
        AssetStorage.PlayerStats.AddRewardOrbs(AMOUNT, false, 0);
        CoreCache.UINavigationHelper.ShowOrbsPopup(AMOUNT, false);
    }

    private static readonly string[] BOSS_ROOMS =
    [
        //"Z0106", // Faceless one
        "Z0421", // Radames
        "Z0730", // Orospina
        "Z0921", // Lesmes & Infanta
        "Z2304", // Afi
        "Z1113", // Benedicta
        "Z1216", // Odon
        "Z1622", // Sidon
        "Z1327", // Susona
        //"Z1808", // Eviterno
        //"Z1809", // Devotion Incarnate
        //"Z2735", // Sol
        //"Z2827", // Asterion
    ];

    private const int AMOUNT = 5;
}
