﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(BlockCodeFixProvider))]
    [Shared]
    public class BlockCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticIdentifiers.SimplifyLazyInitialization,
                    DiagnosticIdentifiers.FormatSingleLineBlock);
            }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out BlockSyntax block))
                return;

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                switch (diagnostic.Id)
                {
                    case DiagnosticIdentifiers.SimplifyLazyInitialization:
                        {
                            CodeAction codeAction = CodeAction.Create(
                                "Simplify lazy initialization",
                                cancellationToken => SimplifyLazyInitializationRefactoring.RefactorAsync(context.Document, block, cancellationToken),
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                    case DiagnosticIdentifiers.FormatSingleLineBlock:
                        {
                            CodeAction codeAction = CodeAction.Create(
                                "Format block",
                                ct => FormatSingleLineBlockAsync(context.Document, block, ct),
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                }
            }
        }

        private static Task<Document> FormatSingleLineBlockAsync(
            Document document,
            BlockSyntax block,
            CancellationToken cancellationToken)
        {
            SyntaxToken closeBrace = block.CloseBraceToken;

            BlockSyntax newBlock = block
                .WithCloseBraceToken(closeBrace.WithLeadingTrivia(closeBrace.LeadingTrivia.Add(CSharpFactory.NewLine())))
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(block, newBlock, cancellationToken);
        }
    }
}