﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper.SourceGenerator
{
    [Generator]
    public class SunMapperExtensionsGenerator : ISourceGenerator
    {
        private const string MapperAttributesNamespace = "SunMapper.Attributes";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {

            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver)
            {
                return;
            }

            var classDeclarations = syntaxReceiver.CandidateClasses;

            var classesToMap = GetMapClassesInfo(context.Compilation, classDeclarations);

            if (classesToMap.Any())
            {
                var sb = new StringBuilder();
                GenerateSunMapperExtensions(context, sb, classesToMap);
                context.AddSource("Generated.cs", sb.ToString());
            }
        }

        private void GenerateSunMapperExtensions(GeneratorExecutionContext context, StringBuilder sb, IEnumerable<MapperInfo> classesToMap)
        {
            const string indent = "     ";
            sb.AppendLine(@"
// <auto-generated/ >
namespace SunMapperExtensions
{
    public static class MapperExtensions
    {");

            foreach (var mapperInfo in classesToMap)
            {
                sb.AppendLine($"{indent}public static bool TryMapTo(this {mapperInfo.Source} source, out {mapperInfo.Destination} destination)");
                sb.AppendLine($"{indent}{{ ");

                sb.AppendLine($"{indent}{indent} throw new System.Exception();");

                sb.AppendLine($"{indent}}}");
            }

                

            sb.AppendLine(@"
    }
}");
        }

        private static IEnumerable<MapperInfo> GetMapClassesInfo(
            Compilation compilation, 
            IEnumerable<ClassDeclarationSyntax> classDeclarations)
        {
            foreach (var classDeclaration in classDeclarations)
            {
                SemanticModel model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                ITypeSymbol? sourceClassType = model.GetDeclaredSymbol(classDeclaration);

                INamedTypeSymbol? attributeSymbol = compilation.GetTypeByMetadataName($"{MapperAttributesNamespace}.MapToAttribute");

                if (sourceClassType is null ||
                    attributeSymbol is null)
                {
                    continue;
                }

                var classAttrubuteSymbols = sourceClassType.GetAttributes();

                if (!IsAttributeContaining(classAttrubuteSymbols, attributeSymbol))
                {
                    continue;
                };

                var attributeDeclarations = GetAttributesByName(classDeclaration, "MapTo");

                foreach (var attributeDeclaration in attributeDeclarations)
                {
                    var typeOfExpression = attributeDeclaration.ArgumentList?.Arguments.First().Expression as TypeOfExpressionSyntax;

                    if (typeOfExpression is null)
                    {
                        continue;
                    }
                    var destinationResouraseType = typeOfExpression.Type;

                    if(destinationResouraseType is null){
                        throw new NullReferenceException();
                    }

                    ITypeSymbol? destination = model.GetTypeInfo(destinationResouraseType).Type;

                    if (destination is null)
                    {
                        throw new NullReferenceException();
                    }

                    yield return new MapperInfo(sourceClassType, destination);
                }
                    
            }

            static bool IsAttributeContaining(IEnumerable<AttributeData> attributes, INamedTypeSymbol attributeToFind)
                => attributes.Any(_ => _.AttributeClass!.Equals(attributeToFind, SymbolEqualityComparer.Default));

            static IEnumerable<AttributeSyntax> GetAttributesByName(ClassDeclarationSyntax classDeclaration, string attributeName)
            {
                foreach (var attributeList in classDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (attribute.Name.ToString() == attributeName)
                        {
                            yield return attribute;
                        }
                    }
                }
            }
        }
    }
}