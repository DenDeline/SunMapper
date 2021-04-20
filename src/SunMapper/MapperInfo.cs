using Microsoft.CodeAnalysis;

namespace SunMapper
{
    public class MapperInfo
    {
        public INamedTypeSymbol Destination { get; set; }
        public INamedTypeSymbol Source { get; set; }

        public MapperInfo(INamedTypeSymbol source, INamedTypeSymbol destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
