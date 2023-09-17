using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Storage;
using BlasII.Randomizer.Items;
using Il2Cpp;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using UnityEngine;

namespace BlasII.Randomizer
{
    public class Randomizer : BlasIIMod
    {
        public Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public DataStorage Data { get; } = new();

        public ItemHandler ItemHandler { get; } = new();

        protected override void OnInitialize()
        {
            Data.Initialize();
            ItemHandler.FakeShuffle();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Keypad7))
                DisplayAllFSMsInScene();
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                if (StatStorage.TryGetModifiableStat("BasePhysicalattack", out var stat))
                    StatStorage.PlayerStats.AddBonus(stat, "test", 100, 0);
            }
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            if (sceneName == "Z1506")
                LoadWeaponSelectRoom();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {

        }

        protected override void OnNewGame(int slot)
        {
            Log("Shuffling items with seed (Not yet)");
        }

        protected override void OnSaveGame(int slot)
        {
            Log("Saving shuffled items to file (Not yet)");
        }

        protected override void OnLoadGame(int slot)
        {
            Log("Loading shuffled items from file (Not yet)");
        }

        protected override void OnResetGame()
        {
            Log("Reseting shuffled items (Not yet)");
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

                if (weapon != CHOSEN_WEAPON)
                {
                    int[] disabledAnimations = new int[] { -1322956020, -786038676, -394840968 };

                    Object.Destroy(statue.GetComponent<BoxCollider2D>());
                    Object.Destroy(statue.GetComponent<PlayMakerFSM>());
                    statue.transform.Find("sprite").GetComponent<Animator>().Play(disabledAnimations[weapon]);
                }
            }
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

        // Will soon be from config
        public const int CHOSEN_WEAPON = 1;

        public void DisplayAllFSMsInScene()
        {
            foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>())
            {
                LogWarning("FMS: " + fsm.name);
                foreach (var state in fsm.FsmStates)
                {
                    Log("State: " + state.Name);
                    foreach (var action in state.Actions)
                    {
                        LogError("Action: " + action.Name + ", " + action.GetIl2CppType().Name);
                    }
                }
            }
        }
    }
}
