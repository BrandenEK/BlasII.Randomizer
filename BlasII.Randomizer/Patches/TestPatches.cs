using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppSystem;
using Il2CppTGK.Audio;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.Patches;

[HarmonyPatch(typeof(BossDeathEffectManager), nameof(BossDeathEffectManager.DeactivateColorGrad))]
class t
{
    public static void Postfix()
    {
        CoreCache.UINavigationHelper.ShowOrbsPopup(5, false);
    }
}
