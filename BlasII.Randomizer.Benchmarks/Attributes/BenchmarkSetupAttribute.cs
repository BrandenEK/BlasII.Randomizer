
namespace BlasII.Randomizer.Benchmarks.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BenchmarkSetupAttribute : Attribute
{
    internal string Target { get; }

    public BenchmarkSetupAttribute(string target)
    {
        Target = target;
    }

    public BenchmarkSetupAttribute()
    {
        Target = null;
    }
}
