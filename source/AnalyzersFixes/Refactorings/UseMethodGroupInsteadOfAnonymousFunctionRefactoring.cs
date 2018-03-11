// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseMethodGroupInsteadOfAnonymousFunctionRefactoring
    {
        //TODO: ?
        private static InvocationExpressionSyntax GetInvocationExpression(SyntaxNode node)
        {
            ExpressionSyntax expression = GetExpression(node)?.WalkDownParentheses();

            if (expression?.Kind() == SyntaxKind.InvocationExpression)
                return (InvocationExpressionSyntax)expression;

            return null;
        }

        private static ExpressionSyntax GetExpression(SyntaxNode node)
        {
            if (node?.Kind() == SyntaxKind.Block)
            {
                var block = (BlockSyntax)node;

                StatementSyntax statement = block.Statements.SingleOrDefault(shouldThrow: false);

                switch (statement?.Kind())
                {
                    case SyntaxKind.ExpressionStatement:
                        return ((ExpressionStatementSyntax)statement).Expression;
                    case SyntaxKind.ReturnStatement:
                        return ((ReturnStatementSyntax)statement).Expression;
                    default:
                        return null;
                }
            }

            return node as ExpressionSyntax;
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            AnonymousFunctionExpressionSyntax anonymousFunction,
            CancellationToken cancellationToken)
        {
            InvocationExpressionSyntax invocationExpression = GetInvocationExpression(anonymousFunction.Body);

            ExpressionSyntax newNode = invocationExpression.Expression;

            SemanticModel semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);

            var methodSymbol = (IMethodSymbol)semanticModel.GetSymbol(invocationExpression, cancellationToken);

            if (methodSymbol.IsReducedExtensionMethod())
                newNode = ((MemberAccessExpressionSyntax)newNode).Name;

            newNode = newNode.WithTriviaFrom(anonymousFunction);

            return await document.ReplaceNodeAsync(anonymousFunction, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
