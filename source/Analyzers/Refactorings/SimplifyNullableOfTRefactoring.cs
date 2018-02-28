// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class SimplifyNullableOfTRefactoring
    {
        public static void AnalyzeGenericName(SyntaxNodeAnalysisContext context)
        {
            var genericName = (GenericNameSyntax)context.Node;

            if (genericName.IsParentKind(
                SyntaxKind.QualifiedName,
                SyntaxKind.UsingDirective,
                SyntaxKind.NameMemberCref,
                SyntaxKind.QualifiedCref))
            {
                return;
            }

            if (IsWithinNameOfExpression(genericName, context.SemanticModel, context.CancellationToken))
                return;

            if (genericName
                .TypeArgumentList?
                .Arguments
                .SingleOrDefault(shouldThrow: false)?
                .IsKind(SyntaxKind.OmittedTypeArgument) != false)
            {
                return;
            }

            if (!(context.SemanticModel.GetSymbol(genericName, context.CancellationToken) is INamedTypeSymbol namedTypeSymbol))
                return;

            if (!namedTypeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyNullableOfT, genericName);
        }

        public static void AnalyzeQualifiedName(SyntaxNodeAnalysisContext context)
        {
            var qualifiedName = (QualifiedNameSyntax)context.Node;

            if (qualifiedName.IsParentKind(SyntaxKind.UsingDirective, SyntaxKind.QualifiedCref))
                return;

            if (IsWithinNameOfExpression(qualifiedName, context.SemanticModel, context.CancellationToken))
                return;

            if (!(context.SemanticModel.GetSymbol(qualifiedName, context.CancellationToken) is INamedTypeSymbol typeSymbol))
                return;

            if (CSharpFacts.IsPredefinedType(typeSymbol.SpecialType))
                return;

            if (!typeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyNullableOfT, qualifiedName);
        }

        public static Task<Document> RefactorAsync(
            Document document,
            TypeSyntax type,
            TypeSyntax nullableType,
            CancellationToken cancellationToken)
        {
            TypeSyntax newType = NullableType(nullableType.WithoutTrivia(), QuestionToken())
                .WithTriviaFrom(type)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(type, newType, cancellationToken);
        }

        private static bool IsWithinNameOfExpression(
            SyntaxNode node,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            for (node = node.Parent; node != null; node = node.Parent)
            {
                SyntaxKind kind = node.Kind();

                if (kind == SyntaxKind.InvocationExpression)
                {
                    if (CSharpUtility.IsNameOfExpression((InvocationExpressionSyntax)node, semanticModel, cancellationToken))
                        return true;
                }
                else if (kind == SyntaxKind.TypeArgumentList)
                {
                    break;
                }

                if (node is StatementSyntax
                    || node is MemberDeclarationSyntax)
                {
                    break;
                }
            }

            return false;
        }
    }
}
