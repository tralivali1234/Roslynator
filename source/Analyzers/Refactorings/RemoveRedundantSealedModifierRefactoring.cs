// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    //TODO: AnalyzeMethodSymbol, AnalyzePropertySymbol
    internal static class RemoveRedundantSealedModifierRefactoring
    {
        public static void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (PropertyDeclarationSyntax)context.Node);
        }

        public static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (MethodDeclarationSyntax)context.Node);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, MemberDeclarationSyntax declaration)
        {
            ISymbol symbol = context.SemanticModel.GetDeclaredSymbol(declaration, context.CancellationToken);

            INamedTypeSymbol containingType = symbol?.ContainingType;

            if (containingType?.TypeKind != TypeKind.Class)
                return;

            if (!containingType.IsSealed)
                return;

            SyntaxToken sealedKeyword = SyntaxInfo.ModifiersInfo(declaration).Modifiers.Find(SyntaxKind.SealedKeyword);

            if (sealedKeyword.Kind() != SyntaxKind.SealedKeyword)
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.RemoveRedundantSealedModifier,
                Location.Create(declaration.SyntaxTree, sealedKeyword.Span));
        }
    }
}
