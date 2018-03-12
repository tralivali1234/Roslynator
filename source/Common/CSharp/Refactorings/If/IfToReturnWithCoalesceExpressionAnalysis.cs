// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal sealed class IfToReturnWithCoalesceExpressionAnalysis : IfAnalysis
    {
        public IfToReturnWithCoalesceExpressionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionSyntax left,
            ExpressionSyntax right,
            bool isYield) : base(ifStatement)
        {
            Left = left;
            Right = right;
            IsYield = isYield;
        }

        public ExpressionSyntax Left { get; }

        public ExpressionSyntax Right { get; }

        public bool IsYield { get; }

        public override IfRefactoringKind Kind
        {
            get
            {
                if (IsYield)
                    return IfRefactoringKind.IfElseToYieldReturnWithCoalesceExpression;

                return (IfStatement.IsSimpleIf())
                    ? IfRefactoringKind.IfReturnToReturnWithCoalesceExpression
                    : IfRefactoringKind.IfElseToReturnWithCoalesceExpression;
            }
        }

        public override string Title
        {
            get { return "Use coalesce expression"; }
        }
    }
}