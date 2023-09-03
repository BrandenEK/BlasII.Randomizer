
//[HarmonyPatch(typeof(InventoryActionBase), nameof(InventoryActionBase.DoAction))]
//class test3
//{
//    public static void Postfix(InventoryItem inv)
//    {
//        Main.Randomizer.LogWarning("Inventory do action: " + inv.id.name);
//    }
//}

// Patch never runs for this ??
//[HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.GiveLoot))]
//class test4
//{
//    public static void Prefix(LootInteractable __instance)
//    {
//        Main.Randomizer.LogWarning("Entering loot interactable");
//        Loot loot = __instance.loot;
//        ItemIDAssetReference item = loot?.itemIdRef;

//        Main.Randomizer.LogWarning("Using loot interactable: " + item.LoadAsset().Result.name);
//    }
//}

//public bool TryGetItem<T>(string name, out T item) where T : ItemID
//{
//    // Get item list
//    InventoryCollection collect = CoreCache.Inventory.Config?.items;
//    if (collect == null)
//    {
//        item = null;
//        return false;
//    }

//    // Loop through items of type and return the right one
//    foreach (ItemID id in collect.GetAllObjectByType<T>())
//    {
//        if (id.name == name)
//        {
//            item = id.Cast<T>();
//            return true;
//        }
//    }

//    item = null;
//    return false;
//}

//public bool TryGetValueStat(ValueStats value, out StatsComponent player, out ValueStatID stat)
//{
//    // Get player stats component
//    player = CoreCache.PlayerSpawn.PlayerInstance?.GetComponent<StatsComponent>();
//    if (player == null)
//    {
//        stat = null;
//        return false;
//    }

//    // Loop through stats for right one
//    foreach (ValueStat valueStat in player.valueStats)
//    {
//        if (valueStat.ID.name == value.ToString())
//        {
//            stat = valueStat.ID;
//            return true;
//        }
//    }

//    // Stat not found
//    stat = null;
//    return false;
//}

//public bool TryGetRangeStat(RangeStats range, out StatsComponent player, out RangeStatID stat)
//{
//    // Get player stats component
//    player = CoreCache.PlayerSpawn.PlayerInstance?.GetComponent<StatsComponent>();
//    if (player == null)
//    {
//        stat = null;
//        return false;
//    }

//    // Loop through stats for right one
//    foreach (RangeStat rangeStat in player.rangeStats)
//    {
//        if (rangeStat.ID.name == range.ToString())
//        {
//            stat = rangeStat.ID;
//            return true;
//        }
//    }

//    // Stat not found
//    stat = null;
//    return false;
//}

//public enum ValueStats
//{
//    Orbs,
//    Tears,
//    AltarPieceUpgrade,
//    RosaryBeadUnlockedSlots,
//}

//public enum RangeStats
//{
//    Fervour,
//    Flask,
//    Guilt,
//    Health,
//    Burnt,
//    Poisoned,
//    Shocked,
//    StaggeredRes,
//    KnockedDownRes,
//    TrueSkill,
//    BerserkMode,
//    OrbExperience,
//}

//private Image itemIcon;

//public void GiveItemAtLocation(string locationId, ItemID item)
//{
//    GetItemIcon().sprite = item.image;
//}

//private Image GetItemIcon()
//{
//    if (itemIcon == null)
//    {
//        itemIcon = UIModder.CreateRect("ItemIcon", UIModder.GameLogicParent)
//            .SetXRange(0, 0)
//            .SetYRange(0, 0)
//            .SetPivot(0, 0)
//            .SetPosition(30, 30)
//            .AddImage();
//    }

//    return itemIcon;
//}

//foreach (var state in statue.GetComponent<PlayMakerFSM>().GetStates())
//{
//    Main.Randomizer.Log(state.name + ": " + state.description);
//    foreach (var action in state.Actions)
//    {
//        Main.Randomizer.LogError(action.name + ": " + action.GetIl2CppType().Name);
//    }
//}