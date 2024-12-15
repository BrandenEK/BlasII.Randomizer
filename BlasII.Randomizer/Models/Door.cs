
namespace BlasII.Randomizer.Models;

/// <summary>
/// Models a transition between two scenes
/// </summary>
public class Door : IUnique
{
    /// <summary>
    /// This door's identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Temporary logic string
    /// </summary>
    public string Logic { get; set; }
}
