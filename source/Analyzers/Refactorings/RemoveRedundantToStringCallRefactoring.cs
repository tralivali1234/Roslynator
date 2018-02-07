// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantToStringCallRefactoring
    {
        public static void Analyze(SyntaxNodeAnalysisContext context, MemberInvocationExpressionInfo invocationInfo)
        {
            if (IsFixable(invocationInfo, context.SemanticModel, context.CancellationToken))
            {
                InvocationExpressionSyntax invocationExpression = invocationInfo.InvocationExpression;

                TextSpan span = TextSpan.FromBounds(invocationInfo.OperatorToken.Span.Start, invocationExpression.Span.End);

                if (!invocationExpression.ContainsDirectives(span))
                {
                    context.ReportDiagnostic(
                        DiagnosticDescriptors.RemoveRedundantToStringCall,
                        Location.Create(invocationExpression.SyntaxTree, span));
                }
            }
        }

        public static bool IsFixable(
            MemberInvocationExpressionInfo invocationInfo,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            InvocationExpressionSyntax invocationExpression = invocationInfo.InvocationExpression;

            MethodInfo methodInfo = semanticModel.GetMethodInfo(invocationExpression, cancellationToken);

            if (methodInfo.Symbol != null
                && methodInfo.IsName("ToString")
                && methodInfo.IsPublic
                && !methodInfo.IsStatic
                && methodInfo.IsReturnType(SpecialType.System_String)
                && !methodInfo.IsGenericMethod
                && !methodInfo.IsExtensionMethod
                && !methodInfo.Parameters.Any())
            {
                INamedTypeSymbol containingType = methodInfo.ContainingType;

                if (containingType?.IsReferenceType == true
                    && containingType.SpecialType != SpecialType.System_Enum)
                {
                    if (containingType.IsString())
                        return true;

                    if (invocationExpression.IsParentKind(SyntaxKind.Interpolation))
                        return IsNotHidden(methodInfo.Symbol, containingType);

                    ExpressionSyntax expression = invocationExpression.WalkUpParentheses();

                    SyntaxNode parent = expression.Parent;

                    if (parent?.IsKind(SyntaxKind.AddExpression) == true
                        && !parent.ContainsDiagnostics
                        && IsNotHidden(methodInfo.Symbol, containingType))
                    {
                        var addExpression = (BinaryExpressionSyntax)expression.Parent;

                        ExpressionSyntax left = addExpression.Left;
                        ExpressionSyntax right = addExpression.Right;

                        if (left == expression)
                        {
                            return IsFixable(invocationInfo, addExpression, right, left, semanticModel, cancellationToken);
                        }
                        else
                        {
                            return IsFixable(invocationInfo, addExpression, left, right, semanticModel, cancellationToken);
                        }
                    }
                }
            }

            return false;
        }

        private static bool IsFixable(
            MemberInvocationExpressionInfo invocationInfo,
            BinaryExpressionSyntax addExpression,
            ExpressionSyntax left,
            ExpressionSyntax right,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            if (semanticModel.GetTypeSymbol(left, cancellationToken)?.SpecialType == SpecialType.System_String)
            {
                BinaryExpressionSyntax newAddExpression = addExpression.ReplaceNode(right, invocationInfo.Expression);

                return semanticModel
                    .GetSpeculativeMethodSymbol(addExpression.SpanStart, newAddExpression)?
                    .ContainingType?
                    .SpecialType == SpecialType.System_String;
            }

            return false;
        }

        private static bool IsNotHidden(IMethodSymbol methodSymbol, INamedTypeSymbol containingType)
        {
            if (containingType.IsObject())
                return true;

            if (methodSymbol.IsOverride)
            {
                IMethodSymbol overriddenMethod = methodSymbol.OverriddenMethod;

                while (overriddenMethod != null)
                {
                    if (overriddenMethod.ContainingType?.SpecialType == SpecialType.System_Object)
                        return true;

                    overriddenMethod = overriddenMethod.OverriddenMethod;
                }
            }

            return false;
        }
    }
}