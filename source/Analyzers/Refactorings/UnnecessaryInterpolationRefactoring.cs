// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UnnecessaryInterpolationRefactoring
    {
        public static void AnalyzeInterpolation(SyntaxNodeAnalysisContext context)
        {
            var interpolation = (InterpolationSyntax)context.Node;

            StringLiteralExpressionInfo stringLiteralInfo = SyntaxInfo.StringLiteralExpressionInfo(interpolation.Expression);

            if (!stringLiteralInfo.Success)
                return;

            if (!(interpolation.Parent is InterpolatedStringExpressionSyntax interpolatedString))
                return;

            if (interpolatedString.StringStartToken.ValueText.Contains("@") != stringLiteralInfo.IsVerbatim)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.UnnecessaryInterpolation, interpolation);
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
