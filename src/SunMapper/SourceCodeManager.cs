using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper
{
    public class SourceCodeManager
    {
        private readonly SyntaxReceiver _syntaxReceiver;

        public SourceCodeManager(SyntaxReceiver syntaxReceiver)
        {
            _syntaxReceiver = syntaxReceiver;
        }

        public Dictionary<INamedTypeSymbol, HashSet<INamedTypeSymbol>> GetMappingClassesByMapToAttribute(Compilation compilation)
        {
            var mappingClasses = new Dictionary<INamedTypeSymbol, HashSet<INamedTypeSymbol>>(SymbolEqualityComparer.Default);
            INamedTypeSymbol searchingAttributeType = compilation.GetTypeByMetadataName($"SunMapper.Common.Attributes.MapToAttribute")!;
            IEnumerable<ClassSourceTreeInfo> candidateClassesInfo = _syntaxReceiver.CandidateClasses;
            
            foreach (var candidateClassInfo in candidateClassesInfo)
            {
                ClassDeclarationSyntax candidateClass = candidateClassInfo.Declaration;
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                
                var searchedAttributes =  candidateClassInfo.Attributes
                    .Where(_ => model.GetTypeInfo(_).Type!.Equals(searchingAttributeType, SymbolEqualityComparer.Default))
                    .ToArray();

                if (!searchedAttributes.Any())
                {
                    continue;
                }

                if (model.GetDeclaredSymbol(candidateClass) is not
                {
                    DeclaredAccessibility: Accessibility.Public
                } sourceClassType)
                {
                    continue;
                }

                var destinationClasses = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
                
                foreach (var searchedAttribute in searchedAttributes)
                {
                    TypeOfExpressionSyntax typeOfExpression = (TypeOfExpressionSyntax) searchedAttribute.ArgumentList!.Arguments.First().Expression;
                    
                    if (model.GetTypeInfo(typeOfExpression.Type).Type is INamedTypeSymbol
                    {
                        DeclaredAccessibility: Accessibility.Public
                    } destinationClassType)
                    {
                        destinationClasses.Add(destinationClassType);
                    };
                }

                if (destinationClasses.Count == 0)
                {
                    continue;
                }
                
                mappingClasses.Add(sourceClassType, destinationClasses);
            }
           
            
            return mappingClasses;
        }
    }
}