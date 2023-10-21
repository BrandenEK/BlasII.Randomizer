using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Settings
{
    /// <summary>
    /// When pressing accept on a slot, open the settings menu instead
    /// When pressing accept on the settings menu, start the game
    /// </summary>
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

        public static void Postfix()
        {
            if (NewGameFlag)
                Main.Randomizer.NewGame();
        }

        public static bool NewGameFlag { get; set; }
    }
}
