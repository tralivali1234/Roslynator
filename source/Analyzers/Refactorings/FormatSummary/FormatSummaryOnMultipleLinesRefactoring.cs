// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings.FormatSummary
{
    internal static class FormatSummaryOnMultipleLinesRefactoring
    {
        public static void AnalyzeSingleLineDocumentationCommentTrivia(SyntaxNodeAnalysisContext context)
        {
            var documentationComment = (DocumentationCommentTriviaSyntax)context.Node;

            XmlElementSyntax summaryElement = documentationComment.SummaryElement();

            if (summaryElement?.StartTag?.IsMissing == false
                && summaryElement.EndTag?.IsMissing == false
                && summaryElement.IsSingleLine(includeExteriorTrivia: false, trim: false))
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.FormatDocumentationSummaryOnMultipleLines,
                    summaryElement);
            }
        }
    }
}