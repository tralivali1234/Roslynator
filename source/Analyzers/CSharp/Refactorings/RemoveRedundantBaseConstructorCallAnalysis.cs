// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantBaseConstructorCallAnalysis
    {
        public static void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            var constructor = (ConstructorDeclarationSyntax)context.Node;

            ConstructorInitializerSyntax initializer = constructor.Initializer;

            if (initializer?.Kind() == SyntaxKind.BaseConstructorInitializer
                && initializer.ArgumentList?.Arguments.Count == 0
                && initializer
                    .DescendantTrivia(initializer.Span)
                    .All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.RemoveRedundantBaseConstructorCall,
                    initializer);
            }
        }
    }
}
