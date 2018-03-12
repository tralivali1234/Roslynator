// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseStringLengthInsteadOfComparisonWithEmptyStringAnalysis
    {
        public static void AnalyzeEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            var equalsExpression = (BinaryExpressionSyntax)context.Node;

            ExpressionSyntax left = equalsExpression.Left;

            if (left?.IsMissing == false)
            {
                ExpressionSyntax right = equalsExpression.Right;

                if (right?.IsMissing == false)
                {
                    SemanticModel semanticModel = context.SemanticModel;
                    CancellationToken cancellationToken = context.CancellationToken;

                    if (CSharpUtility.IsEmptyStringExpression(left, semanticModel, cancellationToken))
                    {
                        if (CSharpUtility.IsStringExpression(right, semanticModel, cancellationToken))
                            ReportDiagnostic(context, equalsExpression);
                    }
                    else if (CSharpUtility.IsEmptyStringExpression(right, semanticModel, cancellationToken)
                        && CSharpUtility.IsStringExpression(left, semanticModel, cancellationToken))
                    {
                        ReportDiagnostic(context, equalsExpression);
                    }
                }
            }
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node)
        {
            if (!node.SpanContainsDirectives())
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.UseStringLengthInsteadOfComparisonWithEmptyString,
                    node);
            }
        }
    }
}
