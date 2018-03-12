// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Refactorings;
using Roslynator.CSharp.Refactorings.If;

namespace Roslynator.CSharp.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfStatementAnalyzer : BaseDiagnosticAnalyzer
    {
        internal static IfAnalysisOptions AnalysisOptions { get; } = new IfAnalysisOptions(
            useCoalesceExpression: true,
            useConditionalExpression: false,
            useBooleanExpression: false,
            useExpression: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.UseCoalesceExpressionInsteadOfIf,
                    DiagnosticDescriptors.ReplaceIfStatementWithReturnStatement,
                    DiagnosticDescriptors.ReplaceIfStatementWithReturnStatementFadeOut,
                    DiagnosticDescriptors.ReplaceIfStatementWithAssignment);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);

            context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
        }

        private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            foreach (IfAnalysis refactoring in IfAnalysis.Analyze(ifStatement, AnalysisOptions, context.SemanticModel, context.CancellationToken))
            {
                Debug.Assert(refactoring.Kind == IfRefactoringKind.IfElseToAssignmentWithCoalesceExpression
                    || refactoring.Kind == IfRefactoringKind.IfElseToAssignmentWithExpression
                    || refactoring.Kind == IfRefactoringKind.IfElseToReturnWithCoalesceExpression
                    || refactoring.Kind == IfRefactoringKind.IfElseToYieldReturnWithCoalesceExpression
                    || refactoring.Kind == IfRefactoringKind.IfReturnToReturnWithCoalesceExpression
                    || refactoring.Kind == IfRefactoringKind.IfElseToReturnWithExpression
                    || refactoring.Kind == IfRefactoringKind.IfElseToYieldReturnWithExpression
                    || refactoring.Kind == IfRefactoringKind.IfReturnToReturnWithExpression, refactoring.Kind.ToString());

                switch (refactoring.Kind)
                {
                    case IfRefactoringKind.IfElseToAssignmentWithCoalesceExpression:
                    case IfRefactoringKind.IfElseToReturnWithCoalesceExpression:
                    case IfRefactoringKind.IfElseToYieldReturnWithCoalesceExpression:
                    case IfRefactoringKind.IfReturnToReturnWithCoalesceExpression:
                        {
                            context.ReportDiagnostic(DiagnosticDescriptors.UseCoalesceExpressionInsteadOfIf, ifStatement);
                            break;
                        }
                    case IfRefactoringKind.IfElseToReturnWithExpression:
                    case IfRefactoringKind.IfElseToYieldReturnWithExpression:
                    case IfRefactoringKind.IfReturnToReturnWithExpression:
                        {
                            context.ReportDiagnostic(DiagnosticDescriptors.ReplaceIfStatementWithReturnStatement, ifStatement);
                            break;
                        }
                    case IfRefactoringKind.IfElseToAssignmentWithExpression:
                        {
                            context.ReportDiagnostic(DiagnosticDescriptors.ReplaceIfStatementWithAssignment, ifStatement);
                            break;
                        }
                }
            }
        }
    }
}
