// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddArgumentListToObjectCreationAnalysis
    {
        public static void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            TypeSyntax type = objectCreationExpression.Type;

            if (type?.IsMissing != false)
                return;

            InitializerExpressionSyntax initializer = objectCreationExpression.Initializer;

            if (initializer?.IsMissing != false)
                return;

            ArgumentListSyntax argumentList = objectCreationExpression.ArgumentList;

            if (argumentList != null)
                return;

            var span = new TextSpan(type.Span.End, 1);

            context.ReportDiagnostic(
                DiagnosticDescriptors.AddArgumentListToObjectCreation,
                Location.Create(objectCreationExpression.SyntaxTree, span));
        }
    }
}
