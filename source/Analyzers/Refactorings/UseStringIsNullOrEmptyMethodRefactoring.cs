// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;
using static Roslynator.CSharp.CSharpTypeFactory;

namespace Roslynator.CSharp.Refactorings
{
    //TODO: test
    internal static class UseStringIsNullOrEmptyMethodRefactoring
    {
        public static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;

            if (binaryExpression.ContainsDiagnostics)
                return;

            if (binaryExpression.SpanContainsDirectives())
                return;

            BinaryExpressionInfo info = SyntaxInfo.BinaryExpressionInfo(binaryExpression);

            if (!info.Success)
                return;

            SyntaxKind kind = binaryExpression.Kind();

            if (kind == SyntaxKind.LogicalOrExpression)
            {
                if (info.Left.IsKind(SyntaxKind.EqualsExpression)
                    && info.Right.IsKind(SyntaxKind.EqualsExpression)
                    && CanRefactor(
                        (BinaryExpressionSyntax)info.Left,
                        (BinaryExpressionSyntax)info.Right,
                        context.SemanticModel,
                        context.CancellationToken))
                {
                    context.ReportDiagnostic(DiagnosticDescriptors.UseStringIsNullOrEmptyMethod, binaryExpression);
                }
            }
            else if (kind == SyntaxKind.LogicalAndExpression)
            {
                if (info.Left.IsKind(SyntaxKind.NotEqualsExpression, SyntaxKind.LogicalNotExpression)
                    && info.Right.IsKind(SyntaxKind.NotEqualsExpression, SyntaxKind.GreaterThanExpression)
                    && CanRefactor(
                        (BinaryExpressionSyntax)info.Left,
                        (BinaryExpressionSyntax)info.Right,
                        context.SemanticModel,
                        context.CancellationToken))
                {
                    context.ReportDiagnostic(DiagnosticDescriptors.UseStringIsNullOrEmptyMethod, binaryExpression);
                }
            }
        }

        //XTODO: opt
        private static bool CanRefactor(
            BinaryExpressionSyntax left,
            BinaryExpressionSyntax right,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(left);

            if (!nullCheck.Success)
                return false;

            ExpressionSyntax expression = nullCheck.Expression;

            if (CSharpFactory.AreEquivalent(expression, right.Left))
            {
                return right.IsKind(SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression)
                    && SymbolEquals(expression, right.Left, semanticModel, cancellationToken)
                    && CSharpUtility.IsEmptyStringExpression(right.Right, semanticModel, cancellationToken);
            }

            if (right.Left.IsKind(SyntaxKind.SimpleMemberAccessExpression))
            {
                var memberAccess = (MemberAccessExpressionSyntax)right.Left;

                return string.Equals(memberAccess.Name.Identifier.ValueText, "Length", StringComparison.Ordinal)
                    && right.Right.IsNumericLiteralExpression("0")
                    && semanticModel.GetSymbol(memberAccess, cancellationToken) is IPropertySymbol propertySymbol
                    && SymbolUtility.IsPublicInstanceProperty(propertySymbol, "Length")
                    && propertySymbol.Type.SpecialType == SpecialType.System_Int32
                    && propertySymbol.ContainingType?.SpecialType == SpecialType.System_String
                    && CSharpFactory.AreEquivalent(expression, memberAccess.Expression)
                    && SymbolEquals(expression, memberAccess.Expression, semanticModel, cancellationToken);
            }

            return false;
        }

        private static bool SymbolEquals(
            ExpressionSyntax expression1,
            ExpressionSyntax expression2,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            return semanticModel.GetSymbol(expression1, cancellationToken)?
                .Equals(semanticModel.GetSymbol(expression2, cancellationToken)) == true;
        }

        public static Task<Document> RefactorAsync(
            Document document,
            BinaryExpressionSyntax binaryExpression,
            CancellationToken cancellationToken)
        {
            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(binaryExpression.Left);

            ExpressionSyntax newNode = SimpleMemberInvocationExpression(
                StringType(),
                IdentifierName("IsNullOrEmpty"),
                Argument(nullCheck.Expression));

            if (nullCheck.IsCheckingNotNull)
                newNode = LogicalNotExpression(newNode);

            newNode = newNode
                .WithTriviaFrom(binaryExpression)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(binaryExpression, newNode, cancellationToken);
        }
    }
}
