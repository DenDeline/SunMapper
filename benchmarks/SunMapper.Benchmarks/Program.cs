using BenchmarkDotNet.Running;

namespace SunMapper.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StandardMappingBenchmarks>();
        }
    }
}