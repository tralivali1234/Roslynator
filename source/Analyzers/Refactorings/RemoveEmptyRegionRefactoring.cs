// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    //XTODO: test
    internal static class RemoveEmptyRegionRefactoring
    {
        public static void AnalyzeRegionDirective(SyntaxNodeAnalysisContext context)
        {
            var regionDirective = (RegionDirectiveTriviaSyntax)context.Node;

            RegionInfo info = SyntaxInfo.RegionInfo(regionDirective);

            if (!info.Success)
                return;

            if (!info.IsEmpty)
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.RemoveEmptyRegion,
                regionDirective.GetLocation(),
                additionalLocations: ImmutableArray.Create(info.EndRegionDirective.GetLocation()));

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyRegionFadeOut, regionDirective.GetLocation());
            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyRegionFadeOut, info.EndRegionDirective.GetLocation());
        }

        public static Task<Document> RefactorAsync(
            Document document,
            RegionDirectiveTriviaSyntax regionDirective,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return document.RemoveRegionAsync(regionDirective, cancellationToken);
        }
    }
}
