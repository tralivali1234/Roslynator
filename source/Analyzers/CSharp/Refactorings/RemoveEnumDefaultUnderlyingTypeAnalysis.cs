// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEnumDefaultUnderlyingTypeAnalysis
    {
        public static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            TypeSyntax type = enumDeclaration.BaseList?.Types.SingleOrDefault(shouldThrow: false)?.Type;

            if (type != null
                && context.SemanticModel.GetTypeSymbol(type, context.CancellationToken).SpecialType == SpecialType.System_Int32)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.RemoveEnumDefaultUnderlyingType, type);
            }
        }
    }
}
