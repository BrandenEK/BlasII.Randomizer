using BlasII.ModdingAPI;
using BlasII.Randomizer.Debugging;
using BlasII.Randomizer.Items;
using Il2Cpp;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.StatsSystem;
using UnityEngine;

namespace BlasII.Randomizer
{
    public class Randomizer : BlasIIMod
    {
        public Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public DataStorage Data { get; } = new();
        public Debugger Debugger { get; } = new();

        public ItemHandler ItemHandler { get; } = new();

        // This is only used to prevent excessively logging quests on main menu
        private bool _leftMainMenu = false;
        public bool HasLeftMainMenu => _leftMainMenu;

        protected override void OnInitialize()
        {
            Data.Initialize();
            ItemHandler.FakeShuffle();
        }

        protected override void OnUpdate()
        {
            Debugger.ProcessInput();
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            if (sceneName == "MainMenu")
                _leftMainMenu = false;
            else if (sceneName == "Z1506")
                LoadWeaponSelectRoom();
        }

        protected override void OnSceneUnloaded(string sceneName)
        {
            if (sceneName == "MainMenu")
                _leftMainMenu = true;
        }

        private StatsComponent _playerStats;
        public StatsComponent PlayerStats
        {
            get
            {
                if (_playerStats == null)
                    _playerStats = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<StatsComponent>();
                return _playerStats;
            }
        }

        private InventoryComponent _playerInventory;
        public InventoryComponent PlayerInventory
        {
            get
            {
                if (_playerInventory == null)
                    _playerInventory = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<InventoryComponent>();
                return _playerInventory;
            }
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

        // Will soon be from config
        public const int CHOSEN_WEAPON = 1;
    }
}
