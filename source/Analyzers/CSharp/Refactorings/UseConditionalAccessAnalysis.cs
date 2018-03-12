// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseConditionalAccessAnalysis
    {
        public static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context, INamedTypeSymbol expressionType)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (!ifStatement.IsSimpleIf())
                return;

            if (ifStatement.ContainsDiagnostics)
                return;

            if (ifStatement.SpanContainsDirectives())
                return;

            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(ifStatement.Condition, allowedStyles: NullCheckStyles.NotEqualsToNull);

            if (!nullCheck.Success)
                return;

            MemberInvocationStatementInfo invocationInfo = SyntaxInfo.MemberInvocationStatementInfo(ifStatement.SingleNonBlockStatementOrDefault());

            if (!invocationInfo.Success)
                return;

            if (!CSharpFactory.AreEquivalent(nullCheck.Expression, invocationInfo.Expression))
                return;

            if (ifStatement.IsInExpressionTree(expressionType, context.SemanticModel, context.CancellationToken))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.UseConditionalAccess, ifStatement);
        }

        public static void AnalyzeLogicalAndExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol expressionType)
        {
            var logicalAndExpression = (BinaryExpressionSyntax)context.Node;

            if (logicalAndExpression.ContainsDiagnostics)
                return;

            ExpressionSyntax expression = SyntaxInfo.NullCheckExpressionInfo(logicalAndExpression.Left, allowedStyles: NullCheckStyles.NotEqualsToNull).Expression;

            if (expression == null)
                return;

            if (context.SemanticModel
                .GetTypeSymbol(expression, context.CancellationToken)?
                .IsReferenceType != true)
            {
                return;
            }

            ExpressionSyntax right = logicalAndExpression.Right?.WalkDownParentheses();

            if (right == null)
                return;

            if (!ValidateRightExpression(right, context.SemanticModel, context.CancellationToken))
                return;

            if (RefactoringUtility.ContainsOutArgumentWithLocal(right, context.SemanticModel, context.CancellationToken))
                return;

            ExpressionSyntax expression2 = FindExpressionThatCanBeConditionallyAccessed(expression, right);

            if (expression2?.SpanContainsDirectives() != false)
                return;

            if (logicalAndExpression.IsInExpressionTree(expressionType, context.SemanticModel, context.CancellationToken))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.UseConditionalAccess, logicalAndExpression);
        }

        internal static ExpressionSyntax FindExpressionThatCanBeConditionallyAccessed(ExpressionSyntax expressionToFind, ExpressionSyntax expression)
        {
            if (expression.IsKind(SyntaxKind.LogicalNotExpression))
                expression = ((PrefixUnaryExpressionSyntax)expression).Operand;

            SyntaxKind kind = expressionToFind.Kind();

            SyntaxToken firstToken = expression.GetFirstToken();

            int start = firstToken.SpanStart;

            SyntaxNode node = firstToken.Parent;

            while (node?.SpanStart == start)
            {
                if (kind == node.Kind()
                    && node.IsParentKind(SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.ElementAccessExpression)
                    && CSharpFactory.AreEquivalent(expressionToFind, node))
                {
                    return (ExpressionSyntax)node;
                }

                node = node.Parent;
            }

            return null;
        }

        private static bool ValidateRightExpression(ExpressionSyntax expression, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            switch (expression.Kind())
            {
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.EqualsExpression:
                    {
                        return ((BinaryExpressionSyntax)expression)
                            .Right?
                            .WalkDownParentheses()
                            .HasConstantNonNullValue(semanticModel, cancellationToken) == true;
                    }
                case SyntaxKind.NotEqualsExpression:
                    {
                        return ((BinaryExpressionSyntax)expression)
                            .Right?
                            .WalkDownParentheses()
                            .Kind() == SyntaxKind.NullLiteralExpression;
                    }
                case SyntaxKind.SimpleMemberAccessExpression:
                case SyntaxKind.InvocationExpression:
                case SyntaxKind.ElementAccessExpression:
                case SyntaxKind.LogicalNotExpression:
                case SyntaxKind.IsExpression:
                case SyntaxKind.IsPatternExpression:
                case SyntaxKind.AsExpression:
                case SyntaxKind.LogicalAndExpression:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private static bool HasConstantNonNullValue(this ExpressionSyntax expression, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            return optional.HasValue
                && optional.Value != null;
        }
    }
}
