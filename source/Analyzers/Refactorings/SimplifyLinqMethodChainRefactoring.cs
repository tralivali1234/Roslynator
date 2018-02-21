// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class SimplifyLinqMethodChainRefactoring
    {
        public static void Analyze(
            SyntaxNodeAnalysisContext context,
            InvocationExpressionSyntax invocation,
            MemberAccessExpressionSyntax memberAccess,
            string methodName)
        {
            if (memberAccess.Expression?.Kind() != SyntaxKind.InvocationExpression)
                return;

            var invocation2 = (InvocationExpressionSyntax)memberAccess.Expression;

            if (invocation2.ArgumentList?.Arguments.Count != 1)
                return;

            if (invocation2.Expression?.Kind() != SyntaxKind.SimpleMemberAccessExpression)
                return;

            var memberAccess2 = (MemberAccessExpressionSyntax)invocation2.Expression;

            if (memberAccess2.Name?.Identifier.ValueText != "Where")
                return;

            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            MethodInfo methodInfo = semanticModel.GetExtensionMethodInfo(invocation, cancellationToken);

            if (methodInfo.Symbol == null)
                return;

            if (!methodInfo.IsLinqExtensionOfIEnumerableOfTWithoutParameters(methodName, semanticModel))
                return;

            MethodInfo methodInfo2 = semanticModel.GetExtensionMethodInfo(invocation2, cancellationToken);

            if (methodInfo2.Symbol == null)
                return;

            if (!methodInfo2.IsLinqWhere(semanticModel, allowImmutableArrayExtension: true))
                return;

            TextSpan span = TextSpan.FromBounds(memberAccess2.Name.Span.Start, invocation.Span.End);

            if (invocation.ContainsDirectives(span))
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.SimplifyLinqMethodChain,
                Location.Create(invocation.SyntaxTree, span));
        }

        public static Task<Document> RefactorAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var memberAccess = (MemberAccessExpressionSyntax)invocation.Expression;

            var invocation2 = (InvocationExpressionSyntax)memberAccess.Expression;

            var memberAccess2 = (MemberAccessExpressionSyntax)invocation2.Expression;

            InvocationExpressionSyntax newNode = invocation2.WithExpression(
                memberAccess2.WithName(memberAccess.Name.WithTriviaFrom(memberAccess2.Name)));

            IEnumerable<SyntaxTrivia> trivia = invocation.DescendantTrivia(TextSpan.FromBounds(invocation2.Span.End, invocation.Span.End));

            if (trivia.Any(f => !f.IsWhitespaceOrEndOfLineTrivia()))
            {
                newNode = newNode.WithTrailingTrivia(trivia.Concat(invocation.GetTrailingTrivia()));
            }
            else
            {
                newNode = newNode.WithTrailingTrivia(invocation.GetTrailingTrivia());
            }

            return document.ReplaceNodeAsync(invocation, newNode, cancellationToken);
        }
    }
}
