// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddParenthesesAccordingToOperatorPrecedenceRefactoring
    {
        //TODO: 
        private static bool IsFixable(SyntaxNode node)
        {
            SyntaxNode parent = node.Parent;

            return parent != null
                && IsFixable(node, parent.Kind());
        }

        private static bool IsFixable(SyntaxNode node, SyntaxKind binaryExpressionKind)
        {
            int groupNumber = GetGroupNumber(binaryExpressionKind);

            return groupNumber != 0
                && groupNumber == GetGroupNumber(node)
                && OperatorPrecedence.GetPrecedence(node) < OperatorPrecedence.GetPrecedence(binaryExpressionKind);
        }

        private static int GetGroupNumber(SyntaxNode node)
        {
            return GetGroupNumber(node.Kind());
        }

        private static int GetGroupNumber(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                    return 1;
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                    return 2;
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return 3;
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                    return 4;
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.ExclusiveOrExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.LogicalOrExpression:
                    return 5;
                default:
                    return 0;
            }
        }

        public static Task<Document> RefactorAsync(
            Document document,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var newNode = (ExpressionSyntax)SyntaxRewriter.Instance.Visit(expression);

            newNode = newNode.Parenthesize(simplifiable: false);

            return document.ReplaceNodeAsync(expression, newNode, cancellationToken);
        }

        private class SyntaxRewriter : CSharpSyntaxRewriter
        {
            private SyntaxRewriter()
            {
            }

            public static SyntaxRewriter Instance { get; } = new SyntaxRewriter();

            public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                ExpressionSyntax left = VisitExpression(node.Left);
                ExpressionSyntax right = VisitExpression(node.Right);

                return node.Update(left, node.OperatorToken, right);
            }

            private ExpressionSyntax VisitExpression(ExpressionSyntax expression)
            {
                bool isFixable = IsFixable(expression);

                expression = (ExpressionSyntax)base.Visit(expression);

                if (isFixable)
                    expression = expression.Parenthesize(simplifiable: false);

                return expression;
            }
        }
    }
}