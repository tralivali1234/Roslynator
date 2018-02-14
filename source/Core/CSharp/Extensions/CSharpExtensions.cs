// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Helpers;

namespace Roslynator.CSharp
{
    public static class CSharpExtensions
    {
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            AttributeSyntax attribute,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, attribute, cancellationToken)
                .Symbol;
        }

        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            ConstructorInitializerSyntax constructorInitializer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, constructorInitializer, cancellationToken)
                .Symbol;
        }

        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            CrefSyntax cref,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, cref, cancellationToken)
                .Symbol;
        }

        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, expression, cancellationToken)
                .Symbol;
        }

        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            OrderingSyntax ordering,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, ordering, cancellationToken)
                .Symbol;
        }

        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            SelectOrGroupClauseSyntax selectOrGroupClause,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, selectOrGroupClause, cancellationToken)
                .Symbol;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            AttributeSyntax attribute,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, attribute, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ConstructorInitializerSyntax constructorInitializer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, constructorInitializer, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, expression, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            SelectOrGroupClauseSyntax selectOrGroupClause,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, selectOrGroupClause, cancellationToken)
                .Type;
        }

        internal static bool IsExplicitConversion(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ITypeSymbol destinationType,
            bool isExplicitInSource = false)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType.Kind == SymbolKind.ErrorType)
                return false;

            if (destinationType.SpecialType == SpecialType.System_Void)
                return false;

            Conversion conversion = semanticModel.ClassifyConversion(
                expression,
                destinationType,
                isExplicitInSource);

            return conversion.IsExplicit;
        }

        internal static bool IsImplicitConversion(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ITypeSymbol destinationType,
            bool isExplicitInSource = false)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType.Kind == SymbolKind.ErrorType)
                return false;

            if (destinationType.SpecialType == SpecialType.System_Void)
                return false;

            Conversion conversion = semanticModel.ClassifyConversion(
                expression,
                destinationType,
                isExplicitInSource);

            return conversion.IsImplicit;
        }

        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            ArgumentSyntax argument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (argument == null)
                throw new ArgumentNullException(nameof(argument));

            return DetermineParameterHelper.DetermineParameter(argument, semanticModel, allowParams, allowCandidate, cancellationToken);
        }

        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            AttributeArgumentSyntax attributeArgument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (attributeArgument == null)
                throw new ArgumentNullException(nameof(attributeArgument));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            return DetermineParameterHelper.DetermineParameter(attributeArgument, semanticModel, allowParams, allowCandidate, cancellationToken);
        }

        public static bool IsDefaultValue(
            this SemanticModel semanticModel,
            ITypeSymbol typeSymbol,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (typeSymbol.Kind == SymbolKind.ErrorType)
                return false;

            SyntaxKind kind = expression.Kind();

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Void:
                    {
                        return false;
                    }
                case SpecialType.System_Boolean:
                    {
                        return semanticModel.IsConstantValue(expression, false, cancellationToken);
                    }
                case SpecialType.System_Char:
                    {
                        return semanticModel.IsConstantValue(expression, '\0', cancellationToken);
                    }
                case SpecialType.System_SByte:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is sbyte value
                            && value == 0;
                    }
                case SpecialType.System_Byte:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is byte value
                            && value == 0;
                    }
                case SpecialType.System_Int16:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is short value
                            && value == 0;
                    }
                case SpecialType.System_UInt16:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is ushort value
                            && value == 0;
                    }
                case SpecialType.System_Int32:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is int value
                            && value == 0;
                    }
                case SpecialType.System_UInt32:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is uint value
                            && value == 0;
                    }
                case SpecialType.System_Int64:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is long value
                            && value == 0;
                    }
                case SpecialType.System_UInt64:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is ulong value
                            && value == 0;
                    }
                case SpecialType.System_Decimal:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is decimal value
                            && value == 0;
                    }
                case SpecialType.System_Single:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is float value
                            && value == 0;
                    }
                case SpecialType.System_Double:
                    {
                        return semanticModel.GetConstantValue(expression, cancellationToken).Value is double value
                            && value == 0;
                    }
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                var enumSymbol = (INamedTypeSymbol)typeSymbol;

                switch (enumSymbol.EnumUnderlyingType.SpecialType)
                {
                    case SpecialType.System_SByte:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is sbyte value
                                && value == 0;
                        }
                    case SpecialType.System_Byte:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is byte value
                                && value == 0;
                        }
                    case SpecialType.System_Int16:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is short value
                                && value == 0;
                        }
                    case SpecialType.System_UInt16:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is ushort value
                                && value == 0;
                        }
                    case SpecialType.System_Int32:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is int value
                                && value == 0;
                        }
                    case SpecialType.System_UInt32:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is uint value
                                && value == 0;
                        }
                    case SpecialType.System_Int64:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is long value
                                && value == 0;
                        }
                    case SpecialType.System_UInt64:
                        {
                            return semanticModel.GetConstantValue(expression, cancellationToken).Value is ulong value
                                && value == 0;
                        }
                }

                return false;
            }

            if (typeSymbol.IsReferenceType)
            {
                if (kind == SyntaxKind.NullLiteralExpression)
                {
                    return true;
                }
                else
                {
                    Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                    if (optional.HasValue)
                        return optional.Value == null;
                }
            }

            if (typeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T)
                && kind == SyntaxKind.NullLiteralExpression)
            {
                return true;
            }

            if (kind == SyntaxKind.DefaultExpression)
            {
                var defaultExpression = (DefaultExpressionSyntax)expression;

                TypeSyntax type = defaultExpression.Type;

                return type != null
                    && typeSymbol.Equals(semanticModel.GetTypeSymbol(type, cancellationToken));
            }

            return false;
        }

        private static bool IsConstantValue(this SemanticModel semanticModel, ExpressionSyntax expression, bool value, CancellationToken cancellationToken = default(CancellationToken))
        {
            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            return optional.HasValue
                && optional.Value is bool value2
                && value == value2;
        }

        private static bool IsConstantValue(this SemanticModel semanticModel, ExpressionSyntax expression, char value, CancellationToken cancellationToken = default(CancellationToken))
        {
            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            return optional.HasValue
                && optional.Value is char value2
                && value == value2;
        }

        public static MethodInfo GetExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetExtensionMethodInfo(semanticModel, expression, ExtensionMethodKind.None, cancellationToken);
        }

        public static MethodInfo GetExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ExtensionMethodKind extensionMethodKind,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (GetSymbol(semanticModel, expression, cancellationToken) is IMethodSymbol methodSymbol)
            {
                ExtensionMethodInfo extensionMethodInfo = ExtensionMethodInfo.Create(methodSymbol, semanticModel, extensionMethodKind);

                if (extensionMethodInfo.Symbol != null)
                {
                    return extensionMethodInfo.MethodInfo;
                }
            }

            return default(MethodInfo);
        }

        public static MethodInfo GetMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (GetSymbol(semanticModel, expression, cancellationToken) is IMethodSymbol methodSymbol)
            {
                return new MethodInfo(methodSymbol, semanticModel);
            }

            return default(MethodInfo);
        }

        internal static MethodDeclarationSyntax GetOtherPart(
            this SemanticModel semanticModel,
            MethodDeclarationSyntax methodDeclaration,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IMethodSymbol methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken);

            IMethodSymbol otherSymbol = methodSymbol.PartialDefinitionPart ?? methodSymbol.PartialImplementationPart;

            if (otherSymbol != null)
                return (MethodDeclarationSyntax)otherSymbol.GetSyntax(cancellationToken);

            return null;
        }
    }
}
