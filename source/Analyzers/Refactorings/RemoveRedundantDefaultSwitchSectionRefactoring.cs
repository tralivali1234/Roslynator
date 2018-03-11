// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantDefaultSwitchSectionRefactoring
    {
        internal static void AnalyzeSwitchStatement(SyntaxNodeAnalysisContext context)
        {
            var switchStatement = (SwitchStatementSyntax)context.Node;

            if (switchStatement.ContainsDiagnostics)
                return;

            SwitchSectionSyntax defaultSection = switchStatement.DefaultSection();

            if (defaultSection == null)
                return;

            if (!ContainsOnlyBreakStatement(defaultSection))
                return;

            if (switchStatement.DescendantNodes(switchStatement.Sections.Span).Any(f => f.IsKind(SyntaxKind.GotoDefaultStatement)))
                return;

            if (!defaultSection
                .DescendantTrivia(defaultSection.Span)
                .All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            {
                return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveRedundantDefaultSwitchSection, defaultSection);
        }

        private static bool ContainsOnlyBreakStatement(SwitchSectionSyntax switchSection)
        {
            StatementSyntax statement = switchSection.Statements.SingleOrDefault(shouldThrow: false);

            switch (statement?.Kind())
            {
                case SyntaxKind.Block:
                    {
                        return ((BlockSyntax)statement)
                            .Statements
                            .SingleOrDefault(shouldThrow: false)?
                            .Kind() == SyntaxKind.BreakStatement;
                    }
                case SyntaxKind.BreakStatement:
                    {
                        return true;
                    }
            }

            return false;
        }

        private static SwitchStatementSyntax GetNewSwitchStatement(SwitchSectionSyntax switchSection, SwitchStatementSyntax switchStatement)
        {
            if (switchSection.GetLeadingTrivia().IsEmptyOrWhitespace())
            {
                int index = switchStatement.Sections.IndexOf(switchSection);

                if (index > 0)
                {
                    SwitchSectionSyntax previousSection = switchStatement.Sections[index - 1];

                    if (previousSection.GetTrailingTrivia().IsEmptyOrWhitespace())
                    {
                        SwitchStatementSyntax newSwitchStatement = switchStatement.RemoveNode(
                            switchSection,
                            SyntaxRemoveOptions.KeepNoTrivia);

                        previousSection = newSwitchStatement.Sections[index - 1];

                        return newSwitchStatement.ReplaceNode(
                            previousSection,
                            previousSection.WithTrailingTrivia(switchSection.GetTrailingTrivia()));
                    }
                }
                else
                {
                    SyntaxToken openBrace = switchStatement.OpenBraceToken;

                    if (!openBrace.IsMissing
                        && openBrace.TrailingTrivia.IsEmptyOrWhitespace())
                    {
                        return switchStatement
                            .RemoveNode(switchSection, SyntaxRemoveOptions.KeepNoTrivia)
                            .WithOpenBraceToken(openBrace.WithTrailingTrivia(switchSection.GetTrailingTrivia()));
                    }
                }
            }

            return switchStatement.RemoveNode(switchSection, SyntaxRemoveOptions.KeepExteriorTrivia);
        }
    }
}
