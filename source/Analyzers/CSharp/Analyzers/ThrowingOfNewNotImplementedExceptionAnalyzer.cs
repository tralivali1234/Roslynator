// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static Roslynator.CSharp.Refactorings.ThrowingOfNewNotImplementedExceptionAnalysis;

namespace Roslynator.CSharp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ThrowingOfNewNotImplementedExceptionAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.ThrowingOfNewNotImplementedException); }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol exceptionSymbol = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_NotImplementedException);

                if (exceptionSymbol == null)
                    return;

                startContext.RegisterSyntaxNodeAction(f => AnalyzeThrowStatement(f, exceptionSymbol), SyntaxKind.ThrowStatement);
                startContext.RegisterSyntaxNodeAction(f => AnalyzeThrowExpression(f, exceptionSymbol), SyntaxKind.ThrowExpression);
            });
        }
    }
}
