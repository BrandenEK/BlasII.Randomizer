using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Persistence;

namespace BlasII.Randomizer
{
    [HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.NewGame))]
    class NewGame_Patch
    {
        public static void Postfix(int slot)
        {
            Main.Randomizer.LogError("New game: " + slot);
        }
    }

    //[HarmonyPatch(typeof(MainMenuWindowLogic), nameof(MainMenuWindowLogic.LoadGame))]
    //class LoadGame_Patch
    //{
    //    public static void Postfix()
    //    {
    //        Main.Randomizer.LogError("Loading game!");
    //    }
    //}

    //[HarmonyPatch(typeof(GuiltManager), nameof(GuiltManager.BuildCurrentPersistentState), typeof(void))]
    //class Mod_Save_Patch
    //{
    //    public static void Postfix()
    //    {
    //        Main.Randomizer.Log("save");
    //    }
    //}

    [HarmonyPatch(typeof(GuiltManager), nameof(GuiltManager.BuildCurrentPersistentState), typeof(PersistentData))]
    class Mod_Save_Patch
    {
        public static void Postfix()
        {
            Main.Randomizer.LogError("Save game: " + CoreCache.SaveData.CurrentSaveSlot);
        }
    }

    [HarmonyPatch(typeof(GuiltManager), nameof(GuiltManager.SetCurrentPersistentState))]
    class Mod_Load_Patch
    {
        public static void Postfix()
        {
            Main.Randomizer.LogError("Load game: " + CoreCache.SaveData.CurrentSaveSlot);
        }
    }

    [HarmonyPatch(typeof(GuiltManager), nameof(GuiltManager.ResetPersistence))]
    class Mod_Reset_Patch
    {
        public static void Postfix()
        {
            Main.Randomizer.LogError("Reset game");
        }
    }

    //[HarmonyPatch(typeof(SaveDataManager), nameof(SaveDataManager.SaveGame), typeof(int))]
    //class Mod_Savexx_Patch
    //{
    //    public static void Postfix(int slot)
    //    {
    //        Main.Randomizer.LogError("Save game: " + slot);
    //    }
    //}

    //[HarmonyPatch(typeof(SaveDataManager), nameof(SaveDataManager.LoadGame))]
    //class Mod_Loadxx_Patch
    //{
    //    public static void Postfix(int slot)
    //    {
    //        Main.Randomizer.LogError("Load game: " + slot);
    //    }
    //}

    //[HarmonyPatch(typeof(SaveDataManager), nameof(SaveDataManager.ResetPersistence))]
    //class Mod_Resetxx_Patch
    //{
    //    public static void Postfix()
    //    {
    //        Main.Randomizer.LogError("Reset game");
    //    }
    //}
}
