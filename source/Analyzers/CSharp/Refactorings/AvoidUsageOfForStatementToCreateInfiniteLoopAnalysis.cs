// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidUsageOfForStatementToCreateInfiniteLoopAnalysis
    {
        public static void AnalyzeForStatement(SyntaxNodeAnalysisContext context)
        {
            var forStatement = (ForStatementSyntax)context.Node;

            if (forStatement.Declaration == null
                && forStatement.Condition == null
                && forStatement.Incrementors.Count == 0
                && forStatement.Initializers.Count == 0
                && !forStatement.OpenParenToken.ContainsDirectives
                && !forStatement.FirstSemicolonToken.ContainsDirectives
                && !forStatement.SecondSemicolonToken.ContainsDirectives
                && !forStatement.CloseParenToken.ContainsDirectives)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidUsageOfForStatementToCreateInfiniteLoop, forStatement.ForKeyword);
            }
        }
    }
}
