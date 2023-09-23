using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Persistence;
using BlasII.ModdingAPI.Storage;
using BlasII.Randomizer.Items;
using Il2Cpp;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using UnityEngine;

namespace BlasII.Randomizer
{
    public class Randomizer : BlasIIMod, IPersistentMod
    {
        public Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public DataStorage Data { get; } = new();

        public ItemHandler ItemHandler { get; } = new();

        // Loaded at init and never changes
        public TempConfig TempConfig { get; private set; }

        protected override void OnInitialize()
        {
            Data.Initialize();

            TempConfig = FileHandler.LoadConfig<TempConfig>();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>())
                {
                    fsm.DisplayActions();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                if (StatStorage.TryGetModifiableStat("BasePhysicalattack", out var stat))
                    StatStorage.PlayerStats.AddBonus(stat, "test", 100, 0);
            }

            // Testing
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
            }
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            if (sceneName == "Z1506")
                LoadWeaponSelectRoom();
            else if (GetQuestBool("ST00", "DREAM_RETURN"))
                LoadBossKeyRoom(sceneName);
        }

        protected override void OnSceneUnloaded(string sceneName)
        {

        }

        protected override void OnNewGameStarted()
        {
            Log($"Shuffling items with seed {TempConfig.seed}");
            ItemHandler.FakeShuffle(TempConfig.seed, TempConfig);
        }

        public SaveData SaveGame()
        {
            Log($"Saved file with {ItemHandler.CollectedLocations.Count} collected locations");
            return new RandomizerSaveData()
            {
                mappedItems = ItemHandler.MappedItems,
                collectedLocations = ItemHandler.CollectedLocations,
                collectedItems = ItemHandler.CollectedItems,
                tempConfig = TempConfig,
            };
        }

        public void LoadGame(SaveData data)
        {
            RandomizerSaveData randomizerData = data as RandomizerSaveData;
            ItemHandler.MappedItems = randomizerData.mappedItems;
            ItemHandler.CollectedLocations = randomizerData.collectedLocations;
            ItemHandler.CollectedItems = randomizerData.collectedItems;

            Log($"Loaded file with {randomizerData.collectedLocations.Count} collected locations");
        }

        public void ResetGame()
        {
            ItemHandler.MappedItems.Clear();
            ItemHandler.CollectedLocations.Clear();
            ItemHandler.CollectedItems.Clear();
        }

        private void LoadWeaponSelectRoom()
        {
            Log("Loading weapon room");

            foreach (var statue in Object.FindObjectsOfType<QuestVarTrigger>())
            {
                int weapon = -1;
                if (statue.name.EndsWith("CENSER"))
                    weapon = 0;
                else if (statue.name.EndsWith("ROSARY"))
                    weapon = 1;
                else if (statue.name.EndsWith("RAPIER"))
                    weapon = 2;
                if (weapon == -1)
                    continue;

                if (weapon != TempConfig.startingWeapon)
                {
                    int[] disabledAnimations = new int[] { -1322956020, -786038676, -394840968 };

                    Object.Destroy(statue.GetComponent<BoxCollider2D>());
                    Object.Destroy(statue.GetComponent<PlayMakerFSM>());
                    statue.transform.Find("sprite").GetComponent<Animator>().Play(disabledAnimations[weapon]);
                }
            }
        }

        private void LoadBossKeyRoom(string sceneName)
        {
            Log("Granting boss key for room: " + sceneName);
            ItemHandler.GiveItemAtLocation(sceneName + ".key");
        }

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

        public void SetQuestValue<T>(string questId, string varId, T value)
        {
            var input = CoreCache.Quest.GetInputQuestVar(questId, varId);
            CoreCache.Quest.SetQuestVarValue(input.questID, input.varID, value);
        }
    }
}
