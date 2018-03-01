// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    //TODO: test
    internal static class SimplifyLambdaExpressionRefactoring
    {
        public static bool CanRefactor(LambdaExpressionSyntax lambda)
        {
            CSharpSyntaxNode body = lambda.Body;

            if (body?.Kind() != SyntaxKind.Block)
                return false;

            var block = (BlockSyntax)body;

            StatementSyntax statement = block.Statements.SingleOrDefault(shouldThrow: false);

            if (statement == null)
                return false;

            ExpressionSyntax expression = GetExpression(statement);

            return expression?.IsSingleLine() == true
                && lambda
                    .DescendantTrivia(TextSpan.FromBounds(lambda.ArrowToken.Span.End, expression.Span.Start))
                    .All(f => f.IsWhitespaceOrEndOfLineTrivia())
                && lambda
                    .DescendantTrivia(TextSpan.FromBounds(expression.Span.End, block.Span.End))
                    .All(f => f.IsWhitespaceOrEndOfLineTrivia());
        }

        public static Task<Document> RefactorAsync(
            Document document,
            LambdaExpressionSyntax lambda,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var block = (BlockSyntax)lambda.Body;
            StatementSyntax statement = block.Statements[0];
            ExpressionSyntax expression = GetNewExpression(statement).WithoutTrivia();

            LambdaExpressionSyntax newLambda = GetNewLambda()
                .WithTriviaFrom(lambda)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(lambda, newLambda, cancellationToken);

            LambdaExpressionSyntax GetNewLambda()
            {
                switch (lambda.Kind())
                {
                    case SyntaxKind.SimpleLambdaExpression:
                        {
                            return ((SimpleLambdaExpressionSyntax)lambda)
                                .WithArrowToken(lambda.ArrowToken.WithoutTrailingTrivia())
                                .WithBody(expression);
                        }
                    case SyntaxKind.ParenthesizedLambdaExpression:
                        {
                            return ((ParenthesizedLambdaExpressionSyntax)lambda)
                                .WithArrowToken(lambda.ArrowToken.WithoutTrailingTrivia())
                                .WithBody(expression);
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
            }
        }

        private static ExpressionSyntax GetExpression(StatementSyntax statement)
        {
            switch (statement.Kind())
            {
                case SyntaxKind.ReturnStatement:
                    return ((ReturnStatementSyntax)statement).Expression;
                case SyntaxKind.ExpressionStatement:
                    return ((ExpressionStatementSyntax)statement).Expression;
                case SyntaxKind.ThrowStatement:
                    return ((ThrowStatementSyntax)statement).Expression;
            }

            return null;
        }

        private static ExpressionSyntax GetNewExpression(StatementSyntax statement)
        {
            switch (statement.Kind())
            {
                case SyntaxKind.ReturnStatement:
                    return ((ReturnStatementSyntax)statement).Expression;
                case SyntaxKind.ExpressionStatement:
                    return ((ExpressionStatementSyntax)statement).Expression;
                case SyntaxKind.ThrowStatement:
                    return SyntaxFactory.ThrowExpression(((ThrowStatementSyntax)statement).Expression);
            }

            return null;
        }
    }
}
