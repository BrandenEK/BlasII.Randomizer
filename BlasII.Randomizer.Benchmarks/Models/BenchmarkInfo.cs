using System.Reflection;

namespace BlasII.Randomizer.Benchmarks.Models;

public class BenchmarkInfo
{
    public string Id { get; }

    public string Name { get; }

    public MethodInfo Method { get; }

    public BenchmarkInfo(string id, string name, MethodInfo method)
    {
        Id = id;
        Name = name;
        Method = method;
    }
}
