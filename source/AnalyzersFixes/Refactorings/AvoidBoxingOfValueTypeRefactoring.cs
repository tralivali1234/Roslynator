// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidBoxingOfValueTypeRefactoring
    {
        public static void AddExpression(SyntaxNodeAnalysisContext context)
        {
            var addExpression = (BinaryExpressionSyntax)context.Node;

            if (addExpression.ContainsDiagnostics)
                return;

            IMethodSymbol methodSymbol = context.SemanticModel.GetMethodSymbol(addExpression, context.CancellationToken);

            if (!SymbolUtility.IsStringAdditionOperator(methodSymbol))
                return;

            ExpressionSyntax expression = GetObjectExpression()?.WalkDownParentheses();

            if (expression == null)
                return;

            if (expression.Kind() == SyntaxKind.AddExpression)
                return;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(expression, context.CancellationToken);

            if (typeSymbol?.IsValueType != true)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.AvoidBoxingOfValueType, expression);

            ExpressionSyntax GetObjectExpression()
            {
                ImmutableArray<IParameterSymbol> parameters = methodSymbol.Parameters;

                if (parameters[0].Type.SpecialType == SpecialType.System_Object)
                {
                    return addExpression.Left;
                }
                else if (parameters[1].Type.SpecialType == SpecialType.System_Object)
                {
                    return addExpression.Right;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void AnalyzeInterpolation(SyntaxNodeAnalysisContext context)
        {
            var interpolation = (InterpolationSyntax)context.Node;

            if (interpolation.ContainsDiagnostics)
                return;

            if (interpolation.AlignmentClause != null)
                return;

            if (interpolation.FormatClause != null)
                return;

            ExpressionSyntax expression = interpolation.Expression?.WalkDownParentheses();

            if (expression == null)
                return;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(expression, context.CancellationToken);

            if (typeSymbol?.IsValueType != true)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.AvoidBoxingOfValueType, expression);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            ExpressionSyntax expression,
            CancellationToken cancellationToken)
        {
            ExpressionSyntax newNode = null;

            if (expression.Kind() == SyntaxKind.CharacterLiteralExpression)
            {
                var literalExpression = (LiteralExpressionSyntax)expression;

                newNode = StringLiteralExpression(literalExpression.Token.ValueText);
            }
            else
            {
                ParenthesizedExpressionSyntax newExpression = expression
                    .WithoutTrivia()
                    .Parenthesize();

                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                if (semanticModel.GetTypeSymbol(expression, cancellationToken).IsConstructedFrom(SpecialType.System_Nullable_T))
                {
                    newNode = ConditionalAccessExpression(
                        newExpression,
                        InvocationExpression(MemberBindingExpression(IdentifierName("ToString")), ArgumentList()));
                }
                else
                {
                    newNode = SimpleMemberInvocationExpression(
                        newExpression,
                        IdentifierName("ToString"),
                        ArgumentList());
                }
            }

            newNode = newNode.WithTriviaFrom(expression);

            return await document.ReplaceNodeAsync(expression, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
