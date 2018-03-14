// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseAttributeUsageAttributeAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.UseAttributeUsageAttribute); }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol attributeSymbol = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_Attribute);
                INamedTypeSymbol attributeUsageAttributeSymbol = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_AttributeUsageAttribute);

                if (attributeSymbol != null
                    && attributeUsageAttributeSymbol != null)
                {
                    startContext.RegisterSymbolAction(
                       nodeContext => AnalyzerNamedTypeSymbol(nodeContext, attributeSymbol, attributeUsageAttributeSymbol),
                       SymbolKind.NamedType);
                }
            });
        }

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
