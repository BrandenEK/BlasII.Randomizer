﻿using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Persistence;
using BlasII.Randomizer.Handlers;
using BlasII.Randomizer.Services;
using BlasII.Randomizer.Shuffle;
using BlasII.Randomizer.Storages;
using Il2Cpp;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.PopupMessages;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer;

/// <summary>
/// An item randomizer for the game Blasphemous 2
/// </summary>
public class Randomizer : BlasIIMod, ISlotPersistentMod<RandomizerSlotData>, IGlobalPersistentMod<RandomizerGlobalData>
{
    internal Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    /// <summary>
    /// The selected settings for the current run
    /// </summary>
    public RandomizerSettings CurrentSettings { get; set; } = RandomizerSettings.DEFAULT;

    // Handlers

    /// <inheritdoc/>
    public ItemHandler ItemHandler { get; private set; }

    // Storages

    /// <inheritdoc/>
    public ItemStorage ItemStorage { get; private set; }
    /// <inheritdoc/>
    public ItemLocationStorage ItemLocationStorage { get; private set; }
    /// <inheritdoc/>
    public DoorStorage DoorStorage { get; private set; }
    /// <inheritdoc/>
    public EmbeddedIconStorage EmbeddedIconStorage { get; private set; }
    /// <inheritdoc/>
    public CustomIconStorage CustomIconStorage { get; private set; }
    /// <inheritdoc/>
    public NameStorage NameStorage { get; private set; }
    /// <inheritdoc/>
    public ExtraInfoStorage ExtraInfoStorage { get; private set; }

    // Properties

    /// <summary>
    /// The total number of seeds generated since v2.2.1
    /// </summary>
    public int TotalSeedsGenerated { get; private set; } = 0;

    /// <summary>
    /// Whether or not randomizer effects should take place.  Used for testing item/location ids
    /// </summary>
    public bool IsRandomizerMode { get; } = true;

    /// <summary>
    /// Whether a new game has just started and the player hasn't received their starting equipment yet
    /// </summary>
    public bool IsNewGame { get; private set; } = false;

    protected override void OnInitialize()
    {
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "DisplaySettings", KeyCode.F8 }
        });
        LocalizationHandler.RegisterDefaultLanguage("en");

        // Initialize storages
        ItemStorage = new ItemStorage();
        ItemLocationStorage = new ItemLocationStorage();
        DoorStorage = new DoorStorage();
        EmbeddedIconStorage = new EmbeddedIconStorage();
        CustomIconStorage = new CustomIconStorage();
        NameStorage = new NameStorage();
        ExtraInfoStorage = new ExtraInfoStorage();

        // Initialize handlers
        ItemHandler = new ItemHandler(true
            ? new PoolsItemShuffler(ItemLocationStorage.AsDictionary, ItemStorage.AsDictionary)
            : new DebugShuffler(ItemLocationStorage.AsDictionary, "Censer"));
    }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterNewGameMenu(new RandomizerMenu());
    }

    protected override void OnUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        if (InputHandler.GetKeyDown("DisplaySettings"))
        {
            DisplaySettings();
        }
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "Z0102")
            LoadWeaponDisplayRoom();
        else if (sceneName == "Z0206")
            LoadTriggerRemovalRoom("Event Trigger", "NPC10_ST22_ANUNCIADA");
        else if (sceneName == "Z0402")
            LoadTemporaryClothRoom();
        else if (sceneName == "Z0419")
            LoadYermaRoom();
        else if (sceneName == "Z0420")
            LoadTriggerRemovalRoom("trigger area", "Arena triggered template and gates");
        else if (sceneName == "Z1323")
            LoadYermaRoom();
        else if (sceneName == "Z1506")
            LoadWeaponSelectRoom();
        else if (sceneName == "Z1612")
            LoadYermaRoom();
        else if (sceneName == "Z2301")
            LoadYermaRoom();
        else if (sceneName == "Z2401")
            LoadCherubRoom();
        else if (sceneName == "Z2501")
            LoadChapelRoom();
        else if (sceneName == "Z2716")
            LoadTriggerRemovalRoom("trigger", "SPGEO_INTERACTABLE_GUILLOTINE");

        CoreCache.Shop.cachedInstancedShops.Clear();
    }

    protected override void OnNewGame()
    {
        ModLog.Info($"Performing shuffle for seed {CurrentSettings.Seed}");
        ItemHandler.ShuffleItems(CurrentSettings.Seed, CurrentSettings);

        AllowPrieDieuWarp();
        SetQuestValue("ST00", "WEAPON_EVENT", true);
        SetQuestValue("ST00", "INTRO", true);
        IsNewGame = true;
        TotalSeedsGenerated++;
    }

    public RandomizerSlotData SaveSlot()
    {
        ModLog.Info($"Saved file with {ItemHandler.CollectedLocations.Count} collected locations");
        return new RandomizerSlotData()
        {
            mappedItems = ItemHandler.MappedItems,
            collectedLocations = ItemHandler.CollectedLocations,
            collectedItems = ItemHandler.CollectedItems,
            settings = CurrentSettings,
        };
    }

    public void LoadSlot(RandomizerSlotData data)
    {
        ItemHandler.MappedItems = data.mappedItems;
        ItemHandler.CollectedLocations = data.collectedLocations;
        ItemHandler.CollectedItems = data.collectedItems;

        CurrentSettings = data.settings;
        ModLog.Info($"Loaded file with {data.collectedLocations.Count} collected locations");
    }

    public void ResetSlot()
    {
        ItemHandler.MappedItems.Clear();
        ItemHandler.CollectedLocations.Clear();
        ItemHandler.CollectedItems.Clear();
    }

    public RandomizerGlobalData SaveGlobal()
    {
        return new RandomizerGlobalData()
        {
            SeedsGenerated = TotalSeedsGenerated
        };
    }

    public void LoadGlobal(RandomizerGlobalData data)
    {
        TotalSeedsGenerated = data.SeedsGenerated;
    }

    private void AllowPrieDieuWarp()
    {
        foreach (var upgrade in CoreCache.PrieDieuManager.config.upgrades)
        {
            if (upgrade.name == "TeleportToHUBUpgrade")
                continue;

            CoreCache.PrieDieuManager.Upgrade(upgrade);
        }
    }

    private void DisplaySettings()
    {
        var message = Resources.FindObjectsOfTypeAll<PopupMessageID>().First(x => x.name == "TESTPOPUP_id");
        CoreCache.UINavigationHelper.ShowPopupMessage(message, false);
    }

    /// <summary>
    /// Gives the starting equipment, only called if it hasnt already given it
    /// </summary>
    public void GiveStartingEquipment()
    {
        ModLog.Info("Giving starting equipment");
        IsNewGame = false;

        // Unlock starting weapon
        var weapon = AssetStorage.Weapons[(WEAPON_IDS)CurrentSettings.RealStartingWeapon];
        CoreCache.EquipmentManager.Unlock(weapon);
        CoreCache.PlayerSpawn.PlayerControllerRef.GetAbility<ChangeWeaponAbility>().ChangeWeapon(weapon);

        // Give lance
        AssetStorage.PlayerInventory.AddItemAsync(AssetStorage.QuestItems["QI70"]);

        // TEMPORARY: lock certain abilities because I have no idea how they persist
        var abilities = new ABILITY_IDS[] { ABILITY_IDS.AirDash, ABILITY_IDS.AirJump, ABILITY_IDS.GlassWalk, ABILITY_IDS.GoldFlask, ABILITY_IDS.MagicRingClimb, ABILITY_IDS.WallClimb };
        foreach (var ability in abilities.Select(x => AssetStorage.Abilities[x]))
            CoreCache.AbilitiesUnlockManager.SetAbility(ability, false);
    }

    // Special rooms

    /// <summary>
    /// Hide the weapon statues in the display room
    /// </summary>
    private void LoadWeaponDisplayRoom()
    {
        string[] statueNames = ["CENSER", "ROSARY", "RAPIER"];
        foreach (var obj in Object.FindObjectsOfType<PlayMakerFSM>().Where(x => statueNames.Any(x.name.EndsWith)))
        {
            ModLog.Info($"Hiding statue: {obj.name}");
            obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Unused
    /// </summary>
    private void LoadWeaponSelectRoom()
    {
        ModLog.Info("Loading weapon room");

        foreach (var statue in Object.FindObjectsOfType<QuestVarTrigger>())
        {
            int weapon;
            if (statue.name.EndsWith("CENSER"))
                weapon = 0;
            else if (statue.name.EndsWith("ROSARY"))
                weapon = 1;
            else if (statue.name.EndsWith("RAPIER"))
                weapon = 2;
            else
                continue;

            if (weapon != CurrentSettings.RealStartingWeapon)
            {
                int[] disabledAnimations = new int[] { -1322956020, -786038676, -394840968 };

                Object.Destroy(statue.GetComponent<BoxCollider2D>());
                Object.Destroy(statue.GetComponent<PlayMakerFSM>());
                statue.transform.Find("sprite").GetComponent<Animator>().Play(disabledAnimations[weapon]);
            }
        }
    }

    /// <summary>
    /// Temporary fix to the cloth bug in SE
    /// </summary>
    private void LoadTemporaryClothRoom()
    {
        if (!Main.Randomizer.ItemHandler.IsLocationCollected("Z0402.l13"))
            return;

        foreach (var loot in Object.FindObjectsOfType<LootInteractable>())
        {
            int index = loot.transform.GetSiblingIndex();
            if (index == 13)
            {
                loot.gameObject.SetActive(false);
                ModLog.Info("[TEMP] Hiding duplicate cloth item");
                return;
            }
        }
    }

    private void LoadCherubRoom()
    {
    }

    private void LoadChapelRoom()
    {
        foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>().Where(x => x.name == "NPC13_ST09_PILGRIM"))
        {
            // Remove this npc's trigger so that he cant give you a key
            ModLog.Info("Removing pilgrim");
            Object.Destroy(fsm.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    private void LoadYermaRoom()
    {
        foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>().Where(x => x.name == "NPC09_ST23_VARALES"))
        {
            // Disable yerma so her quest cant progress
            ModLog.Info("Removing yerma");
            fsm.gameObject.SetActive(false);
        }
    }

    private void LoadTriggerRemovalRoom(string trigger, string parent)
    {
        var colliders = Object.FindObjectsOfType<BoxCollider2D>(true)
            .Where(x => x.gameObject.scene.name.StartsWith(CoreCache.Room.CurrentRoom.Name))
            .OrderBy(x => x.name);

        //foreach (var c in colliders)
        //    ModLog.Info($"{c.name} (Child of {c.transform.parent?.name ?? "none"})");

        var collider = colliders.FirstOrDefault(x => x.name == trigger && x.transform.parent?.name == parent);

        if (collider == null)
        {
            ModLog.Warn($"Failed to find trigger: {parent}/{trigger}");
            return;
        }

        ModLog.Info($"Removing trigger: {parent}/{trigger}");
        Object.Destroy(collider);
    }

    // Quests

    public string GetQuestName(int questId, int varId)
    {
        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);
        var variable = quest.GetVariable(varId);

        return $"{quest.Name}.{variable.id}";
    }

    public bool GetQuestBool(string questId, string varId)
    {
        var input = CoreCache.Quest.GetInputQuestVar(questId, varId);
        return CoreCache.Quest.GetQuestVarBoolValue(input.questID, input.varID);
    }

    public int GetQuestInt(string questId, string varId)
    {
        var input = CoreCache.Quest.GetInputQuestVar(questId, varId);
        return CoreCache.Quest.GetQuestVarIntValue(input.questID, input.varID);
    }

    public void SetQuestValue<T>(string questId, string varId, T value)
    {
        var input = CoreCache.Quest.GetInputQuestVar(questId, varId);
        CoreCache.Quest.SetQuestVarValue(input.questID, input.varID, value);
    }
}
