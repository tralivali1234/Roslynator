// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Syntax;
using static Roslynator.CSharp.CSharpFacts;

namespace Roslynator.CSharp.Refactorings.If
{
    internal static class IfRefactoring2
    {
        public static Task<Document> Refactor(Document document, IfRefactoring refactoring, CancellationToken cancellationToken)
        {
            switch (refactoring.Kind)
            {
                case IfRefactoringKind.IfElseToAssignmentWithCoalesceExpression:
                    break;
                case IfRefactoringKind.IfElseToAssignmentWithExpression:
                    break;
                case IfRefactoringKind.IfElseToAssignmentWithConditionalExpression:
                    break;
                case IfRefactoringKind.IfElseToReturnWithCoalesceExpression:
                    break;
                case IfRefactoringKind.IfElseToReturnWithConditionalExpression:
                    break;
                case IfRefactoringKind.IfElseToReturnWithBooleanExpression:
                    break;
                case IfRefactoringKind.IfElseToReturnWithExpression:
                    break;
                case IfRefactoringKind.IfElseToYieldReturnWithCoalesceExpression:
                    break;
                case IfRefactoringKind.IfElseToYieldReturnWithConditionalExpression:
                    break;
                case IfRefactoringKind.IfElseToYieldReturnWithBooleanExpression:
                    break;
                case IfRefactoringKind.IfElseToYieldReturnWithExpression:
                    break;
                case IfRefactoringKind.IfReturnToReturnWithCoalesceExpression:
                    break;
                case IfRefactoringKind.IfReturnToReturnWithConditionalExpression:
                    break;
                case IfRefactoringKind.IfReturnToReturnWithBooleanExpression:
                    break;
                case IfRefactoringKind.IfReturnToReturnWithExpression:
                    break;
                default:
                    break;
            }
        }
    }
}
