
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models a location that contains a randomized item
/// </summary>
public class ItemLocation
{
    /// <summary>
    /// This location's identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// This location's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Flags that determine how this location will be shuffled
    /// </summary>
    public string Flags { get; set; }

    /// <summary>
    /// Temporary logic string
    /// </summary>
    public string Logic { get; set; }
}
