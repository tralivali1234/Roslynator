// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis
{
    internal static class CallConfigureAwaitAnalysis
    {
        public static bool IsFixable(
            AwaitExpressionSyntax awaitExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ExpressionSyntax expression = awaitExpression.Expression;

            if (expression?.Kind() != SyntaxKind.InvocationExpression)
                return false;

            var methodSymbol = semanticModel.GetSymbol(expression, cancellationToken) as IMethodSymbol;

            return methodSymbol?.ReturnType.IsTaskOrInheritsFromTask(semanticModel) == true
                && semanticModel.GetTypeByMetadataName(MetadataNames.System_Runtime_CompilerServices_ConfiguredTaskAwaitable_T) != null;
        }
    }
}
