// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal abstract class ToAssignmentWithConditionalExpression<TStatement> : ToAssignmentWithConditionalExpression
        where TStatement : StatementSyntax
    {
        protected ToAssignmentWithConditionalExpression(
            TStatement statement,
            IfStatementSyntax ifStatement,
            ExpressionSyntax whenTrue,
            ExpressionSyntax whenFalse) : base(ifStatement, whenTrue, whenFalse)
        {
            Statement = statement;
        }

        public TStatement Statement { get; }

        protected abstract TStatement CreateNewStatement();
    }
}
