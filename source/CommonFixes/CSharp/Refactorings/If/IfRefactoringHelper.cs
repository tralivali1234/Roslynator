// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal static class IfRefactoringHelper
    {
        public static ExpressionSyntax GetBooleanExpression(
            ExpressionSyntax condition,
            ExpressionSyntax expression1,
            ExpressionSyntax expression2,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (expression1.Kind())
            {
                case SyntaxKind.TrueLiteralExpression:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return expression2;
                            case SyntaxKind.FalseLiteralExpression:
                                return condition;
                            default:
                                return LogicalOrExpression(condition, expression2);
                        }
                    }
                case SyntaxKind.FalseLiteralExpression:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return Negation.LogicallyNegate(condition, semanticModel, cancellationToken);
                            case SyntaxKind.FalseLiteralExpression:
                                return expression2;
                            default:
                                return LogicalAndExpression(Negation.LogicallyNegate(condition, semanticModel, cancellationToken), expression2);
                        }
                    }
                default:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return LogicalOrExpression(Negation.LogicallyNegate(condition, semanticModel, cancellationToken), expression1);
                            case SyntaxKind.FalseLiteralExpression:
                                return LogicalAndExpression(condition, expression1);
                            default:
                                throw new InvalidOperationException();
                        }
                    }
            }
        }

        private static BinaryExpressionSyntax LogicalAndExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return CSharpFactory.LogicalAndExpression(
                left.Parenthesize(),
                right.Parenthesize());
        }

        private static BinaryExpressionSyntax LogicalOrExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return CSharpFactory.LogicalOrExpression(
                left.Parenthesize(),
                right.Parenthesize());
        }
    }
}
