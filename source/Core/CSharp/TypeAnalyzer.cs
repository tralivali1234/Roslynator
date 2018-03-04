// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    internal static class TypeAnalyzer
    {
        public static TypeAnalysis AnalyzeType(
            VariableDeclarationSyntax variableDeclaration,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (variableDeclaration == null)
                throw new ArgumentNullException(nameof(variableDeclaration));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = variableDeclaration.Type;

            Debug.Assert(type != null);

            if (type == null)
                return TypeAnalysisFlags.None;

            SeparatedSyntaxList<VariableDeclaratorSyntax> variables = variableDeclaration.Variables;

            Debug.Assert(variables.Any());

            if (!variables.Any())
                return TypeAnalysisFlags.None;

            if (variableDeclaration.IsParentKind(SyntaxKind.FieldDeclaration, SyntaxKind.EventFieldDeclaration))
                return TypeAnalysisFlags.None;

            ExpressionSyntax expression = variables[0].Initializer?.Value?.WalkDownParentheses();

            if (expression == null)
                return TypeAnalysisFlags.None;

            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(type, cancellationToken);

            if (typeSymbol == null)
                return TypeAnalysisFlags.None;

            SymbolKind kind = typeSymbol.Kind;

            if (kind == SymbolKind.ErrorType)
                return TypeAnalysisFlags.None;

            if (kind == SymbolKind.DynamicType)
                return TypeAnalysisFlags.Dynamic;

            var flags = TypeAnalysisFlags.None;

            if (type.IsVar)
            {
                flags |= TypeAnalysisFlags.Implicit;

                if (typeSymbol.SupportsExplicitDeclaration())
                    flags |= TypeAnalysisFlags.SupportsExplicit;
            }
            else
            {
                flags |= TypeAnalysisFlags.Explicit;

                if (variables.Count == 1
                    && (variableDeclaration.Parent as LocalDeclarationStatementSyntax)?.IsConst != true
                    && expression.Kind() != SyntaxKind.NullLiteralExpression
                    && typeSymbol.Equals(semanticModel.GetTypeSymbol(expression, cancellationToken)))
                {
                    flags |= TypeAnalysisFlags.SupportsImplicit;
                }
            }

            if (IsTypeObvious(expression, semanticModel, cancellationToken))
                flags |= TypeAnalysisFlags.TypeObvious;

            return flags;
        }

        public static TypeAnalysis AnalyzeType(
            DeclarationExpressionSyntax declarationExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (declarationExpression == null)
                throw new ArgumentNullException(nameof(declarationExpression));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = declarationExpression.Type;

            if (type == null)
                return TypeAnalysisFlags.None;

            if (!(declarationExpression.Designation is SingleVariableDesignationSyntax singleVariableDesignation))
                return TypeAnalysisFlags.None;

            if (!(semanticModel.GetDeclaredSymbol(singleVariableDesignation, cancellationToken) is ILocalSymbol localSymbol))
                return TypeAnalysisFlags.None;

            ITypeSymbol typeSymbol = localSymbol.Type;

            Debug.Assert(typeSymbol != null);

            if (typeSymbol == null)
                return TypeAnalysisFlags.None;

            SymbolKind kind = typeSymbol.Kind;

            if (kind == SymbolKind.ErrorType)
                return TypeAnalysisFlags.None;

            if (kind == SymbolKind.DynamicType)
                return TypeAnalysisFlags.Dynamic;

            var flags = TypeAnalysisFlags.None;

            if (type.IsVar)
            {
                flags |= TypeAnalysisFlags.Implicit;

                if (typeSymbol.SupportsExplicitDeclaration())
                    flags |= TypeAnalysisFlags.SupportsExplicit;
            }
            else
            {
                flags |= TypeAnalysisFlags.Explicit;
                flags |= TypeAnalysisFlags.SupportsImplicit;
            }

            return flags;
        }

        private static bool IsTypeObvious(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            switch (expression.Kind())
            {
                case SyntaxKind.ObjectCreationExpression:
                case SyntaxKind.ArrayCreationExpression:
                case SyntaxKind.CastExpression:
                case SyntaxKind.AsExpression:
                case SyntaxKind.ThisExpression:
                case SyntaxKind.DefaultExpression:
                    {
                        return true;
                    }
                case SyntaxKind.SimpleMemberAccessExpression:
                    {
                        ISymbol symbol = semanticModel.GetSymbol(expression, cancellationToken);

                        return symbol?.Kind == SymbolKind.Field
                            && symbol.ContainingType?.TypeKind == TypeKind.Enum;
                    }
            }

            return false;
        }

        public static TypeAnalysis AnalyzeType(ForEachStatementSyntax forEachStatement, SemanticModel semanticModel)
        {
            if (forEachStatement == null)
                throw new ArgumentNullException(nameof(forEachStatement));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = forEachStatement.Type;

            Debug.Assert(type != null);

            if (type == null)
                return TypeAnalysisFlags.None;

            ForEachStatementInfo info = semanticModel.GetForEachStatementInfo(forEachStatement);

            ITypeSymbol typeSymbol = info.ElementType;

            Debug.Assert(typeSymbol != null);

            if (typeSymbol == null)
                return TypeAnalysisFlags.None;

            SymbolKind kind = typeSymbol.Kind;

            if (kind == SymbolKind.ErrorType)
                return TypeAnalysisFlags.None;

            if (kind == SymbolKind.DynamicType)
                return TypeAnalysisFlags.Dynamic;

            var flags = TypeAnalysisFlags.None;

            if (type.IsVar)
            {
                flags |= TypeAnalysisFlags.Implicit;

                if (typeSymbol.SupportsExplicitDeclaration())
                    flags |= TypeAnalysisFlags.SupportsExplicit;
            }
            else
            {
                flags |= TypeAnalysisFlags.Explicit;

                if (info.ElementConversion.IsIdentity)
                    flags |= TypeAnalysisFlags.SupportsImplicit;
            }

            return flags;
        }
    }
}
