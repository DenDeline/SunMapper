using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SunMapper.Core.Attributes;
using SunMapper.SyntaxNodes;

namespace SunMapper.Extensions
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        private static readonly Regex CheckAttribute = new(@"Attribute$", RegexOptions.Compiled);

        public static ImmutableArray<AttributeSyntax> GetAttributesByFilter(this ClassDeclarationSyntax declaration,
            Func<AttributeSyntax, bool> predicate)
        {
            List<AttributeSyntax> attributes = new();
            foreach (var attributeList in declaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (predicate.Invoke(attribute))
                    {
                        attributes.Add(attribute);
                    }
                }
            }
            return attributes.ToImmutableArray();
        }

        public static ImmutableArray<AttributeSyntax> GetAttributesByName(this ClassDeclarationSyntax declaration,
            string attributeName)
            => GetAttributesByFilter(declaration, _ => _.Name.ToString() == attributeName);


        public static ImmutableArray<AttributeSyntax> GetAttributesByType(this ClassDeclarationSyntax declaration,
            ITypeSymbol attributeType, Compilation compilation)
        {
            var attributes = declaration.GetAttributesByName(
                CheckAttribute.Replace(attributeType.Name, string.Empty));
        
            if (attributes.Length == 0)
            {
                return attributes;
            }
            
            List<AttributeSyntax> outputAttributes = new();
        
            var model = compilation.GetSemanticModel(declaration.SyntaxTree);
            
            foreach (AttributeSyntax attributeSyntax in attributes)
            {
                var candidateType = model.GetTypeInfo(attributeSyntax).Type;
        
                if (SymbolEqualityComparer.Default.Equals(candidateType, attributeType))
                {
                    outputAttributes.Add(attributeSyntax);
                }
            }
        
            return outputAttributes.ToImmutableArray();
        }

        public static ImmutableArray<MapToAttributeSyntax> GetMapToAttributes(this ClassDeclarationSyntax declaration, Compilation compilation)
        {
            ITypeSymbol searchingAttributeType = compilation.GetTypeByMetadataName(typeof(MapToAttribute).FullName)!;

            var attributes = declaration.GetAttributesByType(searchingAttributeType, compilation).Select(_ => new MapToAttributeSyntax(_)).ToImmutableArray();
            return attributes;
        }   
    }
}