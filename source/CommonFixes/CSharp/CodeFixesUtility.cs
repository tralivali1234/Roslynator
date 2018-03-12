// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp
{
    //TODO: ren
    internal static class CodeFixesUtility
    {
        public static BinaryExpressionSyntax CreateCoalesceExpression(
            ITypeSymbol targetType,
            ExpressionSyntax left,
            ExpressionSyntax right,
            int position,
            SemanticModel semanticModel)
        {
            if (targetType?.SupportsExplicitDeclaration() == true)
            {
                right = CastExpression(
                    targetType.ToMinimalTypeSyntax(semanticModel, position),
                    right.Parenthesize()).WithSimplifierAnnotation();
            }

            return CSharpFactory.CoalesceExpression(
                left.Parenthesize(),
                right.Parenthesize());
        }
    }
}
