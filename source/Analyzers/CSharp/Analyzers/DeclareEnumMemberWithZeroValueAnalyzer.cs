// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DeclareEnumMemberWithZeroValueAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.DeclareEnumMemberWithZeroValue); }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol flagsAttribute = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_FlagsAttribute);

                if (flagsAttribute == null)
                    return;

                startContext.RegisterSymbolAction(f => DeclareEnumMemberWithZeroValueAnalysis.AnalyzeNamedType(f, flagsAttribute), SymbolKind.NamedType);
            });
        }
    }
}