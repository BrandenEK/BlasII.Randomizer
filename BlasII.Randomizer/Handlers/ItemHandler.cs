using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Settings;
using BlasII.Randomizer.Shuffle;
using Il2CppTGK.Game;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlasII.Randomizer.Handlers;

public class ItemHandler
{
    private Dictionary<string, string> _mappedItems = new();

    private List<string> _collectedLocations = new();
    private List<string> _collectedItems = new();

    private readonly IShuffler _shuffler;

    internal ItemHandler(IShuffler shuffler)
    {
        _shuffler = shuffler;
    }

    public Item GetItemAtLocation(string locationId)
    {
        if (_mappedItems.TryGetValue(locationId, out string item))
        {
            return Main.Randomizer.ItemStorage[item];
        }
        else
        {
            ModLog.Error(locationId + " does not have a mapped item!");
            return Main.Randomizer.ItemStorage.InvalidItem;
        }
    }

    public void GiveItemAtLocation(string locationId)
    {
        Item item;
        if (_collectedLocations.Contains(locationId))
        {
            ModLog.Error(locationId + " has already been collected!");
            item = Main.Randomizer.ItemStorage.InvalidItem;
        }
        else
        {
            _collectedLocations.Add(locationId);
            item = GetItemAtLocation(locationId);
        }

        ModLog.Custom($"Giving item at location: {locationId} ({item.Id})", System.Drawing.Color.Green);

        Main.Randomizer.MessageHandler.Broadcast("LOCATION", locationId);
        DisplayItem(item);
        item.GiveReward();
    }

    public void DisplayItem(Item item)
    {
        string message = Main.Randomizer.LocalizationHandler.Localize("popup/item");
        Main.Randomizer.ItemDisplayer.Show(message, item.GetName(), item.GetSprite());
    }

    /// <summary>
    /// Attempts a certain number of times to shuffle the items
    /// </summary>
    public bool ShuffleItems(int seed, RandomizerSettings settings)
    {
        int currentAttempt = 0, maxAttempts = 20;
        var seedGen = new System.Random(seed);

        while (!_shuffler.Shuffle(seed, settings, _mappedItems) && currentAttempt < maxAttempts)
        {
            int failedSeed = seed;
            seed = SettingsGenerator.GetRandomSeed(seedGen);

            ModLog.Warn($"Seed {failedSeed} was invalid! Trying {seed} next.");
            currentAttempt++;
        }

        if (currentAttempt >= maxAttempts)
        {
            ModLog.Error($"Failed to shuffle items in {maxAttempts} attempts!");
            _mappedItems.Clear();
            return false;
        }

        ModLog.Info($"Shuffled {_mappedItems.Count} items!");
        GenerateSpoiler(settings);
        return true;
    }

    private void GenerateSpoiler(RandomizerSettings settings)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Version: {ModInfo.MOD_VERSION}");
        sb.AppendLine($"Date: {System.DateTime.Now.ToString("MM/dd/yyyy")}");
        sb.Append(settings.FormatSpoiler());

        string currentZone = string.Empty;
        foreach (var location in Main.Randomizer.ItemLocationStorage.AsSequence)
        {
            // Make sure it has a valid item
            Item item = GetItemAtLocation(location.Id);

            // Get location text from name storage
            string zoneName = Main.Randomizer.NameStorage.GetZoneName(location);
            string roomName = Main.Randomizer.NameStorage.GetRoomName(location);

            // Display new zone section if different
            if (currentZone != zoneName)
            {
                sb.AppendLine($"\n - {zoneName} -\n");
                currentZone = zoneName;
            }

            // Add location to text
            sb.AppendLine($"{roomName} - {location.Name}: {item.Name}");
        }

        // Save text to file
        string path = Path.Combine(Main.Randomizer.FileHandler.SavegamesFolder, $"savegame_{CoreCache.SaveData.CurrentSaveSlot}_spoiler.txt");
        File.WriteAllText(path, sb.ToString());
    }

    /// <summary>
    /// Adds the item id to the list of collected items
    /// </summary>
    public void SetItemCollected(string itemId)
    {
        _collectedItems.Add(itemId);
    }

    /// <summary>
    /// Checks whether the item has been collected
    /// </summary>
    public bool IsItemCollected(string itemId)
    {
        return _collectedItems.Contains(itemId);
    }

    /// <summary>
    /// Checks how many times the item has been collected
    /// </summary>
    public int AmountItemCollected(string itemId)
    {
        return _collectedItems.Count(x => x == itemId);
    }

    /// <summary>
    /// Checks whether the location has been collected
    /// </summary>
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