// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal sealed class IfToReturnWithExpressionAnalysis : IfAnalysis
    {
        public IfToReturnWithExpressionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionSyntax expression,
            bool isYield,
            bool negate = false) : base(ifStatement)
        {
            Expression = expression;
            Negate = negate;
            IsYield = isYield;
        }

        public ExpressionSyntax Expression { get; }

        public bool Negate { get; }

        public bool IsYield { get; }

        public override IfRefactoringKind Kind
        {
            get
            {
                if (IsYield)
                    return IfRefactoringKind.IfElseToYieldReturnWithExpression;

                return (IfStatement.IsSimpleIf())
                    ? IfRefactoringKind.IfReturnToReturnWithExpression
                    : IfRefactoringKind.IfElseToReturnWithExpression;
            }
        }

        public override string Title
        {
            get
            {
                if (IsYield)
                    return "Replace if-else with yield return";

                return (IfStatement.IsSimpleIf())
                    ? "Replace if-return with return"
                    : "Replace if-else with return";
            }
        }
    }
}