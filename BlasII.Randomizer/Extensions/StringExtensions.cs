
namespace BlasII.Randomizer.Extensions;

internal static class StringExtensions
{
    /// <summary>
    /// Calculates a special location id based on an extra parameter for locations that would have the same id normally
    /// </summary>
    public static string CalculateSpecialId(this string locationId, string extra)
    {
        return locationId switch
        {
            // Confessor items
            "Z05BZ01.i0" => extra switch
            {
                "QI04" => locationId + ".a",
                "QI28" => locationId + ".b",
                _ => string.Empty
            },
            // Palace elders
            "Z0722.i0" => extra switch
            {
                "QI09" => locationId + ".a",
                "QI10" => locationId + ".b",
                _ => string.Empty
            },
            // Battle arenas
            "Z1501.i0" => extra switch
            {
                "FG27" => locationId + ".a",
                "FG28" => locationId + ".b",
                "QI49" => locationId + ".c",
                "QI46" => locationId + ".d",
                "FG32" => locationId + ".e",
                _ => string.Empty
            },
            // Gold rewards
            "Z2150.i0" => extra switch
            {
                "RB103" => locationId + ".a",
                "FG112" => locationId + ".b",
                _ => string.Empty
            },
            _ => locationId
        };
    }
}
