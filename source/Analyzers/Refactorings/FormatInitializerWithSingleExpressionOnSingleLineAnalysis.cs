// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    internal static class FormatInitializerWithSingleExpressionOnSingleLineAnalysis
    {
        public static void AnalyzeInitializerExpression(SyntaxNodeAnalysisContext context)
        {
            var initializer = (InitializerExpressionSyntax)context.Node;

            SeparatedSyntaxList<ExpressionSyntax> expressions = initializer.Expressions;

            ExpressionSyntax expression = expressions.SingleOrDefault(shouldThrow: false);

            if (expression == null)
                return;

            if (initializer.SpanContainsDirectives())
                return;

            if (!expression.IsSingleLine())
                return;

            if (initializer.IsSingleLine())
                return;

            if (!initializer
                .DescendantTrivia(TextSpan.FromBounds(initializer.FullSpan.Start, initializer.Span.End))
                .All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            {
                return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.FormatInitializerWithSingleExpressionOnSingleLine, initializer);
        }
    }
}
