// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class CallThenByInsteadOfOrderByRefactoring
    {
        public static void Analyze(SyntaxNodeAnalysisContext context, MemberInvocationExpressionInfo invocationInfo)
        {
            ExpressionSyntax expression = invocationInfo.Expression;

            if (expression.IsKind(SyntaxKind.InvocationExpression))
            {
                MemberInvocationExpressionInfo invocationInfo2 = SyntaxInfo.MemberInvocationExpressionInfo((InvocationExpressionSyntax)expression);

                if (invocationInfo2.Success)
                {
                    switch (invocationInfo2.NameText)
                    {
                        case "OrderBy":
                        case "OrderByDescending":
                            {
                                if (IsLinqExtensionOfIEnumerableOfT(context, invocationInfo.InvocationExpression)
                                    && IsLinqExtensionOfIEnumerableOfT(context, invocationInfo2.InvocationExpression))
                                {
                                    context.ReportDiagnostic(
                                        DiagnosticDescriptors.CallThenByInsteadOfOrderBy,
                                        invocationInfo.Name,
                                        (invocationInfo.NameText == "OrderByDescending") ? "Descending" : null);
                                }

                                break;
                            }
                    }
                }
            }
        }

        private static bool IsLinqExtensionOfIEnumerableOfT(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocationExpression)
        {
            MethodInfo methodInfo = context.SemanticModel.GetExtensionMethodInfo(invocationExpression, ExtensionMethodKind.None, context.CancellationToken);

            return methodInfo.Symbol != null
                && methodInfo.IsName("OrderBy", "OrderByDescending")
                && methodInfo.IsLinqExtensionOfIEnumerableOfT();
        }

        public static Task<Document> RefactorAsync(
            Document document,
            InvocationExpressionSyntax invocationExpression,
            string newName,
            CancellationToken cancellationToken)
        {
            InvocationExpressionSyntax newInvocationExpression = RefactoringUtility.ChangeInvokedMethodName(invocationExpression, newName);

            return document.ReplaceNodeAsync(invocationExpression, newInvocationExpression, cancellationToken);
        }
    }
}
