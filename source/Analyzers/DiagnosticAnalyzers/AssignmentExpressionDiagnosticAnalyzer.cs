// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static Roslynator.CSharp.Refactorings.RemoveRedundantDelegateCreationRefactoring;

namespace Roslynator.CSharp.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssignmentExpressionDiagnosticAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.RemoveRedundantDelegateCreation,
                    DiagnosticDescriptors.RemoveRedundantDelegateCreationFadeOut);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol eventHandler = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_EventHandler);

                if (eventHandler == null)
                    return;

                INamedTypeSymbol eventHandlerOfT = startContext.Compilation.GetTypeByMetadataName(MetadataNames.System_EventHandler_T);

                if (eventHandlerOfT == null)
                    return;

                startContext.RegisterSyntaxNodeAction(f => AnalyzeAssignmentExpression(f, eventHandler, eventHandlerOfT), SyntaxKind.AddAssignmentExpression);
                startContext.RegisterSyntaxNodeAction(f => AnalyzeAssignmentExpression(f, eventHandler, eventHandlerOfT), SyntaxKind.SubtractAssignmentExpression);
            });
        }
    }
}
