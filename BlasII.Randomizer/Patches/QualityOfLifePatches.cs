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
        __result = Main.Randomizer.CurrentSettings.MartyrdomExperience == 1
            ? SETTINGS_DOUBLE
            : SETTINGS_VANILLA;
    }

    private static readonly QolSettings SETTINGS_VANILLA = new()
    {
        CutsceneSkip = true,
        TutorialSkip = true,
        BossIntroSkip = true,
        ConsistentTyphoon = true,
        KeepEnvoyAltarpieces = true,
        AutoConvertMementos = false,
        DoubleOrbExperience = false,
        MaxPrieDieus = true,
    };

    private static readonly QolSettings SETTINGS_DOUBLE = new()
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
