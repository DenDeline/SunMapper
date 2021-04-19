using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
