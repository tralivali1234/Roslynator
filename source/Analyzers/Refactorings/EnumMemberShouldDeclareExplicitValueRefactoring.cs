// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class EnumMemberShouldDeclareExplicitValueRefactoring
    {
        public static void AnalyzeEnumMemberDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumMember = (EnumMemberDeclarationSyntax)context.Node;

            if (HasImplicitValue(enumMember, context.SemanticModel, context.CancellationToken))
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.EnumMemberShouldDeclareExplicitValue,
                    enumMember.Identifier);
            }
        }

        //TODO: int?
        private static bool HasImplicitValue(EnumMemberDeclarationSyntax enumMember, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            EqualsValueClauseSyntax equalsValue = enumMember.EqualsValue;

            if (equalsValue == null)
            {
                return semanticModel
                    .GetDeclaredSymbol(enumMember, cancellationToken)?
                    .HasConstantValue == true;
            }

            return false;
        }
    }
}
