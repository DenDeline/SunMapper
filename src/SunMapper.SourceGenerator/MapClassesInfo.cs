using Microsoft.CodeAnalysis;

namespace SunMapper.SourceGenerator
{
    public record MapClassesInfo
    {
        public ITypeSymbol SourceClassType { get; }
        public ITypeSymbol DestinationClassType { get; }

        public MapClassesInfo(ITypeSymbol sourceClassType, ITypeSymbol destinationClassType)
        {
            SourceClassType = sourceClassType;
            DestinationClassType = destinationClassType;
        }
    }
}