// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class FormatBinaryOperatorOnNextLineRefactoring
    {
        public static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;

            ExpressionSyntax left = binaryExpression.Left;
            ExpressionSyntax right = binaryExpression.Right;

            if (left?.IsMissing != false)
                return;

            if (right?.IsMissing != false)
                return;

            if (CSharpUtility.IsStringConcatenation(binaryExpression, context.SemanticModel, context.CancellationToken))
                return;

            if (!left.GetTrailingTrivia().All(f => f.IsWhitespaceTrivia()))
                return;

            if (!CheckOperatorTrailingTrivia(binaryExpression.OperatorToken.TrailingTrivia))
                return;

            if (!right.GetLeadingTrivia().IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.FormatBinaryOperatorOnNextLine,
                binaryExpression.OperatorToken);
        }

        private static bool CheckOperatorTrailingTrivia(SyntaxTriviaList triviaList)
        {
            bool result = false;

            foreach (SyntaxTrivia trivia in triviaList)
            {
                switch (trivia.Kind())
                {
                    case SyntaxKind.WhitespaceTrivia:
                        {
                            continue;
                        }
                    case SyntaxKind.EndOfLineTrivia:
                        {
                            result = true;
                            continue;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }

            return result;
        }

        public static Task<Document> RefactorAsync(
            Document document,
            BinaryExpressionSyntax binaryExpression,
            CancellationToken cancellationToken)
        {
            BinaryExpressionSyntax newBinaryExpression = BinaryExpression(
                binaryExpression.Kind(),
                binaryExpression.Left.WithTrailingTrivia(binaryExpression.OperatorToken.TrailingTrivia),
                Token(
                    binaryExpression.Right.GetLeadingTrivia(),
                    binaryExpression.OperatorToken.Kind(),
                    TriviaList(Space)),
                binaryExpression.Right.WithoutLeadingTrivia());

            newBinaryExpression = newBinaryExpression
                .WithTriviaFrom(binaryExpression)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(binaryExpression, newBinaryExpression, cancellationToken);
        }
    }
}
