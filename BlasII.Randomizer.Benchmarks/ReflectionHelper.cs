using BlasII.Randomizer.Benchmarks.Attributes;
using System.Reflection;

namespace BlasII.Randomizer.Benchmarks;

internal static class ReflectionHelper
{
    public static IEnumerable<object> GetParameters<T>(object obj, string name)
    {
        PropertyInfo property = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        return property == null
            ? throw new Exception($"Failed to find property with the name {name}")
            : (IEnumerable<object>)property.GetValue(obj);
    }

    public static IEnumerable<MethodInfo> GetAllSetups<TBenchmark>(string target) where TBenchmark : class
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => HasValidSetupAttribute(x, target));
    }

    private static bool HasValidSetupAttribute(MethodInfo method, string target)
    {
        BenchmarkSetupAttribute attribute = method.GetCustomAttribute<BenchmarkSetupAttribute>();
        return attribute != null && (string.IsNullOrEmpty(attribute.Target) || attribute.Target == target);
    }
}
