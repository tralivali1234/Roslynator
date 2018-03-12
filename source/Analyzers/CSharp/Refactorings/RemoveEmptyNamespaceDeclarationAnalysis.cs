// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEmptyNamespaceDeclarationAnalysis
    {
        public static void AnalyzeNamespaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (NamespaceDeclarationSyntax)context.Node;

            if (declaration.Members.Any())
                return;

            SyntaxToken openBrace = declaration.OpenBraceToken;
            SyntaxToken closeBrace = declaration.CloseBraceToken;

            if (openBrace.IsMissing)
                return;

            if (closeBrace.IsMissing)
                return;

            if (!openBrace.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!closeBrace.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyNamespaceDeclaration, declaration);
        }
    }
}
