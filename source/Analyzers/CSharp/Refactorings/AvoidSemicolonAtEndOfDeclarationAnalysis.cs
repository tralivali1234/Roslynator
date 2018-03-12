// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidSemicolonAtEndOfDeclarationAnalysis
    {
        public static void AnalyzeNamespaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (NamespaceDeclarationSyntax)context.Node;

            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent != null
                && !semicolon.IsMissing)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidSemicolonAtEndOfDeclaration, semicolon);
            }
        }

        public static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (ClassDeclarationSyntax)context.Node;

            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent != null
                && !semicolon.IsMissing)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidSemicolonAtEndOfDeclaration, semicolon);
            }
        }

        public static void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (InterfaceDeclarationSyntax)context.Node;

            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent != null
                && !semicolon.IsMissing)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidSemicolonAtEndOfDeclaration, semicolon);
            }
        }

        public static void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (StructDeclarationSyntax)context.Node;

            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent != null
                && !semicolon.IsMissing)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidSemicolonAtEndOfDeclaration, semicolon);
            }
        }

        public static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (EnumDeclarationSyntax)context.Node;

            SyntaxToken semicolon = declaration.SemicolonToken;

            if (semicolon.Parent != null
                && !semicolon.IsMissing)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.AvoidSemicolonAtEndOfDeclaration, semicolon);
            }
        }
    }
}
