using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game.Cutscenes;
using Il2CppTGK.Game.Tutorial;

namespace BlasII.Randomizer
{
    [HarmonyPatch(typeof(ShowTutorial), nameof(ShowTutorial.OnEnter))]
    class Tutorial_Skip_Patch
    {
        public static bool Prefix(ShowTutorial __instance)
        {
            TutorialID tutorial = __instance.tutorial.Value.Cast<TutorialID>();
            Main.Randomizer.LogWarning("Skipping tutorial: " +  tutorial?.name);

            __instance.Finish();
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.OnEnter))]
    class Cutscene_Skip_Patch
    {
        public static bool Prefix(PlayCutscene __instance)
        {
            Main.Randomizer.LogWarning("Skipping cutscene: " + __instance.cutsceneId?.name);

            __instance.Finish();
            return false;
        }
    }
}
