// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseEventArgsEmptyAnalysis
    {
        public static void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol eventArgsSymbol)
        {
            if (context.Node.SpanContainsDirectives())
                return;

            var objectCreation = (ObjectCreationExpressionSyntax)context.Node;

            if (objectCreation.ArgumentList?.Arguments.Count == 0
                && objectCreation.Initializer == null)
            {
                ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(objectCreation, context.CancellationToken);

                if (typeSymbol?.Equals(eventArgsSymbol) == true)
                    context.ReportDiagnostic(DiagnosticDescriptors.UseEventArgsEmpty, objectCreation);
            }
        }
    }
}
