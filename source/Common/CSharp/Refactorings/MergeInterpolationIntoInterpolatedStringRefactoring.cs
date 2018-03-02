// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class MergeInterpolationIntoInterpolatedStringRefactoring
    {
        public static bool CanRefactor(InterpolationSyntax interpolation)
        {
            StringLiteralExpressionInfo stringLiteralExpressionInfo = SyntaxInfo.StringLiteralExpressionInfo(interpolation.Expression);

            if (!stringLiteralExpressionInfo.Success)
                return false;

            if (!(interpolation.Parent is InterpolatedStringExpressionSyntax interpolatedString))
                return false;

            return interpolatedString.StringStartToken.ValueText.Contains("@") == stringLiteralExpressionInfo.IsVerbatim;
        }

        public static Task<Document> RefactorAsync(
            Document document,
            InterpolationSyntax interpolation,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var interpolatedString = (InterpolatedStringExpressionSyntax)interpolation.Parent;

            string s = interpolatedString.ToString();

            s = s.Substring(0, interpolation.Span.Start - interpolatedString.Span.Start)
                + StringUtility.DoubleBraces(SyntaxInfo.StringLiteralExpressionInfo(interpolation.Expression).InnerText)
                + s.Substring(interpolation.Span.End - interpolatedString.Span.Start);

            var newInterpolatedString = (InterpolatedStringExpressionSyntax)SyntaxFactory.ParseExpression(s)
                .WithTriviaFrom(interpolatedString);

            return document.ReplaceNodeAsync(interpolatedString, newInterpolatedString, cancellationToken);
        }
    }
}
