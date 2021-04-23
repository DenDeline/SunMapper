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

        public bool Equals(MappingClassesInfo? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SymbolEqualityComparer.Default.Equals(Destination, other.Destination) && 
                   SymbolEqualityComparer.Default.Equals(Source, other.Source);
        }
    }
}
