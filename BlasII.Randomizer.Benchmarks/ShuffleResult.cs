
namespace BlasII.Randomizer.Benchmarks;

public class ShuffleResult
{
    public string Name { get; set; }

    public int Seed { get; set; }

    public TimeSpan Time { get; set; }

    public bool Successful { get; set; }
}
