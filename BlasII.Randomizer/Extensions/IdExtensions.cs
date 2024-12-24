using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;
using Il2CppTGK.Game.ShopSystem;

namespace BlasII.Randomizer.Extensions;

internal static class IdExtensions
{
    /// <summary>
    /// Calculates the id for a <see cref="LootInteractable"/>
    /// </summary>
    public static string CalculateId(this LootInteractable interactable)
    {
        string id = $"{CoreCache.Room.CurrentRoom.Name}.l{interactable.transform.GetSiblingIndex()}";
        return id.CalculateSpecialId(interactable.loot?.size.ToString());
    }

    /// <summary>
    /// Calculates the id for a <see cref="LootInteractableST103"/>
    /// </summary>
    public static string CalculateId(this LootInteractableST103 interactable)
    {
        return $"{CoreCache.Room.CurrentRoom.Name}.l{interactable.transform.GetSiblingIndex()}";
    }

    /// <summary>
    /// Calculates the id for a <see cref="AddItem"/>
    /// </summary>
    public static string CalculateId(this AddItem action)
    {
        string id = $"{CoreCache.Room.CurrentRoom.Name}.i{action.owner.transform.GetSiblingIndex()}";
        return id.CalculateSpecialId(action.itemID?.name);
    }

    /// <summary>
    /// Calculates the id for a <see cref="GiveReward"/>
    /// </summary>
    public static string CalculateId(this GiveReward action)
    {
        return $"{CoreCache.Room.CurrentRoom.Name}.r{action.owner.transform.GetSiblingIndex()}";
    }

    /// <summary>
    /// Calculates the id for a <see cref="UnlockWeapon"/>
    /// </summary>
    public static string CalculateId(this UnlockWeapon _)
    {
        return $"{CoreCache.Room.CurrentRoom.Name}.w0";
    }

    /// <summary>
    /// Calculates the id for a <see cref="UpgradeWeaponTier"/>
    /// </summary>
    public static string CalculateId(this UpgradeWeaponTier _)
    {
        return $"{CoreCache.Room.CurrentRoom.Name}.w0";
    }

    /// <summary>
    /// Calculates the id for a <see cref="UnlockAbility"/>
    /// </summary>
    public static string CalculateId(this UnlockAbility _)
    {
        return $"{CoreCache.Room.CurrentRoom.Name}.a0";
    }

    /// <summary>
    /// Calculates the id for a <see cref="Shop"/>
    /// </summary>
    public static string CalculateId(this Shop shop, int orbIdx)
    {
        return $"{shop.name}.o{orbIdx}";
    }

    /// <summary>
    /// Calculates a special location id based on an extra parameter for locations that would have the same id normally
    /// </summary>
    private static string CalculateSpecialId(this string locationId, string extra)
    {
        return locationId switch
        {
            // Cherub basins
            "Z2401.l2" or "Z2401.l3" => extra switch
            {
                "1" => "Z2401.l0.a",
                "2" => "Z2401.l0.b",
                "3" => "Z2401.l0.c",
                _ => string.Empty
            },
            // Confessor items
            "Z05BZ01.i0" => extra switch
            {
                "QI04" => locationId + ".a",
                "QI28" => locationId + ".b",
                _ => string.Empty
            },
            // Palace elders
            "Z0722.i0" => extra switch
            {
                "QI09" => locationId + ".a",
                "QI10" => locationId + ".b",
                _ => string.Empty
            },
            // Battle arenas
            "Z1501.i0" => extra switch
            {
                "FG27" => locationId + ".a",
                "FG28" => locationId + ".b",
                "QI49" => locationId + ".c",
                "QI46" => locationId + ".d",
                "FG32" => locationId + ".e",
                _ => string.Empty
            },
            // Gold rewards
            "Z2150.i0" => extra switch
            {
                "RB103" => locationId + ".a",
                "FG112" => locationId + ".b",
                _ => string.Empty
            },
            _ => locationId
        };
    }
}
