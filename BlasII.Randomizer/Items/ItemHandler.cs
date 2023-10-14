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
                return Main.Randomizer.Data.InvalidItem;
            }
        }

        public void GiveItemAtLocation(string locationId)
        {
            Main.Randomizer.LogWarning("Giving item at location: " +  locationId);

            if (IsRemovedLocation(locationId))
                return;

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
            CoreCache.UINavigationHelper.ShowItemPopup("Obtained", item.Current?.name, item.Current?.Image);
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
        /// Checks if the location should completely skip execution (Normal boss keys)
        /// </summary>
        public bool IsRemovedLocation(string locationId)
        {
            return locationId == "Z1113.i8"
                || locationId == "Z1216.i9"
                || locationId == "Z1327.i7"
                || locationId == "Z1622.i6";
        }

        /// <summary>
        /// Checks if the location has a random item there (Random boss keys)
        /// </summary>
        public bool IsRandomLocation(string locationId)
        {
            return _mappedItems.ContainsKey(locationId);
        }

        public void FakeShuffle(int seed, RandomizerSettings settings)
        {
            if (_shuffler.Shuffle(seed, settings, _mappedItems))
            {
                Main.Randomizer.Log($"Shuffled {_mappedItems.Count} items!");
                GenerateSpoiler(seed);
            }
            else
            {
                Main.Randomizer.LogError("Failed to shuffle items!");
                _mappedItems.Clear();
            }
        }

        private void GenerateSpoiler(int seed)
        {
            StringBuilder header = new(), footer = new();
            header.AppendLine($"Version: {ModInfo.MOD_VERSION}");
            header.AppendLine($"Date: {System.DateTime.Now.ToString("MM/dd/yyyy")}");
            header.AppendLine($"Seed: {seed}\n");
            header.AppendLine("- Boss Keys -\n");

            string currentZoneId = string.Empty;
            foreach (var location in Main.Randomizer.Data.GetAllItemLocations())
            {
                // Add boss key section to header
                if (location.type == ItemLocation.ItemLocationType.BossKey)
                {
                    if (_mappedItems.ContainsKey(location.id))
                        header.AppendLine(location.name);
                    continue;
                }

                // Make sure it has a valid item
                Item item = GetItemAtLocation(location.id);

                // Display new zone section if different
                string locationZoneId = location.id[..3];
                if (currentZoneId != locationZoneId && Main.Randomizer.Data.GetZoneName(locationZoneId, out string locationZoneName))
                {
                    footer.AppendLine($"\n - {locationZoneName} -\n");
                    currentZoneId = locationZoneId;
                }

                // Add location to footer
                footer.AppendLine($"{location.name}: {item.name}");
            }

            // Save text to file
            string fileName = $"spoiler_{CoreCache.SaveData.CurrentSaveSlot}.txt";
            Main.Randomizer.FileHandler.WriteToFile(fileName, header.ToString() + footer.ToString());
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