using BlasII.Randomizer.Benchmarks.Attributes;
using BlasII.Randomizer.Benchmarks.Metrics;
using BlasII.Randomizer.Benchmarks.Models;
using BlasII.Randomizer.Benchmarks.Monitors;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BlasII.Randomizer.Benchmarks;

internal class Core
{
    private readonly static List<BaseMonitor> _monitors = new();
    private static IMetric<BenchmarkResult>[] _metrics = Array.Empty<IMetric<BenchmarkResult>>();

    static void Main(string[] args)
    {
        var cmd = new BenchmarkCommand();
        cmd.Process(args);
        RegisterMonitors(new SuccessRateMonitor(), new AverageTimeMonitor(), new AverageSuccessTimeMonitor());
        RegisterMetrics(new AverageTimeMetric());

        IEnumerable<string> headerInfo = GetHeaderInfo(cmd.MaxIterations);

        object obj = new NewBenchmarks();
        var benchmarks = FindAllBenchmarks<NewBenchmarks>(obj);

        if (!cmd.SkipWarmup)
            RunAllWarmups(obj, benchmarks, cmd.MaxIterations / 5);
        
        List<List<string>> output = RunAllBenchmarks(obj, benchmarks, cmd.MaxIterations);

        var header = new List<string>();
        header.Add("Method");
        header.Add("Parameters");
        foreach (var metric in _metrics)
            header.Add(metric.DisplayName);
        output.Insert(0, header);

        // Need to pad each section and add the pipes
        // Then add the dashed line and then any empty ones

        // Use linq to get a list of MaxColumnLength, then go through each and pad by that amount and add the pipe
        //var maxLengths = output.Select(row => row.Max(str => str.Length));
        //Console.WriteLine(string.Join(" | ", maxLengths));

        foreach (var line in output)
        {
            Console.WriteLine($"| {string.Join(" | ", line)} |");
        }

        DisplayOutput1(benchmarks, headerInfo, cmd.ExportResults);

        if (cmd.WaitForInput)
            Console.ReadKey(true);
    }

    public static void RegisterMonitors(params BaseMonitor[] monitors)
    {
        _monitors.AddRange(monitors);
    }

    public static void RegisterMetrics(params IMetric<BenchmarkResult>[] metrics)
    {
        _metrics = metrics;
    }

    static List<BenchmarkInfo> FindAllBenchmarks<T>(object obj)
    {
        var benchmarks = new List<BenchmarkInfo>();
        var methods = typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            BenchmarkAttribute bAttribute = method.GetCustomAttribute<BenchmarkAttribute>();

            if (bAttribute == null)
                continue;

            BenchmarkParametersAttribute bpAttribute = method.GetCustomAttribute<BenchmarkParametersAttribute>();

            if (bpAttribute == null)
            {
                benchmarks.Add(new BenchmarkInfo(method.Name, bAttribute.Name ?? method.Name, method, null));
                continue;
            }

            IEnumerable<object> parameters = bpAttribute.ParameterProperty != null
                ? GetParameters<T>(obj, bpAttribute.ParameterProperty)
                : bpAttribute.Parameters ?? throw new Exception("You have to add the parameter property or the list");

            benchmarks.AddRange(parameters
                .Select(x => new BenchmarkInfo($"{method.Name}.{x}", bAttribute.Name ?? method.Name, method, new object[] { x })));
        }

        return benchmarks;
    }

    static IEnumerable<string> GetHeaderInfo(int iterationCount)
    {
        var text = new List<string>()
        {
            $" Machine: {Environment.MachineName} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")} ({Environment.ProcessorCount} processors)",
            $" Operating system: {Environment.OSVersion}",
            $" Start time: {DateTime.Now:MM/dd/yy H:mm:ss}",
            $" Max iterations: {iterationCount}",
            $" Debug mode: {Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled)}",
        };

        var line = new string('=', text.Max(x => x.Length) + 1);

        text.Insert(0, string.Empty);
        text.Insert(1, line);
        text.Add(line);
        text.Add(string.Empty);

        return text;
    }

    static void RunAllWarmups(object obj, List<BenchmarkInfo> benchmarks, int iterationCount)
    {
        Console.WriteLine($"Running {benchmarks.Count} warmups");
        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in GetAllSetups<NewBenchmarks>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            RunWarmup(obj, benchmark, iterationCount);
        }
    }

    static void RunWarmup(object obj, BenchmarkInfo benchmark, int iterationCount)
    {
        Console.WriteLine($"Running warmup {benchmark.Id}");

        for (int i = 0; i < iterationCount; i++)
        {
            benchmark.Method.Invoke(obj, benchmark.Parameters);
        }
    }

    static List<List<string>> RunAllBenchmarks(object obj, List<BenchmarkInfo> benchmarks, int iterationCount)
    {
        Console.WriteLine($"Running {benchmarks.Count} benchmarks");

        var output = new List<List<string>>();

        foreach (var benchmark in benchmarks)
        {
            foreach (var setup in GetAllSetups<NewBenchmarks>(benchmark.Method.Name))
                setup.Invoke(obj, null);

            output.Add(RunBenchmark(obj, benchmark, iterationCount));
        }

        return output;
    }

    static List<string> RunBenchmark(object obj, BenchmarkInfo benchmark, int iterationCount)
    {
        Console.WriteLine($"Running benchmark {benchmark.Id}");

        string[] output = new string[_metrics.Length + 2];
        output[0] = benchmark.Name;
        output[1] = benchmark.Parameters?[0].ToString() ?? string.Empty;

        foreach (var metric in _metrics)
            metric.Reset();

        var watch = new Stopwatch();
        for (int i = 0; i < iterationCount; i++)
        {
            watch.Restart();
            BenchmarkResult result = (BenchmarkResult)benchmark.Method.Invoke(obj, benchmark.Parameters);
            watch.Stop();

            foreach (var monitor in _monitors)
                monitor.HandleResult(benchmark.Id, watch.Elapsed, result);
            foreach (var metric in _metrics)
                metric.HandleResult(result, watch.Elapsed);
        }

        for (int i = 0; i < _metrics.Length; i++)
        {
            output[i + 2] = _metrics[i].FormatMetric();
        }
        
        return new List<string>(output);
    }

    static void DisplayOutput1(List<BenchmarkInfo> benchmarks, IEnumerable<string> headerInfo, bool doExport)
    {
        string[,] output = new string[benchmarks.Count + 1, _monitors.Count + 2];

        // Add header row
        output[0, 0] = "Method";
        output[0, 1] = "Parameters";
        for (int i = 0; i < _monitors.Count; i++)
        {
            output[0, i + 2] = _monitors[i].DisplayName;
        }

        // Add data rows
        int row = 0, col = 0;
        foreach (var benchmark in benchmarks)
        {
            output[++row, col] = benchmark.Name;
            output[row, ++col] = benchmark.Parameters?[0].ToString() ?? string.Empty;
            foreach (var monitor in _monitors)
            {
                output[row, ++col] = monitor.FormatResult(benchmark.Id);
            }
            col = 0;
        }

        IEnumerable<string> text = headerInfo.Concat(DisplayOutput2(output));

        foreach (string t in text)
        {
            Console.WriteLine(t);
        }

        if (doExport)
            ExportResults(text);
    }

    static IEnumerable<string> DisplayOutput2(string[,] output)
    {
        var sbs = new StringBuilder[output.GetLength(0)];
        for (int i = 0; i < sbs.Length; i++)
            sbs[i] = new StringBuilder("|");

        for (int col = 0; col < output.GetLength(1); col++)
        {
            // Find maxwidth in the column
            int maxWidth = 0;
            for (int row = 0; row < output.GetLength(0); row++)
            {
                int width = output[row, col].Length;
                if (width > maxWidth)
                    maxWidth = width;
            }

            // Add each row's text
            for (int row = 0; row < output.GetLength(0); row++)
            {
                sbs[row].Append(output[row, col].PadLeft(maxWidth + 1, ' ')).Append(" |");
            }
        }

        string header = sbs[0].ToString();
        var dashLine = new StringBuilder();
        var emptyLine = new StringBuilder();

        foreach (char c in header)
        {
            dashLine.Append(c == '|' ? '|' : '-');
            emptyLine.Append(c == '|' ? '|' : ' ');
        }

        var text = new List<string>()
        {
            header,
            dashLine.ToString()
        };
        text.AddRange(sbs.Skip(1).Select(x => x.ToString()));

        // Add gaps
        string lastMethod = text[2].Substring(2, text[2].IndexOf('|', 2));
        for (int i = 2; i < text.Count; i++)
        {
            string currentMethod = text[i].Substring(2, text[i].IndexOf('|', 2));
            if (currentMethod == lastMethod)
                continue;

            lastMethod = currentMethod;
            text.Insert(i++, emptyLine.ToString());
        }

        return text;
    }

    static void ExportResults(IEnumerable<string> text)
    {
        string filePath = Path.Combine(BASE_DIRECTORY, "BenchmarkResults", "Latest.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        File.WriteAllLines(filePath, text);
    }

    static IEnumerable<object> GetParameters<T>(object obj, string name)
    {
        PropertyInfo property = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        return property == null
            ? throw new Exception($"Failed to find property with the name {name}")
            : (IEnumerable<object>)property.GetValue(obj);
    }

    static IEnumerable<MethodInfo> GetAllSetups<TBenchmark>(string target) where TBenchmark : class
    {
        return typeof(TBenchmark).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => HasValidSetupAttribute(x, target));
    }

    static bool HasValidSetupAttribute(MethodInfo method, string target)
    {
        BenchmarkSetupAttribute attribute = method.GetCustomAttribute<BenchmarkSetupAttribute>();
        return attribute != null && (string.IsNullOrEmpty(attribute.Target) || attribute.Target == target);
    }

    public static string BASE_DIRECTORY { get; } = Assembly.GetExecutingAssembly().Location
        .Substring(0, Assembly.GetExecutingAssembly().Location.IndexOf("BlasII.Randomizer.Benchmarks"));
}
