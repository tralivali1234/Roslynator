// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal sealed class IfElseToYieldReturnWithBooleanExpressionAnalysis : IfToReturnWithBooleanExpressionAnalysis
    {
        public IfElseToYieldReturnWithBooleanExpressionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionSyntax expression1,
            ExpressionSyntax expression2) : base(ifStatement, expression1, expression2)
        {
        }

        public override IfRefactoringKind Kind
        {
            get { return IfRefactoringKind.IfElseToYieldReturnWithBooleanExpression; }
        }

        public override string Title
        {
            get { return "Simplify if-else"; }
        }
    }
}