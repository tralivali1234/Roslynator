// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UnconstrainedTypeParameterCheckedForNullRefactoring
    {
        public static void AnalyzeEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.ContainsDiagnostics)
                return;

            Analyze(context, (BinaryExpressionSyntax)context.Node, NullCheckStyles.EqualsToNull);
        }

        public static void AnalyzeNotEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.ContainsDiagnostics)
                return;

            Analyze(context, (BinaryExpressionSyntax)context.Node, NullCheckStyles.NotEqualsToNull);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, BinaryExpressionSyntax binaryExpression, NullCheckStyles allowedStyles)
        {
            NullCheckExpressionInfo nullCheck = SyntaxInfo.NullCheckExpressionInfo(binaryExpression, allowedStyles: allowedStyles);
            if (nullCheck.Success
                && IsUnconstrainedTypeParameter(context.SemanticModel.GetTypeSymbol(nullCheck.Expression, context.CancellationToken))
                && !binaryExpression.SpanContainsDirectives())
            {
                context.ReportDiagnostic(DiagnosticDescriptors.UnconstrainedTypeParameterCheckedForNull, binaryExpression);
            }
        }

        private static bool IsUnconstrainedTypeParameter(ITypeSymbol typeSymbol)
        {
            return typeSymbol?.IsTypeParameter() == true
                && VerifyConstraint((ITypeParameterSymbol)typeSymbol, allowReference: false, allowValueType: false, allowConstructor: true);
        }

        private static bool VerifyConstraint(
            ITypeParameterSymbol typeParameterSymbol,
            bool allowReference,
            bool allowValueType,
            bool allowConstructor)
        {
            if (typeParameterSymbol == null)
                throw new ArgumentNullException(nameof(typeParameterSymbol));

            if (!CheckConstraint(typeParameterSymbol, allowReference, allowValueType, allowConstructor))
                return false;

            ImmutableArray<ITypeSymbol> constraintTypes = typeParameterSymbol.ConstraintTypes;

            if (!constraintTypes.Any())
                return true;

            var stack = new Stack<ITypeSymbol>(constraintTypes);

            while (stack.Count > 0)
            {
                ITypeSymbol type = stack.Pop();

                switch (type.TypeKind)
                {
                    case TypeKind.Class:
                        {
                            if (!allowReference)
                                return false;

                            break;
                        }
                    case TypeKind.Struct:
                        {
                            if (allowValueType)
                                return false;

                            break;
                        }
                    case TypeKind.Interface:
                        {
                            break;
                        }
                    case TypeKind.TypeParameter:
                        {
                            var typeParameterSymbol2 = (ITypeParameterSymbol)type;

                            if (!CheckConstraint(typeParameterSymbol2, allowReference, allowValueType, allowConstructor))
                                return false;

                            foreach (ITypeSymbol constraintType in typeParameterSymbol2.ConstraintTypes)
                                stack.Push(constraintType);

                            break;
                        }
                    case TypeKind.Error:
                        {
                            return false;
                        }
                    default:
                        {
                            Debug.Fail(type.TypeKind.ToString());
                            return false;
                        }
                }
            }

            return true;
        }

        private static bool CheckConstraint(
            ITypeParameterSymbol typeParameterSymbol,
            bool allowReference,
            bool allowValueType,
            bool allowConstructor)
        {
            return (allowReference || !typeParameterSymbol.HasReferenceTypeConstraint)
                && (allowValueType || !typeParameterSymbol.HasValueTypeConstraint)
                && (allowConstructor || !typeParameterSymbol.HasConstructorConstraint);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            BinaryExpressionSyntax binaryExpression,
            ITypeSymbol typeSymbol,
            CancellationToken cancellationToken)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            INamedTypeSymbol equalityComparerSymbol = semanticModel
                .GetTypeByMetadataName(MetadataNames.System_Collections_Generic_EqualityComparer_T)
                .Construct(typeSymbol);

            ExpressionSyntax newNode = InvocationExpression(
                SimpleMemberAccessExpression(
                    SimpleMemberAccessExpression(equalityComparerSymbol.ToMinimalTypeSyntax(semanticModel, binaryExpression.SpanStart), IdentifierName("Default")), IdentifierName("Equals")),
                ArgumentList(
                    Argument(binaryExpression.Left.WithoutTrivia()),
                    Argument(DefaultExpression(typeSymbol.ToTypeSyntax()))));

            if (binaryExpression.IsKind(SyntaxKind.NotEqualsExpression))
                newNode = LogicalNotExpression(newNode);

            newNode = newNode
                .WithTriviaFrom(binaryExpression)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(binaryExpression, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
