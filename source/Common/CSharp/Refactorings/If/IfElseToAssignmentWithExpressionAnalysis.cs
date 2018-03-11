// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal class IfElseToAssignmentWithExpressionAnalysis : IfAnalysis
    {
        public IfElseToAssignmentWithExpressionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionStatementSyntax expressionStatement) : base(ifStatement)
        {
            ExpressionStatement = expressionStatement;
        }

        public override IfRefactoringKind Kind
        {
            get { return IfRefactoringKind.IfElseToAssignmentWithExpression; }
        }

        public override string Title
        {
            get { return "Replace if-else with assignment"; }
        }

        public ExpressionStatementSyntax ExpressionStatement { get; }
    }
}