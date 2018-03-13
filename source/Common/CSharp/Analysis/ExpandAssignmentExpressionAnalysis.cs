// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis
{
    internal static class ExpandAssignmentExpressionAnalysis
    {
        public static bool CanRefactor(AssignmentExpressionSyntax assignmentExpression)
        {
            return SyntaxInfo.SimpleAssignmentExpressionInfo(assignmentExpression).Success;
        }
    }
}
