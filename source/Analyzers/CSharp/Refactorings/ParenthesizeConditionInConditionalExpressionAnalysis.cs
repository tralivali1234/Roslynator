// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ParenthesizeConditionInConditionalExpressionAnalysis
    {
        public static void AnalyzeConditionalExpression(SyntaxNodeAnalysisContext context)
        {
            var conditionalExpression = (ConditionalExpressionSyntax)context.Node;

            if (conditionalExpression.ContainsDiagnostics)
                return;

            ConditionalExpressionInfo info = SyntaxInfo.ConditionalExpressionInfo(conditionalExpression, walkDownParentheses: false);

            if (!info.Success)
                return;

            if (info.Condition.Kind() == SyntaxKind.ParenthesizedExpression)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.ParenthesizeConditionInConditionalExpression, info.Condition);
        }
    }
}
