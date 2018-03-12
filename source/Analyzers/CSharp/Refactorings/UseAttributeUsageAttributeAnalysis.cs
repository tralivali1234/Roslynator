// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseAttributeUsageAttributeAnalysis
    {
        public static void AnalyzerNamedTypeSymbol(
            SymbolAnalysisContext context,
            INamedTypeSymbol attributeSymbol,
            INamedTypeSymbol attributeUsageAttributeSymbol)
        {
            var symbol = (INamedTypeSymbol)context.Symbol;

            if (symbol.InheritsFrom(attributeSymbol)
                && !symbol.HasAttribute(attributeUsageAttributeSymbol))
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.UseAttributeUsageAttribute,
                    ((ClassDeclarationSyntax)symbol.GetSyntax()).Identifier);
            }
        }
    }
}
