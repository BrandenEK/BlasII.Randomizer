using Il2CppTGK.Game;
using System.Collections.Generic;

namespace BlasII.Randomizer.Items
{
    public class ItemHandler
    {
        private Dictionary<string, string> _mappedItems = new();

        private List<string> _collectedLocations = new();
        private List<string> _collectedItems = new();

        private readonly ItemShuffler _shuffler = new();

        public Item GetItemAtLocation(string locationId)
        {
            if (_mappedItems.TryGetValue(locationId, out string item))
            {
                return Main.Randomizer.Data.GetItem(item);
            }
            else
            {
                Main.Randomizer.LogError(locationId + " does not have a mapped item!");
                return null;
            }
        }

        public void GiveItemAtLocation(string locationId)
        {
            Main.Randomizer.LogWarning("Giving item at location: " +  locationId);

            if (_collectedLocations.Contains(locationId))
            {
                Main.Randomizer.LogError(locationId + " has already been collected!");
                return;
            }

            Item item = GetItemAtLocation(locationId);
            if (item == null)
                return;

            _collectedLocations.Add(locationId);
            item.GiveReward();
            DisplayItem(item);
        }

        public void DisplayItem(Item item)
        {
            CoreCache.UINavigationHelper.ShowItemPopup("Obtained", item.name, item.CurrentImage);
        }

        public bool IsLocationRandomized(string locationId)
        {
            return _mappedItems.ContainsKey(locationId);
        }

        public void FakeShuffle(uint seed, TempConfig config)
        {
            if (_shuffler.Shuffle(seed, config, _mappedItems))
            {
                Main.Randomizer.Log($"Shuffled {_mappedItems.Count} items!");
            }
            else
            {
                Main.Randomizer.LogError("Failed to shuffle items!");
                _mappedItems.Clear();
            }
        }

        public void SetItemCollected(string itemId)
        {
            _collectedItems.Add(itemId);
        }

        public bool IsItemCollected(string itemId)
        {
            return _collectedItems.Contains(itemId);
        }

        // Save data

        public Dictionary<string, string> MappedItems
        {
            get => _mappedItems;
            set => _mappedItems = value ?? new Dictionary<string, string>();
        }

        public List<string> CollectedLocations
        {
            get => _collectedLocations;
            set => _collectedLocations = value ?? new List<string>();
        }

        public List<string> CollectedItems
        {
            get => _collectedItems;
            set => _collectedItems = value ?? new List<string>();
        }
    }
}