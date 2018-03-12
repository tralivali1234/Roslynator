// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal sealed class IfElseToAssignmentWithConditionalExpressionAnalysis : ToAssignmentWithConditionalExpressionAnalysis
    {
        internal IfElseToAssignmentWithConditionalExpressionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionSyntax left,
            ExpressionSyntax right1,
            ExpressionSyntax right2) : base(ifStatement, right1, right2)
        {
            Left = left;
        }

        public ExpressionSyntax Left { get; }

        public override IfRefactoringKind Kind
        {
            get { return IfRefactoringKind.IfElseToAssignmentWithConditionalExpression; }
        }

    }
}
