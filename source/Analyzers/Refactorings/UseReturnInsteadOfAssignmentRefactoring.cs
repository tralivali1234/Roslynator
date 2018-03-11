// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseReturnInsteadOfAssignmentRefactoring
    {
        public static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.ContainsDiagnostics)
                return;

            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.IsSimpleIf())
                return;

            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(ifStatement);
            if (!statementsInfo.Success)
                return;

            int index = statementsInfo.IndexOf(ifStatement);

            ReturnStatementSyntax returnStatement = FindReturnStatementBelow(statementsInfo.Statements, index);

            if (returnStatement == null)
                return;

            if (returnStatement.ContainsDiagnostics)
                return;

            ExpressionSyntax expression = returnStatement.Expression;

            if (expression == null)
                return;

            if (ifStatement.SpanOrTrailingTriviaContainsDirectives())
                return;

            if (returnStatement.SpanOrLeadingTriviaContainsDirectives())
                return;

            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            ISymbol symbol = semanticModel.GetSymbol(expression, cancellationToken);

            if (!IsLocalDeclaredInScopeOrNonRefOrOutParameterOfEnclosingSymbol(symbol, statementsInfo.Parent, semanticModel, cancellationToken))
                return;

            foreach (IfStatementOrElseClause ifOrElse in SyntaxInfo.IfStatementInfo(ifStatement))
            {
                if (!IsSymbolAssignedInLastStatement(ifOrElse, symbol, semanticModel, cancellationToken))
                    return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.UseReturnInsteadOfAssignment, ifStatement);
        }

        public static void AnalyzeSwitchStatement(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.ContainsDiagnostics)
                return;

            var switchStatement = (SwitchStatementSyntax)context.Node;

            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(switchStatement);

            if (!statementsInfo.Success)
                return;

            int index = statementsInfo.IndexOf(switchStatement);

            ReturnStatementSyntax returnStatement = FindReturnStatementBelow(statementsInfo.Statements, index);

            if (returnStatement == null)
                return;

            ExpressionSyntax expression = returnStatement.Expression;

            if (expression == null)
                return;

            if (expression.ContainsDiagnostics)
                return;

            if (switchStatement.SpanOrTrailingTriviaContainsDirectives())
                return;

            if (returnStatement.SpanOrLeadingTriviaContainsDirectives())
                return;

            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            ISymbol symbol = semanticModel.GetSymbol(expression, cancellationToken);

            if (!IsLocalDeclaredInScopeOrNonRefOrOutParameterOfEnclosingSymbol(symbol, statementsInfo.Parent, semanticModel, cancellationToken))
                return;

            if (!switchStatement
                .Sections
                .All(section => IsValueAssignedInLastStatement(section, symbol, semanticModel, cancellationToken)))
            {
                return;
            }

            context.ReportDiagnostic(DiagnosticDescriptors.UseReturnInsteadOfAssignment, switchStatement);
        }

        private static ReturnStatementSyntax FindReturnStatementBelow(SyntaxList<StatementSyntax> statements, int i)
        {
            int count = statements.Count;

            i++;

            while (i <= count - 1)
            {
                if (!statements[i].IsKind(SyntaxKind.LocalFunctionStatement))
                    break;

                i++;
            }

            if (i <= count - 1
                && statements[i].IsKind(SyntaxKind.ReturnStatement))
            {
                return (ReturnStatementSyntax)statements[i];
            }

            return null;
        }

        private static bool IsLocalDeclaredInScopeOrNonRefOrOutParameterOfEnclosingSymbol(ISymbol symbol, SyntaxNode containingNode, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            switch (symbol?.Kind)
            {
                case SymbolKind.Local:
                    {
                        var localSymbol = (ILocalSymbol)symbol;

                        var localDeclarationStatement = localSymbol.GetSyntax(cancellationToken).Parent.Parent as LocalDeclarationStatementSyntax;

                        return localDeclarationStatement?.Parent == containingNode;
                    }
                case SymbolKind.Parameter:
                    {
                        var parameterSymbol = (IParameterSymbol)symbol;

                        if (parameterSymbol.RefKind == RefKind.None)
                        {
                            ISymbol enclosingSymbol = semanticModel.GetEnclosingSymbol(containingNode.SpanStart, cancellationToken);

                            if (enclosingSymbol != null)
                            {
                                ImmutableArray<IParameterSymbol> parameters = enclosingSymbol.ParametersOrDefault();

                                return !parameters.IsDefault
                                    && parameters.Contains(parameterSymbol);
                            }
                        }

                        break;
                    }
            }

            return false;
        }

        private static bool IsSymbolAssignedInLastStatement(IfStatementOrElseClause ifOrElse, ISymbol symbol, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            StatementSyntax statement = GetLastStatementOrDefault(ifOrElse.Statement);

            return IsSymbolAssignedInStatement(symbol, statement, semanticModel, cancellationToken);
        }

        private static bool IsValueAssignedInLastStatement(SwitchSectionSyntax switchSection, ISymbol symbol, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            StatementSyntax statement = GetLastStatementBeforeBreakStatementOrDefault(switchSection);

            return IsSymbolAssignedInStatement(symbol, statement, semanticModel, cancellationToken);
        }

        private static bool IsSymbolAssignedInStatement(ISymbol symbol, StatementSyntax statement, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            SimpleAssignmentStatementInfo assignmentInfo = SyntaxInfo.SimpleAssignmentStatementInfo(statement);

            return assignmentInfo.Success
                && symbol.Equals(semanticModel.GetSymbol(assignmentInfo.Left, cancellationToken));
        }

        private static StatementSyntax GetLastStatementOrDefault(StatementSyntax statement)
        {
            if (statement?.Kind() == SyntaxKind.Block)
            {
                return ((BlockSyntax)statement).Statements.LastOrDefault();
            }
            else
            {
                return statement;
            }
        }

        private static StatementSyntax GetLastStatementBeforeBreakStatementOrDefault(SwitchSectionSyntax switchSection)
        {
            SyntaxList<StatementSyntax> statements = GetStatements(switchSection);

            if (statements.Count > 1
                && statements.Last().IsKind(SyntaxKind.BreakStatement))
            {
                return statements[statements.Count - 2];
            }

            return null;
        }

        private static SyntaxList<StatementSyntax> GetStatements(SwitchSectionSyntax switchSection)
        {
            SyntaxList<StatementSyntax> statements = switchSection.Statements;

            if (statements.Count == 1
                && statements[0].IsKind(SyntaxKind.Block))
            {
                return ((BlockSyntax)statements[0]).Statements;
            }

            return statements;
        }
    }
}
