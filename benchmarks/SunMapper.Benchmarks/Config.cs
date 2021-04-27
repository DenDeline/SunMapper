using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace SunMapper.Benchmarks
{
    public class Config : ManualConfig
    {
        public Config()
        {
            AddJob(Job.Default);
            AddDiagnoser(MemoryDiagnoser.Default);
            AddColumn(new TagColumn("Library", name => name.Split('_')[0]));
        }
    }
}