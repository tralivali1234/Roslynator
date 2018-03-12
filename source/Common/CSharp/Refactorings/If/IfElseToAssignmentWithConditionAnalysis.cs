// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal sealed class IfElseToAssignmentWithConditionAnalysis : IfAnalysis
    {
        public IfElseToAssignmentWithConditionAnalysis(
            IfStatementSyntax ifStatement,
            ExpressionSyntax left,
            ExpressionSyntax right,
            bool negate) : base(ifStatement)
        {
            Left = left;
            Right = right;
            Negate = negate;
        }

        public override IfRefactoringKind Kind
        {
            get { return IfRefactoringKind.IfElseToAssignmentWithCondition; }
        }

        public override string Title
        {
            get { return "Replace if-else with assignment"; }
        }

        public ExpressionSyntax Left { get; }

        public ExpressionSyntax Right { get; }

        public bool Negate { get; }
    }
}