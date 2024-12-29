
namespace BlasII.Randomizer.Benchmarks.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class GlobalSetupAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BenchmarkSetupAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class IterationSetupAttribute : Attribute
{

}
