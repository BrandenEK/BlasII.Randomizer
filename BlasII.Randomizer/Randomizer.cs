using BlasII.ModdingAPI;
using BlasII.Randomizer.Debugging;
using BlasII.Randomizer.Items;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.StatsSystem;

namespace BlasII.Randomizer
{
    public class Randomizer : BlasIIMod
    {
        public Randomizer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        public DataStorage Data { get; } = new();
        public Debugger Debugger { get; } = new();

        public ItemHandler ItemHandler { get; } = new();

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
    }
}
