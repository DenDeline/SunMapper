using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SunMapper.SyntaxNodes
{
    internal class MapToAttributeSyntax
    {
        public AttributeSyntax Raw { get; }

        public MapToAttributeSyntax(AttributeSyntax rawAttribute)
        {
            Raw = rawAttribute; 
        }
        
        public TypeSyntax GetDestinationTypeSyntax()
        {
            var typeOfExpression = (TypeOfExpressionSyntax) Raw.ArgumentList!.Arguments.First().Expression;
            return typeOfExpression.Type;
        }
    }
}