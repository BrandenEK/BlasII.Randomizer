using BlasII.Randomizer.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace BlasII.Randomizer.Debug;

internal class Core
{
    static void Main()
    {
        Console.WriteLine("Blasphemous 2 Randomizer Debug Console");

        string dataFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "..", "resources", "data", "Randomizer");
        var itemLocations = LoadJsonDictionary<ItemLocation>(Path.Combine(dataFolder, "item-locations.json"));
        var items = LoadJsonDictionary<Item>(Path.Combine(dataFolder, "items.json"));
        var doors = LoadJsonDictionary<Door>(Path.Combine(dataFolder, "doors.json"));

        var debugger = new Debugger(itemLocations, items, doors);
        debugger.Debug();
    }

    private static Dictionary<string, T> LoadJsonDictionary<T>(string path) where T : IUnique
    {
        string json = File.ReadAllText(path);
        T[] values = JsonConvert.DeserializeObject<T[]>(json);

        return values.ToDictionary(x => x.Id, x => x);
    }
}
