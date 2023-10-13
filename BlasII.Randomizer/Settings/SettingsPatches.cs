using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Settings
{
    [HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.NewGame))]
    class NewGame_Settings_Patch
    {
        public static bool Prefix(int slot)
        {
            if (NewGameFlag)
                return true;

            Main.Randomizer.SettingsHandler.OpenSettingsMenu(slot);
            return false;
        }

        public static bool NewGameFlag { get; set; }
    }
}
