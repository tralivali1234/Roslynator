// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    //TODO: EmbeddedStatement
    internal static class EmbeddedStatementHelper
    {
        public static StatementSyntax GetEmbeddedStatement(IfStatementSyntax ifStatement)
        {
            return NullIfBlock(ifStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(ElseClauseSyntax elseClause, bool allowIfStatement = true)
        {
            StatementSyntax statement = elseClause.Statement;

            if (statement == null)
                return null;

            SyntaxKind kind = statement.Kind();

            if (kind == SyntaxKind.Block)
                return null;

            if (!allowIfStatement
                && kind == SyntaxKind.IfStatement)
            {
                return null;
            }

            return statement;
        }

        public static StatementSyntax GetEmbeddedStatement(CommonForEachStatementSyntax forEachStatement)
        {
            return NullIfBlock(forEachStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(ForStatementSyntax forStatement)
        {
            return NullIfBlock(forStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(UsingStatementSyntax usingStatement, bool allowUsingStatement = true)
        {
            StatementSyntax statement = usingStatement.Statement;

            if (statement == null)
                return null;

            SyntaxKind kind = statement.Kind();

            if (kind == SyntaxKind.Block)
                return null;

            if (!allowUsingStatement
                && kind == SyntaxKind.UsingStatement)
            {
                return null;
            }

            return statement;
        }

        public static StatementSyntax GetEmbeddedStatement(WhileStatementSyntax whileStatement)
        {
            return NullIfBlock(whileStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(DoStatementSyntax doStatement)
        {
            return NullIfBlock(doStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(LockStatementSyntax lockStatement)
        {
            return NullIfBlock(lockStatement.Statement);
        }

        public static StatementSyntax GetEmbeddedStatement(FixedStatementSyntax fixedStatement)
        {
            return NullIfBlock(fixedStatement.Statement);
        }

        private static StatementSyntax NullIfBlock(StatementSyntax statement)
        {
            return (statement?.Kind() == SyntaxKind.Block) ? null : statement;
        }

        public static StatementSyntax GetEmbeddedStatement(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.IfStatement:
                    return GetEmbeddedStatement((IfStatementSyntax)node);
                case SyntaxKind.ForEachStatement:
                case SyntaxKind.ForEachVariableStatement:
                    return GetEmbeddedStatement((CommonForEachStatementSyntax)node);
                case SyntaxKind.ForStatement:
                    return GetEmbeddedStatement((ForStatementSyntax)node);
                case SyntaxKind.WhileStatement:
                    return GetEmbeddedStatement((WhileStatementSyntax)node);
                case SyntaxKind.DoStatement:
                    return GetEmbeddedStatement((DoStatementSyntax)node);
                case SyntaxKind.LockStatement:
                    return GetEmbeddedStatement((LockStatementSyntax)node);
                case SyntaxKind.FixedStatement:
                    return GetEmbeddedStatement((FixedStatementSyntax)node);
                case SyntaxKind.UsingStatement:
                    return GetEmbeddedStatement((UsingStatementSyntax)node);
                case SyntaxKind.ElseClause:
                    return GetEmbeddedStatement((ElseClauseSyntax)node);
            }

            return null;
        }

        internal static bool FormattingSupportsEmbeddedStatement(IfStatementSyntax containingNode)
        {
            return containingNode.Condition?.IsMultiLine() != true;
        }

        internal static bool FormattingSupportsEmbeddedStatement(DoStatementSyntax doStatement)
        {
            return doStatement.Condition?.IsMultiLine() != true;
        }

        internal static bool FormattingSupportsEmbeddedStatement(CommonForEachStatementSyntax forEachStatement)
        {
            return forEachStatement.SyntaxTree.IsSingleLineSpan(forEachStatement.ParenthesesSpan());
        }

        internal static bool FormattingSupportsEmbeddedStatement(ForStatementSyntax forStatement)
        {
            return forStatement.Statement?.Kind() == SyntaxKind.EmptyStatement
                || forStatement.SyntaxTree.IsSingleLineSpan(forStatement.ParenthesesSpan());
        }

        internal static bool FormattingSupportsEmbeddedStatement(UsingStatementSyntax usingStatement)
        {
            return usingStatement.DeclarationOrExpression()?.IsMultiLine() != true;
        }

        internal static bool FormattingSupportsEmbeddedStatement(WhileStatementSyntax whileStatement)
        {
            return whileStatement.Condition?.IsMultiLine() != true
                || whileStatement.Statement?.Kind() == SyntaxKind.EmptyStatement;
        }

        internal static bool FormattingSupportsEmbeddedStatement(LockStatementSyntax lockStatement)
        {
            return lockStatement.Expression?.IsMultiLine() != true;
        }

        internal static bool FormattingSupportsEmbeddedStatement(FixedStatementSyntax fixedStatement)
        {
            return fixedStatement.Declaration?.IsMultiLine() != true;
        }
    }
}
