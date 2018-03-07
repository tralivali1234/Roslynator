// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveRedundantConstructorRefactoring
    {
        public static void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            var constructor = (ConstructorDeclarationSyntax)context.Node;

            if (constructor.ContainsDiagnostics)
                return;

            if (constructor.ParameterList?.Parameters.Any() != false)
                return;

            if (constructor.Body?.Statements.Any() != false)
                return;

            SyntaxTokenList modifiers = constructor.Modifiers;

            if (!modifiers.Contains(SyntaxKind.PublicKeyword))
                return;

            if (modifiers.Contains(SyntaxKind.StaticKeyword))
                return;

            ConstructorInitializerSyntax initializer = constructor.Initializer;

            if (initializer != null
                && initializer.ArgumentList?.Arguments.Any() != false)
            {
                return;
            }

            if (!IsSingleInstanceConstructor(constructor))
                return;

            if (constructor.HasDocumentationComment())
                return;

            if (!constructor.DescendantTrivia(constructor.Span).All(f => f.IsWhitespaceOrEndOfLineTrivia()))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveRedundantConstructor, constructor);
        }

        private static bool IsSingleInstanceConstructor(ConstructorDeclarationSyntax constructor)
        {
            MemberDeclarationListInfo info = SyntaxInfo.MemberDeclarationListInfo(constructor.Parent);

            return info.Success
                && info
                    .Members
                    .OfType<ConstructorDeclarationSyntax>()
                    .All(f => f.Equals(constructor) || f.Modifiers.Contains(SyntaxKind.StaticKeyword));
        }

        public static Task<Document> RefactorAsync(
            Document document,
            ConstructorDeclarationSyntax constructorDeclaration,
            CancellationToken cancellationToken)
        {
            return document.RemoveMemberAsync(constructorDeclaration, cancellationToken);
        }
    }
}
