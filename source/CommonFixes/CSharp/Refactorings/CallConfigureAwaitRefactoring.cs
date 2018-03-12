// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class CallConfigureAwaitRefactoring
    {
        public static Task<Document> RefactorAsync(
            Document document,
            AwaitExpressionSyntax awaitExpression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var invocation = (InvocationExpressionSyntax)awaitExpression.Expression;

            InvocationExpressionSyntax newInvocation = InvocationExpression(
                SimpleMemberAccessExpression(invocation.WithoutTrailingTrivia(), IdentifierName("ConfigureAwait")),
                ArgumentList(Argument(FalseLiteralExpression())));

            newInvocation = newInvocation.WithTrailingTrivia(invocation.GetTrailingTrivia());

            return document.ReplaceNodeAsync(invocation, newInvocation, cancellationToken);
        }
    }
}
