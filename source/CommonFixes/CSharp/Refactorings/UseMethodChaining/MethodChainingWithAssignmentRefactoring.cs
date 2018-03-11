// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.UseMethodChaining
{
    internal class MethodChainingWithAssignmentRefactoring : UseMethodChainingRefactoring
    {
        protected override InvocationExpressionSyntax GetInvocationExpression(ExpressionStatementSyntax expressionStatement)
        {
            if (!(expressionStatement.Expression is AssignmentExpressionSyntax assignmentExpression))
                return null;

            if (assignmentExpression.Kind() != SyntaxKind.SimpleAssignmentExpression)
                return null;

            return assignmentExpression.Right as InvocationExpressionSyntax;
        }
    }
}