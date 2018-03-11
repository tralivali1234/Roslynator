// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class FormatEachEnumMemberOnSeparateLineRefactoring
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

        public static Task<Document> RefactorAsync(
            Document document,
            EnumDeclarationSyntax enumDeclaration,
            CancellationToken cancellationToken)
        {
            var rewriter = new SyntaxRewriter(enumDeclaration);

            SyntaxNode newNode = rewriter.Visit(enumDeclaration)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(enumDeclaration, newNode, cancellationToken);
        }

        private class SyntaxRewriter : CSharpSyntaxRewriter
        {
            private readonly SyntaxToken[] _separators;

            public SyntaxRewriter(EnumDeclarationSyntax enumDeclaration)
            {
                _separators = enumDeclaration.Members.GetSeparators().ToArray();
            }

            public override SyntaxToken VisitToken(SyntaxToken token)
            {
                if (_separators.Contains(token)
                    && !token.TrailingTrivia.Contains(SyntaxKind.EndOfLineTrivia))
                {
                    return token.TrimTrailingTrivia().AppendToTrailingTrivia(CSharpFactory.NewLine());
                }

                return base.VisitToken(token);
            }
        }
    }
}
