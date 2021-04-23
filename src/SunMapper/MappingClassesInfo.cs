using Microsoft.CodeAnalysis;

namespace SunMapper
{
    public class MappingClassesInfo
    {
        public INamedTypeSymbol Destination { get; }
        public INamedTypeSymbol Source { get; }

        public MappingClassesInfo(INamedTypeSymbol source, INamedTypeSymbol destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
