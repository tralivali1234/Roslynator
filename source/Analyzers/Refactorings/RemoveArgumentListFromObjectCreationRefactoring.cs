// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveArgumentListFromObjectCreationRefactoring
    {
        public static void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            if (objectCreationExpression.Type?.IsMissing != false)
                return;

            if (objectCreationExpression.Initializer?.IsMissing != false)
                return;

            ArgumentListSyntax argumentList = objectCreationExpression.ArgumentList;

            if (argumentList?.Arguments.Any() != false)
                return;

            SyntaxToken openParen = argumentList.OpenParenToken;
            SyntaxToken closeParen = argumentList.CloseParenToken;

            if (openParen.IsMissing)
                return;

            if (closeParen.IsMissing)
                return;

            if (!openParen.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!closeParen.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveArgumentListFromObjectCreation, argumentList);
        }
    }
}
