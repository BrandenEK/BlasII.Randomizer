using BlasII.ModdingAPI.Assets;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shuffle;

namespace BlasII.Randomizer.Benchmarks.Metrics;

public class AverageSphereMetric : IMetric<BenchmarkResult>
{
    public string DisplayName { get; } = "Avg. Sphere Count";

    private int successfulSpheres;
    private int successfulAttempts;

    public void HandleResult(BenchmarkResult result, TimeSpan time)
    {
        if (!result.WasSuccessful)
            return;

        successfulSpheres += CalculateSphereCount(result.Mapping, result.Settings);
        successfulAttempts++;
    }

    public string FormatMetric()
    {
        if (successfulAttempts == 0)
            throw new DivideByZeroException("Attempts was zero");

        double value = (double)successfulSpheres / successfulAttempts;
        return $"{value:F1}";
    }

    public void Reset()
    {
        successfulSpheres = 0;
        successfulAttempts = 0;
    }

    private int CalculateSphereCount(Dictionary<string, string> mapping, RandomizerSettings settings)
    {
        int sphere = 0;
        var remainingLocations = new List<ItemLocation>(Core.ItemLocations.Values);
        var reachedLocations = new List<ItemLocation>();
        var inventory = BlasphemousInventory.CreateNewInventory(settings);
        inventory.Add(((WEAPON_IDS)settings.RealStartingWeapon).ToString());

        while (remainingLocations.Count > 0)
        {
            Console.WriteLine("Running sphjere: " + sphere);

            foreach (var location in remainingLocations)
            {
                if (!inventory.Evaluate(location.Logic))
                    continue;

                // This location is now reachable
                reachedLocations.Add(location);
            }

            foreach (var location in reachedLocations)
            {
                remainingLocations.Remove(location);

                Item item = Core.Items[mapping[location.Id]];
                if (item.Progression)
                    inventory.Add(item.Id);
            }

            reachedLocations.Clear();
            sphere++;

            if (sphere >= Core.ItemLocations.Count)
                throw new Exception($"Not all locations are reachable in sphere calculation");
        }

        return sphere;
    }
}
