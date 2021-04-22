using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper
{
    /// <summary>
    ///  Information about source tree of particular class 
    /// </summary>
    public class ClassSourceTreeInfo
    {
        public ClassDeclarationSyntax Declaration { get; }
        
        public IEnumerable<AttributeSyntax> Attributes { get; }
        
        public ClassSourceTreeInfo(
            ClassDeclarationSyntax classDeclaration,
            IEnumerable<AttributeSyntax> attributesDeclaration
        )
        {
            Attributes = attributesDeclaration;
            Declaration = classDeclaration;
        }
    }
}