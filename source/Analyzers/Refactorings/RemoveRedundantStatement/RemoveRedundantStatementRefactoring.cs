// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Refactorings.RemoveRedundantStatement
{
    internal static class RemoveRedundantStatementRefactoring
    {
        public static RemoveRedundantContinueStatementRefactoring ContinueStatement { get; } = new RemoveRedundantContinueStatementRefactoring();

        public static RemoveRedundantReturnStatementRefactoring ReturnStatement { get; } = new RemoveRedundantReturnStatementRefactoring();

        public static RemoveRedundantYieldBreakStatementRefactoring YieldBreakStatement { get; } = new RemoveRedundantYieldBreakStatementRefactoring();
    }
}
