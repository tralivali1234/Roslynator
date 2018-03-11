// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveUnnecessaryElseClauseRefactoring
    {
        public static bool IsFixable(ElseClauseSyntax elseClause)
        {
            if (elseClause.Statement?.IsKind(SyntaxKind.IfStatement) != false)
                return false;

            if (!(elseClause.Parent is IfStatementSyntax ifStatement))
                return false;

            if (!ifStatement.IsTopmostIf())
                return false;

            StatementSyntax statement = ifStatement.Statement;

            if (statement is BlockSyntax block)
                statement = block.Statements.LastOrDefault();

            return statement != null
                && CSharpFacts.IsJumpStatement(statement.Kind());
        }
    }
}