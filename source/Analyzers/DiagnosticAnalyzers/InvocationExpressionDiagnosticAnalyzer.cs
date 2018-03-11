// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Refactorings;
using Roslynator.CSharp.Refactorings.UseInsteadOfCountMethod;
using Roslynator.CSharp.Refactorings.UseMethodChaining;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InvocationExpressionDiagnosticAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.SimplifyLinqMethodChain,
                    DiagnosticDescriptors.UseCountOrLengthPropertyInsteadOfAnyMethod,
                    DiagnosticDescriptors.UseCountOrLengthPropertyInsteadOfCountMethod,
                    DiagnosticDescriptors.CallAnyInsteadOfCount,
                    DiagnosticDescriptors.UseBitwiseOperationInsteadOfCallingHasFlag,
                    DiagnosticDescriptors.RemoveRedundantToStringCall,
                    DiagnosticDescriptors.RemoveRedundantStringToCharArrayCall,
                    DiagnosticDescriptors.CallCastInsteadOfSelect,
                    DiagnosticDescriptors.CombineEnumerableWhereMethodChain,
                    DiagnosticDescriptors.CombineEnumerableWhereMethodChainFadeOut,
                    DiagnosticDescriptors.CallFindInsteadOfFirstOrDefault,
                    DiagnosticDescriptors.UseElementAccessInsteadOfElementAt,
                    DiagnosticDescriptors.UseElementAccessInsteadOfFirst,
                    DiagnosticDescriptors.UseRegexInstanceInsteadOfStaticMethod,
                    DiagnosticDescriptors.CallExtensionMethodAsInstanceMethod,
                    DiagnosticDescriptors.OptimizeStringBuilderAppendCall,
                    DiagnosticDescriptors.AvoidBoxingOfValueType,
                    DiagnosticDescriptors.CallThenByInsteadOfOrderBy,
                    DiagnosticDescriptors.UseMethodChaining,
                    DiagnosticDescriptors.AvoidNullReferenceException,
                    DiagnosticDescriptors.UseStringComparison,
                    DiagnosticDescriptors.UseNameOfOperator,
                    DiagnosticDescriptors.RemoveRedundantCast,
                    DiagnosticDescriptors.SimplifyLogicalNegation,
                    DiagnosticDescriptors.CallStringConcatInsteadOfStringJoin);
           }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);

            context.RegisterSyntaxNodeAction(AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (UseBitwiseOperationInsteadOfCallingHasFlagRefactoring.CanRefactor(invocation, context.SemanticModel, context.CancellationToken)
                && !invocation.SpanContainsDirectives())
            {
                context.ReportDiagnostic(DiagnosticDescriptors.UseBitwiseOperationInsteadOfCallingHasFlag, invocation);
            }

            RemoveRedundantStringToCharArrayCallRefactoring.Analyze(context, invocation);

            CombineEnumerableWhereAndAnyRefactoring.AnalyzeInvocationExpression(context);

            if (!invocation.ContainsDiagnostics)
            {
                if (!invocation.SpanContainsDirectives())
                {
                    CallExtensionMethodAsInstanceMethodAnalysisResult analysis = CallExtensionMethodAsInstanceMethodRefactoring.Analyze(invocation, context.SemanticModel, allowAnyExpression: false, cancellationToken: context.CancellationToken);

                    if (analysis.Success
                        && context.SemanticModel
                            .GetEnclosingNamedType(analysis.InvocationExpression.SpanStart, context.CancellationToken)?
                            .Equals(analysis.MethodSymbol.ContainingType) == false)
                    {
                        context.ReportDiagnostic(DiagnosticDescriptors.CallExtensionMethodAsInstanceMethod, invocation);
                    }
                }

                MemberInvocationExpressionInfo invocationInfo = SyntaxInfo.MemberInvocationExpressionInfo(invocation);

                if (invocationInfo.Success)
                {
                    if (!invocation.SpanContainsDirectives())
                        UseRegexInstanceInsteadOfStaticMethodRefactoring.Analyze(context, invocationInfo);

                    string methodName = invocationInfo.NameText;

                    AvoidNullReferenceExceptionRefactoring.Analyze(context, invocationInfo);

                    CallStringConcatInsteadOfStringJoinRefactoring.Analyze(context, invocationInfo);

                    int argumentCount = invocationInfo.Arguments.Count;

                    switch (argumentCount)
                    {
                        case 0:
                            {
                                switch (methodName)
                                {
                                    case "Any":
                                        {
                                            UseCountOrLengthPropertyInsteadOfAnyMethodRefactoring.Analyze(context, invocationInfo);

                                            SimplifyLinqMethodChainRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "Cast":
                                        {
                                            CallOfTypeInsteadOfWhereAndCastRefactoring.Analyze(context, invocationInfo);
                                            RemoveRedundantCastRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "Count":
                                        {
                                            UseInsteadOfCountMethodRefactoring.Analyze(context, invocationInfo);
                                            SimplifyLinqMethodChainRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "First":
                                        {
                                            if (!invocationInfo.Expression.IsKind(SyntaxKind.InvocationExpression)
                                                && UseElementAccessInsteadOfFirstRefactoring.CanRefactor(invocationInfo, context.SemanticModel, context.CancellationToken))
                                            {
                                                context.ReportDiagnostic(DiagnosticDescriptors.UseElementAccessInsteadOfFirst, invocationInfo.Name);
                                            }

                                            SimplifyLinqMethodChainRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "ToString":
                                        {
                                            RemoveRedundantToStringCallRefactoring.Analyze(context, invocationInfo);
                                            UseNameOfOperatorRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "ToLower":
                                    case "ToLowerInvariant":
                                    case "ToUpper":
                                    case "ToUpperInvariant":
                                        {
                                            UseStringComparisonRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "FirstOrDefault":
                                    case "Last":
                                    case "LastOrDefault":
                                    case "LongCount":
                                    case "Single":
                                    case "SingleOrDefault":
                                        {
                                            SimplifyLinqMethodChainRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                }

                                break;
                            }
                        case 1:
                            {
                                switch (methodName)
                                {
                                    case "All":
                                    case "Any":
                                        {
                                            SimplifyLogicalNegationRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "ElementAt":
                                        {
                                            if (!invocationInfo.Expression.IsKind(SyntaxKind.InvocationExpression)
                                                && UseElementAccessInsteadOfElementAtRefactoring.CanRefactor(invocationInfo, context.SemanticModel, context.CancellationToken))
                                            {
                                                context.ReportDiagnostic(DiagnosticDescriptors.UseElementAccessInsteadOfElementAt, invocationInfo.Name);
                                            }

                                            break;
                                        }
                                    case "FirstOrDefault":
                                        {
                                            CallFindInsteadOfFirstOrDefaultRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                    case "Where":
                                        {
                                            CombineEnumerableWhereMethodChainRefactoring.Analyze(context, invocationInfo);
                                            break;
                                        }
                                }

                                break;
                            }
                    }

                    switch (methodName)
                    {
                        case "Append":
                        case "AppendLine":
                        case "AppendFormat":
                        case "Insert":
                            {
                                OptimizeStringBuilderAppendCallRefactoring.Analyze(context, invocationInfo);
                                break;
                            }
                        case "Select":
                            {
                                if (argumentCount == 1
                                    || argumentCount == 2)
                                {
                                    CallCastInsteadOfSelectRefactoring.Analyze(context, invocationInfo);
                                }

                                break;
                            }
                        case "OrderBy":
                        case "OrderByDescending":
                            {
                                if (argumentCount == 1
                                    || argumentCount == 2
                                    || argumentCount == 3)
                                {
                                    CallThenByInsteadOfOrderByRefactoring.Analyze(context, invocationInfo);
                                }

                                break;
                            }
                    }

                    if (UseMethodChainingRefactoring.IsFixable(invocationInfo, context.SemanticModel, context.CancellationToken))
                        context.ReportDiagnostic(DiagnosticDescriptors.UseMethodChaining, invocationInfo.InvocationExpression);
                }
            }
        }
    }
}
