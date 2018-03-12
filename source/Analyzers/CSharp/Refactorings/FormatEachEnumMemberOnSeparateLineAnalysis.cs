// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    internal static class FormatEachEnumMemberOnSeparateLineAnalysis
    {
        public static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            SeparatedSyntaxList<EnumMemberDeclarationSyntax> members = enumDeclaration.Members;

            if (members.Count <= 1)
                return;

            int previousIndex = members[0].GetSpanStartLine();

            for (int i = 1; i < members.Count; i++)
            {
                if (members[i].GetSpanStartLine() == previousIndex)
                {
                    TextSpan span = TextSpan.FromBounds(
                        members.First().SpanStart,
                        members.Last().Span.End);

                    context.ReportDiagnostic(
                        DiagnosticDescriptors.FormatEachEnumMemberOnSeparateLine,
                        Location.Create(enumDeclaration.SyntaxTree, span));

                    return;
                }

                previousIndex = members[i].GetSpanEndLine();
            }
        }
    }
}
