using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System;
using System.Text;

namespace SunMapper.SourceGenerator
{
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax 
            {
                AttributeLists:
                {
                    Count: > 0
                }
            } classDeclaration)  
            {
                CandidateClasses.Add(classDeclaration);
            }
        }
    }
}