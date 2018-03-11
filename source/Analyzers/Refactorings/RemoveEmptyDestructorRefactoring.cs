// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEmptyDestructorRefactoring
    {
        public static void AnalyzeDestructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            var destructor = (DestructorDeclarationSyntax)context.Node;

            if (destructor.ContainsDiagnostics)
                return;

            if (destructor.ContainsDirectives)
                return;

            if (destructor.AttributeLists.Any())
                return;

            if (destructor.Body?.Statements.Count != 0)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyDestructor, destructor);
        }
    }
}
