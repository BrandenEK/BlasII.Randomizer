using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Persistence;
using BlasII.Randomizer.Items;
using BlasII.Randomizer.Services;
using BlasII.Randomizer.Shuffle;
using BlasII.Randomizer.Storages;
using Il2Cpp;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.PopupMessages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer;

/// <summary>
/// An item randomizer for the game Blasphemous 2
/// </summary>
public class Randomizer : BlasIIMod, IPersistentMod
{
    internal Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public DataStorage Data { get; } = new();

    public ItemHandler ItemHandler { get; } = new(new PoolsItemShuffler());

    public RandomizerSettings CurrentSettings { get; set; } = RandomizerSettings.DefaultSettings;

    // Storages
    internal IconStorage IconStorage { get; private set; }

    /// <summary>
    /// Whether or not randomizer effects should take place.  Used for testing item/location ids
    /// </summary>
    public bool IsRandomizerMode { get; } = true;

    protected override void OnInitialize()
    {
        InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
        {
            { "DisplaySettings", KeyCode.F8 }
        });
        LocalizationHandler.RegisterDefaultLanguage("en");

        Data.Initialize();

        // Initialize storages
        IconStorage = new IconStorage();

        //ShuffleTest(new ForwardItemShuffler(), 777, 500);
        //ShuffleTest(new ReverseItemShuffler(), 777, 500);
        //ShuffleTest(new PoolsItemShuffler(), 777, 500);
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
            DisplaySettings();
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "Z0102")
            LoadWeaponDisplayRoom();
        else if (sceneName == "Z0206")
            LoadTriggerRemovalRoom("Event Trigger");
        else if (sceneName == "Z0420")
            LoadTriggerRemovalRoom("trigger area");
        else if (sceneName == "Z1506")
            LoadWeaponSelectRoom();
        else if (sceneName == "Z2401")
            LoadCherubRoom();
        else if (sceneName == "Z2501")
            LoadChapelRoom();

        CoreCache.Shop.cachedInstancedShops.Clear();
    }

    protected override void OnSceneUnloaded(string sceneName)
    {
    }

    // Algorithm efficiency
    // Forward: 500/500 (63.1)
    // Reverse: 373/500 (27.9)
    //   Pools: 379/500 (28.0)

    // This is old data before 1.0.0 that allowed false negatives
    // Initial forward: 500/500 (61.2)
    // Initial reverse: 490/500 (25.3)
    private void ShuffleTest(IShuffler shuffler, int initialSeed, int totalTries)
    {
        var seedGen = new System.Random(initialSeed);
        var output = new Dictionary<string, string>();
        var settings = RandomizerSettings.DefaultSettings;

        int successfulTries = 0;
        double runningTime = 0;

        for (int i = 0; i < totalTries; i++)
        {
            int seed = seedGen.Next(1, RandomizerSettings.MAX_SEED + 1);
            var stopwatch = Stopwatch.StartNew();

            if (shuffler.Shuffle(seed, settings, output))
            {
                successfulTries++;
                runningTime += stopwatch.Elapsed.TotalMilliseconds;
            }
        }

        ModLog.Error($"Successful attempts: {successfulTries}/{totalTries}");
        ModLog.Error($"Average time: {System.Math.Round(runningTime / successfulTries, 1)} ms");
    }

    protected override void OnNewGame()
    {
        ModLog.Info($"Performing shuffle for seed {CurrentSettings.seed}");
        ItemHandler.ShuffleItems(CurrentSettings.seed, CurrentSettings);
        AllowPrieDieuWarp();
    }

    public SaveData SaveGame()
    {
        ModLog.Info($"Saved file with {ItemHandler.CollectedLocations.Count} collected locations");
        return new RandomizerSaveData()
        {
            mappedItems = ItemHandler.MappedItems,
            collectedLocations = ItemHandler.CollectedLocations,
            collectedItems = ItemHandler.CollectedItems,
            settings = CurrentSettings,
        };
    }

    public void LoadGame(SaveData data)
    {
        RandomizerSaveData randomizerData = data as RandomizerSaveData;
        ItemHandler.MappedItems = randomizerData.mappedItems;
        ItemHandler.CollectedLocations = randomizerData.collectedLocations;
        ItemHandler.CollectedItems = randomizerData.collectedItems;

        CurrentSettings = randomizerData.settings;
        ModLog.Info($"Loaded file with {randomizerData.collectedLocations.Count} collected locations");
    }

    public void ResetGame()
    {
        ItemHandler.MappedItems.Clear();
        ItemHandler.CollectedLocations.Clear();
        ItemHandler.CollectedItems.Clear();
    }

    private void AllowPrieDieuWarp()
    {
        foreach (var upgrade in CoreCache.PrieDieuManager.config.upgrades)
        {
            if (upgrade.name == "TeleportToAnotherPrieuDieuUpgrade" ||
                upgrade.name == "FervourFillUpgrade")
                CoreCache.PrieDieuManager.Upgrade(upgrade);
        }
    }

    private void DisplaySettings()
    {
        var message = Resources.FindObjectsOfTypeAll<PopupMessageID>().First(x => x.name == "TESTPOPUP_id");
        CoreCache.UINavigationHelper.ShowPopupMessage(message, false);
    }

    // Special rooms

    /// <summary>
    /// Gives the starting weapon.  Called from the quote patch
    /// </summary>
    public void LoadStartingRoom()
    {
        ModLog.Info("Giving starting weapon");
        var weapon = AssetStorage.Weapons[(WEAPON_IDS)CurrentSettings.RealStartingWeapon];

        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ABILITY_IDS.Jump], true);
        CoreCache.AbilitiesUnlockManager.SetAbility(AssetStorage.Abilities[ABILITY_IDS.Dash], true);

        CoreCache.EquipmentManager.Unlock(weapon);
        CoreCache.PlayerSpawn.PlayerControllerRef.GetAbility<ChangeWeaponAbility>().ChangeWeapon(weapon);
        SetQuestValue("ST00", "WEAPON_EVENT", true);
    }

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

    private void LoadCherubRoom()
    {
    }

    private void LoadChapelRoom()
    {
        foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>())
        {
            if (fsm.name == "NPC13_ST09_PILGRIM")
            {
                // Remove this npc's trigger so that he cant give you a key
                Object.Destroy(fsm.gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }

    private void LoadTriggerRemovalRoom(string trigger)
    {
        foreach (var collider in Object.FindObjectsOfType<BoxCollider2D>(true))
        {
            //LogWarning(collider.name);
            if (collider.name == trigger)
            {
                ModLog.Warn("Removing trigger: " + trigger);
                Object.Destroy(collider);
                return;
            }
        }
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
