// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    //TODO: test
    internal static class UseIsOperatorInsteadOfAsOperatorRefactoring
    {
        public static void AnalyzeEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (BinaryExpressionSyntax)context.Node);
        }

        public static void AnalyzeNotEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (BinaryExpressionSyntax)context.Node);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, BinaryExpressionSyntax binaryExpression)
        {
            if (binaryExpression.SpanContainsDirectives())
                return;

            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(binaryExpression);

            if (!nullCheck.Success)
                return;

            AsExpressionInfo asExpressionInfo = SyntaxInfo.AsExpressionInfo(nullCheck.Expression);

            if (!asExpressionInfo.Success)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.UseIsOperatorInsteadOfAsOperator, binaryExpression);
        }

        public static Task<Document> RefactorAsync(
            Document document,
            BinaryExpressionSyntax binaryExpression,
            CancellationToken cancellationToken)
        {
            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(binaryExpression);

            AsExpressionInfo asExpressionInfo = SyntaxInfo.AsExpressionInfo(nullCheck.Expression);

            ExpressionSyntax newNode = IsExpression(asExpressionInfo.Expression, asExpressionInfo.Type);

            if (nullCheck.IsCheckingNull)
                newNode = LogicalNotExpression(newNode.WithoutTrivia().Parenthesize()).WithTriviaFrom(newNode);

            newNode = newNode
                .Parenthesize()
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(binaryExpression, newNode, cancellationToken);
        }
    }
}
