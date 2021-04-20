using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper
{
    public class ClassInfo
    {
        public ClassDeclarationSyntax ClassDeclaration { get; }
        public IEnumerable<AttributeSyntax> Attributes { get; }
        
        public ClassInfo(
            ClassDeclarationSyntax classDeclaration,
            IEnumerable<AttributeSyntax> attributesDeclaration
        )
        {
            Attributes = attributesDeclaration;
            ClassDeclaration = classDeclaration;
        }
    }
}