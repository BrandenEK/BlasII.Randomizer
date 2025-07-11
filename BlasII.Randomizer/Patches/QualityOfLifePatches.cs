using BlasII.QualityOfLife;
using HarmonyLib;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// Use a special version of the qol settings
/// </summary>
[HarmonyPatch(typeof(QualityOfLife.QualityOfLife), nameof(QualityOfLife.QualityOfLife.CurrentSettings), MethodType.Getter)]
class QualityOfLife_CurrentSettings_Patch
{
    public static void Postfix(ref QolSettings __result)
    {
        __result = RANDO_SETTINGS;
    }

    private static readonly QolSettings RANDO_SETTINGS = new QolSettings()
    {
        CutsceneSkip = true,
        TutorialSkip = true,
        BossIntroSkip = true,
        ConsistentTyphoon = true,
        KeepEnvoyAltarpieces = true,
        AutoConvertMementos = false,
        DoubleOrbExperience = true,
        MaxPrieDieus = true,
    };
}
