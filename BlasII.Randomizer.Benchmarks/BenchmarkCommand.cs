using Basalt.CommandParser;

namespace BlasII.Randomizer.Benchmarks;

public class BenchmarkCommand : CommandData
{
    [BooleanArgument('w', "waitinput")]
    public bool WaitForInput { get; set; } = false;

    //[BooleanArgument('s', "skipwarmup")]
    //public bool SkipWarmup { get; set; } = false;

    [IntegerArgument('i', "iterations")]
    public int MaxIterations { get; set; } = 1000;

    [BooleanArgument('e', "export")]
    public bool ExportResults { get; set; } = true;
}
