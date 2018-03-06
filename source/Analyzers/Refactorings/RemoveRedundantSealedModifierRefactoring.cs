// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantSealedModifierRefactoring
    {
        public static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            ISymbol symbol = context.Symbol;

            if (((IMethodSymbol)symbol).MethodKind != MethodKind.Ordinary)
                return;

            Analyze(context, symbol);
        }

        public static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            Analyze(context, context.Symbol);
        }

        private static void Analyze(SymbolAnalysisContext context, ISymbol symbol)
        {
            if (symbol.IsImplicitlyDeclared)
                return;

            if (!symbol.IsSealed)
                return;

            if (symbol.ContainingType?.IsSealed != true)
                return;

            Debug.Assert(symbol.ContainingType.TypeKind == TypeKind.Class, symbol.ContainingType.TypeKind.ToString());

            SyntaxNode node = symbol.GetSyntax(context.CancellationToken);

            Debug.Assert(node.IsKind(SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration, SyntaxKind.IndexerDeclaration), node.Kind().ToString());

            ModifiersInfo info = SyntaxInfo.ModifiersInfo(node);

            Debug.Assert(info.IsSealed, info.Modifiers.ToString());

            if (!info.IsSealed)
                return;

            SyntaxToken sealedKeyword = info.Modifiers.Find(SyntaxKind.SealedKeyword);

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveRedundantSealedModifier, sealedKeyword);
        }
    }
}
