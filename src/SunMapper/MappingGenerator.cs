﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SunMapper
{
    [Generator]
    public class MappingGenerator : ISourceGenerator
    {
        private const string GlobalIndent = "\t";
        
        private const string MapperExtensionsStub = @"
// <auto-generated/ >
namespace SunMapper.Generated.Extensions
{
    public static class SunMapperExtensions
    {

    }
}
";

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

            SourceCodeManager codeManager = new(syntaxReceiver);
            var mappingClasses = codeManager.GetMappingClassesByMapToAttribute(context.Compilation);
            
            GenerateMapperExtensions(context, mappingClasses);
        }
        
        private void GenerateMapperExtensions(GeneratorExecutionContext context, IEnumerable<MappingClassesInfo> classesToMap)
        {
            const string indent = "";

            if (!classesToMap.Any())
            {
                context.AddSource("SunMapperGenerated", SourceText.From(MapperExtensionsStub, Encoding.UTF8));
                return;
            }
            
            var sb = new StringBuilder();
            
            GenerateMapperExtensionsNamespace(classesToMap, sb, indent);

            context.AddSource("SunMapperGenerated", SourceText.From(sb.ToString(), Encoding.UTF8));
        }


        private void GenerateMapperExtensionsNamespace(IEnumerable<MappingClassesInfo> info, StringBuilder sb, string indent)
        {
            sb.AppendLine($"{indent}// <auto-generated/ >");
            sb.AppendLine($"{indent}namespace SunMapper.Generated.Extensions");
            sb.AppendLine($"{indent}{{");
            
            GenerateMapperClassDeclaration(info, sb, indent + GlobalIndent);
            
            sb.AppendLine($"{indent}}}");
        }
    
        private void GenerateMapperClassDeclaration(IEnumerable<MappingClassesInfo> classesToMap, StringBuilder sb, string indent)
        {
            sb.AppendLine($"{indent}public static class SunMapperExtensions");
            sb.AppendLine($"{indent}{{");
            
            foreach (var classToMap in classesToMap)
            {
                GenerateExtensionMethod(classToMap,sb , indent + GlobalIndent);
                
                sb.AppendLine();
            }
            
            sb.AppendLine($"{indent}}}");
        }   

        private void GenerateExtensionMethod(MappingClassesInfo info, StringBuilder sb, string indent)
        {
            // TODO: 
            if (info.Destination.DeclaredAccessibility != Accessibility.Public ||
                info.Source.DeclaredAccessibility != Accessibility.Public)
            {
                return;
            }

            sb.AppendLine($"{indent}public static bool TryMapTo(this {info.Source} source, out {info.Destination} destination)");
            sb.AppendLine($"{indent}{{ ");
                
            GenerateExtensionMethodBody(info,sb, indent + GlobalIndent);

            sb.AppendLine($"{indent}}}");
        }

        private void GenerateExtensionMethodBody(MappingClassesInfo info, StringBuilder sb, string indent)
        {
            //TODO: Clean up this hell
            
            sb.AppendLine($"{indent}try {{");
            sb.AppendLine($"{indent}{GlobalIndent}destination = new()");
            sb.AppendLine($"{indent}{GlobalIndent}{{");
            
            var destinationClassProperties = info.Destination.GetMembers().OfType<IPropertySymbol>().ToArray();
            var sourceClassProperties = info.Source.GetMembers().OfType<IPropertySymbol>().ToArray();

            
            foreach (var destinationClassProperty in destinationClassProperties)
            {
                if (sourceClassProperties.Any(_ =>  
                    _.Name == destinationClassProperty.Name &&  
                    _.Type.Equals(destinationClassProperty.Type, SymbolEqualityComparer.Default)))
                {
                    sb.AppendLine($"{indent}{GlobalIndent}{GlobalIndent}{destinationClassProperty.Name} = source.{destinationClassProperty.Name},");
                }
            }
            sb.AppendLine($"{indent}{GlobalIndent}}};");
            sb.AppendLine();
            sb.AppendLine($"{indent}{GlobalIndent}return true;");
            sb.AppendLine($"{indent}}}");
            sb.AppendLine($"{indent}catch (System.Exception) {{");
            sb.AppendLine($"{indent}{GlobalIndent}destination = null;");
            sb.AppendLine();
            sb.AppendLine($"{indent}{GlobalIndent}return false;");
            sb.AppendLine($"{indent}}};");
        }
    }
}