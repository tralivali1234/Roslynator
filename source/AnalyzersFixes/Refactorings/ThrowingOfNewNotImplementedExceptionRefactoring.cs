// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ThrowingOfNewNotImplementedExceptionRefactoring
    {
        public static void AnalyzeThrowStatement(SyntaxNodeAnalysisContext context, INamedTypeSymbol exceptionSymbol)
        {
            var throwStatement = (ThrowStatementSyntax)context.Node;

            Analyze(context, throwStatement.Expression, exceptionSymbol);
        }

        internal static void AnalyzeThrowExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol exceptionSymbol)
        {
            var throwExpression = (ThrowExpressionSyntax)context.Node;

            Analyze(context, throwExpression.Expression, exceptionSymbol);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, ExpressionSyntax expression, INamedTypeSymbol exceptionSymbol)
        {
            if (expression?.Kind() != SyntaxKind.ObjectCreationExpression)
                return;

            var objectCreationExpression = (ObjectCreationExpressionSyntax)expression;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(objectCreationExpression, context.CancellationToken);

            if (typeSymbol == null)
                return;

            if (!typeSymbol.Equals(exceptionSymbol))
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.ThrowingOfNewNotImplementedException,
                expression);
        }
    }
}
