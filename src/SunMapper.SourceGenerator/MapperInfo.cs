using Microsoft.CodeAnalysis;

namespace SunMapper.SourceGenerator
{
    public class MapperInfo
    {
        public ITypeSymbol Destination { get; set; }
        public ITypeSymbol Source { get; set; }

        public MapperInfo(ITypeSymbol source, ITypeSymbol destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
