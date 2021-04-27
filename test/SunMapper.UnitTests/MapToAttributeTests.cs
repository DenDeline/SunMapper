using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SunMapper.Common.Attributes;
using Xunit;

namespace SunMapper.UnitTests
{
    public class MapToAttributeTests
    {
        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new []{ CSharpSyntaxTree.ParseText(source) },
                new []{ MetadataReference.CreateFromFile(typeof(MapToAttribute).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        [Fact]
        public void Should_GenerateExtensionNamespace_Always()
        {
            //arrange
            Compilation inputCompilation = CreateCompilation(@"
namespace TestNamespace {

}
");

            MappingGenerator generator = new();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            
            //act

            driver = driver.RunGenerators(inputCompilation);
            
            //assert

            GeneratorDriverRunResult generatorResult = driver.GetRunResult();
            Assert.True(generatorResult.GeneratedTrees.Length == 1);
            Assert.True(generatorResult.Diagnostics.IsEmpty);
        }

        [Theory]
        [InlineData("EnglishText")]
        [InlineData("日本語のテクスト")]
        [InlineData("ТекстНаРусскомЯзыке")]
        public void Should_GenerateExtensionsNamespaceWithSourceClassName_When_MapToAttributeIsAppendedToSourceClass(string sourceClassName)    
        {
            //arrange
            
            Compilation inputCompilation = CreateCompilation(@$"
using SunMapper.Common.Attributes;

namespace TestNamespace {{
    [MapTo(typeof({sourceClassName}Dto))]
    public class {sourceClassName} 
    {{
 
    }}

    public class {sourceClassName}Dto
    {{

    }}
}}
");

            MappingGenerator generator = new();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            //act
            
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);

            //assert
            
            Assert.True(diagnostics.IsEmpty);
            Assert.True(outputCompilation.GetDiagnostics().Any());
            
            GeneratorDriverRunResult driverResult = driver.GetRunResult(); 
            
            Assert.True(driverResult.GeneratedTrees.Length == 1);
            Assert.True(outputCompilation.ContainsSymbolsWithName($"{sourceClassName}GeneratedExtensions", SymbolFilter.Type));
        }
        
        
        
    }
}