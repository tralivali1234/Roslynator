// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantDelegateCreationRefactoring
    {
        public static void AnalyzeAssignmentExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol eventHandler, INamedTypeSymbol eventHandlerOfT)
        {
            var assignmentExpression = (AssignmentExpressionSyntax)context.Node;

            AssignmentExpressionInfo info = SyntaxInfo.AssignmentExpressionInfo(assignmentExpression);

            if (!info.Success)
                return;

            if (info.Right.Kind() != SyntaxKind.ObjectCreationExpression)
                return;

            var objectCreation = (ObjectCreationExpressionSyntax)info.Right;

            if (objectCreation.SpanContainsDirectives())
                return;

            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(objectCreation, cancellationToken);

            if (typeSymbol == null)
                return;

            if (!typeSymbol.Equals(eventHandler)
                && !typeSymbol.IsConstructedFrom(eventHandlerOfT))
            {
                return;
            }

            ExpressionSyntax expression = objectCreation
                .ArgumentList?
                .Arguments
                .SingleOrDefault(shouldThrow: false)?
                .Expression;

            if (expression == null)
                return;

            if (!(semanticModel.GetSymbol(expression, cancellationToken) is IMethodSymbol))
                return;

            if (semanticModel.GetSymbol(info.Left, cancellationToken)?.IsEvent() != true)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveRedundantDelegateCreation, info.Right);

            context.ReportToken(DiagnosticDescriptors.RemoveRedundantDelegateCreationFadeOut, objectCreation.NewKeyword);
            context.ReportNode(DiagnosticDescriptors.RemoveRedundantDelegateCreationFadeOut, objectCreation.Type);
            context.ReportParentheses(DiagnosticDescriptors.RemoveRedundantDelegateCreationFadeOut, objectCreation.ArgumentList);
        }

        public static Task<Document> RefactorAsync(
            Document document,
            ObjectCreationExpressionSyntax objectCreation,
            CancellationToken cancellationToken)
        {
            ExpressionSyntax expression = objectCreation
                .ArgumentList
                .Arguments
                .First()
                .Expression;

            IEnumerable<SyntaxTrivia> leadingTrivia = objectCreation
                .DescendantTrivia(TextSpan.FromBounds(objectCreation.FullSpan.Start, expression.SpanStart));

            IEnumerable<SyntaxTrivia> trailingTrivia = objectCreation
                .DescendantTrivia(TextSpan.FromBounds(expression.Span.End, objectCreation.FullSpan.End));

            ExpressionSyntax newExpression = expression
                .WithLeadingTrivia(leadingTrivia)
                .WithTrailingTrivia(trailingTrivia)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(objectCreation, newExpression, cancellationToken);
        }
    }
}
