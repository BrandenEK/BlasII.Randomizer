using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Debug;

internal class Debugger
{
    private readonly Dictionary<string, ItemLocation> _itemLocations;
    private readonly Dictionary<string, Item> _items;
    private readonly Dictionary<string, Door> _doors;

    public Debugger(Dictionary<string, ItemLocation> itemLocations, Dictionary<string, Item> items, Dictionary<string, Door> doors)
    {
        _itemLocations = itemLocations;
        _items = items;
        _doors = doors;
    }

    public void Debug()
    {
        Console.WriteLine($"Item locations: {_itemLocations.Count}");
        Console.WriteLine($"Items: {_items.Count}");
        Console.WriteLine($"Doors: {_doors.Count}");
    }
}
