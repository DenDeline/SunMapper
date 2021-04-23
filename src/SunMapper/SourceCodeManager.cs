using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
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

        public IEnumerable<MappingClassesInfo> GetMappingClassesByMapToAttribute(Compilation compilation)
        {
            List<MappingClassesInfo> mappingClasses = new();
            INamedTypeSymbol searchingAttributeType = compilation.GetTypeByMetadataName($"SunMapper.Common.Attributes.MapToAttribute")!;
            IEnumerable<ClassSourceTreeInfo> candidateClassesInfo = _syntaxReceiver.CandidateClasses;
            
            foreach (var candidateClassInfo in candidateClassesInfo)
            {
                ClassDeclarationSyntax candidateClass = candidateClassInfo.Declaration;
                SemanticModel model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                
                IEnumerable<AttributeSyntax> candidateClassAttributes = candidateClassInfo.Attributes;
                
                var searchedAttributes =  candidateClassAttributes
                    .Where(_ => model.GetTypeInfo(_).Type!.Equals(searchingAttributeType, SymbolEqualityComparer.Default))
                    .ToArray();

                if (!searchedAttributes.Any())
                {
                    continue;
                }
                
                if (model.GetDeclaredSymbol(candidateClass) is not INamedTypeSymbol sourceClassType)
                {
                    continue;
                }
                
                foreach (var searchedAttribute in searchedAttributes)
                {
                    TypeOfExpressionSyntax typeOfExpression = (TypeOfExpressionSyntax) searchedAttribute.ArgumentList!.Arguments.First().Expression;
                    TypeSyntax destinationResourceType = typeOfExpression.Type;
                    
                    if (model.GetTypeInfo(destinationResourceType).Type is not INamedTypeSymbol destinationClassType)
                    {
                        continue;
                    }

                    mappingClasses.Add(new MappingClassesInfo(sourceClassType, destinationClassType));
                }
            }

            return mappingClasses;
        }
    }
}