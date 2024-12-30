using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores Info objects for the Randomizer
/// </summary>
public abstract class BaseInfoStorage<TValue> where TValue : class, IUnique
{
    /// <summary>
    /// The internal info storage
    /// </summary>
    protected readonly Dictionary<string, TValue> _values = [];

    /// <summary>
    /// Initializes the storage by loading a json list from the data folder
    /// </summary>
    public BaseInfoStorage(string fileName, string type)
    {
        if (!Main.Randomizer.FileHandler.LoadDataAsJson(fileName, out TValue[] values))
            return;

        foreach (TValue value in values)
            _values.Add(value.Id, value);
        
        ModLog.Info($"Loaded {_values.Count} {type}!");
    }

    /// <summary>
    /// Returns the infos as an IEnumerable
    /// </summary>
    public IEnumerable<TValue> AsSequence => _values.Values;

    /// <summary>
    /// Returns the infos as a Dictionary
    /// </summary>
    public Dictionary<string, TValue> AsDictionary => _values;

    /// <summary>
    /// Returns the specified info
    /// </summary>
    public TValue this[string id]
    {
        get
        {
            if (!_values.TryGetValue(id, out TValue value))
                throw new System.Exception($"{GetType().Name}: The id {id} does not exist");

            return value;
        }
    }

    /// <summary>
    /// Tries to return the specified info
    /// </summary>
    public bool TryGetValue(string id, out TValue value)
    {
        return _values.TryGetValue(id, out value);
    }
}
