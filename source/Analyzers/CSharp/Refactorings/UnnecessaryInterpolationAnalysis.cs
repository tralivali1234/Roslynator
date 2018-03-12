// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UnnecessaryInterpolationAnalysis
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
    }
}
