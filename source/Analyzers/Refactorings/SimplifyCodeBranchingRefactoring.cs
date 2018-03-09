// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
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
    internal static class SimplifyCodeBranchingRefactoring
    {
        internal static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.Condition?.WalkDownParentheses().IsMissing != false)
                return;

            StatementSyntax statement = ifStatement.Statement;

            if (statement == null)
                return;

            if ((statement as BlockSyntax)?.Statements.Any() == false)
            {
                if (IsFixableIfElse(ifStatement))
                    context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCodeBranching, ifStatement);
            }
            else
            {
                ElseClauseSyntax elseClause = ifStatement.Else;

                if (elseClause != null)
                {
                    if (IsFixableIfElseInsideWhile(ifStatement, elseClause))
                        context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCodeBranching, ifStatement);
                }
                else if (IsFixableIfInsideWhileOrDo(ifStatement))
                {
                    context.ReportDiagnostic(DiagnosticDescriptors.SimplifyCodeBranching, ifStatement);
                }
            }
        }

        private static bool IsFixableIfElse(IfStatementSyntax ifStatement)
        {
            if (ifStatement.SpanContainsDirectives())
                return false;

            ElseClauseSyntax elseClause = ifStatement.Else;

            if (elseClause == null)
                return false;

            StatementSyntax whenFalse = elseClause.Statement;

            if (whenFalse == null)
                return false;

            SyntaxKind kind = whenFalse.Kind();

            if (kind == SyntaxKind.IfStatement)
            {
                var nestedIf = (IfStatementSyntax)whenFalse;

                if (nestedIf.Condition?.WalkDownParentheses().IsMissing != false)
                    return false;

                StatementSyntax statement = nestedIf.Statement;

                if (statement == null)
                    return false;

                if ((statement as BlockSyntax)?.Statements.Any() == false)
                    return false;
            }
            else if (kind == SyntaxKind.Block)
            {
                if (!((BlockSyntax)whenFalse).Statements.Any())
                    return false;
            }

            return true;
        }

        private static bool IsFixableIfElseInsideWhile(
            IfStatementSyntax ifStatement,
            ElseClauseSyntax elseClause)
        {
            if (elseClause.SingleNonBlockStatementOrDefault()?.Kind() != SyntaxKind.BreakStatement)
                return false;

            SyntaxNode parent = ifStatement.Parent;

            if (parent is BlockSyntax block)
            {
                if (block.Statements.Count != 1)
                    return false;

                parent = block.Parent;
            }

            if (!(parent is WhileStatementSyntax whileStatement))
                return false;

            if (whileStatement.SpanContainsDirectives())
                return false;

            if (whileStatement.Condition?.WalkDownParentheses().Kind() != SyntaxKind.TrueLiteralExpression)
                return false;

            return true;
        }

        private static bool IsFixableIfInsideWhileOrDo(IfStatementSyntax ifStatement)
        {
            SyntaxNode parent = ifStatement.Parent;

            if (parent.Kind() != SyntaxKind.Block)
                return false;

            if (ifStatement.SingleNonBlockStatementOrDefault()?.Kind() != SyntaxKind.BreakStatement)
                return false;

            var block = (BlockSyntax)parent;

            SyntaxList<StatementSyntax> statements = block.Statements;

            int count = statements.Count;

            if (count == 1)
                return false;

            int index = statements.IndexOf(ifStatement);

            if (index != 0
                && index != count - 1)
            {
                return false;
            }

            parent = block.Parent;

            SyntaxKind kind = parent.Kind();

            if (kind == SyntaxKind.WhileStatement)
            {
                var whileStatement = (WhileStatementSyntax)parent;

                if (whileStatement.SpanContainsDirectives())
                    return false;

                if (whileStatement.Condition?.WalkDownParentheses().Kind() != SyntaxKind.TrueLiteralExpression)
                    return false;
            }
            else if (kind == SyntaxKind.DoStatement)
            {
                var doStatement = (DoStatementSyntax)parent;

                if (doStatement.SpanContainsDirectives())
                    return false;

                if (doStatement.Condition?.WalkDownParentheses().Kind() != SyntaxKind.TrueLiteralExpression)
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            IfStatementSyntax ifStatement,
            CancellationToken cancellationToken)
        {
            ExpressionSyntax condition = ifStatement.Condition;

            ElseClauseSyntax elseClause = ifStatement.Else;

            if ((ifStatement.Statement as BlockSyntax)?.Statements.Any() == false)
            {
                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                ExpressionSyntax newCondition = Negation.LogicallyNegate(condition, semanticModel, cancellationToken);

                StatementSyntax statement = elseClause.Statement;

                if (statement is IfStatementSyntax nestedIf)
                {
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
            else if (elseClause != null)
            {
                WhileStatementSyntax whileStatement = null;

                SyntaxNode newNode = null;

                if (ifStatement.Parent is BlockSyntax block)
                {
                    whileStatement = (WhileStatementSyntax)block.Parent;
                }
                else
                {
                    block = Block();
                    whileStatement = (WhileStatementSyntax)ifStatement.Parent;
                }

                cancellationToken.ThrowIfCancellationRequested();

                BlockSyntax newBlock = (ifStatement.Statement is BlockSyntax ifBlock)
                    ? block.WithStatements(ifBlock.Statements)
                    : block.WithStatements(SingletonList(ifStatement.Statement));

                newNode = whileStatement.Update(
                    whileStatement.WhileKeyword,
                    whileStatement.OpenParenToken,
                    ifStatement.Condition,
                    whileStatement.CloseParenToken,
                    newBlock);

                newNode = newNode.WithFormatterAnnotation();

                return await document.ReplaceNodeAsync(whileStatement, newNode, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var block = (BlockSyntax)ifStatement.Parent;

                SyntaxList<StatementSyntax> statements = block.Statements;

                BlockSyntax newBlock = block.WithStatements(statements.Remove(ifStatement));

                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                ExpressionSyntax newCondition = Negation.LogicallyNegate(condition, semanticModel, cancellationToken);

                SyntaxNode newNode = block.Parent;

                switch (block.Parent)
                {
                    case WhileStatementSyntax whileStatement:
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (statements.IsFirst(ifStatement))
                            {
                                newNode = whileStatement.Update(
                                    whileStatement.WhileKeyword,
                                    whileStatement.OpenParenToken,
                                    newCondition,
                                    whileStatement.CloseParenToken,
                                    newBlock);
                            }
                            else
                            {
                                newNode = DoStatement(
                                    Token(whileStatement.WhileKeyword.LeadingTrivia, SyntaxKind.DoKeyword, whileStatement.CloseParenToken.TrailingTrivia),
                                    newBlock.WithoutTrailingTrivia(),
                                    WhileKeyword(),
                                    OpenParenToken(),
                                    newCondition,
                                    CloseParenToken(),
                                    SemicolonToken().WithTrailingTrivia(newBlock.GetTrailingTrivia()));
                            }

                            break;
                        }
                    case DoStatementSyntax doStatement:
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (statements.IsLast(ifStatement))
                            {
                                newNode = doStatement.Update(
                                    doStatement.DoKeyword,
                                    newBlock,
                                    doStatement.WhileKeyword,
                                    doStatement.OpenParenToken,
                                    newCondition,
                                    doStatement.CloseParenToken,
                                    doStatement.SemicolonToken);
                            }
                            else
                            {
                                newNode = WhileStatement(
                                    Token(doStatement.DoKeyword.LeadingTrivia, SyntaxKind.WhileKeyword, SyntaxTriviaList.Empty),
                                    OpenParenToken(),
                                    newCondition,
                                    Token(SyntaxTriviaList.Empty, SyntaxKind.CloseParenToken, doStatement.DoKeyword.TrailingTrivia),
                                    newBlock.WithTrailingTrivia(doStatement.GetTrailingTrivia()));
                            }

                            break;
                        }
                    default:
                        {
                            Debug.Fail(block.Parent.Kind().ToString());
                            break;
                        }
                }

                newNode = newNode.WithFormatterAnnotation();

                return await document.ReplaceNodeAsync(block.Parent, newNode, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
