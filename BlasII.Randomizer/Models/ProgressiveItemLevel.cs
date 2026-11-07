using System.Collections.Generic;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Handles the data for an individual level of a progressive quest item
/// </summary>
public class ProgressiveItemLevel(string id, params List<IFunction> functions)
{
    /// <summary>
    /// The ID of the level of the item
    /// </summary>
    public string Id { get; } = id;

    private readonly List<IFunction> _functions = functions;

    /// <summary>
    /// Performs all functions for when this item is received
    /// </summary>
    public void PerformFunctions()
    {
        foreach (var function in _functions)
            function.Execute();
    }
}
