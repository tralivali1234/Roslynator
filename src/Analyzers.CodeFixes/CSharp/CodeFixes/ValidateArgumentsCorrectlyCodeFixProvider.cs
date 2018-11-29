﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ValidateArgumentsCorrectlyCodeFixProvider))]
    [Shared]
    public class ValidateArgumentsCorrectlyCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.ValidateArgumentsCorrectly); }
        }

        public override FixAllProvider GetFixAllProvider()
        {
            return null;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out StatementSyntax statement))
                return;

            Diagnostic diagnostic = context.Diagnostics[0];

            CodeAction codeAction = CodeAction.Create(
                "Validate arguments correctly",
                cancellationToken => RefactorAsync(context.Document, statement, cancellationToken),
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private static async Task<Document> RefactorAsync(
            Document document,
            StatementSyntax statement,
            CancellationToken cancellationToken)
        {
            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(statement);

            var methodDeclaration = (MethodDeclarationSyntax)statementsInfo.Parent.Parent;

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            string name = methodDeclaration.Identifier.ValueText;

            if (!name.EndsWith("Iterator", StringComparison.Ordinal))
                name += "Iterator";

            name = NameGenerator.Default.EnsureUniqueLocalName(name, semanticModel, statement.SpanStart, cancellationToken: cancellationToken);

            SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

            int index = statements.IndexOf(statement);

            List<StatementSyntax> localFunctionStatements = statements.Skip(index).ToList();

            int lastIndex = localFunctionStatements.Count - 1;

            localFunctionStatements[lastIndex] = localFunctionStatements[lastIndex].WithoutTrailingTrivia();

            LocalFunctionStatementSyntax localFunction = LocalFunctionStatement(
                default(SyntaxTokenList),
                methodDeclaration.ReturnType.WithoutTrivia(),
                Identifier(name).WithRenameAnnotation(),
                ParameterList(),
                Block(localFunctionStatements).WithTrailingTrivia(statements.Last().GetTrailingTrivia()));

            localFunction = localFunction.WithFormatterAnnotation();

            ReturnStatementSyntax returnStatement = ReturnStatement(
                Token(SyntaxKind.ReturnKeyword).WithLeadingTrivia(statement.GetLeadingTrivia()),
                InvocationExpression(IdentifierName(name)),
                SemicolonToken());

            SyntaxList<StatementSyntax> newStatements = statements.ReplaceRange(
                index,
                statements.Count - index,
                new StatementSyntax[] { returnStatement, localFunction });

            return await document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken).ConfigureAwait(false);
        }
    }
}
