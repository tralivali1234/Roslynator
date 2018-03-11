// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantCommaInInitializerRefactoring
    {
        public static void AnalyzeInitializerExpression(SyntaxNodeAnalysisContext context)
        {
            var initializer = (InitializerExpressionSyntax)context.Node;

            SeparatedSyntaxList<ExpressionSyntax> expressions = initializer.Expressions;

            if (!expressions.Any())
                return;

            int count = expressions.Count;

            if (count != expressions.SeparatorCount)
                return;

            SyntaxToken token = expressions.GetSeparator(count - 1);

            Debug.Assert(!token.IsMissing);

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveRedundantCommaInInitializer, token);
        }
    }
}
