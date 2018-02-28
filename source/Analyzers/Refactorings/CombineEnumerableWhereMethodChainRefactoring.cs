// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class CombineEnumerableWhereMethodChainRefactoring
    {
        public static void Analyze(
            SyntaxNodeAnalysisContext context,
            MemberInvocationExpressionInfo invocationInfo)
        {
            MemberInvocationExpressionInfo invocationInfo2 = SyntaxInfo.MemberInvocationExpressionInfo(invocationInfo.Expression);

            if (!invocationInfo2.Success)
                return;

            if (invocationInfo2.Arguments.Count != 1)
                return;

            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            if (!string.Equals(invocationInfo2.NameText, "Where", StringComparison.Ordinal))
                return;

            IMethodSymbol methodSymbol2 = semanticModel.GetReducedExtensionMethodInfo(invocationInfo2.InvocationExpression, cancellationToken).Symbol;

            if (methodSymbol2 == null)
                return;

            if (!SymbolUtility.IsLinqExtensionOfIEnumerableOfT(methodSymbol2, semanticModel, "Where", parameterCount: 2))
                return;

            if (SymbolUtility.IsPredicateFunc(
                methodSymbol2.Parameters[1].Type,
                methodSymbol2.TypeArguments[0],
                semanticModel))
            {
                IMethodSymbol methodSymbol = semanticModel.GetReducedExtensionMethodInfo(invocationInfo.InvocationExpression, cancellationToken).Symbol;

                if (methodSymbol != null
                    && SymbolUtility.IsLinqWhere(methodSymbol, semanticModel))
                {
                    Analyze(context, invocationInfo, invocationInfo2);
                }
            }
            else if (SymbolUtility.IsPredicateFunc(
                methodSymbol2.Parameters[1].Type,
                methodSymbol2.TypeArguments[0],
                semanticModel.Compilation.GetSpecialType(SpecialType.System_Int32),
                semanticModel))
            {
                IMethodSymbol methodSymbol = semanticModel.GetReducedExtensionMethodInfo(invocationInfo.InvocationExpression, cancellationToken).Symbol;

                if (methodSymbol != null
                    && SymbolUtility.IsLinqWhereWithIndex(methodSymbol, semanticModel))
                {
                    Analyze(context, invocationInfo, invocationInfo2);
                }
            }
        }

        private static void Analyze(
            SyntaxNodeAnalysisContext context,
            MemberInvocationExpressionInfo invocationInfo,
            MemberInvocationExpressionInfo invocationInfo2)
        {
            ExpressionSyntax expression = invocationInfo.Arguments.First().Expression;

            if (!AreEquivalentLambdas(expression, invocationInfo2.Arguments.First().Expression))
                return;

            InvocationExpressionSyntax invocation = invocationInfo.InvocationExpression;

            TextSpan span = TextSpan.FromBounds(invocationInfo2.Name.Span.Start, invocation.Span.End);

            if (invocation.ContainsDirectives(span))
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.CombineEnumerableWhereMethodChain,
                Location.Create(invocation.SyntaxTree, span));

            TextSpan fadeOutSpan = TextSpan.FromBounds(
                invocationInfo.OperatorToken.Span.Start,
                ((LambdaExpressionSyntax)expression).ArrowToken.Span.End);

            context.ReportDiagnostic(DiagnosticDescriptors.CombineEnumerableWhereMethodChainFadeOut, Location.Create(invocation.SyntaxTree, fadeOutSpan));
            context.ReportDiagnostic(DiagnosticDescriptors.CombineEnumerableWhereMethodChainFadeOut, invocation.ArgumentList.CloseParenToken);
        }

        private static bool AreEquivalentLambdas(ExpressionSyntax expression1, ExpressionSyntax expression2)
        {
            if (expression1 == null)
                return false;

            if (expression2 == null)
                return false;

            SyntaxKind kind = expression1.Kind();

            if (kind != expression2.Kind())
                return false;

            if (kind == SyntaxKind.SimpleLambdaExpression)
            {
                var lambda1 = (SimpleLambdaExpressionSyntax)expression1;
                var lambda2 = (SimpleLambdaExpressionSyntax)expression2;

                return ParameterIdentifierEquals(lambda1.Parameter, lambda2.Parameter)
                    && lambda1.Body is ExpressionSyntax
                    && lambda2.Body is ExpressionSyntax;
            }
            else if (kind == SyntaxKind.ParenthesizedLambdaExpression)
            {
                var lambda1 = (ParenthesizedLambdaExpressionSyntax)expression1;
                var lambda2 = (ParenthesizedLambdaExpressionSyntax)expression2;

                ParameterListSyntax parameterList1 = lambda1.ParameterList;
                ParameterListSyntax parameterList2 = lambda2.ParameterList;

                if (parameterList1 != null
                    && parameterList2 != null)
                {
                    SeparatedSyntaxList<ParameterSyntax> parameters1 = parameterList1.Parameters;
                    SeparatedSyntaxList<ParameterSyntax> parameters2 = parameterList2.Parameters;

                    if (parameters1.Count == parameters2.Count)
                    {
                        for (int i = 0; i < parameters1.Count; i++)
                        {
                            if (!ParameterIdentifierEquals(parameters1[i], parameters2[i]))
                                return false;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        private static bool ParameterIdentifierEquals(ParameterSyntax parameter1, ParameterSyntax parameter2)
        {
            return parameter1?.Identifier.ValueText.Equals(parameter2.Identifier.ValueText, StringComparison.Ordinal) == true;
        }

        public static Task<Document> RefactorAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            CancellationToken cancellationToken)
        {
            var memberAccess = (MemberAccessExpressionSyntax)invocation.Expression;

            var invocation2 = (InvocationExpressionSyntax)memberAccess.Expression;

            ExpressionSyntax expression1 = GetCondition(invocation);
            ExpressionSyntax expression2 = GetCondition(invocation2);

            InvocationExpressionSyntax newInvocation = invocation2.ReplaceNode(
                expression2,
                LogicalAndExpression(
                    expression2.Parenthesize(),
                    expression1.Parenthesize()));

            var newMemberAccess = (MemberAccessExpressionSyntax)newInvocation.Expression;

            SyntaxTriviaList trailingTrivia = invocation.GetTrailingTrivia();

            IEnumerable<SyntaxTrivia> trivia = invocation.DescendantTrivia(TextSpan.FromBounds(invocation2.Span.End, memberAccess.Name.SpanStart));

            if (trivia.Any(f => !f.IsWhitespaceOrEndOfLineTrivia()))
                trailingTrivia = trailingTrivia.InsertRange(0, trivia);

            newInvocation = newInvocation
                .WithExpression(newMemberAccess)
                .WithTrailingTrivia(trailingTrivia)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(invocation, newInvocation, cancellationToken);
        }

        private static ExpressionSyntax GetCondition(InvocationExpressionSyntax invocation)
        {
            var lambda = (LambdaExpressionSyntax)invocation.ArgumentList.Arguments.First().Expression;

            return (ExpressionSyntax)lambda.Body;
        }
    }
}
