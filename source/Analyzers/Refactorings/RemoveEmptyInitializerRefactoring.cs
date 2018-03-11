// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEmptyInitializerRefactoring
    {
        public static void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            if (objectCreationExpression.ContainsDiagnostics)
                return;

            TypeSyntax type = objectCreationExpression.Type;

            if (type?.IsMissing != false)
                return;

            InitializerExpressionSyntax initializer = objectCreationExpression.Initializer;

            if (initializer?.Expressions.Any() != false)
                return;

            if (!initializer.OpenBraceToken.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!initializer.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyInitializer, initializer);
        }
    }
}
