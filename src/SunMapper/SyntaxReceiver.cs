﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper
{
    public class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassSourceTreeInfo> CandidateClasses { get; } = new();

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
                List<AttributeSyntax> attributes = new();
                
                foreach (var attributeList in classDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        attributes.Add(attribute);
                    }
                }
                
                CandidateClasses.Add(new ClassSourceTreeInfo(classDeclaration, attributes));
            }
        }
    }
}