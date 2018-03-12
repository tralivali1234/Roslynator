// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidUsageOfUsingAliasDirectiveAnalysis
    {
        public static void Analyze(SyntaxNodeAnalysisContext context, UsingDirectiveSyntax usingDirective)
        {
            if (usingDirective.Alias == null)
                return;

            if (usingDirective.ContainsDiagnostics)
                return;

            if (usingDirective.SpanContainsDirectives())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.AvoidUsageOfUsingAliasDirective, usingDirective);
        }
    }
}
