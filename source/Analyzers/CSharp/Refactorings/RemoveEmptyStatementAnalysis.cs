// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEmptyStatementAnalysis
    {
        public static void AnalyzeEmptyStatement(SyntaxNodeAnalysisContext context)
        {
            SyntaxNode emptyStatement = context.Node;

            SyntaxNode parent = emptyStatement.Parent;

            if (parent == null)
                return;

            SyntaxKind kind = parent.Kind();

            if (kind == SyntaxKind.LabeledStatement)
                return;

            if (CSharpFacts.CanHaveEmbeddedStatement(kind))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyStatement, emptyStatement);
        }
    }
}
