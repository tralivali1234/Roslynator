// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class SimplifyConditionRefactoring
    {
        internal static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.SpanContainsDirectives())
                return;

            if (ifStatement.Condition?.IsMissing != false)
                return;

            ElseClauseSyntax elseClause = ifStatement.Else;

            if (elseClause != null)
            {
                AnalyzeIfElse(context, ifStatement, elseClause);
            }
            else
            {
                AnalyzeIfInsideWhileOrDo(context, ifStatement);
            }
        }

        private static void AnalyzeIfElse(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement, ElseClauseSyntax elseClause)
        {
            if (!(ifStatement.Statement is BlockSyntax block))
                return;

            if (block.Statements.Any())
                return;

            StatementSyntax whenFalse = elseClause.Statement;

            if (whenFalse == null)
                return;

            SyntaxKind kind = whenFalse.Kind();

            if (kind == SyntaxKind.IfStatement)
            {
                var nestedIf = (IfStatementSyntax)whenFalse;

                if (nestedIf.Condition?.IsMissing != false)
                    return;

                StatementSyntax statement = nestedIf.Statement;

                if (statement == null)
                    return;

                if ((statement as BlockSyntax)?.Statements.Any() == false)
                    return;
            }
            else if (kind == SyntaxKind.Block)
            {
                if (!((BlockSyntax)whenFalse).Statements.Any())
                    return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCondition, ifStatement);
        }

        private static void AnalyzeIfInsideWhileOrDo(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement)
        {
            if (!(ifStatement.Parent is BlockSyntax block))
                return;

            SyntaxList<StatementSyntax> statements = block.Statements;

            if (statements.Count == 1)
                return;

            if (!statements.IsLast(ifStatement))
                return;

            if (ifStatement.SingleNonBlockStatementOrDefault()?.Kind() != SyntaxKind.BreakStatement)
                return;

            SyntaxNode parent = block.Parent;
            SyntaxKind kind = parent.Kind();

            if (kind == SyntaxKind.WhileStatement)
            {
                if (((WhileStatementSyntax)parent).Condition?.IsMissing != false)
                    return;
            }
            else if (kind == SyntaxKind.DoStatement)
            {
                if (((DoStatementSyntax)parent).Condition?.IsMissing != false)
                    return;
            }
            else
            {
                return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCondition, ifStatement);
        }

        internal static void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
        {
            var whileStatement = (WhileStatementSyntax)context.Node;

            if (whileStatement.SpanContainsDirectives())
                return;

            if (whileStatement.Condition?.WalkDownParentheses().Kind() != SyntaxKind.TrueLiteralExpression)
                return;

            StatementSyntax statement = whileStatement.Statement;

            if (statement == null)
                return;

            SyntaxKind kind = statement.Kind();

            IfStatementSyntax ifStatement = null;

            if (kind == SyntaxKind.IfStatement)
            {
                ifStatement = (IfStatementSyntax)statement;
            }
            else if (kind == SyntaxKind.Block)
            {
                var block = (BlockSyntax)statement;

                ifStatement = block.Statements.SingleOrDefault(shouldThrow: false) as IfStatementSyntax;
            }

            if (ifStatement?.Condition?.IsMissing != false)
                return;

            if (ifStatement.Else != null)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCondition, whileStatement);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            IfStatementSyntax ifStatement,
            CancellationToken cancellationToken)
        {
            ExpressionSyntax condition = ifStatement.Condition;

            ElseClauseSyntax elseClause = ifStatement.Else;

            StatementSyntax statement = elseClause.Statement;

            if (elseClause != null)
            {
                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                ExpressionSyntax newCondition = Negation.LogicallyNegate(condition, semanticModel, cancellationToken);

                if (statement.Kind() == SyntaxKind.IfStatement)
                {
                    var nestedIf = (IfStatementSyntax)statement;

                    newCondition = LogicalAndExpression(newCondition.Parenthesize(), nestedIf.Condition.Parenthesize());

                    statement = nestedIf.Statement;
                }

                cancellationToken.ThrowIfCancellationRequested();

                IfStatementSyntax newNode = ifStatement.Update(
                    ifStatement.IfKeyword,
                    ifStatement.OpenParenToken,
                    newCondition,
                    ifStatement.CloseParenToken,
                    statement,
                    default(ElseClauseSyntax));

                newNode = newNode.WithFormatterAnnotation();

                return await document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var block = (BlockSyntax)ifStatement.Parent;

                SyntaxList<StatementSyntax> statements = block.Statements;

                BlockSyntax newBlock = block.WithStatements(statements.RemoveAt(statements.Count - 1));

                SyntaxNode newNode = block.Parent;

                switch (block.Parent)
                {
                    case WhileStatementSyntax whileStatement:
                        {
                            SyntaxToken whileKeyword = whileStatement.WhileKeyword;

                            newNode = DoStatement(
                                Token(whileKeyword.LeadingTrivia, SyntaxKind.DoKeyword, whileKeyword.TrailingTrivia),
                                newBlock,
                                WhileKeyword(),
                                OpenParenToken(),
                                ifStatement.Condition,
                                CloseParenToken(),
                                SemicolonToken());

                            break;
                        }
                    case DoStatementSyntax doStatement:
                        {
                            newNode = doStatement.Update(
                                doStatement.DoKeyword,
                                newBlock,
                                doStatement.WhileKeyword,
                                doStatement.OpenParenToken,
                                ifStatement.Condition,
                                doStatement.CloseParenToken,
                                doStatement.SemicolonToken);

                            break;
                        }
                    default:
                        {
                            Debug.Fail(block.Parent.Kind().ToString());
                            break;
                        }
                }

                return await document.ReplaceNodeAsync(block.Parent, newNode, cancellationToken).ConfigureAwait(false);
            }
        }

        public static Task<Document> RefactorAsync(
            Document document,
            WhileStatementSyntax whileStatement,
            CancellationToken cancellationToken)
        {
            StatementSyntax statement = whileStatement.Statement;

            ExpressionSyntax condition = whileStatement.Condition;

            IfStatementSyntax ifStatement = null;

            if (statement is BlockSyntax block)
            {
                ifStatement = (IfStatementSyntax)block.Statements.SingleOrDefault();
            }
            else
            {
                ifStatement = (IfStatementSyntax)statement;
            }

            cancellationToken.ThrowIfCancellationRequested();

            WhileStatementSyntax newNode = whileStatement.Update(
                whileStatement.WhileKeyword,
                whileStatement.OpenParenToken,
                ifStatement.Condition,
                whileStatement.CloseParenToken,
                ifStatement.Statement);

            newNode = newNode.WithFormatterAnnotation();

            return document.ReplaceNodeAsync(whileStatement, newNode, cancellationToken);
        }
    }
}
