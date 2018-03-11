// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class CallFindInsteadOfFirstOrDefaultRefactoring
    {
        public static void Analyze(
            SyntaxNodeAnalysisContext context,
            MemberInvocationExpressionInfo invocationInfo)
        {
            SemanticModel semanticModel = context.SemanticModel;
            CancellationToken cancellationToken = context.CancellationToken;

            IMethodSymbol methodSymbol = semanticModel.GetReducedExtensionMethodInfo(invocationInfo.InvocationExpression, cancellationToken).Symbol;

            if (methodSymbol == null)
                return;

            if (!SymbolUtility.IsLinqExtensionOfIEnumerableOfTWithPredicate(methodSymbol, semanticModel, "FirstOrDefault"))
                return;

            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(invocationInfo.Expression, cancellationToken);

            if (typeSymbol == null)
                return;

            if (typeSymbol.Kind == SymbolKind.ArrayType
                && ((IArrayTypeSymbol)typeSymbol).Rank == 1)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.CallFindInsteadOfFirstOrDefault, invocationInfo.Name);
            }
            else if (typeSymbol.IsConstructedFrom(semanticModel.GetTypeByMetadataName(MetadataNames.System_Collections_Generic_List_T)))
            {
                context.ReportDiagnostic(DiagnosticDescriptors.CallFindInsteadOfFirstOrDefault, invocationInfo.Name);
            }
        }

        internal static async Task<Document> RefactorAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            CancellationToken cancellationToken)
        {
            MemberInvocationExpressionInfo info = SyntaxInfo.MemberInvocationExpressionInfo(invocation);

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(info.Expression, cancellationToken);

            if ((typeSymbol as IArrayTypeSymbol)?.Rank == 1)
            {
                NameSyntax arrayName = ParseName("System.Array")
                    .WithLeadingTrivia(invocation.GetLeadingTrivia())
                    .WithSimplifierAnnotation();

                MemberAccessExpressionSyntax newMemberAccess = SimpleMemberAccessExpression(
                    arrayName,
                    info.OperatorToken,
                    IdentifierName("Find").WithTriviaFrom(info.Name));

                ArgumentListSyntax argumentList = invocation.ArgumentList;

                InvocationExpressionSyntax newInvocation = InvocationExpression(
                    newMemberAccess,
                    ArgumentList(
                        Argument(info.Expression.WithoutTrivia()),
                        argumentList.Arguments.First()
                    ).WithTriviaFrom(argumentList));

                return await document.ReplaceNodeAsync(invocation, newInvocation, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                IdentifierNameSyntax newName = IdentifierName("Find").WithTriviaFrom(info.Name);

                return await document.ReplaceNodeAsync(info.Name, newName, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
