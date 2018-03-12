// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidLockingOnPubliclyAccessibleInstanceAnalysis
    {
        public static void AnalyzeLockStatement(SyntaxNodeAnalysisContext context)
        {
            var lockStatement = (LockStatementSyntax)context.Node;

            ExpressionSyntax expression = lockStatement.Expression;

            if (expression?.IsKind(SyntaxKind.ThisExpression, SyntaxKind.TypeOfExpression) != true)
                return;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(expression, context.CancellationToken);

            if (typeSymbol == null)
                return;

            if (!typeSymbol.DeclaredAccessibility.Is(
                Accessibility.Public,
                Accessibility.Protected,
                Accessibility.ProtectedOrInternal))
            {
                return;
            }

            context.ReportDiagnostic(
                DiagnosticDescriptors.AvoidLockingOnPubliclyAccessibleInstance,
                expression,
                expression.ToString());
        }
    }
}
