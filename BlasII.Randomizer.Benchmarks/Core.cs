using BlasII.Randomizer.Benchmarks.Exporters;
using BlasII.Randomizer.Benchmarks.Metrics;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    public static Dictionary<string, ItemLocation> ItemLocations { get; private set; }
    public static Dictionary<string, Item> Items { get; private set; }
    public static Dictionary<string, Door> Doors { get; private set; }

    static void Main(string[] args)
    {
        var cmd = new BenchmarkCommand();
        cmd.Process(args);

        string dataFolder = Path.Combine(BASE_DIRECTORY, "resources", "data", "Randomizer");
        ItemLocations = LoadJsonDictionary<ItemLocation>(Path.Combine(dataFolder, "itemlocations.json"));
        Items = LoadJsonDictionary<Item>(Path.Combine(dataFolder, "items.json"));
        Doors = LoadJsonDictionary<Door>(Path.Combine(dataFolder, "doors.json"));
        Console.WriteLine($"Item locations: {ItemLocations.Count}, Items: {Items.Count}, Doors: {Doors.Count}");

        var runner = new BenchmarkRunner<BenchmarkResult>()
            .AddExporter(new ConsoleExporter())
            .AddExporter(cmd.ExportResults, () => new FileExporter(Path.Combine(BASE_DIRECTORY, "BenchmarkResults")))
            .AddMetrics(new SuccessRateMetric(), new AverageTimeMetric(), new AverageSuccessTimeMetric(), new AverageSphereMetric());

        runner.Run<NewBenchmarks>(cmd.MaxIterations);

        if (cmd.WaitForInput)
            Console.ReadKey(true);
    }

    private static Dictionary<string, T> LoadJsonDictionary<T>(string path) where T : IUnique
    {
        string json = File.ReadAllText(path);
        T[] values = JsonConvert.DeserializeObject<T[]>(json);

        return values.ToDictionary(x => x.Id, x => x);
    }

    public static string BASE_DIRECTORY { get; } = Assembly.GetExecutingAssembly().Location
        .Substring(0, Assembly.GetExecutingAssembly().Location.IndexOf("BlasII.Randomizer.Benchmarks"));
}
