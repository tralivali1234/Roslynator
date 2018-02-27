// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemovePartialModifierFromTypeWithSinglePartRefactoring
    {
        public static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var symbol = (INamedTypeSymbol)context.Symbol;

            if (!symbol.TypeKind.Is(TypeKind.Class, TypeKind.Struct, TypeKind.Interface))
                return;

            SyntaxReference syntaxReference = symbol.DeclaringSyntaxReferences.SingleOrDefault(shouldThrow: false);

            if (syntaxReference == null)
                return;

            if (!(syntaxReference.GetSyntax(context.CancellationToken) is MemberDeclarationSyntax memberDeclaration))
                return;

            SyntaxToken partialKeyword = SyntaxInfo.ModifiersInfo(memberDeclaration).Modifiers.Find(SyntaxKind.PartialKeyword);

            if (!partialKeyword.IsKind(SyntaxKind.PartialKeyword))
                return;

            if (SyntaxInfo.MemberDeclarationsInfo(memberDeclaration)
                .Members
                .Any(member => member.Kind() == SyntaxKind.MethodDeclaration && ((MethodDeclarationSyntax)member).Modifiers.Contains(SyntaxKind.PartialKeyword)))
            {
                return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.RemovePartialModifierFromTypeWithSinglePart, partialKeyword);
        }
    }
}
