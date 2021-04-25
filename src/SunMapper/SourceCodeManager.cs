using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SunMapper.Extensions;

namespace SunMapper
{
    public class SourceCodeManager
    {
        private readonly SyntaxReceiver _syntaxReceiver;

        public SourceCodeManager(SyntaxReceiver syntaxReceiver)
        {
            _syntaxReceiver = syntaxReceiver;
        }
        
        public Dictionary<INamedTypeSymbol, ISet<INamedTypeSymbol>> GetMappingClassesByMapToAttribute(Compilation compilation)
        {
            var mappingClasses = new Dictionary<INamedTypeSymbol, ISet<INamedTypeSymbol>>(SymbolEqualityComparer.Default);
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
                
#nullable disable
                ISet<INamedTypeSymbol> destinationClasses = null;
#nullable enable
                if (isSourceClassContaining)
                {
                    destinationClasses = mappingClasses[sourceClassType];
                }
                else
                {
                    destinationClasses = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
                }

                foreach (var searchedAttribute in mapToAttributes.Select(_ => _.GetDestinationTypeSyntax()))
                {
                    if (model.GetTypeInfo(searchedAttribute).Type is INamedTypeSymbol
                    {
                        DeclaredAccessibility: Accessibility.Public
                    } destinationClassType)
                    {
                        destinationClasses.Add(destinationClassType);
                    };
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