using Il2CppTGK.Game;
using System.Collections.Generic;
using System.Text;

namespace BlasII.Randomizer.Items
{
    public class ItemHandler
    {
        private Dictionary<string, string> _mappedItems = new();

        private List<string> _collectedLocations = new();
        private List<string> _collectedItems = new();

        private readonly ItemShufflerReverse _shuffler = new();

        public Item GetItemAtLocation(string locationId)
        {
            if (_mappedItems.TryGetValue(locationId, out string item))
            {
                return Main.Randomizer.Data.GetItem(item);
            }
            else
            {
                Main.Randomizer.LogError(locationId + " does not have a mapped item!");
                return Main.Randomizer.Data.InvalidItem;
            }
        }

        public void GiveItemAtLocation(string locationId)
        {
            Main.Randomizer.LogWarning("Giving item at location: " +  locationId);

            Item item;
            if (_collectedLocations.Contains(locationId))
            {
                Main.Randomizer.LogError(locationId + " has already been collected!");
                item = Main.Randomizer.Data.InvalidItem;
            }
            else
            {
                _collectedLocations.Add(locationId);
                item = GetItemAtLocation(locationId);
            }

            item.RemovePreviousItem(); // Eventually change this to have different classes for prog items
            item.Upgraded?.GiveReward();
            DisplayItem(item);
        }

        public void DisplayItem(Item item)
        {
            CoreCache.UINavigationHelper.ShowItemPopup(
                Main.Randomizer.LocalizationHandler.Localize("otex"),
                item.Current?.DisplayName,
                item.Current?.Image);
        }

        /// <summary>
        /// Checks if the location should do normal execution (Weapon select room)
        /// </summary>
        public bool IsVanillaLocation(string locationId)
        {
            return locationId == "Z1506.w0"
                || locationId == "Z1506.a0"
                || locationId == "Z1064.i0";
        }

        /// <summary>
        /// Attempts a certain number of times to shuffle the items
        /// </summary>
        public bool ShuffleItems(int seed, RandomizerSettings settings)
        {
            int currentAttempt = 0, maxAttempts = 20;

            while (!_shuffler.Shuffle(seed + currentAttempt, settings, _mappedItems) && currentAttempt < maxAttempts)
            {
                Main.Randomizer.LogWarning($"Seed {seed + currentAttempt} was invalid!  Trying next...");
                currentAttempt++;
            }

            if (currentAttempt >= maxAttempts)
            {
                Main.Randomizer.LogError($"Failed to shuffle items in {maxAttempts} attempts");
                _mappedItems.Clear();
                return false;
            }

            Main.Randomizer.Log($"Shuffled {_mappedItems.Count} items!");
            GenerateSpoiler(settings);
            return true;
        }

        private void GenerateSpoiler(RandomizerSettings settings)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Version: {ModInfo.MOD_VERSION}");
            sb.AppendLine($"Date: {System.DateTime.Now.ToString("MM/dd/yyyy")}");
            sb.Append(settings.FormatSpoiler());

            string currentZoneId = string.Empty;
            foreach (var location in Main.Randomizer.Data.ItemLocationList)
            {
                // Make sure it has a valid item
                Item item = GetItemAtLocation(location.id);

                // Display new zone section if different
                string locationZoneId = location.id[..3];
                if (currentZoneId != locationZoneId && Main.Randomizer.Data.GetZoneName(locationZoneId, out string locationZoneName))
                {
                    sb.AppendLine($"\n - {locationZoneName} -\n");
                    currentZoneId = locationZoneId;
                }

                // Add location to text
                sb.AppendLine($"{location.name}: {item.name}");
            }

            // Save text to file
            string fileName = $"spoiler_{CoreCache.SaveData.CurrentSaveSlot}.txt";
            Main.Randomizer.FileHandler.WriteToFile(fileName, sb.ToString());
        }

        public void SetItemCollected(string itemId)
        {
            _collectedItems.Add(itemId);
        }

        public bool IsItemCollected(string itemId)
        {
            return _collectedItems.Contains(itemId);
        }

        public bool IsLocationCollected(string locationId)
        {
            return _collectedLocations.Contains(locationId);
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