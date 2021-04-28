using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SunMapper.Extensions;

namespace SunMapper
{
    internal class FluentApiManager
    {
        private readonly SyntaxReceiver _syntaxReceiver;

        public FluentApiManager(SyntaxReceiver syntaxReceiver)
        {
            _syntaxReceiver = syntaxReceiver;
        }
        
        public IDictionary<INamedTypeSymbol, ISet<INamedTypeSymbol>> GetMappingClasses(GeneratorExecutionContext context)
        {
            Compilation compilation = context.Compilation;
// https://github.com/dotnet/roslyn-analyzers/issues/4568            
#pragma warning disable RS1024
            var mappingClasses = new Dictionary<INamedTypeSymbol, ISet<INamedTypeSymbol>>(SymbolEqualityComparer.Default);
#pragma warning restore

            
            foreach (var candidateClass in _syntaxReceiver.CandidateClasses)
            {   
                var mapToAttributes =  candidateClass.GetMapToAttributes(compilation);
                if (mapToAttributes.Length == 0)
                {
                    continue;
                }

                var model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                
                if (model.GetDeclaredSymbol(candidateClass) is not
                {
                    DeclaredAccessibility: Accessibility.Public
                } sourceClassType)
                {
                    continue;
                }

                var isSourceClassContaining = mappingClasses.ContainsKey(sourceClassType);
                
// https://github.com/dotnet/roslyn-analyzers/issues/4568            
#pragma warning disable RS1024
                ISet<INamedTypeSymbol> destinationClasses = isSourceClassContaining ? 
                    mappingClasses[sourceClassType] : 
                    new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
#pragma warning restore
                
                foreach (var searchedAttribute in mapToAttributes)
                {
                    var destinationType = searchedAttribute.GetDestinationTypeSyntax();
                    if (model.GetTypeInfo(destinationType).Type is INamedTypeSymbol
                    {
                        DeclaredAccessibility: Accessibility.Public
                    } destinationClassType)
                    {
                        destinationClasses.Add(destinationClassType);
                    }
                }

                if (!isSourceClassContaining)
                {
                    mappingClasses.Add(sourceClassType, destinationClasses);
                }
            }
            return mappingClasses;
        }
    }
}