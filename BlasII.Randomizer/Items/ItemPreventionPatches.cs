using HarmonyLib;
using Il2CppPlaymaker.Inventory;

namespace BlasII.Randomizer.Items
{
    [HarmonyPatch(typeof(IsItemOwned), nameof(IsItemOwned.OnEnter))]
    class Check_ItemOwned_Patch
    {
        public static void Postfix(IsItemOwned __instance)
        {
            Main.Randomizer.LogWarning($"{__instance.Owner.name} is checking for item: {__instance.itemID.name}");
        }
    }
}
